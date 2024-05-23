using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Recipe_Blog.Models;
using Recipe_Blog.PDFGenerator;
using Recipe_Blog.EmailSending;
using System.Net.Mail;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.Identity;


namespace Recipe_Blog.Controllers
{
    public class UserController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        // GET: TestAboutus
        public  IActionResult aboutus()
        {
            return _context.Aboutus != null ?
                        View( _context.Aboutus.First()) :
                        Problem("Entity set 'ModelContext.Aboutus'  is null.");
        }


        // GET: Card
        public async Task<IActionResult> Card(decimal? id)
        {
            if (id == null) return NotFound();
            var recipe = await _context.Recipes.SingleOrDefaultAsync(r=>r.Id==id);
            if (recipe == null) return NotFound();
            ViewBag.RecipeCard = recipe;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Card(PaymentViewModel paymentViewModel, decimal RecipeId)
        {

             var uid = HttpContext.Session.GetInt32("userSession");
             if (uid == null) return RedirectToAction("Login","Auth");
             Visa card =await  _context.Visas.SingleOrDefaultAsync(x => x.UserId == uid)??new Visa();
            if(card==null) return RedirectToAction("Login", "Auth");
            var recipe = _context.Recipes.SingleOrDefault(x=>x.Id==RecipeId);


            if (ModelState.IsValid)
            {
                if (paymentViewModel.CardId.ToString() != card?.Cardnumber.ToString())
                {
                    ModelState.AddModelError("", "Wrong Card number");
                    return View(paymentViewModel);
                }
                if (paymentViewModel.Cvc != card.Cvc)
                {
                    ModelState.AddModelError("", "Wrong cvc");
                    return View(paymentViewModel);
                }
                if (paymentViewModel.Fullname != card.Nameoncard)
                {
                    ModelState.AddModelError("", "Wrong Name on Card");
                    return View(paymentViewModel);
                }

                if (
                    (double)card!.Amount! < ((double) recipe!.Price! * 0.16)+ (double)recipe.Price)
                {
                    ModelState.AddModelError("", "your Balance is lower than total price");
                    return View(paymentViewModel);
                }
                Request request=new Request();
                request.RecipeId=RecipeId;
                request.UserId = uid;
                decimal totalPrice = Convert.ToDecimal(recipe.Price * Convert.ToDecimal(0.16)) + (decimal)recipe.Price;
                request.Tax = ((double)recipe.Price * 0.16);
                _context.Add(request);
                await _context.SaveChangesAsync();
                card.Amount -= (decimal)totalPrice;
                _context.Update(card);
                await _context.SaveChangesAsync();
                PurchasedPDF(RecipeId);
                return RedirectToAction(nameof(Index));
            }
            
            ModelState.AddModelError("", "Something went Wrong");
            return View(paymentViewModel);
        }

        public bool BuyRecipe(decimal recipeId, decimal userId)
        {
                var recipe = _context.Recipes.Find(recipeId);
                if (recipe == null) return false;

                var request = new Request()
                {
                    RecipeId = recipeId,
                    UserId = userId,
                    Requestdate = DateTimeOffset.Now
                };

                _context.Requests.Add(request);
                _context.SaveChanges();
                return true;
        }


        public bool ProcessPayment(decimal userId, long cardNumber, byte cvc, decimal amount)
        {
            // Mock payment processing
            // You should replace this with actual payment gateway integration
            bool paymentSuccess = true; // Assume payment is successful

            if (paymentSuccess)
            {
                // Save payment details

                var visa = new Visa()
                {
                    UserId = userId,
                    Cardnumber = cardNumber,
                    Cvc = cvc,
                 //   Amount = amount,
                    Expdate = "12/24" // Example expiry date
                };

                    _context.Visas.Add(visa);
                    _context.SaveChanges();
                

                // Send invoice
                SendInvoice(userId, amount);
                return true;
            }

            return false;
        }

        public void SendInvoice(decimal userId, decimal amount)
        {
            var login = _context.Logins.Find(userId);
            if (login != null)
            {
                // Generate PDF Invoice
                var pdfPath = GeneratePdfInvoice(login, amount);

                // Send Email
                SendEmail(login.Email, pdfPath);
            }
        }

        private string GeneratePdfInvoice(Login login, decimal amount)
        {
            // Use iTextSharp or similar library to generate a PDF invoice
            // Return the path to the generated PDF
            string pdfPath = "/path/to/invoice.pdf";
            return pdfPath;
        }

        private void SendEmail(string emailAddress, string pdfPath)
        {
            using (var message = new MailMessage("your-email@example.com", emailAddress))
            {
                message.Subject = "Your Recipe Purchase Invoice";
                message.Body = "Please find attached your invoice for the recipe purchase.";
                message.Attachments.Add(new Attachment(pdfPath));

                using (var client = new SmtpClient("smtp.example.com"))
                {
                    client.Send(message);
                }
            }
        }


        public IActionResult PurchasedPDF(decimal id)
        {

            var recipe = _context.Recipes
                                 .Include(r => r.Category)
                                 .Include(r => r.User)
                                 .Include(r => r.RecipeStatus)
                                 .FirstOrDefault(r => r.Id == id);

            if (recipe == null)
            {
                return NotFound();
            }

            var pdfUpdater = new PDFGenerating(_context, _webHostEnvironment);
            string filePath = pdfUpdater.UpdateRecipePdf(recipe);

            var userId = HttpContext.Session.GetInt32("userSession");
            var login = _context.Logins.Include(x => x.User).SingleOrDefault(x => x.UserId == userId);

            if (login == null)
            {
                return BadRequest("User not found.");
            }

            string customerEmail = login.Email;
            string subject = "Discover the Authentic Taste of Shishani: New Recipe Inside!";
            string body = $@"Hello {login.User.Firstname} {login.User.Lastname},

We are thrilled to bring you a brand-new recipe from the Shishani Recipe Blog that will transport your taste buds straight to the heart of traditional cuisine.

This week’s feature: Authentic Shishani Delight

This recipe is a family treasure, passed down through generations, and it’s packed with the rich, aromatic flavors that define Shishani cooking. Whether you’re a seasoned cook or just starting your culinary journey, our step-by-step instructions make it easy to create this delicious dish at home.

Ingredients:
A list of fresh, accessible ingredients that you can find at your local grocery store

Instructions:
Simple, clear, and detailed steps to ensure your success

But that’s not all! We’ve also included a special section on the cultural significance of this dish and how it’s traditionally served. It’s not just about cooking; it’s about connecting with the rich heritage and stories behind every bite.

Ready to get started? Click here to view the full recipe on our blog.

We can’t wait to hear about your cooking experience! Share your photos and tag us with #ShishaniRecipe for a chance to be featured on our social media.

Happy cooking!

Warm regards,
Shishani Recipe Blog Team";

            EmailGenerator emailGenerator = new EmailGenerator();
            emailGenerator.SendEmailWithPDF(customerEmail, subject, body, filePath);

            return RedirectToAction(nameof(Index));
        }

        //public IActionResult PurchasedPDF(decimal id)
        //{
        //    var recipe = _context.Recipes
        //                         .Include(r => r.Category)
        //                         .Include(r => r.User)
        //                         .Include(r => r.RecipeStatus)
        //                         .FirstOrDefault(r => r.Id == id);
        //    if (recipe == null)
        //    {
        //        return NotFound();
        //    }
        //    var pdfUpdater = new PDFGenerating(_context, _webHostEnvironment);
        //    string filePath = pdfUpdater.UpdateRecipePdf(recipe);

        //    var userId = HttpContext.Session.GetInt32("userSession");
        //    var login =_context.Logins.Include(x=>x.User).Where(x=>x.UserId == userId).SingleOrDefault();

        //    string customerEmail = login.Email;
        //    string subject = "Discover the Authentic Taste of Shishani: New Recipe Inside!";
        //    string body = $"Hello {login.User.Firstname} {login.User.Lastname},\r\n\r\nWe are thrilled to bring you a brand-new recipe from the Shishani Recipe Blog that will transport your taste buds straight to the heart of traditional cuisine.\r\n\r\nThis week’s feature: Authentic Shishani Delight\r\n\r\nThis recipe is a family treasure, passed down through generations, and it’s packed with the rich, aromatic flavors that define Shishani cooking. Whether you’re a seasoned cook or just starting your culinary journey, our step-by-step instructions make it easy to create this delicious dish at home.\r\n\r\nIngredients:\r\nA list of fresh, accessible ingredients that you can find at your local grocery store\r\nInstructions:\r\nSimple, clear, and detailed steps to ensure your success\r\nBut that’s not all! We’ve also included a special section on the cultural significance of this dish and how it’s traditionally served. It’s not just about cooking; it’s about connecting with the rich heritage and stories behind every bite.\r\n\r\nReady to get started? Click here to view the full recipe on our blog.\r\n\r\nWe can’t wait to hear about your cooking experience! Share your photos and tag us with #ShishaniRecipe for a chance to be featured on our social media.\r\n\r\nHappy cooking!\r\n\r\nWarm regards,\r\n\r\nShishani Recipe Blog Team";

        //    EmailGenerator emailGenerator = new EmailGenerator();
        //    emailGenerator.SendEmailWithPDF(customerEmail, subject, body, filePath);


        //    // You can return the path or a success message, or trigger further actions
        //    return RedirectToAction(nameof(Index));
        //}

        // GET: UserController
        public async Task<ActionResult> Index()
        {
            ViewBag.homepage = _context.Homepages.FirstOrDefaultAsync();
            var users = _context.Users.ToList(); 
            var categories = _context.Categories.ToList(); 
            var recipes=_context.Recipes.Where(x=>x.RecipeStatusId==2).ToList();
            var testimonials= _context.Testimonials.Where(x=>x.TestimonialStatusId==2).ToList();
            var chef=_context.Users.Where(u=>u.RoleId==2).ToList();
            var home= _context.Homepages.ToList();
            var contact=_context.Contactus.ToList();
            var model = Tuple.Create<IEnumerable<User>, IEnumerable<Category>, IEnumerable<Recipe>, IEnumerable<Testimonial>, IEnumerable<User>, IEnumerable<Homepage>>(users,categories,recipes,testimonials,chef,home); 
            
            return View(model);
        }

        // GET: Visas
        //public async Task<IActionResult> Cards()
        //{
        //    var modelContext = _context.Visas.Include(v => v.User);
        //    return View(await modelContext.ToListAsync());
        //}


        //GET : contactus 
        public ActionResult ContactUs()
        {
                
            return View( );
        }
        [HttpPost]
        public ActionResult ContactUs( [Bind("Id,Username,Useremail,Subject,Message")]Contactu c) 
        {
            if (ModelState.IsValid)
            {
                _context.Add(c);
               _context.SaveChangesAsync();
                return RedirectToAction(nameof(ContactUs));
            }

            return View(c);
        }

        // GET: UserController
        public async Task<ActionResult> Testimonials()
        {
            var testimonials = _context.Testimonials
            .Include(u=>u.User).Where(u=>u.User.Id==u.UserId)
            .ToList();
            return View(testimonials);
        }

        // GET: UserCategories
        public async Task<IActionResult> Categories()
        {
            return _context.Categories != null ?
                        View(await _context.Categories.ToListAsync()) :
                        Problem("Entity set 'ModelContext.Categories'  is null.");
        }
        // GET: UserCategories/Details/5
        public async Task<IActionResult> CategoryDetails(decimal? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(u=>u.Recipes)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            ViewBag.Category = category;
            return View(category);
        }
        // GET: Recipes
        public async Task<IActionResult> Recipes()
        {
            var modelContext = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.User)
                .OrderBy(x => x.Creationdate)
                .ToListAsync();
            return View( modelContext);
        }
        [HttpPost]
        public async Task<IActionResult> Recipes(string? recipe)
        {
            var modelContext = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.User)
                .OrderBy(x => x.Creationdate)
                .ToListAsync();
            if(!String.IsNullOrEmpty(recipe))
                modelContext=modelContext.Where(x=>x.Name.ToLower().Contains(recipe.ToLower())).ToList();
                
            return View(modelContext);
        }
        // GET: RecipeDetails
        public async Task<IActionResult> RecipeDetails(decimal? id)
        {
            var modelContext = _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.User)
                .SingleOrDefaultAsync(x => x.Id == id);
            return View(await modelContext);
        }

        //GET : Chefs
        public async Task<IActionResult> Chefs()
		{
			var modelContext = _context.Users
				.Include(r => r.Recipes)
				.Include(r => r.Logins)
                .Where(r=>r.RoleId==2)
				.OrderBy(x => x.Recipes.Count());
			return View(await modelContext.ToListAsync());
		}
        //GET : ChefDetails
        public async Task<IActionResult> ChefDetails(decimal? id)
        {
            var modelContext = _context.Users
                .Include(r => r.Recipes)
                .Include(r => r.Logins)
                .Where(r => r.Id == id).SingleOrDefaultAsync();
            return View(await modelContext);
        }
        public ActionResult Shop()
        {
            return View();
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/CreateTestimonials
        public ActionResult CreateTestimonials()
        {
            return View();
        }

        // POST: UserController/CreateTestimonials
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTestimonials(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // GET: UserController/AcceptedTestimonials
        public async Task<ActionResult> AcceptedTestimonials()
        {
            var model=_context.Testimonials.Include(u=>u.User).Where(u=>u.TestimonialStatusId==2).ToListAsync();
            return View(await model);
        }
        
        //GET: user/Checkout
        public async Task<IActionResult> Checkout(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.User)
                .Include(r => r.User.Logins)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }
            var r = _context.Recipes.SingleOrDefault(u=>u.Id==id);
            ViewBag.currentUser = HttpContext.Session.GetInt32("userSession");

            ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View(recipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Purchase purchase)
        {
            if (!ModelState.IsValid)
            {
                return View(purchase);
            }

            var userData = await _context.Users.Include(v => v.Visas).SingleOrDefaultAsync();
            var visaData = _context.Visas.SingleOrDefaultAsync(check => check.Id == purchase.UserId);
            if (visaData == null) return NotFound();
            //ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id", request.RecipeId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", request.UserId);
            foreach (var i in userData.Visas)
            {
                //if (i.Amount < purchase.Amount)
                //{
                //    ModelState.AddModelError("", "you dont have a money");
                //    return View(purchase);
                //}
                if (i.Cardnumber != purchase.Cardnumber)
                {
                    ModelState.AddModelError("", "your card number is invalid");
                    return View(purchase);
                }
                if (i.Cvc != purchase.Cvc)
                {
                    ModelState.AddModelError("", "your Cvc number is invalid");
                    return View(purchase);
                }
                //if (i.Expdate != purchase.Expdate)
                //{
                //    ModelState.AddModelError("", "your Expire date number is invalid");
                //    return View(purchase);
                //}
                if (i.Nameoncard.ToLower() != purchase.Nameoncard.ToLower().Trim())
                {
                    ModelState.AddModelError("", "your Name is invalid");
                    return View(purchase);
                }
            }
            Request request = new();
            request.UserId = purchase.UserId;
            request.RecipeId = purchase.RecipeId;
            request.Requestdate = DateTime.Now;
            _context.Add(request);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> EditProfile(decimal? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Roleid", "Roleid", user.RoleId);
            return View(user);
        }

        // POST: Users/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(decimal id, [Bind("Id,Firstname,Lastname,Birthdate,RoleId,Imgpath")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Roleid", "Roleid", user.RoleId);
            return View(user);
        }


        // GET: Users/EditAccount/5
        public async Task<IActionResult> EditAccount(decimal? id)
        {
            if (id == null || _context.Logins == null)
            {
                return NotFound();
            }

            var user = await _context.Logins.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            
            return View(user);
        }

        // POST: Users/EditAccount/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(decimal id, [Bind("Id,Email,UserName,Password,UserId")] Login login)
        {
            if (id != login.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(login);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(login.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(login);
        }


        // GET: Users/ProfileDetails/5
        public async Task<IActionResult> ProfileDetails(decimal? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        // GET: Users/AccountDetails/5
        public async Task<IActionResult> AccountDetails(decimal? id)
        {
            if (id == null || _context.Logins == null)
            {
                return NotFound();
            }

            var login = await _context.Logins
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (login == null)
            {
                return NotFound();
            }

            return View(login);
        }
        private bool UserExists(decimal id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool CategoryExists(decimal id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

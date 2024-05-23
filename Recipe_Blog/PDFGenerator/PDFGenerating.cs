using iTextSharp.text.pdf;
using iTextSharp.text;
using Recipe_Blog.Models;
using Microsoft.AspNetCore.Mvc;

namespace Recipe_Blog.PDFGenerator
{
    public class PDFGenerating
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PDFGenerating(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public string UpdateRecipePdf(Recipe recipe)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string pdfDirectory = Path.Combine(wwwRootPath, "PDF");

            if (!Directory.Exists(pdfDirectory))
            {
                Directory.CreateDirectory(pdfDirectory);
            }

            string fileName = $"Shishani.Recipes.Blog.{recipe.UserId}.{recipe.User.Firstname}.{recipe.User.Lastname}.{recipe.Name}.pdf";
            string path = Path.Combine(pdfDirectory, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                Document document = new Document();
                PdfWriter.GetInstance(document, stream);
                document.Open();

                var shishaniRecipe = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20);
                document.Add(new Paragraph("Thank You For buying From Shishani Recipe Blog.", shishaniRecipe));
                document.Add(new Paragraph("\n"));

                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                document.Add(new Paragraph("Recipe : " + recipe.Name, titleFont));
                document.Add(new Paragraph("\n"));

                var subTitleFont = FontFactory.GetFont(FontFactory.HELVETICA, 15);
                document.Add(new Paragraph($"Chef: {recipe.User?.Firstname} {recipe.User?.Lastname}", subTitleFont));
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph($"Category : {recipe.Category?.Name}", subTitleFont));
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph($"Posted At: {recipe.Creationdate/*.ToString("dd/MMMM/yyyy")*/}", subTitleFont));
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph($"Price: ${recipe.Price}", subTitleFont));
                document.Add(new Paragraph("\n"));

                var bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 14);
                document.Add(new Paragraph("Description: ", bodyFont));
                document.Add(new Paragraph(recipe.Description + ".", bodyFont));
                document.Add(new Paragraph("\n"));

                document.Add(new Paragraph($"Ingredients Used In {recipe.Name}: ", bodyFont));
                document.Add(new Paragraph(recipe.Ingredients + ".", bodyFont));
                document.Add(new Paragraph("\n"));

                document.Add(new Paragraph($"Instructions To Make {recipe.Name}: ", bodyFont));
                document.Add(new Paragraph(recipe.Instructions + ".", bodyFont));

                document.Close();
            }
            return path;
        }
    }

    //public class PDFGenerating
    //{
    //    private readonly ModelContext _context;
    //    private readonly IWebHostEnvironment _webHostEnvironment;
    //    public PDFGenerating(ModelContext context, IWebHostEnvironment webHostEnvironment)
    //    {
    //        _context = context;
    //        _webHostEnvironment = webHostEnvironment;
    //    }


    //    public string UpdateRecipePdf(Recipe recipe)
    //    {
    //        string wwwRootPath = _webHostEnvironment.WebRootPath;
    //        string pdfDirectory = Path.Combine(wwwRootPath, "PDF");

    //        if (!Directory.Exists(pdfDirectory))
    //        {
    //            Directory.CreateDirectory(pdfDirectory);
    //        }

    //        string fileName = $"Shishani.Recipes.Blog{recipe.UserId}.{recipe.User.Firstname}.{recipe.User.Lastname}.{recipe.Name}.pdf";
    //        string path = Path.Combine(pdfDirectory, fileName);

    //        using (var stream = new FileStream(path, FileMode.Create))
    //        {
    //            Document document = new Document();
    //            PdfWriter.GetInstance(document, stream);
    //            document.Open();

    //            // Title
    //            var shishaniRecipe = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20);
    //            document.Add(new Paragraph("Thank You For buying From Shishani Recipe Blog.", shishaniRecipe));
    //            document.Add(new Paragraph("\n"));
    //            // Title
    //            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
    //            document.Add(new Paragraph("Recipe : " + recipe.Name, titleFont));
    //            document.Add(new Paragraph("\n"));
    //            // Subtitle
    //            var subTitleFont = FontFactory.GetFont(FontFactory.HELVETICA, 15);
    //            document.Add(new Paragraph($"Chef: {recipe.User?.Firstname} {recipe.User?.Lastname}", subTitleFont));
    //            document.Add(new Paragraph("\n"));
    //            document.Add(new Paragraph($"Category : {recipe.Category?.Name}", subTitleFont));
    //            document.Add(new Paragraph("\n"));
    //            document.Add(new Paragraph($"Posted At: {recipe.Creationdate.ToString("dd/MMMM/yyyy")}", subTitleFont));
    //            document.Add(new Paragraph("\n"));
    //            document.Add(new Paragraph($"Price: ${recipe.Price}", subTitleFont));
    //            document.Add(new Paragraph("\n"));

    //            // Description
    //            var bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 14);
    //            document.Add(new Paragraph("Description: ", bodyFont));
    //            document.Add(new Paragraph(recipe.Description + ".", bodyFont));
    //            document.Add(new Paragraph("\n"));

    //            // Ingredients
    //            document.Add(new Paragraph($"Ingredients Used In {recipe.Name}: ", bodyFont));
    //            document.Add(new Paragraph(recipe.Ingredients +".", bodyFont));
    //            document.Add(new Paragraph("\n"));

    //            // Instructions
    //            document.Add(new Paragraph($"Instructions To Make {recipe.Name}: ", bodyFont));
    //            document.Add(new Paragraph(recipe.Instructions + ".", bodyFont));

    //            //// Image
    //            //if (!string.IsNullOrEmpty(recipe.Imgpath))
    //            //{
    //            //    try
    //            //    {
    //            //        var image = iTextSharp.text.Image.GetInstance(recipe.Imgpath);
    //            //        image.ScaleToFit(250f, 250f);
    //            //        image.Alignment = Element.ALIGN_CENTER;
    //            //        document.Add(image);
    //            //    }
    //            //    catch (Exception ex)
    //            //    {
    //            //    }
    //            //}
    //            document.Close();
    //        }
    //        return path;
    //    }
    //}
}

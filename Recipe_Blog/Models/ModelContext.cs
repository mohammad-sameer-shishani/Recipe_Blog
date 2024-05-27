using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Recipe_Blog.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aboutu> Aboutus { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Contactu> Contactus { get; set; }

    public virtual DbSet<Homepage> Homepages { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Visa> Visas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("USER ID=C##PROJECT;PASSWORD=Test321;DATA SOURCE=localhost:1521/xe");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##PROJECT")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Aboutu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008710");

            entity.ToTable("ABOUTUS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.AboutCreator)
                .HasColumnType("CLOB")
                .HasColumnName("ABOUT_CREATOR");
            entity.Property(e => e.AboutusContent)
                .HasColumnType("CLOB")
                .HasColumnName("ABOUTUS_CONTENT");
            entity.Property(e => e.Ingpath)
                .HasColumnType("CLOB")
                .HasColumnName("INGPATH");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008652");

            entity.ToTable("CATEGORY");

            entity.HasIndex(e => e.Name, "SYS_C008653").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Imgpath)
                .HasColumnType("CLOB")
                .HasColumnName("IMGPATH");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008692");

            entity.ToTable("COMMENTS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Message)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("MESSAGE");
            entity.Property(e => e.RecipeId)
                .HasColumnType("NUMBER")
                .HasColumnName("RECIPE_ID");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Comments)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("RECIPE___FK");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("USER__ID_FK");
        });

        modelBuilder.Entity<Contactu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008708");

            entity.ToTable("CONTACTUS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Message)
                .HasColumnType("CLOB")
                .HasColumnName("MESSAGE");
            entity.Property(e => e.Subject)
                .HasColumnType("CLOB")
                .HasColumnName("SUBJECT");
            entity.Property(e => e.Useremail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USEREMAIL");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USERNAME");
        });

        modelBuilder.Entity<Homepage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008712");

            entity.ToTable("HOMEPAGE");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Copyright)
                .HasColumnType("CLOB")
                .HasColumnName("COPYRIGHT");
            entity.Property(e => e.FooterEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FOOTER_EMAIL");
            entity.Property(e => e.FooterName)
                .HasColumnType("CLOB")
                .HasColumnName("FOOTER_NAME");
            entity.Property(e => e.FooterPhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FOOTER_PHONE_NUMBER");
            entity.Property(e => e.HeroImg)
                .HasColumnType("CLOB")
                .HasColumnName("HERO_IMG");
            entity.Property(e => e.Logo)
                .HasColumnType("CLOB")
                .HasColumnName("LOGO");
            entity.Property(e => e.NavbarTitle)
                .HasColumnType("CLOB")
                .HasColumnName("NAVBAR_TITLE");
            entity.Property(e => e.SupportPhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SUPPORT_PHONE_NUMBER");
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008647");

            entity.ToTable("LOGIN");

            entity.HasIndex(e => e.Email, "SYS_C008648").IsUnique();

            entity.HasIndex(e => e.UserName, "SYS_C008649").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USER_NAME");

            entity.HasOne(d => d.User).WithMany(p => p.Logins)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("ROLE_ID_FK");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008659");

            entity.ToTable("RECIPE");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.CategoryId)
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORY_ID");
            entity.Property(e => e.Creationdate)
                .HasDefaultValueSql("SYSTIMESTAMP")
                .HasColumnType("TIMESTAMP(6) WITH TIME ZONE")
                .HasColumnName("CREATIONDATE");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Imgpath)
                .HasColumnType("CLOB")
                .HasColumnName("IMGPATH");
            entity.Property(e => e.Ingredients)
                .HasColumnType("CLOB")
                .HasColumnName("INGREDIENTS");
            entity.Property(e => e.Instructions)
                .HasColumnType("CLOB")
                .HasColumnName("INSTRUCTIONS");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Price)
                .HasColumnType("FLOAT")
                .HasColumnName("PRICE");
            entity.Property(e => e.RecipeStatusId)
                .HasColumnType("NUMBER")
                .HasColumnName("RECIPE_STATUS_ID");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Category).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("CATEGORY_FK");

            entity.HasOne(d => d.RecipeStatus).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.RecipeStatusId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("RECIPE_FK1");

            entity.HasOne(d => d.User).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("USERS_ID_FK");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008672");

            entity.ToTable("REQUEST");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.RecipeId)
                .HasColumnType("NUMBER")
                .HasColumnName("RECIPE_ID");
            entity.Property(e => e.Requestdate)
                .HasDefaultValueSql("SYSTIMESTAMP")
                .HasColumnType("TIMESTAMP(6) WITH TIME ZONE")
                .HasColumnName("REQUESTDATE");
            entity.Property(e => e.Tax)
                .HasColumnType("FLOAT")
                .HasColumnName("TAX");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Requests)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("RECIPE__FK");

            entity.HasOne(d => d.User).WithMany(p => p.Requests)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("USERS__FK");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("SYS_C008635");

            entity.ToTable("ROLE");

            entity.HasIndex(e => e.Rolename, "SYS_C008636").IsUnique();

            entity.Property(e => e.Roleid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Rolename)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ROLENAME");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Statusid).HasName("SYS_C008699");

            entity.ToTable("STATUS");

            entity.Property(e => e.Statusid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("STATUSID");
            entity.Property(e => e.Statusname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("STATUSNAME");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008655");

            entity.ToTable("TESTIMONIAL");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Content)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("CONTENT");
            entity.Property(e => e.Creationdate)
                .HasDefaultValueSql("SYSTIMESTAMP")
                .HasColumnType("TIMESTAMP(6) WITH TIME ZONE")
                .HasColumnName("CREATIONDATE");
            entity.Property(e => e.TestimonialStatusId)
                .HasColumnType("NUMBER")
                .HasColumnName("TESTIMONIAL_STATUS_ID");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.TestimonialStatus).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.TestimonialStatusId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("TESTIMONIAL_FK1");

            entity.HasOne(d => d.User).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("USER_ID_FK");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008639");

            entity.ToTable("USERS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Birthdate)
                .HasColumnType("DATE")
                .HasColumnName("BIRTHDATE");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FIRSTNAME");
            entity.Property(e => e.Imgpath)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("IMGPATH");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LASTNAME");
            entity.Property(e => e.RoleId)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLE_ID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("ROLE_FK");
        });

        modelBuilder.Entity<Visa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008679");

            entity.ToTable("VISA");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Amount)
                .HasColumnType("FLOAT")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.Cardnumber)
                .HasPrecision(16)
                .HasColumnName("CARDNUMBER");
            entity.Property(e => e.Cvc)
                .HasPrecision(3)
                .HasColumnName("CVC");
            entity.Property(e => e.Expdate)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("EXPDATE");
            entity.Property(e => e.Nameoncard)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAMEONCARD");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Visas)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("USER_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

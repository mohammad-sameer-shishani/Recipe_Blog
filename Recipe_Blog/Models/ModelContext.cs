using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Recipe_Blog.Models;

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

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

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

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008652");

            entity.ToTable("CATEGORY");

            entity.HasIndex(e => e.Name, "SYS_C008653").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
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

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008663");

            entity.ToTable("INGREDIENT");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.RecipeId)
                .HasColumnType("NUMBER")
                .HasColumnName("RECIPE_ID");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Ingredients)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("RECIPE_ID_FK");
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
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Price)
                .HasColumnType("FLOAT")
                .HasColumnName("PRICE");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Category).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("CATEGORY_FK");

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
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("USER_ID_FK");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008639");

            entity.ToTable("USERS");

            entity.HasIndex(e => e.Firstname, "SYS_C008640").IsUnique();

            entity.HasIndex(e => e.Lastname, "SYS_C008641").IsUnique();

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
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("GENDER");
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

            entity.HasIndex(e => e.Cardnumber, "SYS_C008680").IsUnique();

            entity.HasIndex(e => e.Cvc, "SYS_C008681").IsUnique();

            entity.HasIndex(e => e.Nameoncard, "SYS_C008682").IsUnique();

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
                .HasColumnType("DATE")
                .HasColumnName("EXPDATE");
            entity.Property(e => e.Nameoncard)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAMEONCARD");
            entity.Property(e => e.RequestId)
                .HasColumnType("NUMBER")
                .HasColumnName("REQUEST_ID");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Request).WithMany(p => p.Visas)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("REQUEST__FK");

            entity.HasOne(d => d.User).WithMany(p => p.Visas)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("USER_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<Recipe_Blog.Models.UserViewModel>? UserViewModel { get; set; }
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MVCManukauTech.ViewModels;

namespace MVCManukauTech.Models.DB
{
    public partial class XSpy4CoreContext : DbContext
    {
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Reviews> Reviews { get; set; }
        //custom viewmodels
        public virtual DbSet<CatalogViewModel> CatalogViewModel { get; set;}
        public virtual DbSet<OrderDetailsQueryForCart> OrderDetailsQueryForCart { get; set; }
        public virtual DbSet<GrandTotalViewModel> GrandTotalViewModel { get; set; }

        //20180312 JPC experiment - try running without this apparently empty constructor
        //  find that it is necessary.
        public XSpy4CoreContext(DbContextOptions<XSpy4CoreContext> options)
        : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasIndex(e => e.UserId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryId).ValueGeneratedNever();

                entity.Property(e => e.CategoryName).HasMaxLength(50);
            });

            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.LineNumber });

                entity.Property(e => e.ProductId).HasMaxLength(20);

                entity.Property(e => e.UnitCost).HasColumnType("money");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_OrderDetails_Products");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.CardOwner).HasMaxLength(50);

                entity.Property(e => e.CardType).HasMaxLength(20);

                entity.Property(e => e.CustomerName).HasMaxLength(100);

                entity.Property(e => e.DateOfOrder).HasColumnType("datetime");

                entity.Property(e => e.DateOfSession).HasColumnType("datetime");

                entity.Property(e => e.DateOfShipping).HasColumnType("datetime");

                entity.Property(e => e.DeliveryAddress).HasMaxLength(150);

                entity.Property(e => e.EmailAddress).HasMaxLength(255);

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.Notes).HasMaxLength(255);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.SessionId).HasMaxLength(25);

                entity.Property(e => e.Username).HasMaxLength(20);

                entity.HasOne(d => d.OrderStatus)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OrderStatusId)
                    .HasConstraintName("FK_Orders_OrderStatus");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.Property(e => e.OrderStatusId).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(255);
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.ProductId)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.DownloadFileName).HasMaxLength(20);

                entity.Property(e => e.ImageFileName).HasMaxLength(20);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Products_Categories");
            });

            modelBuilder.Entity<Reviews>(entity =>
            {
                entity.HasKey(e => e.ReviewId);

                entity.Property(e => e.ReviewId).ValueGeneratedNever();

                entity.Property(e => e.CustomerEmail).HasMaxLength(50);

                entity.Property(e => e.CustomerName).HasMaxLength(50);

                entity.Property(e => e.ProductId).HasMaxLength(20);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Reviews_Products");
            });
        }
    }
}

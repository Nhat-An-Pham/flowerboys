﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Models.Models
{
    public partial class TraibanhoaContext : DbContext
    {
        public TraibanhoaContext()
        {
        }

        public TraibanhoaContext(DbContextOptions<TraibanhoaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Basket> Baskets { get; set; }
        public virtual DbSet<BasketDetail> BasketDetails { get; set; }
        public virtual DbSet<BasketSubCate> BasketSubCates { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<RequestBasket> RequestBaskets { get; set; }
        public virtual DbSet<RequestBasketDetail> RequestBasketDetails { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<Type> Types { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=MSI\\MONKINAM;Uid=sa;Pwd=Monki123;Database=Traibanhoa;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Basket>(entity =>
            {
                entity.ToTable("Basket");

                entity.Property(e => e.BasketId)
                    .ValueGeneratedNever()
                    .HasColumnName("basketId");

                entity.Property(e => e.BasketPrice)
                    .HasColumnType("money")
                    .HasColumnName("basketPrice");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(100)
                    .HasColumnName("imageURL");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .HasColumnName("title");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedDate");

                entity.Property(e => e.View).HasColumnName("view");
            });

            modelBuilder.Entity<BasketDetail>(entity =>
            {
                entity.HasKey(e => new { e.BasketId, e.ProductId });

                entity.ToTable("BasketDetail");

                entity.Property(e => e.BasketId).HasColumnName("basketId");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Basket)
                    .WithMany(p => p.BasketDetails)
                    .HasForeignKey(d => d.BasketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BasketDetail_Basket");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.BasketDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BasketDetail_Product");
            });

            modelBuilder.Entity<BasketSubCate>(entity =>
            {
                entity.HasKey(e => new { e.BasketId, e.SubCateId });

                entity.ToTable("BasketSubCate");

                entity.Property(e => e.BasketId).HasColumnName("basketId");

                entity.Property(e => e.SubCateId).HasColumnName("subCateId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Basket)
                    .WithMany(p => p.BasketSubCates)
                    .HasForeignKey(d => d.BasketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BasketSubCate_Basket");

                entity.HasOne(d => d.SubCate)
                    .WithMany(p => p.BasketSubCates)
                    .HasForeignKey(d => d.SubCateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BasketSubCate_SubCategory");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("categoryId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .HasColumnName("name");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.CustomerId)
                    .ValueGeneratedNever()
                    .HasColumnName("customerId");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(100)
                    .HasColumnName("avatar");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.IsBlocked).HasColumnName("isBlocked");

                entity.Property(e => e.IsGoogle).HasColumnName("isGoogle");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Phonenumber)
                    .HasMaxLength(50)
                    .HasColumnName("phonenumber");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedDate");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.OrderId)
                    .ValueGeneratedNever()
                    .HasColumnName("orderId");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.IsRequest).HasColumnName("isRequest");

                entity.Property(e => e.OrderBy).HasColumnName("orderBy");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasColumnName("orderDate");

                entity.Property(e => e.OrderStatus).HasColumnName("orderStatus");

                entity.Property(e => e.PaymentMethod).HasColumnName("paymentMethod");

                entity.Property(e => e.Phonenumber)
                    .HasMaxLength(50)
                    .HasColumnName("phonenumber");

                entity.Property(e => e.ShippedAddress).HasColumnName("shippedAddress");

                entity.Property(e => e.ShippedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("shippedDate");

                entity.Property(e => e.TotalPrice)
                    .HasColumnType("money")
                    .HasColumnName("totalPrice");

                entity.HasOne(d => d.OrderByNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OrderBy)
                    .HasConstraintName("FK_Order_Customer");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.OrderDetailId)
                    .ValueGeneratedNever()
                    .HasColumnName("orderDetailId");

                entity.Property(e => e.BasketId).HasColumnName("basketId");

                entity.Property(e => e.OrderId).HasColumnName("orderId");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.RequestBasketId).HasColumnName("requestBasketId");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderDetail_Order");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_OrderDetail_Product");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.ProductId)
                    .ValueGeneratedNever()
                    .HasColumnName("productId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ForSelling).HasColumnName("forSelling");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Picture)
                    .HasMaxLength(100)
                    .HasColumnName("picture");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TypeId).HasColumnName("typeId");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedDate");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_Product_Type");
            });

            modelBuilder.Entity<RequestBasket>(entity =>
            {
                entity.ToTable("RequestBasket");

                entity.Property(e => e.RequestBasketId)
                    .ValueGeneratedNever()
                    .HasColumnName("requestBasketId");

                entity.Property(e => e.ConfirmBy).HasColumnName("confirmBy");

                entity.Property(e => e.CreateBy).HasColumnName("createBy");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.EstimatePrice)
                    .HasColumnType("money")
                    .HasColumnName("estimatePrice");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(100)
                    .HasColumnName("imageURL");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .HasColumnName("phone");

                entity.Property(e => e.RequestStatus).HasColumnName("requestStatus");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .HasColumnName("title");

                entity.HasOne(d => d.ConfirmByNavigation)
                    .WithMany(p => p.RequestBaskets)
                    .HasForeignKey(d => d.ConfirmBy)
                    .HasConstraintName("FK_RequestBasket_User");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.RequestBaskets)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_RequestBasket_Customer");
            });

            modelBuilder.Entity<RequestBasketDetail>(entity =>
            {
                entity.HasKey(e => new { e.RequestBasketId, e.ProductId });

                entity.ToTable("RequestBasketDetail");

                entity.Property(e => e.RequestBasketId).HasColumnName("requestBasketId");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.RequestBasketDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestBasketDetail_Product");

                entity.HasOne(d => d.RequestBasket)
                    .WithMany(p => p.RequestBasketDetails)
                    .HasForeignKey(d => d.RequestBasketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestBasketDetail_RequestBasket");
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.ToTable("SubCategory");

                entity.Property(e => e.SubCategoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("subCategoryId");

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .HasColumnName("name");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.SubCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_SubCategory_Category");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.Property(e => e.TransactionId)
                    .ValueGeneratedNever()
                    .HasColumnName("transactionId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.OrderId).HasColumnName("orderId");

                entity.Property(e => e.TotalAmount)
                    .HasColumnType("money")
                    .HasColumnName("totalAmount");

                entity.Property(e => e.TransactionStatus).HasColumnName("transactionStatus");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_Transaction_Order");
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.ToTable("Type");

                entity.Property(e => e.TypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("typeId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedDate");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("userId");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(100)
                    .HasColumnName("avatar");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.IsBlocked).HasColumnName("isBlocked");

                entity.Property(e => e.IsGoogle).HasColumnName("isGoogle");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Phonenumber)
                    .HasMaxLength(50)
                    .HasColumnName("phonenumber");

                entity.Property(e => e.Role).HasColumnName("role");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updatedDate");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

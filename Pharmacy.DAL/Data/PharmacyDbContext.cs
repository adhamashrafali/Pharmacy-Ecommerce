using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pharmacy.DAL.Entities;

namespace Pharmacy.DAL.Data;

public partial class PharmacyDbContext : DbContext
{
    public PharmacyDbContext(DbContextOptions<PharmacyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<CouponUsage> CouponUsages { get; set; }

    public virtual DbSet<CustomerOrderCount> CustomerOrderCounts { get; set; }

    public virtual DbSet<DeliveryZone> DeliveryZones { get; set; }

    public virtual DbSet<DriverOrder> DriverOrders { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }

    public virtual DbSet<PrescriptionImage> PrescriptionImages { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductDiscount> ProductDiscounts { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductRequest> ProductRequests { get; set; }

    public virtual DbSet<ProductRequestImage> ProductRequestImages { get; set; }

    public virtual DbSet<WishlistItem> WishlistItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Addresse__3214EC077BA392CB");

            entity.HasIndex(e => e.CustomerId, "IX_Addresses_Customer");

            entity.HasIndex(e => e.CustomerId, "UX_Addresses_Default")
                .IsUnique()
                .HasFilter("([IsDefault]=(1))");

            entity.Property(e => e.Apartment).HasMaxLength(30);
            entity.Property(e => e.BuildingNumber).HasMaxLength(30);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Directions).HasMaxLength(500);
            entity.Property(e => e.District).HasMaxLength(100);
            entity.Property(e => e.Floor).HasMaxLength(20);
            entity.Property(e => e.Governorate).HasMaxLength(100);
            entity.Property(e => e.Label).HasMaxLength(80);
            entity.Property(e => e.Landmark).HasMaxLength(200);
            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Street).HasMaxLength(200);

            entity.HasOne(d => d.Zone).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.ZoneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Addresses__ZoneI__46E78A0C");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AuditLog__3214EC07746B275A");

            entity.HasIndex(e => new { e.EntityType, e.EntityId, e.OccurredAt }, "IX_AuditLogs_Entity").IsDescending(false, false, true);

            entity.Property(e => e.Action).HasMaxLength(80);
            entity.Property(e => e.EntityId).HasMaxLength(80);
            entity.Property(e => e.EntityType).HasMaxLength(80);
            entity.Property(e => e.OccurredAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Brands__3214EC0704E16850");

            entity.HasIndex(e => e.Name, "UQ__Brands__737584F645F3FC79").IsUnique();

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Carts__A4AE64D80B491236");

            entity.Property(e => e.CustomerId).ValueGeneratedNever();
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Version)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => new { e.CustomerId, e.ProductId }).HasName("PK__CartItem__6FEEA8B4AA374AAF");

            entity.HasOne(d => d.Customer).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItems__Custo__6EF57B66");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItems__Produ__6FE99F9F");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC075183AE74");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.NameAr).HasMaxLength(200);
            entity.Property(e => e.NameEn).HasMaxLength(200);

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__Categorie__Paren__5441852A");
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Coupons__3214EC0746DA657E");

            entity.HasIndex(e => e.NormalizedCode, "UQ__Coupons__A3C8B172B1851E0A")
    .IsUnique()
    .HasFilter(null);

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.DiscountType)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.DiscountValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EndsAt).HasPrecision(3);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MaximumDiscount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinimumSubtotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NormalizedCode)
                .HasMaxLength(50)
                .HasComputedColumnSql("(upper(ltrim(rtrim([Code]))))", true);
            entity.Property(e => e.StartsAt).HasPrecision(3);
        });

        modelBuilder.Entity<CouponUsage>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__CouponUs__C3905BCFE3CA8181");

            entity.HasIndex(e => new { e.CouponId, e.State, e.OrderId }, "IX_CouponUsages_Limits");

            entity.Property(e => e.OrderId).ValueGeneratedNever();
            entity.Property(e => e.AppliedAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ReservedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.State)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Order).WithMany(p => p.CouponUsages)
                .HasPrincipalKey(p => new { p.Id, p.CouponId })
                .HasForeignKey(d => new { d.OrderId, d.CouponId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CouponUsages__3C34F16F");
        });

        modelBuilder.Entity<CustomerOrderCount>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("CustomerOrderCounts");
        });

        modelBuilder.Entity<DeliveryZone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Delivery__3214EC077885A421");

            entity.HasIndex(e => e.NameEn, "UQ__Delivery__EE1C70AFC2C6B28F").IsUnique();

            entity.HasIndex(e => e.NameAr, "UQ__Delivery__EE1CD26CEE50558A").IsUnique();

            entity.Property(e => e.Fee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.NameAr).HasMaxLength(150);
            entity.Property(e => e.NameEn).HasMaxLength(150);
        });

        modelBuilder.Entity<DriverOrder>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("DriverOrders");

            entity.Property(e => e.CodAmount).HasColumnType("decimal(21, 2)");
            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.OrderId).ValueGeneratedOnAdd();
            entity.Property(e => e.ShippingApartment).HasMaxLength(30);
            entity.Property(e => e.ShippingBuildingNumber).HasMaxLength(30);
            entity.Property(e => e.ShippingCity).HasMaxLength(100);
            entity.Property(e => e.ShippingDirections).HasMaxLength(500);
            entity.Property(e => e.ShippingDistrict).HasMaxLength(100);
            entity.Property(e => e.ShippingFloor).HasMaxLength(20);
            entity.Property(e => e.ShippingGovernorate).HasMaxLength(100);
            entity.Property(e => e.ShippingLandmark).HasMaxLength(200);
            entity.Property(e => e.ShippingName).HasMaxLength(200);
            entity.Property(e => e.ShippingPhone).HasMaxLength(16);
            entity.Property(e => e.ShippingStreet).HasMaxLength(200);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manufact__3214EC07227AA4F3");

            entity.HasIndex(e => e.Name, "UQ__Manufact__737584F6FDF7F693").IsUnique();

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC07FCB916BE");

            entity.ToTable(tb => tb.HasTrigger("TR_Orders_Locked"));

            entity.HasIndex(e => new { e.CustomerId, e.CreatedAt }, "IX_Orders_Customer_Date").IsDescending(false, true);

            entity.HasIndex(e => new { e.DriverId, e.Status }, "IX_Orders_Driver_Status").HasFilter("([DriverId] IS NOT NULL)");

            entity.HasIndex(e => new { e.Status, e.CreatedAt }, "IX_Orders_Status_Date");

            entity.HasIndex(e => new { e.CustomerId, e.CheckoutKey }, "UQ__Orders__494B2CA83216AC65").IsUnique();

            entity.HasIndex(e => new { e.Id, e.CouponId }, "UQ__Orders__8190431D4DE975A4").IsUnique();

            entity.Property(e => e.CancellationReason).HasMaxLength(1000);
            entity.Property(e => e.ConfirmedAt).HasPrecision(3);
            entity.Property(e => e.CouponCodeSnapshot).HasMaxLength(50);
            entity.Property(e => e.CouponDiscount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CouponId).IsRequired();
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DeliveryFee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DirectDiscount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.OrderType)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.ShippingApartment).HasMaxLength(30);
            entity.Property(e => e.ShippingBuildingNumber).HasMaxLength(30);
            entity.Property(e => e.ShippingCity).HasMaxLength(100);
            entity.Property(e => e.ShippingDirections).HasMaxLength(500);
            entity.Property(e => e.ShippingDistrict).HasMaxLength(100);
            entity.Property(e => e.ShippingFloor).HasMaxLength(20);
            entity.Property(e => e.ShippingGovernorate).HasMaxLength(100);
            entity.Property(e => e.ShippingLandmark).HasMaxLength(200);
            entity.Property(e => e.ShippingName).HasMaxLength(200);
            entity.Property(e => e.ShippingPhone).HasMaxLength(16);
            entity.Property(e => e.ShippingStreet).HasMaxLength(200);
            entity.Property(e => e.ShippingZoneAr).HasMaxLength(150);
            entity.Property(e => e.ShippingZoneEn).HasMaxLength(150);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("PendingReview");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Total)
                .HasComputedColumnSql("((([Subtotal]-[DirectDiscount])-[CouponDiscount])+[DeliveryFee])", true)
                .HasColumnType("decimal(21, 2)");
            entity.Property(e => e.UnavailablePolicy)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Version)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.ZoneReview)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("NotNeeded");

            entity.HasOne(d => d.Coupon).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CouponId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__CouponId__17036CC0");

            entity.HasOne(d => d.Zone).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ZoneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__ZoneId__123EB7A3");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderIte__3214EC07E6871793");

            entity.ToTable(tb => tb.HasTrigger("TR_OrderItems_Locked"));

            entity.HasIndex(e => new { e.ProductId, e.OrderId }, "IX_OrderItems_Product");

            entity.HasIndex(e => new { e.OrderId, e.ProductId }, "UQ__OrderIte__08D097A24285F7BB").IsUnique();

            entity.Property(e => e.CouponDiscount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DirectDiscount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LineTotal)
                .HasComputedColumnSql("(([UnitPrice]*[Quantity]-[DirectDiscount])-[CouponDiscount])", true)
                .HasColumnType("decimal(31, 2)");
            entity.Property(e => e.NameAr).HasMaxLength(200);
            entity.Property(e => e.NameEn).HasMaxLength(200);
            entity.Property(e => e.ReplacementReason).HasMaxLength(1000);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Order__2180FB33");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Produ__22751F6C");
        });

      

        modelBuilder.Entity<OrderStatusHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderSta__3214EC072CEA999B");

            entity.ToTable("OrderStatusHistory");

            entity.HasIndex(e => new { e.OrderId, e.Id }, "IX_OrderStatusHistory_Order");

            entity.HasIndex(e => e.OrderId, "UX_OrderStatusHistory_Delivered")
                .IsUnique()
                .HasFilter("([NewStatus]='Delivered')");

            entity.Property(e => e.ChangedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.NewStatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PreviousStatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Reason).HasMaxLength(1000);

            entity.HasOne(d => d.Order).WithOne(p => p.OrderStatusHistory)
                .HasForeignKey<OrderStatusHistory>(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderStat__Order__339FAB6E");
        });

        

        modelBuilder.Entity<PrescriptionImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Prescrip__3214EC07252606B3");

            entity.HasIndex(e => e.OrderId, "IX_PrescriptionImages_Order");

            entity.HasIndex(e => e.ObjectKey, "UQ__Prescrip__E7B6593ECE583510").IsUnique();

            entity.Property(e => e.ObjectKey).HasMaxLength(512);
            entity.Property(e => e.UploadedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Order).WithMany(p => p.PrescriptionImages)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Prescript__Order__2B0A656D");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC0708E4F0AB");

            entity.HasIndex(e => new { e.ProductType, e.IsActive, e.IsAvailable }, "IX_Products_Browse");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.NameAr).HasMaxLength(200);
            entity.Property(e => e.NameEn).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Version)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK__Products__BrandI__5AEE82B9");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Products)
                .HasForeignKey(d => d.ManufacturerId)
                .HasConstraintName("FK__Products__Manufa__59FA5E80");

            entity.HasMany(d => d.Categories).WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ProductCa__Categ__6477ECF3"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ProductCa__Produ__6383C8BA"),
                    j =>
                    {
                        j.HasKey("ProductId", "CategoryId").HasName("PK__ProductC__159C556D4C6B25AD");
                        j.ToTable("ProductCategories");
                        j.HasIndex(new[] { "CategoryId", "ProductId" }, "IX_ProductCategories_Category");
                    });
        });

        modelBuilder.Entity<ProductDiscount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductD__3214EC072BB4670D");

            entity.HasIndex(e => new { e.ProductId, e.IsActive, e.StartsAt, e.EndsAt }, "IX_ProductDiscounts_Active");

            entity.Property(e => e.DiscountType)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.DiscountValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EndsAt).HasPrecision(3);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MinQuantity).HasDefaultValue(1);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.StartsAt).HasPrecision(3);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductDiscounts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductDi__Produ__778AC167");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductI__3214EC07D725F0F7");

            entity.HasIndex(e => e.ProductId, "UX_ProductImages_Primary")
                .IsUnique()
                .HasFilter("([IsPrimary]=(1))");

            entity.Property(e => e.ObjectKey).HasMaxLength(512);

            entity.HasOne(d => d.Product).WithOne(p => p.ProductImage)
                .HasForeignKey<ProductImage>(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductIm__Produ__6754599E");
        });

        modelBuilder.Entity<ProductRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductR__3214EC078D74DF05");

            entity.HasIndex(e => new { e.Status, e.CreatedAt }, "IX_ProductRequests_Queue");

            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF__ProductRe__Creat__40F9A68C");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ReviewNotes).HasMaxLength(1000);
            entity.Property(e => e.ReviewedAt).HasPrecision(3);
            entity.Property(e => e.Status)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasDefaultValue("Pending", "DF__ProductRe__Statu__3F115E1A");

            entity.HasOne(d => d.Order).WithMany(p => p.ProductRequests)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_ProductRequests_Orders");
        });

        modelBuilder.Entity<ProductRequestImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductR__3214EC07125284DE");

            entity.HasIndex(e => e.ObjectKey, "UQ__ProductR__E7B6593E7477DF6F").IsUnique();

            entity.Property(e => e.ObjectKey).HasMaxLength(512);
            entity.Property(e => e.UploadedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Request).WithMany(p => p.ProductRequestImages)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductRe__Reque__44CA3770");
        });


        modelBuilder.Entity<WishlistItem>(entity =>
        {
            entity.HasKey(e => new { e.CustomerId, e.ProductId }).HasName("PK__Wishlist__6FEEA8B4F4229F23");

            entity.Property(e => e.AddedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Product).WithMany(p => p.WishlistItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WishlistI__Produ__73BA3083");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace JeanCraftLibrary.Entity;

public partial class JeanCraftContext : DbContext
{
    public JeanCraftContext()
    {
    }

    public JeanCraftContext(DbContextOptions<JeanCraftContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Component> Components { get; set; }

    public virtual DbSet<ComponentType> ComponentTypes { get; set; }

    public virtual DbSet<DesignOne> DesignOnes { get; set; }

    public virtual DbSet<DesignThree> DesignThrees { get; set; }

    public virtual DbSet<DesignTwo> DesignTwos { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public virtual DbSet<ProductInventory> ProductInventories { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());
    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
        var strConn = config["ConnectionStrings:DefaultConnection"];
        return strConn;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_Users");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Image).HasMaxLength(1000);
            entity.Property(e => e.Phonenumber).HasMaxLength(20);
            entity.Property(e => e.UserName).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Accounts_Roles");
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Address");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Detail)
                .HasMaxLength(1000)
                .IsFixedLength();
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Address_Accounts");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Carts_Products");

            entity.HasMany(d => d.Users).WithMany(p => p.Carts)
                .UsingEntity<Dictionary<string, object>>(
                    "ShoppingCart",
                    r => r.HasOne<Account>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ShoppingCart_Accounts"),
                    l => l.HasOne<CartItem>().WithMany()
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ShoppingCart_CartItems"),
                    j =>
                    {
                        j.HasKey("CartId", "UserId");
                        j.ToTable("ShoppingCart");
                        j.IndexerProperty<Guid>("CartId").HasColumnName("CartID");
                        j.IndexerProperty<Guid>("UserId").HasColumnName("UserID");
                    });
        });

        modelBuilder.Entity<Component>(entity =>
        {
            entity.Property(e => e.ComponentId)
                .ValueGeneratedNever()
                .HasColumnName("ComponentID");
            entity.Property(e => e.Description).HasMaxLength(30);
            entity.Property(e => e.Image)
                .HasMaxLength(1000)
                .IsFixedLength();

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Components)
                .HasForeignKey(d => d.Type)
                .HasConstraintName("FK_Components_ComponentTypes");
        });

        modelBuilder.Entity<ComponentType>(entity =>
        {
            entity.HasKey(e => e.TypeId);

            entity.Property(e => e.TypeId)
                .ValueGeneratedNever()
                .HasColumnName("TypeID");
            entity.Property(e => e.Description).HasMaxLength(30);
        });

        modelBuilder.Entity<DesignOne>(entity =>
        {
            entity.ToTable("DesignOne");

            entity.Property(e => e.DesignOneId)
                .ValueGeneratedNever()
                .HasColumnName("DesignOneID");

            entity.HasOne(d => d.BackPocketNavigation).WithMany(p => p.DesignOneBackPocketNavigations)
                .HasForeignKey(d => d.BackPocket)
                .HasConstraintName("FK_DesignOne_Components");

            entity.HasOne(d => d.CuffsNavigation).WithMany(p => p.DesignOneCuffsNavigations)
                .HasForeignKey(d => d.Cuffs)
                .HasConstraintName("FK_DesignOne_Components1");

            entity.HasOne(d => d.FitNavigation).WithMany(p => p.DesignOneFitNavigations)
                .HasForeignKey(d => d.Fit)
                .HasConstraintName("FK_DesignOne_Components2");

            entity.HasOne(d => d.FlyNavigation).WithMany(p => p.DesignOneFlyNavigations)
                .HasForeignKey(d => d.Fly)
                .HasConstraintName("FK_DesignOne_Components3");

            entity.HasOne(d => d.FrontPocketNavigation).WithMany(p => p.DesignOneFrontPocketNavigations)
                .HasForeignKey(d => d.FrontPocket)
                .HasConstraintName("FK_DesignOne_Components5");

            entity.HasOne(d => d.LengthNavigation).WithMany(p => p.DesignOneLengthNavigations)
                .HasForeignKey(d => d.Length)
                .HasConstraintName("FK_DesignOne_Components6");
        });

        modelBuilder.Entity<DesignThree>(entity =>
        {
            entity.ToTable("DesignThree");

            entity.Property(e => e.DesignThreeId)
                .ValueGeneratedNever()
                .HasColumnName("DesignThreeID");
            entity.Property(e => e.Characters).HasMaxLength(10);

            entity.HasOne(d => d.BranchBackPatchNavigation).WithMany(p => p.DesignThreeBranchBackPatchNavigations)
                .HasForeignKey(d => d.BranchBackPatch)
                .HasConstraintName("FK_DesignThree_Components");

            entity.HasOne(d => d.ButtonAndRivetNavigation).WithMany(p => p.DesignThreeButtonAndRivetNavigations)
                .HasForeignKey(d => d.ButtonAndRivet)
                .HasConstraintName("FK_DesignThree_Components1");

            entity.HasOne(d => d.EmbroideryColorNavigation).WithMany(p => p.DesignThreeEmbroideryColorNavigations)
                .HasForeignKey(d => d.EmbroideryColor)
                .HasConstraintName("FK_DesignThree_Components2");

            entity.HasOne(d => d.EmbroideryFontNavigation).WithMany(p => p.DesignThreeEmbroideryFontNavigations)
                .HasForeignKey(d => d.EmbroideryFont)
                .HasConstraintName("FK_DesignThree_Components3");

            entity.HasOne(d => d.MonoGramNavigation).WithMany(p => p.DesignThreeMonoGramNavigations)
                .HasForeignKey(d => d.MonoGram)
                .HasConstraintName("FK_DesignThree_Components4");

            entity.HasOne(d => d.StitchingThreadColorNavigation).WithMany(p => p.DesignThreeStitchingThreadColorNavigations)
                .HasForeignKey(d => d.StitchingThreadColor)
                .HasConstraintName("FK_DesignThree_Components5");
        });

        modelBuilder.Entity<DesignTwo>(entity =>
        {
            entity.ToTable("DesignTwo");

            entity.Property(e => e.DesignTwoId)
                .ValueGeneratedNever()
                .HasColumnName("DesignTwoID");

            entity.HasOne(d => d.FabricColorNavigation).WithMany(p => p.DesignTwoFabricColorNavigations)
                .HasForeignKey(d => d.FabricColor)
                .HasConstraintName("FK_DesignTwo_Components1");

            entity.HasOne(d => d.FinishingNavigation).WithMany(p => p.DesignTwoFinishingNavigations)
                .HasForeignKey(d => d.Finishing)
                .HasConstraintName("FK_DesignTwo_Components");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(1000);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TotalCost).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Address).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK_Orders_Address");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Orders_Accounts");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId });

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Orders");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Products");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Method).HasMaxLength(50);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_Payments_Orders");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("ProductID");
            entity.Property(e => e.DesignOneId).HasColumnName("DesignOneID");
            entity.Property(e => e.DesignThreeId).HasColumnName("DesignThreeID");
            entity.Property(e => e.DesignTwoId).HasColumnName("DesignTwoID");
            entity.Property(e => e.Image).HasMaxLength(1000);

            entity.HasOne(d => d.DesignOne).WithMany(p => p.Products)
                .HasForeignKey(d => d.DesignOneId)
                .HasConstraintName("FK_Products_DesignOne");

            entity.HasOne(d => d.DesignThree).WithMany(p => p.Products)
                .HasForeignKey(d => d.DesignThreeId)
                .HasConstraintName("FK_Products_DesignThree");

            entity.HasOne(d => d.DesignTwo).WithMany(p => p.Products)
                .HasForeignKey(d => d.DesignTwoId)
                .HasConstraintName("FK_Products_DesignTwo");
        });

        modelBuilder.Entity<ProductInventory>(entity =>
        {
            entity.ToTable("ProductInventory");

            entity.Property(e => e.ProductInventoryId).ValueGeneratedNever();

            entity.HasOne(d => d.ProductInventoryNavigation).WithOne(p => p.ProductInventory)
                .HasForeignKey<ProductInventory>(d => d.ProductInventoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductInventory_Products");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

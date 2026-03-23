using Arily.Catalog;
using Arily.Collection;
using Arily.Crm;
using Arily.Finance;
using Arily.Inventory;
using Arily.Sales;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Arily.EntityFrameworkCore;

public static class ArilyDbContextModelBuilderExtensions
{
    public static void ConfigureArily(this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.ConfigureCatalog();
        builder.ConfigureCrm();
        builder.ConfigureCollection();
        builder.ConfigureInventory();
        builder.ConfigureFinance();
        builder.ConfigureSales();
    }

    // ──────────────────────────────────────────────
    // CATALOG
    // ──────────────────────────────────────────────
    private static void ConfigureCatalog(this ModelBuilder builder)
    {
        builder.Entity<ProductCategory>(b =>
        {
            b.ToTable(ArilyConsts.DbTablePrefix + "ProductCategories", ArilyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Code).IsRequired().HasMaxLength(50);
            b.Property(x => x.Name).IsRequired().HasMaxLength(200);
            b.Property(x => x.Description).HasMaxLength(500);
            b.HasIndex(x => new { x.TenantId, x.Code }).IsUnique();
        });

        builder.Entity<UnitOfMeasure>(b =>
        {
            b.ToTable(ArilyConsts.DbTablePrefix + "UnitOfMeasures", ArilyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Code).IsRequired().HasMaxLength(20);
            b.Property(x => x.Name).IsRequired().HasMaxLength(100);
            b.HasIndex(x => new { x.TenantId, x.Code }).IsUnique();
        });

        builder.Entity<ProductGrade>(b =>
        {
            b.ToTable(ArilyConsts.DbTablePrefix + "ProductGrades", ArilyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Code).IsRequired().HasMaxLength(20);
            b.Property(x => x.Name).IsRequired().HasMaxLength(100);
            b.HasIndex(x => new { x.TenantId, x.ProductId, x.Code }).IsUnique();
        });

        builder.Entity<Product>(b =>
        {
            b.ToTable(ArilyConsts.DbTablePrefix + "Products", ArilyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Code).IsRequired().HasMaxLength(50);
            b.Property(x => x.Name).IsRequired().HasMaxLength(200);
            b.Property(x => x.DefaultLossRate).HasColumnType("numeric(8,4)");
            b.Property(x => x.Note).HasMaxLength(1000);
            b.HasIndex(x => new { x.TenantId, x.Code }).IsUnique();
            b.HasIndex(x => new { x.TenantId, x.Status });
        });
    }

    // ──────────────────────────────────────────────
    // CRM
    // ──────────────────────────────────────────────
    private static void ConfigureCrm(this ModelBuilder builder)
    {
        builder.Entity<Farmer>(b =>
        {
            b.ToTable(ArilyConsts.DbTablePrefix + "Farmers", ArilyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Code).IsRequired().HasMaxLength(50);
            b.Property(x => x.FullName).IsRequired().HasMaxLength(200);
            b.Property(x => x.NickName).HasMaxLength(100);
            b.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(20);
            b.Property(x => x.ProvinceCode).HasMaxLength(20);
            b.Property(x => x.DistrictCode).HasMaxLength(20);
            b.Property(x => x.WardCode).HasMaxLength(20);
            b.Property(x => x.Address).HasMaxLength(500);
            b.Property(x => x.Note).HasMaxLength(1000);
            b.HasIndex(x => new { x.TenantId, x.Code }).IsUnique();
            b.HasIndex(x => new { x.TenantId, x.PhoneNumber });
            b.HasIndex(x => new { x.TenantId, x.Status });
        });

        builder.Entity<FarmerGarden>(b =>
        {
            b.ToTable(ArilyConsts.DbTablePrefix + "FarmerGardens", ArilyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.GardenName).IsRequired().HasMaxLength(200);
            b.Property(x => x.AreaHectare).HasColumnType("numeric(10,4)");
            b.Property(x => x.EstimatedYieldKg).HasColumnType("numeric(18,3)");
            b.Property(x => x.Address).HasMaxLength(500);
            b.Property(x => x.Note).HasMaxLength(1000);
            b.HasIndex(x => new { x.TenantId, x.FarmerId });
        });

        builder.Entity<Customer>(b =>
        {
            b.ToTable(ArilyConsts.DbTablePrefix + "Customers", ArilyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Code).IsRequired().HasMaxLength(50);
            b.Property(x => x.FullName).IsRequired().HasMaxLength(200);
            b.Property(x => x.PhoneNumber).HasMaxLength(20);
            b.Property(x => x.Email).HasMaxLength(200);
            b.Property(x => x.CustomerType).HasMaxLength(50);
            b.Property(x => x.ProvinceCode).HasMaxLength(20);
            b.Property(x => x.DistrictCode).HasMaxLength(20);
            b.Property(x => x.Address).HasMaxLength(500);
            b.Property(x => x.Note).HasMaxLength(1000);
            b.HasIndex(x => new { x.TenantId, x.Code }).IsUnique();
            b.HasIndex(x => new { x.TenantId, x.Status });
        });
    }

    // ──────────────────────────────────────────────
    // COLLECTION
    // ──────────────────────────────────────────────
    private static void ConfigureCollection(this ModelBuilder builder)
    {
        builder.Entity<CollectionSession>(b =>
        {
            b.ToTable(ArilyConsts.DbTablePrefix + "CollectionSessions", ArilyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Code).IsRequired().HasMaxLength(50);
            b.Property(x => x.Name).IsRequired().HasMaxLength(200);
            b.Property(x => x.RegionProvinceCode).HasMaxLength(20);
            b.Property(x => x.RegionDistrictCode).HasMaxLength(20);
            b.Property(x => x.Note).HasMaxLength(1000);
            b.HasIndex(x => new { x.TenantId, x.Code }).IsUnique();
            b.HasIndex(x => new { x.TenantId, x.Status });
            b.HasIndex(x => new { x.TenantId, x.SessionDate });
        });

        builder.Entity<PurchaseOrder>(b =>
        {
            b.ToTable(ArilyConsts.DbTablePrefix + "PurchaseOrders", ArilyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.OrderNo).IsRequired().HasMaxLength(50);
            b.Property(x => x.ExpectedQuantityKg).HasColumnType("numeric(18,3)");
            b.Property(x => x.UnitPrice).HasColumnType("numeric(18,2)");
            b.Property(x => x.GrossAmount).HasColumnType("numeric(18,2)");
            b.Property(x => x.AdvanceAmount).HasColumnType("numeric(18,2)");
            b.Property(x => x.LossAmount).HasColumnType("numeric(18,2)");
            b.Property(x => x.NetPayableAmount).HasColumnType("numeric(18,2)");
            b.Property(x => x.Note).HasMaxLength(1000);
            b.HasIndex(x => new { x.TenantId, x.OrderNo }).IsUnique();
            b.HasIndex(x => new { x.TenantId, x.FarmerId });
            b.HasIndex(x => new { x.TenantId, x.SessionId });
            b.HasIndex(x => new { x.TenantId, x.Status });
            b.HasMany(x => x.Details).WithOne().HasForeignKey(x => x.PurchaseOrderId).IsRequired();
        });

        builder.Entity<PurchaseOrderDetail>(b =>
        {
            b.ToTable(ArilyConsts.DbTablePrefix + "PurchaseOrderDetails", ArilyConsts.DbSchema);
            b.HasKey(x => x.Id);
            b.Property(x => x.GradeName).HasMaxLength(100);
            b.Property(x => x.QuantityKg).HasColumnType("numeric(18,3)");
            b.Property(x => x.UnitPrice).HasColumnType("numeric(18,2)");
            b.Property(x => x.Amount).HasColumnType("numeric(18,2)");
            b.Property(x => x.Note).HasMaxLength(500);
        });

        builder.Entity<WeighingTicket>(b =>
        {
            b.ToTable(ArilyConsts.DbTablePrefix + "WeighingTickets", ArilyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.TicketNo).IsRequired().HasMaxLength(50);
            b.Property(x => x.GrossWeightKg).HasColumnType("numeric(18,3)");
            b.Property(x => x.TareWeightKg).HasColumnType("numeric(18,3)");
            b.Property(x => x.NetWeightKg).HasColumnType("numeric(18,3)");
            b.Property(x => x.Note).HasMaxLength(500);
            b.HasIndex(x => new { x.TenantId, x.PurchaseOrderId });
        });

        builder.Entity<PurchaseAdvance>(b =>
        {
            b.ToTable(ArilyConsts.DbTablePrefix + "PurchaseAdvances", ArilyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Amount).HasColumnType("numeric(18,2)");
            b.Property(x => x.PaymentMethod).IsRequired().HasMaxLength(50);
            b.Property(x => x.Note).HasMaxLength(500);
            b.HasIndex(x => new { x.TenantId, x.FarmerId });
            b.HasIndex(x => new { x.TenantId, x.PurchaseOrderId });
        });
    }

    // ──────────────────────────────────────────────
    // INVENTORY
    // ──────────────────────────────────────────────
    private static void ConfigureInventory(this ModelBuilder builder)
    {
        builder.Entity<Warehouse>(b =>
        {
            b.ToTable(ArilyConsts.DbTablePrefix + "Warehouses", ArilyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Code).IsRequired().HasMaxLength(50);
            b.Property(x => x.Name).IsRequired().HasMaxLength(200);
            b.Property(x => x.Address).HasMaxLength(500);
            b.Property(x => x.ProvinceCode).HasMaxLength(20);
            b.Property(x => x.Note).HasMaxLength(1000);
            b.HasIndex(x => new { x.TenantId, x.Code }).IsUnique();
        });

        builder.Entity<Lot>(b =>
        {
            b.ToTable(ArilyConsts.DbTablePrefix + "Lots", ArilyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.LotCode).IsRequired().HasMaxLength(50);
            b.Property(x => x.ReceivedQuantityKg).HasColumnType("numeric(18,3)");
            b.Property(x => x.CurrentQuantityKg).HasColumnType("numeric(18,3)");
            b.Property(x => x.Note).HasMaxLength(1000);
            b.HasIndex(x => new { x.TenantId, x.LotCode }).IsUnique();
            b.HasIndex(x => new { x.TenantId, x.FarmerId });
            b.HasIndex(x => new { x.TenantId, x.Status });
        });

        builder.Entity<InventoryLot>(b =>
        {
            b.ToTable(ArilyConsts.DbTablePrefix + "InventoryLots", ArilyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.OnHandQuantityKg).HasColumnType("numeric(18,3)");
            b.Property(x => x.ReservedQuantityKg).HasColumnType("numeric(18,3)");
            b.Ignore(x => x.AvailableQuantityKg); // computed, not stored
            b.HasIndex(x => new { x.TenantId, x.WarehouseId, x.LotId }).IsUnique();
            b.HasIndex(x => new { x.TenantId, x.ProductId });
        });
    }

    // ──────────────────────────────────────────────
    // FINANCE
    // ──────────────────────────────────────────────
    private static void ConfigureFinance(this ModelBuilder builder)
    {
        builder.Entity<LossAdjustmentOrder>(b =>
        {
            b.ToTable(ArilyConsts.DbTablePrefix + "LossAdjustmentOrders", ArilyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.AdjustmentNo).IsRequired().HasMaxLength(50);
            b.Property(x => x.LossQuantityKg).HasColumnType("numeric(18,3)");
            b.Property(x => x.LossAmount).HasColumnType("numeric(18,2)");
            b.Property(x => x.ReasonCode).HasMaxLength(50);
            b.Property(x => x.Note).HasMaxLength(1000);
            b.HasIndex(x => new { x.TenantId, x.FarmerId });
            b.HasIndex(x => new { x.TenantId, x.LotId });
        });

        builder.Entity<FarmerDebtLedger>(b =>
        {
            b.ToTable(ArilyConsts.DbTablePrefix + "FarmerDebtLedgers", ArilyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Amount).HasColumnType("numeric(18,2)");
            b.Property(x => x.RunningBalance).HasColumnType("numeric(18,2)");
            b.Property(x => x.ReferenceNo).HasMaxLength(100);
            b.Property(x => x.Note).HasMaxLength(1000);
            b.HasIndex(x => new { x.TenantId, x.FarmerId });
            b.HasIndex(x => new { x.TenantId, x.TransactionDate });
        });
    }

    // ──────────────────────────────────────────────
    // SALES
    // ──────────────────────────────────────────────
    private static void ConfigureSales(this ModelBuilder builder)
    {
        builder.Entity<SalesOrder>(b =>
        {
            b.ToTable(ArilyConsts.DbTablePrefix + "SalesOrders", ArilyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.OrderNo).IsRequired().HasMaxLength(50);
            b.Property(x => x.TotalAmount).HasColumnType("numeric(18,2)");
            b.Property(x => x.DiscountAmount).HasColumnType("numeric(18,2)");
            b.Property(x => x.NetAmount).HasColumnType("numeric(18,2)");
            b.Property(x => x.PaidAmount).HasColumnType("numeric(18,2)");
            b.Property(x => x.DebtAmount).HasColumnType("numeric(18,2)");
            b.Property(x => x.Note).HasMaxLength(1000);
            b.HasIndex(x => new { x.TenantId, x.OrderNo }).IsUnique();
            b.HasIndex(x => new { x.TenantId, x.CustomerId });
            b.HasIndex(x => new { x.TenantId, x.Status });
        });

        builder.Entity<CustomerDebtLedger>(b =>
        {
            b.ToTable(ArilyConsts.DbTablePrefix + "CustomerDebtLedgers", ArilyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Amount).HasColumnType("numeric(18,2)");
            b.Property(x => x.RunningBalance).HasColumnType("numeric(18,2)");
            b.Property(x => x.ReferenceNo).HasMaxLength(100);
            b.Property(x => x.Note).HasMaxLength(1000);
            b.HasIndex(x => new { x.TenantId, x.CustomerId });
            b.HasIndex(x => new { x.TenantId, x.TransactionDate });
        });
    }
}

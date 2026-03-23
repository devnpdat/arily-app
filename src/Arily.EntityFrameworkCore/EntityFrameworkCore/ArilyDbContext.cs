using Arily.Catalog;
using Arily.Collection;
using Arily.Crm;
using Arily.Finance;
using Arily.Inventory;
using Arily.Sales;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Arily.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class ArilyDbContext :
    AbpDbContext<ArilyDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    #region ABP Identity & Tenant

    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    #region Catalog

    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductGrade> ProductGrades { get; set; }

    #endregion

    #region CRM

    public DbSet<Farmer> Farmers { get; set; }
    public DbSet<FarmerGarden> FarmerGardens { get; set; }
    public DbSet<Customer> Customers { get; set; }

    #endregion

    #region Collection

    public DbSet<CollectionSession> CollectionSessions { get; set; }
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
    public DbSet<WeighingTicket> WeighingTickets { get; set; }
    public DbSet<PurchaseAdvance> PurchaseAdvances { get; set; }

    #endregion

    #region Inventory

    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Lot> Lots { get; set; }
    public DbSet<InventoryLot> InventoryLots { get; set; }

    #endregion

    #region Finance

    public DbSet<LossAdjustmentOrder> LossAdjustmentOrders { get; set; }
    public DbSet<FarmerDebtLedger> FarmerDebtLedgers { get; set; }

    #endregion

    #region Sales

    public DbSet<SalesOrder> SalesOrders { get; set; }
    public DbSet<CustomerDebtLedger> CustomerDebtLedgers { get; set; }

    #endregion

    public ArilyDbContext(DbContextOptions<ArilyDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        builder.ConfigureArily();
    }
}

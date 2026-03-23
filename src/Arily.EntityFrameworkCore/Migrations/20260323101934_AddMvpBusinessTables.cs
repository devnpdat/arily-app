using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arily.Migrations
{
    /// <inheritdoc />
    public partial class AddMvpBusinessTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppCollectionSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SessionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ProductCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    RegionProvinceCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    RegionDistrictCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    BuyerUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCollectionSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppCustomerDebtLedgers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    SalesOrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    LedgerType = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    RunningBalance = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ReferenceNo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCustomerDebtLedgers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppCustomers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CustomerType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ProvinceCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DistrictCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCustomers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppFarmerDebtLedgers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    FarmerId = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    LedgerType = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    RunningBalance = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ReferenceNo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppFarmerDebtLedgers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppFarmerGardens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    FarmerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    GardenName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AreaHectare = table.Column<decimal>(type: "numeric(10,4)", nullable: true),
                    EstimatedYieldKg = table.Column<decimal>(type: "numeric(18,3)", nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppFarmerGardens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppFarmers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NickName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ProvinceCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DistrictCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    WardCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    ReputationScore = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppFarmers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppInventoryLots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    LotId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    GradeId = table.Column<Guid>(type: "uuid", nullable: true),
                    OnHandQuantityKg = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    ReservedQuantityKg = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppInventoryLots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppLossAdjustmentOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    LotId = table.Column<Guid>(type: "uuid", nullable: false),
                    FarmerId = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    AdjustmentNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LossQuantityKg = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    LossAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ReasonCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ApprovedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLossAdjustmentOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppLots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    LotCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    FarmerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceivedQuantityKg = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    CurrentQuantityKg = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppProductCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProductCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppProductGrades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProductGrades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProductCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    UnitOfMeasureId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DefaultLossRate = table.Column<decimal>(type: "numeric(8,4)", nullable: false),
                    Note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPurchaseAdvances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    FarmerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AdvancedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPurchaseAdvances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    FarmerId = table.Column<Guid>(type: "uuid", nullable: false),
                    GardenId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ExpectedQuantityKg = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AdvanceAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    LossAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    NetPayableAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPurchaseOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSalesOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrderNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DebtAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSalesOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUnitOfMeasures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUnitOfMeasures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppWarehouses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ProvinceCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppWarehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppWeighingTickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    GrossWeightKg = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    TareWeightKg = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    NetWeightKg = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    WeighedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PrintedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppWeighingTickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPurchaseOrderDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    GradeId = table.Column<Guid>(type: "uuid", nullable: true),
                    GradeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    QuantityKg = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPurchaseOrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPurchaseOrderDetails_AppPurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "AppPurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppCollectionSessions_TenantId_Code",
                table: "AppCollectionSessions",
                columns: new[] { "TenantId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppCollectionSessions_TenantId_SessionDate",
                table: "AppCollectionSessions",
                columns: new[] { "TenantId", "SessionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_AppCollectionSessions_TenantId_Status",
                table: "AppCollectionSessions",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_AppCustomerDebtLedgers_TenantId_CustomerId",
                table: "AppCustomerDebtLedgers",
                columns: new[] { "TenantId", "CustomerId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppCustomerDebtLedgers_TenantId_TransactionDate",
                table: "AppCustomerDebtLedgers",
                columns: new[] { "TenantId", "TransactionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_AppCustomers_TenantId_Code",
                table: "AppCustomers",
                columns: new[] { "TenantId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppCustomers_TenantId_Status",
                table: "AppCustomers",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_AppFarmerDebtLedgers_TenantId_FarmerId",
                table: "AppFarmerDebtLedgers",
                columns: new[] { "TenantId", "FarmerId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppFarmerDebtLedgers_TenantId_TransactionDate",
                table: "AppFarmerDebtLedgers",
                columns: new[] { "TenantId", "TransactionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_AppFarmerGardens_TenantId_FarmerId",
                table: "AppFarmerGardens",
                columns: new[] { "TenantId", "FarmerId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppFarmers_TenantId_Code",
                table: "AppFarmers",
                columns: new[] { "TenantId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppFarmers_TenantId_PhoneNumber",
                table: "AppFarmers",
                columns: new[] { "TenantId", "PhoneNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_AppFarmers_TenantId_Status",
                table: "AppFarmers",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_AppInventoryLots_TenantId_ProductId",
                table: "AppInventoryLots",
                columns: new[] { "TenantId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppInventoryLots_TenantId_WarehouseId_LotId",
                table: "AppInventoryLots",
                columns: new[] { "TenantId", "WarehouseId", "LotId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppLossAdjustmentOrders_TenantId_FarmerId",
                table: "AppLossAdjustmentOrders",
                columns: new[] { "TenantId", "FarmerId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppLossAdjustmentOrders_TenantId_LotId",
                table: "AppLossAdjustmentOrders",
                columns: new[] { "TenantId", "LotId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppLots_TenantId_FarmerId",
                table: "AppLots",
                columns: new[] { "TenantId", "FarmerId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppLots_TenantId_LotCode",
                table: "AppLots",
                columns: new[] { "TenantId", "LotCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppLots_TenantId_Status",
                table: "AppLots",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_AppProductCategories_TenantId_Code",
                table: "AppProductCategories",
                columns: new[] { "TenantId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppProductGrades_TenantId_ProductId_Code",
                table: "AppProductGrades",
                columns: new[] { "TenantId", "ProductId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppProducts_TenantId_Code",
                table: "AppProducts",
                columns: new[] { "TenantId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppProducts_TenantId_Status",
                table: "AppProducts",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_AppPurchaseAdvances_TenantId_FarmerId",
                table: "AppPurchaseAdvances",
                columns: new[] { "TenantId", "FarmerId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppPurchaseAdvances_TenantId_PurchaseOrderId",
                table: "AppPurchaseAdvances",
                columns: new[] { "TenantId", "PurchaseOrderId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppPurchaseOrderDetails_PurchaseOrderId",
                table: "AppPurchaseOrderDetails",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPurchaseOrders_TenantId_FarmerId",
                table: "AppPurchaseOrders",
                columns: new[] { "TenantId", "FarmerId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppPurchaseOrders_TenantId_OrderNo",
                table: "AppPurchaseOrders",
                columns: new[] { "TenantId", "OrderNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppPurchaseOrders_TenantId_SessionId",
                table: "AppPurchaseOrders",
                columns: new[] { "TenantId", "SessionId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppPurchaseOrders_TenantId_Status",
                table: "AppPurchaseOrders",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_AppSalesOrders_TenantId_CustomerId",
                table: "AppSalesOrders",
                columns: new[] { "TenantId", "CustomerId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppSalesOrders_TenantId_OrderNo",
                table: "AppSalesOrders",
                columns: new[] { "TenantId", "OrderNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppSalesOrders_TenantId_Status",
                table: "AppSalesOrders",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_AppUnitOfMeasures_TenantId_Code",
                table: "AppUnitOfMeasures",
                columns: new[] { "TenantId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppWarehouses_TenantId_Code",
                table: "AppWarehouses",
                columns: new[] { "TenantId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppWeighingTickets_TenantId_PurchaseOrderId",
                table: "AppWeighingTickets",
                columns: new[] { "TenantId", "PurchaseOrderId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppCollectionSessions");

            migrationBuilder.DropTable(
                name: "AppCustomerDebtLedgers");

            migrationBuilder.DropTable(
                name: "AppCustomers");

            migrationBuilder.DropTable(
                name: "AppFarmerDebtLedgers");

            migrationBuilder.DropTable(
                name: "AppFarmerGardens");

            migrationBuilder.DropTable(
                name: "AppFarmers");

            migrationBuilder.DropTable(
                name: "AppInventoryLots");

            migrationBuilder.DropTable(
                name: "AppLossAdjustmentOrders");

            migrationBuilder.DropTable(
                name: "AppLots");

            migrationBuilder.DropTable(
                name: "AppProductCategories");

            migrationBuilder.DropTable(
                name: "AppProductGrades");

            migrationBuilder.DropTable(
                name: "AppProducts");

            migrationBuilder.DropTable(
                name: "AppPurchaseAdvances");

            migrationBuilder.DropTable(
                name: "AppPurchaseOrderDetails");

            migrationBuilder.DropTable(
                name: "AppSalesOrders");

            migrationBuilder.DropTable(
                name: "AppUnitOfMeasures");

            migrationBuilder.DropTable(
                name: "AppWarehouses");

            migrationBuilder.DropTable(
                name: "AppWeighingTickets");

            migrationBuilder.DropTable(
                name: "AppPurchaseOrders");
        }
    }
}

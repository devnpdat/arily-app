using System;
using System.Threading.Tasks;
using Arily.Catalog;
using Arily.Crm;
using Arily.Enums;
using Arily.Inventory;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Arily.DbMigrator;

/// <summary>
/// Seed dữ liệu mẫu cho môi trường dev/demo.
/// Chỉ chạy nếu chưa có dữ liệu (idempotent).
/// </summary>
public class ArilyDemoDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<UnitOfMeasure, Guid> _uomRepository;
    private readonly IRepository<ProductCategory, Guid> _categoryRepository;
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly IRepository<Warehouse, Guid> _warehouseRepository;
    private readonly IRepository<Farmer, Guid> _farmerRepository;
    private readonly IRepository<Customer, Guid> _customerRepository;

    public ArilyDemoDataSeedContributor(
        IRepository<UnitOfMeasure, Guid> uomRepository,
        IRepository<ProductCategory, Guid> categoryRepository,
        IRepository<Product, Guid> productRepository,
        IRepository<Warehouse, Guid> warehouseRepository,
        IRepository<Farmer, Guid> farmerRepository,
        IRepository<Customer, Guid> customerRepository)
    {
        _uomRepository = uomRepository;
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
        _farmerRepository = farmerRepository;
        _customerRepository = customerRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await SeedUnitOfMeasuresAsync(context.TenantId);
        await SeedProductCategoriesAsync(context.TenantId);
        await SeedProductsAsync(context.TenantId);
        await SeedWarehousesAsync(context.TenantId);
        await SeedFarmersAsync(context.TenantId);
        await SeedCustomersAsync(context.TenantId);
    }

    private async Task SeedUnitOfMeasuresAsync(Guid? tenantId)
    {
        if (await _uomRepository.GetCountAsync() > 0)
            return;

        var uoms = new[]
        {
            new UnitOfMeasure(Guid.NewGuid(), tenantId, "KG",  "Kilogram"),
            new UnitOfMeasure(Guid.NewGuid(), tenantId, "TAN", "Tấn"),
            new UnitOfMeasure(Guid.NewGuid(), tenantId, "THUNG", "Thùng"),
            new UnitOfMeasure(Guid.NewGuid(), tenantId, "BAO", "Bao"),
        };

        await _uomRepository.InsertManyAsync(uoms, autoSave: true);
    }

    private async Task SeedProductCategoriesAsync(Guid? tenantId)
    {
        if (await _categoryRepository.GetCountAsync() > 0)
            return;

        var categories = new[]
        {
            new ProductCategory(Guid.NewGuid(), tenantId, "SAU-RIENG",  "Sầu riêng"),
            new ProductCategory(Guid.NewGuid(), tenantId, "MIT",        "Mít"),
            new ProductCategory(Guid.NewGuid(), tenantId, "XOAI",       "Xoài"),
            new ProductCategory(Guid.NewGuid(), tenantId, "THANH-LONG", "Thanh long"),
            new ProductCategory(Guid.NewGuid(), tenantId, "CHUOI",      "Chuối"),
        };

        await _categoryRepository.InsertManyAsync(categories, autoSave: true);
    }

    private async Task SeedProductsAsync(Guid? tenantId)
    {
        if (await _productRepository.GetCountAsync() > 0)
            return;

        var kgUom = await _uomRepository.FindAsync(x => x.Code == "KG");
        if (kgUom == null) return;

        var catSauRieng  = await _categoryRepository.FindAsync(x => x.Code == "SAU-RIENG");
        var catMit       = await _categoryRepository.FindAsync(x => x.Code == "MIT");
        var catXoai      = await _categoryRepository.FindAsync(x => x.Code == "XOAI");
        var catThanhLong = await _categoryRepository.FindAsync(x => x.Code == "THANH-LONG");

        if (catSauRieng == null || catMit == null || catXoai == null || catThanhLong == null)
            return;

        var products = new[]
        {
            new Product(Guid.NewGuid(), tenantId, catSauRieng.Id,  kgUom.Id, "SR-RI6",   "Sầu riêng Ri6")
                { DefaultLossRate = 0.05m },
            new Product(Guid.NewGuid(), tenantId, catSauRieng.Id,  kgUom.Id, "SR-DONA",  "Sầu riêng Dona")
                { DefaultLossRate = 0.05m },
            new Product(Guid.NewGuid(), tenantId, catMit.Id,       kgUom.Id, "MIT-NGHIA", "Mít Thái nghĩa")
                { DefaultLossRate = 0.08m },
            new Product(Guid.NewGuid(), tenantId, catXoai.Id,      kgUom.Id, "XOAI-CAT", "Xoài cát Hoà Lộc")
                { DefaultLossRate = 0.04m },
            new Product(Guid.NewGuid(), tenantId, catThanhLong.Id, kgUom.Id, "TL-DO",    "Thanh long ruột đỏ")
                { DefaultLossRate = 0.06m },
        };

        await _productRepository.InsertManyAsync(products, autoSave: true);
    }

    private async Task SeedWarehousesAsync(Guid? tenantId)
    {
        if (await _warehouseRepository.GetCountAsync() > 0)
            return;

        var warehouses = new[]
        {
            new Warehouse(Guid.NewGuid(), tenantId, "KHO-TG",  "Kho Tiền Giang")
                { Address = "123 Quốc lộ 1A, Mỹ Tho, Tiền Giang", ProvinceCode = "82" },
            new Warehouse(Guid.NewGuid(), tenantId, "KHO-BDI", "Kho Bình Điền")
                { Address = "Chợ đầu mối nông sản Bình Điền, Quận 8, TP.HCM", ProvinceCode = "79" },
            new Warehouse(Guid.NewGuid(), tenantId, "KHO-CT",  "Kho Cần Thơ")
                { Address = "Khu công nghiệp Trà Nóc, Bình Thủy, Cần Thơ", ProvinceCode = "92" },
        };

        await _warehouseRepository.InsertManyAsync(warehouses, autoSave: true);
    }

    private async Task SeedFarmersAsync(Guid? tenantId)
    {
        if (await _farmerRepository.GetCountAsync() > 0)
            return;

        var farmers = new[]
        {
            new Farmer(Guid.NewGuid(), tenantId, "ND001", "Nguyễn Văn An",   "0901234501")
                { NickName = "Chú An", ProvinceCode = "82", DistrictCode = "82001",
                  Address = "Xã Đạo Thạnh, TP Mỹ Tho, Tiền Giang", ReputationScore = 85 },
            new Farmer(Guid.NewGuid(), tenantId, "ND002", "Trần Thị Bình",   "0901234502")
                { NickName = "Cô Bình", ProvinceCode = "82", DistrictCode = "82003",
                  Address = "Xã Bình Phú, Cai Lậy, Tiền Giang", ReputationScore = 90 },
            new Farmer(Guid.NewGuid(), tenantId, "ND003", "Lê Văn Cường",    "0901234503")
                { NickName = "Anh Cường", ProvinceCode = "82", DistrictCode = "82005",
                  Address = "Xã Long Tiên, Cai Lậy, Tiền Giang", ReputationScore = 70 },
            new Farmer(Guid.NewGuid(), tenantId, "ND004", "Phạm Thị Dung",   "0901234504")
                { NickName = "Chị Dung", ProvinceCode = "89", DistrictCode = "89001",
                  Address = "Xã Tân Lập, Mộc Hoá, Long An", ReputationScore = 80 },
            new Farmer(Guid.NewGuid(), tenantId, "ND005", "Huỳnh Văn Em",    "0901234505")
                { NickName = "Chú Em", ProvinceCode = "92", DistrictCode = "92001",
                  Address = "Xã Nhơn Nghĩa, Phong Điền, Cần Thơ", ReputationScore = 75 },
        };

        await _farmerRepository.InsertManyAsync(farmers, autoSave: true);
    }

    private async Task SeedCustomersAsync(Guid? tenantId)
    {
        if (await _customerRepository.GetCountAsync() > 0)
            return;

        var customers = new[]
        {
            new Customer(Guid.NewGuid(), tenantId, "KH001", "Vựa Minh Tâm")
                { PhoneNumber = "0281234501", CustomerType = "Vựa",
                  Address = "Chợ đầu mối Thủ Đức, TP.HCM", ProvinceCode = "79" },
            new Customer(Guid.NewGuid(), tenantId, "KH002", "Công ty TNHH Bình Điền Xanh")
                { PhoneNumber = "0281234502", CustomerType = "Doanh nghiệp",
                  Email = "binhdienly@email.com", Address = "Bình Điền, Quận 8, TP.HCM", ProvinceCode = "79" },
            new Customer(Guid.NewGuid(), tenantId, "KH003", "Siêu thị Big C Cần Thơ")
                { PhoneNumber = "0291234503", CustomerType = "Siêu thị",
                  Address = "Đường 30/4, Ninh Kiều, Cần Thơ", ProvinceCode = "92" },
            new Customer(Guid.NewGuid(), tenantId, "KH004", "Vựa Thanh Hương")
                { PhoneNumber = "0731234504", CustomerType = "Vựa",
                  Address = "Chợ Mỹ Tho, Tiền Giang", ProvinceCode = "82", Status = CommonStatus.Active },
        };

        await _customerRepository.InsertManyAsync(customers, autoSave: true);
    }
}

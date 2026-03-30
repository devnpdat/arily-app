using System;
using System.Threading.Tasks;
using Arily.Crm;
using Arily.Enums;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Arily;

/// <summary>
/// Seed dữ liệu CRM cho test: 2 nông dân + 2 khách hàng với ID cố định.
/// </summary>
public class ArilyCrmTestDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    public static readonly Guid FarmerAnId    = Guid.Parse("10000000-0000-0000-0000-000000000001");
    public static readonly Guid FarmerBinhId  = Guid.Parse("10000000-0000-0000-0000-000000000002");
    public static readonly Guid CustomerVuaId = Guid.Parse("20000000-0000-0000-0000-000000000001");
    public static readonly Guid CustomerDnId  = Guid.Parse("20000000-0000-0000-0000-000000000002");

    private readonly IRepository<Farmer, Guid> _farmerRepository;
    private readonly IRepository<Customer, Guid> _customerRepository;

    public ArilyCrmTestDataSeedContributor(
        IRepository<Farmer, Guid> farmerRepository,
        IRepository<Customer, Guid> customerRepository)
    {
        _farmerRepository = farmerRepository;
        _customerRepository = customerRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await SeedFarmersAsync(context.TenantId);
        await SeedCustomersAsync(context.TenantId);
    }

    private async Task SeedFarmersAsync(Guid? tenantId)
    {
        await _farmerRepository.InsertAsync(
            new Farmer(FarmerAnId, tenantId, "ND001", "Nguyễn Văn An", "0901000001")
            {
                NickName = "Chú An",
                ProvinceCode = "82",
                Status = FarmerStatus.Active
            }, autoSave: true);

        await _farmerRepository.InsertAsync(
            new Farmer(FarmerBinhId, tenantId, "ND002", "Trần Thị Bình", "0901000002")
            {
                NickName = "Cô Bình",
                ProvinceCode = "92",
                Status = FarmerStatus.Inactive
            }, autoSave: true);
    }

    private async Task SeedCustomersAsync(Guid? tenantId)
    {
        await _customerRepository.InsertAsync(
            new Customer(CustomerVuaId, tenantId, "KH001", "Vựa Minh Tâm")
            {
                PhoneNumber = "0281000001",
                CustomerType = "Vựa",
                ProvinceCode = "79",
                Status = CommonStatus.Active
            }, autoSave: true);

        await _customerRepository.InsertAsync(
            new Customer(CustomerDnId, tenantId, "KH002", "Công ty Bình Điền Xanh")
            {
                PhoneNumber = "0281000002",
                CustomerType = "Doanh nghiệp",
                ProvinceCode = "79",
                Status = CommonStatus.Active
            }, autoSave: true);
    }
}

using System;
using System.Threading.Tasks;
using Arily.Crm;
using Arily.Crm.Customers;
using Arily.Enums;
using Shouldly;
using Xunit;

namespace Arily.EntityFrameworkCore.Applications;

public class CustomerAppServiceTests : ArilyEntityFrameworkCoreTestBase
{
    private readonly ICustomerAppService _customerAppService;

    public CustomerAppServiceTests()
    {
        _customerAppService = GetRequiredService<ICustomerAppService>();
    }

    // ── GET ────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAsync_Should_Return_Customer()
    {
        var customer = await _customerAppService.GetAsync(ArilyCrmTestDataSeedContributor.CustomerVuaId);

        customer.ShouldNotBeNull();
        customer.Code.ShouldBe("KH001");
        customer.FullName.ShouldBe("Vựa Minh Tâm");
        customer.PhoneNumber.ShouldBe("0281000001");
        customer.CustomerType.ShouldBe("Vựa");
    }

    // ── GET LIST ───────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetListAsync_No_Filter_Should_Return_All()
    {
        var result = await _customerAppService.GetListAsync(new GetCustomerListInput());

        result.TotalCount.ShouldBeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task GetListAsync_Filter_By_Name_Should_Return_Matching()
    {
        var result = await _customerAppService.GetListAsync(new GetCustomerListInput
        {
            Filter = "Vựa Minh Tâm"
        });

        result.TotalCount.ShouldBe(1);
        result.Items[0].Code.ShouldBe("KH001");
    }

    [Fact]
    public async Task GetListAsync_Filter_By_Phone_Should_Return_Matching()
    {
        var result = await _customerAppService.GetListAsync(new GetCustomerListInput
        {
            Filter = "0281000002"
        });

        result.TotalCount.ShouldBe(1);
        result.Items[0].Code.ShouldBe("KH002");
    }

    [Fact]
    public async Task GetListAsync_Filter_By_CustomerType_Should_Return_Matching()
    {
        var result = await _customerAppService.GetListAsync(new GetCustomerListInput
        {
            CustomerType = "Vựa"
        });

        result.TotalCount.ShouldBe(1);
        result.Items[0].Code.ShouldBe("KH001");
    }

    [Fact]
    public async Task GetListAsync_Filter_By_ProvinceCode_Should_Return_All_In_Province()
    {
        var result = await _customerAppService.GetListAsync(new GetCustomerListInput
        {
            ProvinceCode = "79"
        });

        result.TotalCount.ShouldBe(2);
    }

    [Fact]
    public async Task GetListAsync_Filter_By_Status_Should_Return_Matching()
    {
        var result = await _customerAppService.GetListAsync(new GetCustomerListInput
        {
            Status = CommonStatus.Active
        });

        result.Items.ShouldAllBe(c => c.Status == CommonStatus.Active);
    }

    // ── CREATE ─────────────────────────────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_Should_Create_New_Customer()
    {
        var dto = new CreateUpdateCustomerDto
        {
            Code         = "KH099",
            FullName     = "Siêu thị Test",
            PhoneNumber  = "0299000099",
            CustomerType = "Siêu thị",
            ProvinceCode = "92",
            Status       = CommonStatus.Active
        };

        var created = await _customerAppService.CreateAsync(dto);

        created.ShouldNotBeNull();
        created.Id.ShouldNotBe(Guid.Empty);
        created.Code.ShouldBe("KH099");
        created.CustomerType.ShouldBe("Siêu thị");
    }

    // ── UPDATE ─────────────────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateAsync_Should_Update_Customer()
    {
        var dto = new CreateUpdateCustomerDto
        {
            Code         = "KH001-UPD",
            FullName     = "Vựa Minh Tâm (mới)",
            PhoneNumber  = "0281000001",
            CustomerType = "Vựa",
            ProvinceCode = "79",
            Status       = CommonStatus.Active
        };

        var updated = await _customerAppService.UpdateAsync(ArilyCrmTestDataSeedContributor.CustomerVuaId, dto);

        updated.Code.ShouldBe("KH001-UPD");
        updated.FullName.ShouldBe("Vựa Minh Tâm (mới)");
    }

    // ── DELETE ─────────────────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteAsync_Should_Remove_Customer()
    {
        await _customerAppService.DeleteAsync(ArilyCrmTestDataSeedContributor.CustomerDnId);

        await Should.ThrowAsync<Volo.Abp.Domain.Entities.EntityNotFoundException>(async () =>
        {
            await _customerAppService.GetAsync(ArilyCrmTestDataSeedContributor.CustomerDnId);
        });
    }
}

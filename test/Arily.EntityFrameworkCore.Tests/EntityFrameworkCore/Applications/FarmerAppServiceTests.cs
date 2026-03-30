using System;
using System.Threading.Tasks;
using Arily.Crm;
using Arily.Crm.Farmers;
using Arily.Enums;
using Shouldly;
using Xunit;

namespace Arily.EntityFrameworkCore.Applications;

public class FarmerAppServiceTests : ArilyEntityFrameworkCoreTestBase
{
    private readonly IFarmerAppService _farmerAppService;

    public FarmerAppServiceTests()
    {
        _farmerAppService = GetRequiredService<IFarmerAppService>();
    }

    // ── GET ────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAsync_Should_Return_Farmer()
    {
        var farmer = await _farmerAppService.GetAsync(ArilyCrmTestDataSeedContributor.FarmerAnId);

        farmer.ShouldNotBeNull();
        farmer.Code.ShouldBe("ND001");
        farmer.FullName.ShouldBe("Nguyễn Văn An");
        farmer.PhoneNumber.ShouldBe("0901000001");
        farmer.ProvinceCode.ShouldBe("82");
    }

    // ── GET LIST ───────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetListAsync_No_Filter_Should_Return_All()
    {
        var result = await _farmerAppService.GetListAsync(new GetFarmerListInput());

        result.TotalCount.ShouldBeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task GetListAsync_Filter_By_Name_Should_Return_Matching()
    {
        var result = await _farmerAppService.GetListAsync(new GetFarmerListInput
        {
            Filter = "Nguyễn Văn An"
        });

        result.TotalCount.ShouldBe(1);
        result.Items[0].Code.ShouldBe("ND001");
    }

    [Fact]
    public async Task GetListAsync_Filter_By_NickName_Should_Return_Matching()
    {
        var result = await _farmerAppService.GetListAsync(new GetFarmerListInput
        {
            Filter = "Chú An"
        });

        result.TotalCount.ShouldBe(1);
        result.Items[0].Code.ShouldBe("ND001");
    }

    [Fact]
    public async Task GetListAsync_Filter_By_Phone_Should_Return_Matching()
    {
        var result = await _farmerAppService.GetListAsync(new GetFarmerListInput
        {
            Filter = "0901000002"
        });

        result.TotalCount.ShouldBe(1);
        result.Items[0].Code.ShouldBe("ND002");
    }

    [Fact]
    public async Task GetListAsync_Filter_By_Status_Active_Should_Return_Active_Only()
    {
        var result = await _farmerAppService.GetListAsync(new GetFarmerListInput
        {
            Status = FarmerStatus.Active
        });

        result.Items.ShouldAllBe(f => f.Status == FarmerStatus.Active);
    }

    [Fact]
    public async Task GetListAsync_Filter_By_ProvinceCode_Should_Return_Matching()
    {
        var result = await _farmerAppService.GetListAsync(new GetFarmerListInput
        {
            ProvinceCode = "82"
        });

        result.TotalCount.ShouldBe(1);
        result.Items[0].Code.ShouldBe("ND001");
    }

    // ── CREATE ─────────────────────────────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_Should_Create_New_Farmer()
    {
        var dto = new CreateUpdateFarmerDto
        {
            Code        = "ND099",
            FullName    = "Lê Văn Test",
            PhoneNumber = "0909000099",
            NickName    = "Anh Test",
            ProvinceCode = "79",
            Status      = FarmerStatus.Active
        };

        var created = await _farmerAppService.CreateAsync(dto);

        created.ShouldNotBeNull();
        created.Id.ShouldNotBe(Guid.Empty);
        created.Code.ShouldBe("ND099");
        created.FullName.ShouldBe("Lê Văn Test");
    }

    // ── UPDATE ─────────────────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateAsync_Should_Update_Farmer()
    {
        var dto = new CreateUpdateFarmerDto
        {
            Code        = "ND001-UPD",
            FullName    = "Nguyễn Văn An (đã cập nhật)",
            PhoneNumber = "0901000001",
            NickName    = "Chú An",
            ProvinceCode = "82",
            Status      = FarmerStatus.Active
        };

        var updated = await _farmerAppService.UpdateAsync(ArilyCrmTestDataSeedContributor.FarmerAnId, dto);

        updated.Code.ShouldBe("ND001-UPD");
        updated.FullName.ShouldBe("Nguyễn Văn An (đã cập nhật)");
    }

    // ── DELETE ─────────────────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteAsync_Should_Remove_Farmer()
    {
        await _farmerAppService.DeleteAsync(ArilyCrmTestDataSeedContributor.FarmerBinhId);

        await Should.ThrowAsync<Volo.Abp.Domain.Entities.EntityNotFoundException>(async () =>
        {
            await _farmerAppService.GetAsync(ArilyCrmTestDataSeedContributor.FarmerBinhId);
        });
    }
}

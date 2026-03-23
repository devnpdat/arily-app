using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Crm.Farmers;

public class GetFarmerListInput : PagedAndSortedResultRequestDto
{
    /// <summary>Tìm theo tên, biệt danh, số điện thoại, mã</summary>
    public string? Filter { get; set; }

    public FarmerStatus? Status { get; set; }

    public string? ProvinceCode { get; set; }
}

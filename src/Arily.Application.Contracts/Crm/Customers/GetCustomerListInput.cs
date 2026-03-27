using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Crm.Customers;

public class GetCustomerListInput : PagedAndSortedResultRequestDto
{
    /// <summary>Tìm theo tên, số điện thoại, email, mã</summary>
    public string? Filter { get; set; }

    public CommonStatus? Status { get; set; }

    public string? CustomerType { get; set; }

    public string? ProvinceCode { get; set; }
}

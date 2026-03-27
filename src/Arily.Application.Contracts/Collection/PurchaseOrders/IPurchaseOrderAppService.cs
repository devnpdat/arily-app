using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Arily.Collection.PurchaseOrders;

public interface IPurchaseOrderAppService : IApplicationService
{
    Task<PurchaseOrderDto> GetAsync(Guid id);
    Task<PagedResultDto<PurchaseOrderDto>> GetListAsync(GetPurchaseOrderListInput input);
    Task<PurchaseOrderDto> CreateAsync(CreatePurchaseOrderDto input);
    Task DeleteAsync(Guid id);
}

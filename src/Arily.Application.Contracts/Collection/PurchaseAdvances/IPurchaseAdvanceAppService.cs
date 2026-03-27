using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Arily.Collection.PurchaseAdvances;

public interface IPurchaseAdvanceAppService : IApplicationService
{
    Task<PurchaseAdvanceDto> GetAsync(Guid id);
    Task<ListResultDto<PurchaseAdvanceDto>> GetListAsync(Guid purchaseOrderId);
    Task<PurchaseAdvanceDto> CreateAsync(CreatePurchaseAdvanceDto input);
    Task DeleteAsync(Guid id);
}

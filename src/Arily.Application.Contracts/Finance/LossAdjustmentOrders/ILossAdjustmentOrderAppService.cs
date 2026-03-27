using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Arily.Finance.LossAdjustmentOrders;

public interface ILossAdjustmentOrderAppService : IApplicationService
{
    Task<LossAdjustmentOrderDto> GetAsync(Guid id);
    Task<PagedResultDto<LossAdjustmentOrderDto>> GetListAsync(GetLossAdjustmentOrderListInput input);
    Task<LossAdjustmentOrderDto> CreateAsync(CreateLossAdjustmentOrderDto input);
    Task DeleteAsync(Guid id);
}

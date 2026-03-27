using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Finance.LossAdjustmentOrders;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Finance;

[RemoteService(IsEnabled = false)]
public class LossAdjustmentOrderAppService : ArilyAppService, ILossAdjustmentOrderAppService
{
    private readonly IRepository<LossAdjustmentOrder, Guid> _repository;

    public LossAdjustmentOrderAppService(IRepository<LossAdjustmentOrder, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<LossAdjustmentOrderDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return ObjectMapper.Map<LossAdjustmentOrder, LossAdjustmentOrderDto>(entity);
    }

    public async Task<PagedResultDto<LossAdjustmentOrderDto>> GetListAsync(GetLossAdjustmentOrderListInput input)
    {
        var query = await _repository.GetQueryableAsync();

        query = query
            .WhereIf(input.FarmerId.HasValue, x => x.FarmerId == input.FarmerId!.Value)
            .WhereIf(input.PurchaseOrderId.HasValue, x => x.PurchaseOrderId == input.PurchaseOrderId!.Value)
            .WhereIf(input.Status.HasValue, x => x.Status == input.Status!.Value);

        var totalCount = query.Count();

        var items = query
            .OrderByDescending(x => x.CreationTime)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        return new PagedResultDto<LossAdjustmentOrderDto>(
            totalCount,
            ObjectMapper.Map<List<LossAdjustmentOrder>, List<LossAdjustmentOrderDto>>(items)
        );
    }

    public async Task<LossAdjustmentOrderDto> CreateAsync(CreateLossAdjustmentOrderDto input)
    {
        var entity = new LossAdjustmentOrder(
            GuidGenerator.Create(),
            CurrentTenant.Id,
            input.LotId,
            input.FarmerId,
            input.PurchaseOrderId,
            input.AdjustmentNo,
            input.LossQuantityKg,
            input.LossAmount
        );

        entity.ReasonCode = input.ReasonCode;
        entity.Note = input.Note;

        await _repository.InsertAsync(entity);

        return ObjectMapper.Map<LossAdjustmentOrder, LossAdjustmentOrderDto>(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }
}

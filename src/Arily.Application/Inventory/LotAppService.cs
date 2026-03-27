using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Inventory.Lots;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Inventory;

[RemoteService(IsEnabled = false)]
public class LotAppService : ArilyAppService, ILotAppService
{
    private readonly IRepository<Lot, Guid> _lotRepository;

    public LotAppService(IRepository<Lot, Guid> lotRepository)
    {
        _lotRepository = lotRepository;
    }

    public async Task<LotDto> GetAsync(Guid id)
    {
        var lot = await _lotRepository.GetAsync(id);
        return ObjectMapper.Map<Lot, LotDto>(lot);
    }

    public async Task<PagedResultDto<LotDto>> GetListAsync(GetLotListInput input)
    {
        var query = await _lotRepository.GetQueryableAsync();

        query = query
            .WhereIf(input.WarehouseId.HasValue, x => x.WarehouseId == input.WarehouseId!.Value)
            .WhereIf(input.ProductId.HasValue, x => x.ProductId == input.ProductId!.Value)
            .WhereIf(input.FarmerId.HasValue, x => x.FarmerId == input.FarmerId!.Value)
            .WhereIf(input.Status.HasValue, x => x.Status == input.Status!.Value);

        var totalCount = query.Count();

        var lots = query
            .OrderByDescending(x => x.ReceivedAt)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        return new PagedResultDto<LotDto>(
            totalCount,
            ObjectMapper.Map<List<Lot>, List<LotDto>>(lots)
        );
    }

    public async Task<LotDto> CreateAsync(CreateLotDto input)
    {
        var lot = new Lot(
            GuidGenerator.Create(),
            CurrentTenant.Id,
            input.LotCode,
            input.PurchaseOrderId,
            input.FarmerId,
            input.ProductId,
            input.WarehouseId,
            input.ReceivedQuantityKg,
            input.ReceivedAt
        );

        lot.Note = input.Note;

        await _lotRepository.InsertAsync(lot);

        return ObjectMapper.Map<Lot, LotDto>(lot);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _lotRepository.DeleteAsync(id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Inventory.InventoryLots;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Inventory;

[RemoteService(IsEnabled = false)]
public class InventoryLotAppService : ArilyAppService, IInventoryLotAppService
{
    private readonly IRepository<InventoryLot, Guid> _inventoryLotRepository;

    public InventoryLotAppService(IRepository<InventoryLot, Guid> inventoryLotRepository)
    {
        _inventoryLotRepository = inventoryLotRepository;
    }

    public async Task<InventoryLotDto> GetAsync(Guid id)
    {
        var inventoryLot = await _inventoryLotRepository.GetAsync(id);
        return ObjectMapper.Map<InventoryLot, InventoryLotDto>(inventoryLot);
    }

    public async Task<PagedResultDto<InventoryLotDto>> GetListAsync(GetInventoryLotListInput input)
    {
        var query = await _inventoryLotRepository.GetQueryableAsync();

        query = query
            .WhereIf(input.WarehouseId.HasValue, x => x.WarehouseId == input.WarehouseId!.Value)
            .WhereIf(input.LotId.HasValue, x => x.LotId == input.LotId!.Value)
            .WhereIf(input.ProductId.HasValue, x => x.ProductId == input.ProductId!.Value);

        var totalCount = query.Count();

        var items = query
            .OrderByDescending(x => x.LastUpdatedAt)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        return new PagedResultDto<InventoryLotDto>(
            totalCount,
            ObjectMapper.Map<List<InventoryLot>, List<InventoryLotDto>>(items)
        );
    }
}

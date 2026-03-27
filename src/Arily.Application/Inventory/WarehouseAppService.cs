using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Enums;
using Arily.Inventory.Warehouses;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Inventory;

[RemoteService(IsEnabled = false)]
public class WarehouseAppService : ArilyAppService, IWarehouseAppService
{
    private readonly IRepository<Warehouse, Guid> _warehouseRepository;

    public WarehouseAppService(IRepository<Warehouse, Guid> warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<WarehouseDto> GetAsync(Guid id)
    {
        var warehouse = await _warehouseRepository.GetAsync(id);
        return ObjectMapper.Map<Warehouse, WarehouseDto>(warehouse);
    }

    public async Task<PagedResultDto<WarehouseDto>> GetListAsync(GetWarehouseListInput input)
    {
        var query = await _warehouseRepository.GetQueryableAsync();

        query = query
            .WhereIf(
                !input.Filter.IsNullOrWhiteSpace(),
                x => x.Name.Contains(input.Filter!) || x.Code.Contains(input.Filter!)
            )
            .WhereIf(input.Status.HasValue, x => x.Status == input.Status!.Value);

        var totalCount = query.Count();

        var warehouses = query
            .OrderBy(x => x.Name)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        return new PagedResultDto<WarehouseDto>(
            totalCount,
            ObjectMapper.Map<List<Warehouse>, List<WarehouseDto>>(warehouses)
        );
    }

    public async Task<WarehouseDto> CreateAsync(CreateUpdateWarehouseDto input)
    {
        var warehouse = new Warehouse(
            GuidGenerator.Create(),
            CurrentTenant.Id,
            input.Code,
            input.Name
        );

        warehouse.Address = input.Address;
        warehouse.ProvinceCode = input.ProvinceCode;
        warehouse.Note = input.Note;
        warehouse.Status = input.Status;

        await _warehouseRepository.InsertAsync(warehouse);

        return ObjectMapper.Map<Warehouse, WarehouseDto>(warehouse);
    }

    public async Task<WarehouseDto> UpdateAsync(Guid id, CreateUpdateWarehouseDto input)
    {
        var warehouse = await _warehouseRepository.GetAsync(id);

        warehouse.Code = input.Code;
        warehouse.Name = input.Name;
        warehouse.Address = input.Address;
        warehouse.ProvinceCode = input.ProvinceCode;
        warehouse.Note = input.Note;
        warehouse.Status = input.Status;

        await _warehouseRepository.UpdateAsync(warehouse);

        return ObjectMapper.Map<Warehouse, WarehouseDto>(warehouse);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _warehouseRepository.DeleteAsync(id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Inventory.Warehouses;
using Arily.Redis;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Inventory;

[RemoteService(IsEnabled = false)]
public class WarehouseAppService : ArilyAppService, IWarehouseAppService
{
    private readonly IRepository<Warehouse, Guid> _repository;
    private readonly IRedisService _redis;

    public WarehouseAppService(
        IRepository<Warehouse, Guid> repository,
        IRedisService redis)
    {
        _repository = repository;
        _redis = redis;
    }

    public async Task<WarehouseDto> GetAsync(Guid id)
    {
        var key = RedisKeys.Warehouse(CurrentTenant.Id, id);
        var cached = await _redis.StringGetAsync<WarehouseDto>(key);
        if (cached != null) return cached;

        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Warehouse, WarehouseDto>(entity);
        await _redis.StringSetAsync(key, dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task<PagedResultDto<WarehouseDto>> GetListAsync(GetWarehouseListInput input)
    {
        var hashKey = RedisKeys.WarehouseList(CurrentTenant.Id);
        var allItems = await _redis.HashGetAllAsync<WarehouseDto>(hashKey);

        if (allItems == null)
        {
            var entities = (await _repository.GetQueryableAsync()).OrderBy(x => x.Name).ToList();
            allItems = ObjectMapper.Map<List<Warehouse>, List<WarehouseDto>>(entities);
            var entries = allItems.Select(x => (x.Id.ToString(), x)).ToList();
            await _redis.HashLoadAsync(hashKey, entries, RedisTtl.MasterData);
        }

        var filtered = allItems.AsEnumerable();
        if (!input.Filter.IsNullOrWhiteSpace())
            filtered = filtered.Where(x => x.Name.Contains(input.Filter!) || x.Code.Contains(input.Filter!));
        if (input.Status.HasValue)
            filtered = filtered.Where(x => x.Status == input.Status!.Value);

        var result = filtered.OrderBy(x => x.Name).ToList();
        return new PagedResultDto<WarehouseDto>(
            result.Count,
            result.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
    }

    public async Task<WarehouseDto> CreateAsync(CreateUpdateWarehouseDto input)
    {
        var entity = new Warehouse(GuidGenerator.Create(), CurrentTenant.Id, input.Code, input.Name)
        {
            Address = input.Address,
            ProvinceCode = input.ProvinceCode,
            Note = input.Note,
            Status = input.Status
        };
        await _repository.InsertAsync(entity);
        var dto = ObjectMapper.Map<Warehouse, WarehouseDto>(entity);

        await _redis.StringSetAsync(RedisKeys.Warehouse(CurrentTenant.Id, entity.Id), dto, RedisTtl.MasterData);
        await _redis.HashSetAsync(RedisKeys.WarehouseList(CurrentTenant.Id), entity.Id.ToString(), dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task<WarehouseDto> UpdateAsync(Guid id, CreateUpdateWarehouseDto input)
    {
        var entity = await _repository.GetAsync(id);
        entity.Code = input.Code;
        entity.Name = input.Name;
        entity.Address = input.Address;
        entity.ProvinceCode = input.ProvinceCode;
        entity.Note = input.Note;
        entity.Status = input.Status;
        await _repository.UpdateAsync(entity);
        var dto = ObjectMapper.Map<Warehouse, WarehouseDto>(entity);

        await _redis.StringSetAsync(RedisKeys.Warehouse(CurrentTenant.Id, id), dto, RedisTtl.MasterData);
        await _redis.HashSetAsync(RedisKeys.WarehouseList(CurrentTenant.Id), id.ToString(), dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
        await _redis.KeyDeleteAsync(RedisKeys.Warehouse(CurrentTenant.Id, id));
        await _redis.HashDeleteAsync(RedisKeys.WarehouseList(CurrentTenant.Id), id.ToString());
    }
}

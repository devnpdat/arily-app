using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Catalog.UnitOfMeasures;
using Arily.Redis;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Catalog;

[RemoteService(IsEnabled = false)]
public class UnitOfMeasureAppService : ArilyAppService, IUnitOfMeasureAppService
{
    private readonly IRepository<UnitOfMeasure, Guid> _repository;
    private readonly IRedisService _redis;

    public UnitOfMeasureAppService(
        IRepository<UnitOfMeasure, Guid> repository,
        IRedisService redis)
    {
        _repository = repository;
        _redis = redis;
    }

    public async Task<UnitOfMeasureDto> GetAsync(Guid id)
    {
        var key = RedisKeys.UnitOfMeasure(CurrentTenant.Id, id);
        var cached = await _redis.StringGetAsync<UnitOfMeasureDto>(key);
        if (cached != null) return cached;

        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<UnitOfMeasure, UnitOfMeasureDto>(entity);
        await _redis.StringSetAsync(key, dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task<PagedResultDto<UnitOfMeasureDto>> GetListAsync(GetUnitOfMeasureListInput input)
    {
        var hashKey = RedisKeys.UnitOfMeasureList(CurrentTenant.Id);
        var allItems = await _redis.HashGetAllAsync<UnitOfMeasureDto>(hashKey);

        if (allItems == null)
        {
            var entities = (await _repository.GetQueryableAsync()).OrderBy(x => x.Name).ToList();
            allItems = ObjectMapper.Map<List<UnitOfMeasure>, List<UnitOfMeasureDto>>(entities);
            var entries = allItems.Select(x => (x.Id.ToString(), x)).ToList();
            await _redis.HashLoadAsync(hashKey, entries, RedisTtl.MasterData);
        }

        var filtered = allItems.AsEnumerable();
        if (!input.Filter.IsNullOrWhiteSpace())
            filtered = filtered.Where(x => x.Name.Contains(input.Filter!) || x.Code.Contains(input.Filter!));
        if (input.Status.HasValue)
            filtered = filtered.Where(x => x.Status == input.Status!.Value);

        var result = filtered.OrderBy(x => x.Name).ToList();
        return new PagedResultDto<UnitOfMeasureDto>(
            result.Count,
            result.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
    }

    public async Task<UnitOfMeasureDto> CreateAsync(CreateUpdateUnitOfMeasureDto input)
    {
        var entity = new UnitOfMeasure(GuidGenerator.Create(), CurrentTenant.Id, input.Code, input.Name)
        {
            Status = input.Status
        };
        await _repository.InsertAsync(entity);
        var dto = ObjectMapper.Map<UnitOfMeasure, UnitOfMeasureDto>(entity);

        await _redis.StringSetAsync(RedisKeys.UnitOfMeasure(CurrentTenant.Id, entity.Id), dto, RedisTtl.MasterData);
        await _redis.HashSetAsync(RedisKeys.UnitOfMeasureList(CurrentTenant.Id), entity.Id.ToString(), dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task<UnitOfMeasureDto> UpdateAsync(Guid id, CreateUpdateUnitOfMeasureDto input)
    {
        var entity = await _repository.GetAsync(id);
        entity.Code = input.Code;
        entity.Name = input.Name;
        entity.Status = input.Status;
        await _repository.UpdateAsync(entity);
        var dto = ObjectMapper.Map<UnitOfMeasure, UnitOfMeasureDto>(entity);

        await _redis.StringSetAsync(RedisKeys.UnitOfMeasure(CurrentTenant.Id, id), dto, RedisTtl.MasterData);
        await _redis.HashSetAsync(RedisKeys.UnitOfMeasureList(CurrentTenant.Id), id.ToString(), dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
        await _redis.KeyDeleteAsync(RedisKeys.UnitOfMeasure(CurrentTenant.Id, id));
        await _redis.HashDeleteAsync(RedisKeys.UnitOfMeasureList(CurrentTenant.Id), id.ToString());
    }
}

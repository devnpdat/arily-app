using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Catalog.ProductCategories;
using Arily.Redis;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Catalog;

[RemoteService(IsEnabled = false)]
public class ProductCategoryAppService : ArilyAppService, IProductCategoryAppService
{
    private readonly IRepository<ProductCategory, Guid> _repository;
    private readonly IRedisService _redis;

    public ProductCategoryAppService(
        IRepository<ProductCategory, Guid> repository,
        IRedisService redis)
    {
        _repository = repository;
        _redis = redis;
    }

    public async Task<ProductCategoryDto> GetAsync(Guid id)
    {
        var key = RedisKeys.ProductCategory(CurrentTenant.Id, id);
        var cached = await _redis.StringGetAsync<ProductCategoryDto>(key);
        if (cached != null) return cached;

        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<ProductCategory, ProductCategoryDto>(entity);
        await _redis.StringSetAsync(key, dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task<PagedResultDto<ProductCategoryDto>> GetListAsync(GetProductCategoryListInput input)
    {
        var hashKey = RedisKeys.ProductCategoryList(CurrentTenant.Id);
        var allItems = await _redis.HashGetAllAsync<ProductCategoryDto>(hashKey);

        if (allItems == null)
        {
            var entities = (await _repository.GetQueryableAsync()).OrderBy(x => x.Name).ToList();
            allItems = ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryDto>>(entities);
            var entries = allItems.Select(x => (x.Id.ToString(), x)).ToList();
            await _redis.HashLoadAsync(hashKey, entries, RedisTtl.MasterData);
        }

        var filtered = allItems.AsEnumerable();
        if (!input.Filter.IsNullOrWhiteSpace())
            filtered = filtered.Where(x => x.Name.Contains(input.Filter!) || x.Code.Contains(input.Filter!));
        if (input.Status.HasValue)
            filtered = filtered.Where(x => x.Status == input.Status!.Value);

        var result = filtered.OrderBy(x => x.Name).ToList();
        return new PagedResultDto<ProductCategoryDto>(
            result.Count,
            result.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
    }

    public async Task<ProductCategoryDto> CreateAsync(CreateUpdateProductCategoryDto input)
    {
        var entity = new ProductCategory(GuidGenerator.Create(), CurrentTenant.Id, input.Code, input.Name)
        {
            Description = input.Description,
            Status = input.Status
        };
        await _repository.InsertAsync(entity);
        var dto = ObjectMapper.Map<ProductCategory, ProductCategoryDto>(entity);

        var strKey  = RedisKeys.ProductCategory(CurrentTenant.Id, entity.Id);
        var hashKey = RedisKeys.ProductCategoryList(CurrentTenant.Id);
        await _redis.StringSetAsync(strKey, dto, RedisTtl.MasterData);
        await _redis.HashSetAsync(hashKey, entity.Id.ToString(), dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task<ProductCategoryDto> UpdateAsync(Guid id, CreateUpdateProductCategoryDto input)
    {
        var entity = await _repository.GetAsync(id);
        entity.Code = input.Code;
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.Status = input.Status;
        await _repository.UpdateAsync(entity);
        var dto = ObjectMapper.Map<ProductCategory, ProductCategoryDto>(entity);

        var strKey  = RedisKeys.ProductCategory(CurrentTenant.Id, id);
        var hashKey = RedisKeys.ProductCategoryList(CurrentTenant.Id);
        await _redis.StringSetAsync(strKey, dto, RedisTtl.MasterData);
        await _redis.HashSetAsync(hashKey, id.ToString(), dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
        await _redis.KeyDeleteAsync(RedisKeys.ProductCategory(CurrentTenant.Id, id));
        await _redis.HashDeleteAsync(RedisKeys.ProductCategoryList(CurrentTenant.Id), id.ToString());
    }
}

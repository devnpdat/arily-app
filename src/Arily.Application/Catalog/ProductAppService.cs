using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Catalog.Products;
using Arily.Redis;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Catalog;

[RemoteService(IsEnabled = false)]
public class ProductAppService : ArilyAppService, IProductAppService
{
    private readonly IRepository<Product, Guid> _repository;
    private readonly IRedisService _redis;

    public ProductAppService(
        IRepository<Product, Guid> repository,
        IRedisService redis)
    {
        _repository = repository;
        _redis = redis;
    }

    public async Task<ProductDto> GetAsync(Guid id)
    {
        var key = RedisKeys.Product(CurrentTenant.Id, id);
        var cached = await _redis.StringGetAsync<ProductDto>(key);
        if (cached != null) return cached;

        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Product, ProductDto>(entity);
        await _redis.StringSetAsync(key, dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task<PagedResultDto<ProductDto>> GetListAsync(GetProductListInput input)
    {
        var hashKey = RedisKeys.ProductList(CurrentTenant.Id);
        var allItems = await _redis.HashGetAllAsync<ProductDto>(hashKey);

        if (allItems == null)
        {
            var entities = (await _repository.GetQueryableAsync()).OrderBy(x => x.Name).ToList();
            allItems = ObjectMapper.Map<List<Product>, List<ProductDto>>(entities);
            var entries = allItems.Select(x => (x.Id.ToString(), x)).ToList();
            await _redis.HashLoadAsync(hashKey, entries, RedisTtl.MasterData);
        }

        var filtered = allItems.AsEnumerable();
        if (!input.Filter.IsNullOrWhiteSpace())
            filtered = filtered.Where(x => x.Name.Contains(input.Filter!) || x.Code.Contains(input.Filter!));
        if (input.Status.HasValue)
            filtered = filtered.Where(x => x.Status == input.Status!.Value);
        if (input.ProductCategoryId.HasValue)
            filtered = filtered.Where(x => x.ProductCategoryId == input.ProductCategoryId!.Value);

        var result = filtered.OrderBy(x => x.Name).ToList();
        return new PagedResultDto<ProductDto>(
            result.Count,
            result.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
    }

    public async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
    {
        var entity = new Product(
            GuidGenerator.Create(), CurrentTenant.Id,
            input.ProductCategoryId, input.UnitOfMeasureId,
            input.Code, input.Name)
        {
            DefaultLossRate = input.DefaultLossRate,
            Note = input.Note,
            Status = input.Status
        };
        await _repository.InsertAsync(entity);
        var dto = ObjectMapper.Map<Product, ProductDto>(entity);

        await _redis.StringSetAsync(RedisKeys.Product(CurrentTenant.Id, entity.Id), dto, RedisTtl.MasterData);
        await _redis.HashSetAsync(RedisKeys.ProductList(CurrentTenant.Id), entity.Id.ToString(), dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input)
    {
        var entity = await _repository.GetAsync(id);
        entity.ProductCategoryId = input.ProductCategoryId;
        entity.UnitOfMeasureId = input.UnitOfMeasureId;
        entity.Code = input.Code;
        entity.Name = input.Name;
        entity.DefaultLossRate = input.DefaultLossRate;
        entity.Note = input.Note;
        entity.Status = input.Status;
        await _repository.UpdateAsync(entity);
        var dto = ObjectMapper.Map<Product, ProductDto>(entity);

        await _redis.StringSetAsync(RedisKeys.Product(CurrentTenant.Id, id), dto, RedisTtl.MasterData);
        await _redis.HashSetAsync(RedisKeys.ProductList(CurrentTenant.Id), id.ToString(), dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
        await _redis.KeyDeleteAsync(RedisKeys.Product(CurrentTenant.Id, id));
        await _redis.HashDeleteAsync(RedisKeys.ProductList(CurrentTenant.Id), id.ToString());
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Catalog.ProductGrades;
using Arily.Redis;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Catalog;

[RemoteService(IsEnabled = false)]
public class ProductGradeAppService : ArilyAppService, IProductGradeAppService
{
    private readonly IRepository<ProductGrade, Guid> _repository;
    private readonly IRedisService _redis;

    public ProductGradeAppService(
        IRepository<ProductGrade, Guid> repository,
        IRedisService redis)
    {
        _repository = repository;
        _redis = redis;
    }

    public async Task<ProductGradeDto> GetAsync(Guid productId, Guid id)
    {
        var key = RedisKeys.ProductGrade(CurrentTenant.Id, id);
        var cached = await _redis.StringGetAsync<ProductGradeDto>(key);
        if (cached != null) return cached;

        var entity = await _repository.GetAsync(x => x.Id == id && x.ProductId == productId);
        var dto = ObjectMapper.Map<ProductGrade, ProductGradeDto>(entity);
        await _redis.StringSetAsync(key, dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task<ListResultDto<ProductGradeDto>> GetListAsync(Guid productId)
    {
        var hashKey = RedisKeys.ProductGradeList(CurrentTenant.Id, productId);
        var allItems = await _redis.HashGetAllAsync<ProductGradeDto>(hashKey);

        if (allItems == null)
        {
            var entities = (await _repository.GetQueryableAsync())
                .Where(x => x.ProductId == productId)
                .OrderBy(x => x.SortOrder)
                .ToList();
            allItems = ObjectMapper.Map<List<ProductGrade>, List<ProductGradeDto>>(entities);
            var entries = allItems.Select(x => (x.Id.ToString(), x)).ToList();
            await _redis.HashLoadAsync(hashKey, entries, RedisTtl.MasterData);
        }

        return new ListResultDto<ProductGradeDto>(allItems.OrderBy(x => x.SortOrder).ToList());
    }

    public async Task<ProductGradeDto> CreateAsync(Guid productId, CreateUpdateProductGradeDto input)
    {
        var entity = new ProductGrade(GuidGenerator.Create(), CurrentTenant.Id, productId, input.Code, input.Name)
        {
            SortOrder = input.SortOrder,
            Status = input.Status
        };
        await _repository.InsertAsync(entity);
        var dto = ObjectMapper.Map<ProductGrade, ProductGradeDto>(entity);

        await _redis.StringSetAsync(RedisKeys.ProductGrade(CurrentTenant.Id, entity.Id), dto, RedisTtl.MasterData);
        await _redis.HashSetAsync(RedisKeys.ProductGradeList(CurrentTenant.Id, productId), entity.Id.ToString(), dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task<ProductGradeDto> UpdateAsync(Guid productId, Guid id, CreateUpdateProductGradeDto input)
    {
        var entity = await _repository.GetAsync(x => x.Id == id && x.ProductId == productId);
        entity.Code = input.Code;
        entity.Name = input.Name;
        entity.SortOrder = input.SortOrder;
        entity.Status = input.Status;
        await _repository.UpdateAsync(entity);
        var dto = ObjectMapper.Map<ProductGrade, ProductGradeDto>(entity);

        await _redis.StringSetAsync(RedisKeys.ProductGrade(CurrentTenant.Id, id), dto, RedisTtl.MasterData);
        await _redis.HashSetAsync(RedisKeys.ProductGradeList(CurrentTenant.Id, productId), id.ToString(), dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task DeleteAsync(Guid productId, Guid id)
    {
        await _repository.DeleteAsync(x => x.Id == id && x.ProductId == productId);
        await _redis.KeyDeleteAsync(RedisKeys.ProductGrade(CurrentTenant.Id, id));
        await _redis.HashDeleteAsync(RedisKeys.ProductGradeList(CurrentTenant.Id, productId), id.ToString());
    }
}

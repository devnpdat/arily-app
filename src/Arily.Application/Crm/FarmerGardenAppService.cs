using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Crm.FarmerGardens;
using Arily.Redis;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Crm;

[RemoteService(IsEnabled = false)]
public class FarmerGardenAppService : ArilyAppService, IFarmerGardenAppService
{
    private readonly IRepository<FarmerGarden, Guid> _gardenRepository;
    private readonly IRepository<Farmer, Guid> _farmerRepository;
    private readonly IRedisService _redis;

    public FarmerGardenAppService(
        IRepository<FarmerGarden, Guid> gardenRepository,
        IRepository<Farmer, Guid> farmerRepository,
        IRedisService redis)
    {
        _gardenRepository = gardenRepository;
        _farmerRepository = farmerRepository;
        _redis = redis;
    }

    public async Task<FarmerGardenDto> GetAsync(Guid farmerId, Guid id)
    {
        var key = RedisKeys.FarmerGarden(CurrentTenant.Id, id);
        var cached = await _redis.StringGetAsync<FarmerGardenDto>(key);
        if (cached != null) return cached;

        var entity = await _gardenRepository.GetAsync(x => x.Id == id && x.FarmerId == farmerId);
        var dto = ObjectMapper.Map<FarmerGarden, FarmerGardenDto>(entity);
        await _redis.StringSetAsync(key, dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task<ListResultDto<FarmerGardenDto>> GetListAsync(Guid farmerId)
    {
        var hashKey = RedisKeys.FarmerGardenList(CurrentTenant.Id, farmerId);
        var allItems = await _redis.HashGetAllAsync<FarmerGardenDto>(hashKey);

        if (allItems == null)
        {
            var entities = (await _gardenRepository.GetQueryableAsync())
                .Where(x => x.FarmerId == farmerId)
                .OrderBy(x => x.GardenName)
                .ToList();
            allItems = ObjectMapper.Map<List<FarmerGarden>, List<FarmerGardenDto>>(entities);
            var entries = allItems.Select(x => (x.Id.ToString(), x)).ToList();
            await _redis.HashLoadAsync(hashKey, entries, RedisTtl.MasterData);
        }

        return new ListResultDto<FarmerGardenDto>(allItems.OrderBy(x => x.GardenName).ToList());
    }

    public async Task<FarmerGardenDto> CreateAsync(Guid farmerId, CreateUpdateFarmerGardenDto input)
    {
        await _farmerRepository.GetAsync(farmerId);

        var entity = new FarmerGarden(
            GuidGenerator.Create(), CurrentTenant.Id,
            farmerId, input.ProductCategoryId, input.GardenName)
        {
            AreaHectare = input.AreaHectare,
            EstimatedYieldKg = input.EstimatedYieldKg,
            Latitude = input.Latitude,
            Longitude = input.Longitude,
            Address = input.Address,
            Note = input.Note,
            Status = input.Status
        };
        await _gardenRepository.InsertAsync(entity);
        var dto = ObjectMapper.Map<FarmerGarden, FarmerGardenDto>(entity);

        await _redis.StringSetAsync(RedisKeys.FarmerGarden(CurrentTenant.Id, entity.Id), dto, RedisTtl.MasterData);
        await _redis.HashSetAsync(RedisKeys.FarmerGardenList(CurrentTenant.Id, farmerId), entity.Id.ToString(), dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task<FarmerGardenDto> UpdateAsync(Guid farmerId, Guid id, CreateUpdateFarmerGardenDto input)
    {
        var entity = await _gardenRepository.GetAsync(x => x.Id == id && x.FarmerId == farmerId);
        entity.ProductCategoryId = input.ProductCategoryId;
        entity.GardenName = input.GardenName;
        entity.AreaHectare = input.AreaHectare;
        entity.EstimatedYieldKg = input.EstimatedYieldKg;
        entity.Latitude = input.Latitude;
        entity.Longitude = input.Longitude;
        entity.Address = input.Address;
        entity.Note = input.Note;
        entity.Status = input.Status;
        await _gardenRepository.UpdateAsync(entity);
        var dto = ObjectMapper.Map<FarmerGarden, FarmerGardenDto>(entity);

        await _redis.StringSetAsync(RedisKeys.FarmerGarden(CurrentTenant.Id, id), dto, RedisTtl.MasterData);
        await _redis.HashSetAsync(RedisKeys.FarmerGardenList(CurrentTenant.Id, farmerId), id.ToString(), dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task DeleteAsync(Guid farmerId, Guid id)
    {
        await _gardenRepository.DeleteAsync(x => x.Id == id && x.FarmerId == farmerId);
        await _redis.KeyDeleteAsync(RedisKeys.FarmerGarden(CurrentTenant.Id, id));
        await _redis.HashDeleteAsync(RedisKeys.FarmerGardenList(CurrentTenant.Id, farmerId), id.ToString());
    }
}

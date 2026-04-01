using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Crm.Farmers;
using Arily.Redis;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Crm;

public class FarmerAppService : ArilyAppService, IFarmerAppService
{
    private readonly IRepository<Farmer, Guid> _repository;
    private readonly IRedisService _redis;

    public FarmerAppService(
        IRepository<Farmer, Guid> repository,
        IRedisService redis)
    {
        _repository = repository;
        _redis = redis;
    }

    public async Task<FarmerDto> GetAsync(Guid id)
    {
        var key = RedisKeys.Farmer(CurrentTenant.Id, id);
        var cached = await _redis.StringGetAsync<FarmerDto>(key);
        if (cached != null) return cached;

        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Farmer, FarmerDto>(entity);
        await _redis.StringSetAsync(key, dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task<PagedResultDto<FarmerDto>> GetListAsync(GetFarmerListInput input)
    {
        var hashKey = RedisKeys.FarmerList(CurrentTenant.Id);
        var allItems = await _redis.HashGetAllAsync<FarmerDto>(hashKey);

        if (allItems == null)
        {
            var entities = (await _repository.GetQueryableAsync()).OrderBy(x => x.FullName).ToList();
            allItems = ObjectMapper.Map<List<Farmer>, List<FarmerDto>>(entities);
            var entries = allItems.Select(x => (x.Id.ToString(), x)).ToList();
            await _redis.HashLoadAsync(hashKey, entries, RedisTtl.MasterData);
        }

        var filtered = allItems.AsEnumerable();
        if (!input.Filter.IsNullOrWhiteSpace())
            filtered = filtered.Where(x =>
                x.FullName.Contains(input.Filter!) ||
                x.Code.Contains(input.Filter!) ||
                x.PhoneNumber.Contains(input.Filter!) ||
                (x.NickName != null && x.NickName.Contains(input.Filter!)));
        if (input.Status.HasValue)
            filtered = filtered.Where(x => x.Status == input.Status!.Value);
        if (!input.ProvinceCode.IsNullOrWhiteSpace())
            filtered = filtered.Where(x => x.ProvinceCode == input.ProvinceCode);

        var result = filtered.OrderBy(x => x.FullName).ToList();
        return new PagedResultDto<FarmerDto>(
            result.Count,
            result.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
    }

    public async Task<FarmerDto> CreateAsync(CreateUpdateFarmerDto input)
    {
        var entity = new Farmer(GuidGenerator.Create(), CurrentTenant.Id, input.Code, input.FullName, input.PhoneNumber)
        {
            NickName = input.NickName,
            ProvinceCode = input.ProvinceCode,
            DistrictCode = input.DistrictCode,
            WardCode = input.WardCode,
            Address = input.Address,
            Latitude = input.Latitude,
            Longitude = input.Longitude,
            Note = input.Note,
            Status = input.Status
        };
        await _repository.InsertAsync(entity);
        var dto = ObjectMapper.Map<Farmer, FarmerDto>(entity);

        await _redis.StringSetAsync(RedisKeys.Farmer(CurrentTenant.Id, entity.Id), dto, RedisTtl.MasterData);
        await _redis.HashSetAsync(RedisKeys.FarmerList(CurrentTenant.Id), entity.Id.ToString(), dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task<FarmerDto> UpdateAsync(Guid id, CreateUpdateFarmerDto input)
    {
        var entity = await _repository.GetAsync(id);
        entity.Code = input.Code;
        entity.FullName = input.FullName;
        entity.NickName = input.NickName;
        entity.PhoneNumber = input.PhoneNumber;
        entity.ProvinceCode = input.ProvinceCode;
        entity.DistrictCode = input.DistrictCode;
        entity.WardCode = input.WardCode;
        entity.Address = input.Address;
        entity.Latitude = input.Latitude;
        entity.Longitude = input.Longitude;
        entity.Note = input.Note;
        entity.Status = input.Status;
        await _repository.UpdateAsync(entity);
        var dto = ObjectMapper.Map<Farmer, FarmerDto>(entity);

        await _redis.StringSetAsync(RedisKeys.Farmer(CurrentTenant.Id, id), dto, RedisTtl.MasterData);
        await _redis.HashSetAsync(RedisKeys.FarmerList(CurrentTenant.Id), id.ToString(), dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
        await _redis.KeyDeleteAsync(RedisKeys.Farmer(CurrentTenant.Id, id));
        await _redis.HashDeleteAsync(RedisKeys.FarmerList(CurrentTenant.Id), id.ToString());
    }
}

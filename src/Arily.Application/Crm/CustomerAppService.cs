using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Crm.Customers;
using Arily.Redis;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Crm;

[Volo.Abp.RemoteService(IsEnabled = false)]
public class CustomerAppService : ArilyAppService, ICustomerAppService
{
    private readonly IRepository<Customer, Guid> _repository;
    private readonly IRedisService _redis;

    public CustomerAppService(
        IRepository<Customer, Guid> repository,
        IRedisService redis)
    {
        _repository = repository;
        _redis = redis;
    }

    public async Task<CustomerDto> GetAsync(Guid id)
    {
        var key = RedisKeys.Customer(CurrentTenant.Id, id);
        var cached = await _redis.StringGetAsync<CustomerDto>(key);
        if (cached != null) return cached;

        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Customer, CustomerDto>(entity);
        await _redis.StringSetAsync(key, dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task<PagedResultDto<CustomerDto>> GetListAsync(GetCustomerListInput input)
    {
        var hashKey = RedisKeys.CustomerList(CurrentTenant.Id);
        var allItems = await _redis.HashGetAllAsync<CustomerDto>(hashKey);

        if (allItems == null)
        {
            var entities = (await _repository.GetQueryableAsync()).OrderBy(x => x.FullName).ToList();
            allItems = ObjectMapper.Map<List<Customer>, List<CustomerDto>>(entities);
            var entries = allItems.Select(x => (x.Id.ToString(), x)).ToList();
            await _redis.HashLoadAsync(hashKey, entries, RedisTtl.MasterData);
        }

        var filtered = allItems.AsEnumerable();
        if (!input.Filter.IsNullOrWhiteSpace())
            filtered = filtered.Where(x =>
                x.FullName.Contains(input.Filter!) ||
                x.Code.Contains(input.Filter!) ||
                (x.PhoneNumber != null && x.PhoneNumber.Contains(input.Filter!)) ||
                (x.Email != null && x.Email.Contains(input.Filter!)));
        if (input.Status.HasValue)
            filtered = filtered.Where(x => x.Status == input.Status!.Value);
        if (!input.CustomerType.IsNullOrWhiteSpace())
            filtered = filtered.Where(x => x.CustomerType == input.CustomerType);
        if (!input.ProvinceCode.IsNullOrWhiteSpace())
            filtered = filtered.Where(x => x.ProvinceCode == input.ProvinceCode);

        var result = filtered.OrderBy(x => x.FullName).ToList();
        return new PagedResultDto<CustomerDto>(
            result.Count,
            result.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
    }

    public async Task<CustomerDto> CreateAsync(CreateUpdateCustomerDto input)
    {
        var entity = new Customer(GuidGenerator.Create(), CurrentTenant.Id, input.Code, input.FullName)
        {
            PhoneNumber = input.PhoneNumber,
            Email = input.Email,
            CustomerType = input.CustomerType,
            ProvinceCode = input.ProvinceCode,
            DistrictCode = input.DistrictCode,
            Address = input.Address,
            Note = input.Note,
            Status = input.Status
        };
        await _repository.InsertAsync(entity);
        var dto = ObjectMapper.Map<Customer, CustomerDto>(entity);

        await _redis.StringSetAsync(RedisKeys.Customer(CurrentTenant.Id, entity.Id), dto, RedisTtl.MasterData);
        await _redis.HashSetAsync(RedisKeys.CustomerList(CurrentTenant.Id), entity.Id.ToString(), dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task<CustomerDto> UpdateAsync(Guid id, CreateUpdateCustomerDto input)
    {
        var entity = await _repository.GetAsync(id);
        entity.Code = input.Code;
        entity.FullName = input.FullName;
        entity.PhoneNumber = input.PhoneNumber;
        entity.Email = input.Email;
        entity.CustomerType = input.CustomerType;
        entity.ProvinceCode = input.ProvinceCode;
        entity.DistrictCode = input.DistrictCode;
        entity.Address = input.Address;
        entity.Note = input.Note;
        entity.Status = input.Status;
        await _repository.UpdateAsync(entity);
        var dto = ObjectMapper.Map<Customer, CustomerDto>(entity);

        await _redis.StringSetAsync(RedisKeys.Customer(CurrentTenant.Id, id), dto, RedisTtl.MasterData);
        await _redis.HashSetAsync(RedisKeys.CustomerList(CurrentTenant.Id), id.ToString(), dto, RedisTtl.MasterData);
        return dto;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
        await _redis.KeyDeleteAsync(RedisKeys.Customer(CurrentTenant.Id, id));
        await _redis.HashDeleteAsync(RedisKeys.CustomerList(CurrentTenant.Id), id.ToString());
    }
}

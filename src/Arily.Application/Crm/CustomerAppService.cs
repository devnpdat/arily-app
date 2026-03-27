using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Crm.Customers;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Crm;

[Volo.Abp.RemoteService(IsEnabled = false)]
public class CustomerAppService : ArilyAppService, ICustomerAppService
{
    private readonly IRepository<Customer, Guid> _customerRepository;

    public CustomerAppService(IRepository<Customer, Guid> customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto> GetAsync(Guid id)
    {
        var customer = await _customerRepository.GetAsync(id);
        return ObjectMapper.Map<Customer, CustomerDto>(customer);
    }

    public async Task<PagedResultDto<CustomerDto>> GetListAsync(GetCustomerListInput input)
    {
        var query = await _customerRepository.GetQueryableAsync();

        query = query
            .WhereIf(
                !input.Filter.IsNullOrWhiteSpace(),
                x => x.FullName.Contains(input.Filter!) ||
                     x.Code.Contains(input.Filter!) ||
                     (x.PhoneNumber != null && x.PhoneNumber.Contains(input.Filter!)) ||
                     (x.Email != null && x.Email.Contains(input.Filter!))
            )
            .WhereIf(input.Status.HasValue, x => x.Status == input.Status!.Value)
            .WhereIf(!input.CustomerType.IsNullOrWhiteSpace(), x => x.CustomerType == input.CustomerType)
            .WhereIf(!input.ProvinceCode.IsNullOrWhiteSpace(), x => x.ProvinceCode == input.ProvinceCode);

        var totalCount = query.Count();

        var customers = query
            .OrderBy(x => x.FullName)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        return new PagedResultDto<CustomerDto>(
            totalCount,
            ObjectMapper.Map<List<Customer>, List<CustomerDto>>(customers)
        );
    }

    public async Task<CustomerDto> CreateAsync(CreateUpdateCustomerDto input)
    {
        var customer = new Customer(
            GuidGenerator.Create(),
            CurrentTenant.Id,
            input.Code,
            input.FullName
        );

        customer.PhoneNumber = input.PhoneNumber;
        customer.Email = input.Email;
        customer.CustomerType = input.CustomerType;
        customer.ProvinceCode = input.ProvinceCode;
        customer.DistrictCode = input.DistrictCode;
        customer.Address = input.Address;
        customer.Note = input.Note;
        customer.Status = input.Status;

        await _customerRepository.InsertAsync(customer);

        return ObjectMapper.Map<Customer, CustomerDto>(customer);
    }

    public async Task<CustomerDto> UpdateAsync(Guid id, CreateUpdateCustomerDto input)
    {
        var customer = await _customerRepository.GetAsync(id);

        customer.Code = input.Code;
        customer.FullName = input.FullName;
        customer.PhoneNumber = input.PhoneNumber;
        customer.Email = input.Email;
        customer.CustomerType = input.CustomerType;
        customer.ProvinceCode = input.ProvinceCode;
        customer.DistrictCode = input.DistrictCode;
        customer.Address = input.Address;
        customer.Note = input.Note;
        customer.Status = input.Status;

        await _customerRepository.UpdateAsync(customer);

        return ObjectMapper.Map<Customer, CustomerDto>(customer);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _customerRepository.DeleteAsync(id);
    }
}

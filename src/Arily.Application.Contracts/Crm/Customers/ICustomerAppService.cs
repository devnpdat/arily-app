using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Arily.Crm.Customers;

public interface ICustomerAppService : IApplicationService
{
    Task<CustomerDto> GetAsync(Guid id);
    Task<PagedResultDto<CustomerDto>> GetListAsync(GetCustomerListInput input);
    Task<CustomerDto> CreateAsync(CreateUpdateCustomerDto input);
    Task<CustomerDto> UpdateAsync(Guid id, CreateUpdateCustomerDto input);
    Task DeleteAsync(Guid id);
}

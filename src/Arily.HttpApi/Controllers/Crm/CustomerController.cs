using System;
using System.Threading.Tasks;
using Arily.Crm.Customers;
using Arily.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;

namespace Arily.Controllers.Crm;

[ApiController]
[Route("api/app/customers")]
[Authorize(ArilyPermissions.Customers.Default)]
public class CustomerController : ArilyController
{
    private readonly ICustomerAppService _customerAppService;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(ICustomerAppService customerAppService, ILogger<CustomerController> logger)
    {
        _customerAppService = customerAppService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    [Authorize(ArilyPermissions.Customers.Default)]
    public async Task<CustomerDto> GetAsync(Guid id)
    {
        _logger.LogInformation("GetCustomer: id={Id}", id);
        return await _customerAppService.GetAsync(id);
    }

    [HttpGet]
    [Authorize(ArilyPermissions.Customers.Default)]
    public async Task<PagedResultDto<CustomerDto>> GetListAsync([FromQuery] GetCustomerListInput input)
    {
        _logger.LogInformation("GetCustomerList: filter={Filter} status={Status} type={Type} province={Province}",
            input.Filter, input.Status, input.CustomerType, input.ProvinceCode);
        return await _customerAppService.GetListAsync(input);
    }

    [HttpPost]
    [Authorize(ArilyPermissions.Customers.Create)]
    public async Task<CustomerDto> CreateAsync([FromBody] CreateUpdateCustomerDto input)
    {
        _logger.LogInformation("CreateCustomer: code={Code} name={FullName}", input.Code, input.FullName);
        return await _customerAppService.CreateAsync(input);
    }

    [HttpPut("{id:guid}")]
    [Authorize(ArilyPermissions.Customers.Edit)]
    public async Task<CustomerDto> UpdateAsync(Guid id, [FromBody] CreateUpdateCustomerDto input)
    {
        _logger.LogInformation("UpdateCustomer: id={Id} code={Code} name={FullName}", id, input.Code, input.FullName);
        return await _customerAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(ArilyPermissions.Customers.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("DeleteCustomer: id={Id}", id);
        await _customerAppService.DeleteAsync(id);
    }
}

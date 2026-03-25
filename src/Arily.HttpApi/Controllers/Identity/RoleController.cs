using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace Arily.Controllers.Identity;

[ApiController]
[Route("api/app/roles")]
[Authorize]
public class RoleController : ArilyController
{
    private readonly IIdentityRoleAppService _roleAppService;
    private readonly ILogger<RoleController> _logger;

    public RoleController(IIdentityRoleAppService roleAppService, ILogger<RoleController> logger)
    {
        _roleAppService = roleAppService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    public async Task<IdentityRoleDto> GetAsync(Guid id)
    {
        _logger.LogInformation("GetRole: id={Id}", id);
        return await _roleAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<IdentityRoleDto>> GetListAsync([FromQuery] GetIdentityRolesInput input)
    {
        _logger.LogInformation("GetRoleList: filter={Filter}", input.Filter);
        return await _roleAppService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<IdentityRoleDto> CreateAsync([FromBody] IdentityRoleCreateDto input)
    {
        _logger.LogInformation("CreateRole: name={Name}", input.Name);
        return await _roleAppService.CreateAsync(input);
    }

    [HttpPut("{id:guid}")]
    public async Task<IdentityRoleDto> UpdateAsync(Guid id, [FromBody] IdentityRoleUpdateDto input)
    {
        _logger.LogInformation("UpdateRole: id={Id} name={Name}", id, input.Name);
        return await _roleAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id:guid}")]
    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("DeleteRole: id={Id}", id);
        await _roleAppService.DeleteAsync(id);
    }
}

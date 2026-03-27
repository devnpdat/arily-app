using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Arily.Inventory.Warehouses;

public interface IWarehouseAppService : IApplicationService
{
    Task<WarehouseDto> GetAsync(Guid id);
    Task<PagedResultDto<WarehouseDto>> GetListAsync(GetWarehouseListInput input);
    Task<WarehouseDto> CreateAsync(CreateUpdateWarehouseDto input);
    Task<WarehouseDto> UpdateAsync(Guid id, CreateUpdateWarehouseDto input);
    Task DeleteAsync(Guid id);
}

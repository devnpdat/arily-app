using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Arily.Inventory.InventoryLots;

public interface IInventoryLotAppService : IApplicationService
{
    Task<InventoryLotDto> GetAsync(Guid id);
    Task<PagedResultDto<InventoryLotDto>> GetListAsync(GetInventoryLotListInput input);
}

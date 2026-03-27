using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Arily.Inventory.Lots;

public interface ILotAppService : IApplicationService
{
    Task<LotDto> GetAsync(Guid id);
    Task<PagedResultDto<LotDto>> GetListAsync(GetLotListInput input);
    Task<LotDto> CreateAsync(CreateLotDto input);
    Task DeleteAsync(Guid id);
}

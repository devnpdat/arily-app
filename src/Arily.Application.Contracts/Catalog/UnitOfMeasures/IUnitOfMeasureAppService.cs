using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Arily.Catalog.UnitOfMeasures;

public interface IUnitOfMeasureAppService : IApplicationService
{
    Task<UnitOfMeasureDto> GetAsync(Guid id);
    Task<PagedResultDto<UnitOfMeasureDto>> GetListAsync(GetUnitOfMeasureListInput input);
    Task<UnitOfMeasureDto> CreateAsync(CreateUpdateUnitOfMeasureDto input);
    Task<UnitOfMeasureDto> UpdateAsync(Guid id, CreateUpdateUnitOfMeasureDto input);
    Task DeleteAsync(Guid id);
}

using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
namespace Arily.Crm.Farmers;

public interface IFarmerAppService : IApplicationService
{
    Task<FarmerDto> GetAsync(Guid id);
    Task<PagedResultDto<FarmerDto>> GetListAsync(GetFarmerListInput input);
    Task<FarmerDto> CreateAsync(CreateUpdateFarmerDto input);
    Task<FarmerDto> UpdateAsync(Guid id, CreateUpdateFarmerDto input);
    Task DeleteAsync(Guid id);
}

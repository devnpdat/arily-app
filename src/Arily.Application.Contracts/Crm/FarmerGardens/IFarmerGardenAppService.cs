using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Arily.Crm.FarmerGardens;

public interface IFarmerGardenAppService : IApplicationService
{
    Task<FarmerGardenDto> GetAsync(Guid farmerId, Guid id);
    Task<ListResultDto<FarmerGardenDto>> GetListAsync(Guid farmerId);
    Task<FarmerGardenDto> CreateAsync(Guid farmerId, CreateUpdateFarmerGardenDto input);
    Task<FarmerGardenDto> UpdateAsync(Guid farmerId, Guid id, CreateUpdateFarmerGardenDto input);
    Task DeleteAsync(Guid farmerId, Guid id);
}

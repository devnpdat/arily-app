using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Arily.Finance.FarmerDebtLedgers;

public interface IFarmerDebtLedgerAppService : IApplicationService
{
    Task<FarmerDebtLedgerDto> GetAsync(Guid id);
    Task<PagedResultDto<FarmerDebtLedgerDto>> GetListAsync(GetFarmerDebtLedgerListInput input);
}

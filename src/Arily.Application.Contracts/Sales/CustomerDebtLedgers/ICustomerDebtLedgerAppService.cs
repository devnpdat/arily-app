using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Arily.Sales.CustomerDebtLedgers;

public interface ICustomerDebtLedgerAppService : IApplicationService
{
    Task<CustomerDebtLedgerDto> GetAsync(Guid id);
    Task<PagedResultDto<CustomerDebtLedgerDto>> GetListAsync(GetCustomerDebtLedgerListInput input);
}

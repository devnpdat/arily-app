using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Arily.Sales.SalesOrders;

public interface ISalesOrderAppService : IApplicationService
{
    Task<SalesOrderDto> GetAsync(Guid id);
    Task<PagedResultDto<SalesOrderDto>> GetListAsync(GetSalesOrderListInput input);
    Task<SalesOrderDto> CreateAsync(CreateUpdateSalesOrderDto input);
    Task<SalesOrderDto> UpdateAsync(Guid id, CreateUpdateSalesOrderDto input);
    Task DeleteAsync(Guid id);
    Task<SalesOrderDto> ConfirmAsync(Guid id);
    Task<SalesOrderDto> DeliverAsync(Guid id);
    Task<SalesOrderDto> CompleteAsync(Guid id);
    Task<SalesOrderDto> CancelAsync(Guid id);
}

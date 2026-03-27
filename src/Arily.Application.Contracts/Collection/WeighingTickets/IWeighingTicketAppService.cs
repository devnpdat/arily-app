using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Arily.Collection.WeighingTickets;

public interface IWeighingTicketAppService : IApplicationService
{
    Task<WeighingTicketDto> GetAsync(Guid id);
    Task<ListResultDto<WeighingTicketDto>> GetListAsync(Guid purchaseOrderId);
    Task<WeighingTicketDto> CreateAsync(CreateWeighingTicketDto input);
    Task DeleteAsync(Guid id);
}

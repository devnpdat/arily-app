using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Collection.WeighingTickets;
using Arily.Enums;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Collection;

[RemoteService(IsEnabled = false)]
public class WeighingTicketAppService : ArilyAppService, IWeighingTicketAppService
{
    private readonly IRepository<WeighingTicket, Guid> _weighingTicketRepository;
    private readonly IRepository<PurchaseOrder, Guid> _purchaseOrderRepository;

    public WeighingTicketAppService(
        IRepository<WeighingTicket, Guid> weighingTicketRepository,
        IRepository<PurchaseOrder, Guid> purchaseOrderRepository)
    {
        _weighingTicketRepository = weighingTicketRepository;
        _purchaseOrderRepository = purchaseOrderRepository;
    }

    public async Task<WeighingTicketDto> GetAsync(Guid id)
    {
        var ticket = await _weighingTicketRepository.GetAsync(id);
        return ObjectMapper.Map<WeighingTicket, WeighingTicketDto>(ticket);
    }

    public async Task<ListResultDto<WeighingTicketDto>> GetListAsync(Guid purchaseOrderId)
    {
        var query = await _weighingTicketRepository.GetQueryableAsync();

        var tickets = query
            .Where(x => x.PurchaseOrderId == purchaseOrderId)
            .OrderByDescending(x => x.WeighedAt)
            .ToList();

        return new ListResultDto<WeighingTicketDto>(
            ObjectMapper.Map<List<WeighingTicket>, List<WeighingTicketDto>>(tickets)
        );
    }

    public async Task<WeighingTicketDto> CreateAsync(CreateWeighingTicketDto input)
    {
        var order = await _purchaseOrderRepository.FindAsync(input.PurchaseOrderId);
        if (order == null)
            throw new UserFriendlyException("Không tìm thấy đơn mua hàng.");

        if (order.Status == PurchaseOrderStatus.Cancelled)
            throw new UserFriendlyException("Không thể tạo phiếu cân cho đơn mua đã hủy.");

        var ticket = new WeighingTicket(
            GuidGenerator.Create(),
            CurrentTenant.Id,
            input.PurchaseOrderId,
            input.TicketNo,
            input.GrossWeightKg,
            input.TareWeightKg,
            input.WeighedAt
        );

        ticket.Note = input.Note;

        await _weighingTicketRepository.InsertAsync(ticket);

        return ObjectMapper.Map<WeighingTicket, WeighingTicketDto>(ticket);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _weighingTicketRepository.DeleteAsync(id);
    }
}

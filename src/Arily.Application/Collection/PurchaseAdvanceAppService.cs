using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Collection.PurchaseAdvances;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Collection;

[RemoteService(IsEnabled = false)]
public class PurchaseAdvanceAppService : ArilyAppService, IPurchaseAdvanceAppService
{
    private readonly IRepository<PurchaseAdvance, Guid> _purchaseAdvanceRepository;
    private readonly IRepository<PurchaseOrder, Guid> _purchaseOrderRepository;

    public PurchaseAdvanceAppService(
        IRepository<PurchaseAdvance, Guid> purchaseAdvanceRepository,
        IRepository<PurchaseOrder, Guid> purchaseOrderRepository)
    {
        _purchaseAdvanceRepository = purchaseAdvanceRepository;
        _purchaseOrderRepository = purchaseOrderRepository;
    }

    public async Task<PurchaseAdvanceDto> GetAsync(Guid id)
    {
        var advance = await _purchaseAdvanceRepository.GetAsync(id);
        return ObjectMapper.Map<PurchaseAdvance, PurchaseAdvanceDto>(advance);
    }

    public async Task<ListResultDto<PurchaseAdvanceDto>> GetListAsync(Guid purchaseOrderId)
    {
        var query = await _purchaseAdvanceRepository.GetQueryableAsync();

        var advances = query
            .Where(x => x.PurchaseOrderId == purchaseOrderId)
            .OrderByDescending(x => x.AdvancedAt)
            .ToList();

        return new ListResultDto<PurchaseAdvanceDto>(
            ObjectMapper.Map<List<PurchaseAdvance>, List<PurchaseAdvanceDto>>(advances)
        );
    }

    public async Task<PurchaseAdvanceDto> CreateAsync(CreatePurchaseAdvanceDto input)
    {
        var order = await _purchaseOrderRepository.FindAsync(input.PurchaseOrderId);
        if (order == null)
            throw new UserFriendlyException("Không tìm thấy đơn mua hàng.");

        var advance = new PurchaseAdvance(
            GuidGenerator.Create(),
            CurrentTenant.Id,
            input.PurchaseOrderId,
            input.FarmerId,
            input.Amount,
            input.PaymentMethod,
            input.AdvancedAt
        );

        advance.Note = input.Note;

        await _purchaseAdvanceRepository.InsertAsync(advance);

        return ObjectMapper.Map<PurchaseAdvance, PurchaseAdvanceDto>(advance);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _purchaseAdvanceRepository.DeleteAsync(id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Collection.PurchaseOrders;
using Arily.Enums;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Collection;

[RemoteService(IsEnabled = false)]
public class PurchaseOrderAppService : ArilyAppService, IPurchaseOrderAppService
{
    private readonly IRepository<PurchaseOrder, Guid> _purchaseOrderRepository;
    private readonly IRepository<CollectionSession, Guid> _sessionRepository;

    public PurchaseOrderAppService(
        IRepository<PurchaseOrder, Guid> purchaseOrderRepository,
        IRepository<CollectionSession, Guid> sessionRepository)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _sessionRepository = sessionRepository;
    }

    public async Task<PurchaseOrderDto> GetAsync(Guid id)
    {
        var order = await _purchaseOrderRepository.GetAsync(id);
        return ObjectMapper.Map<PurchaseOrder, PurchaseOrderDto>(order);
    }

    public async Task<PagedResultDto<PurchaseOrderDto>> GetListAsync(GetPurchaseOrderListInput input)
    {
        var query = await _purchaseOrderRepository.GetQueryableAsync();

        query = query
            .WhereIf(input.SessionId.HasValue, x => x.SessionId == input.SessionId!.Value)
            .WhereIf(input.FarmerId.HasValue, x => x.FarmerId == input.FarmerId!.Value)
            .WhereIf(input.Status.HasValue, x => x.Status == input.Status!.Value);

        var totalCount = query.Count();

        var orders = query
            .OrderByDescending(x => x.PurchaseDate)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        return new PagedResultDto<PurchaseOrderDto>(
            totalCount,
            ObjectMapper.Map<List<PurchaseOrder>, List<PurchaseOrderDto>>(orders)
        );
    }

    public async Task<PurchaseOrderDto> CreateAsync(CreatePurchaseOrderDto input)
    {
        var session = await _sessionRepository.GetAsync(input.SessionId);

        if (session.Status != CollectionSessionStatus.Open)
            throw new UserFriendlyException("Chỉ có thể tạo đơn mua trong phiên đang mở.");

        var order = new PurchaseOrder(
            GuidGenerator.Create(),
            CurrentTenant.Id,
            input.SessionId,
            input.FarmerId,
            input.ProductId,
            input.OrderNo,
            input.PurchaseDate
        );

        order.GardenId = input.GardenId;
        order.ExpectedQuantityKg = input.ExpectedQuantityKg;
        order.UnitPrice = input.UnitPrice;
        order.GrossAmount = input.ExpectedQuantityKg * input.UnitPrice;
        order.Note = input.Note;

        await _purchaseOrderRepository.InsertAsync(order);

        return ObjectMapper.Map<PurchaseOrder, PurchaseOrderDto>(order);
    }

    public async Task DeleteAsync(Guid id)
    {
        var order = await _purchaseOrderRepository.GetAsync(id);

        if (order.Status != PurchaseOrderStatus.Draft)
            throw new UserFriendlyException("Chỉ có thể xóa đơn mua ở trạng thái Draft.");

        await _purchaseOrderRepository.DeleteAsync(id);
    }
}

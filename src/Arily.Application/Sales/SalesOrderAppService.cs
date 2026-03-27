using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Enums;
using Arily.Sales.SalesOrders;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Sales;

[RemoteService(IsEnabled = false)]
public class SalesOrderAppService : ArilyAppService, ISalesOrderAppService
{
    private readonly IRepository<SalesOrder, Guid> _repository;

    public SalesOrderAppService(IRepository<SalesOrder, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<SalesOrderDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return ObjectMapper.Map<SalesOrder, SalesOrderDto>(entity);
    }

    public async Task<PagedResultDto<SalesOrderDto>> GetListAsync(GetSalesOrderListInput input)
    {
        var query = await _repository.GetQueryableAsync();

        query = query
            .WhereIf(
                !input.Filter.IsNullOrWhiteSpace(),
                x => x.OrderNo.Contains(input.Filter!)
            )
            .WhereIf(input.CustomerId.HasValue, x => x.CustomerId == input.CustomerId!.Value)
            .WhereIf(input.Status.HasValue, x => x.Status == input.Status!.Value)
            .WhereIf(input.OrderDateFrom.HasValue, x => x.OrderDate >= input.OrderDateFrom!.Value)
            .WhereIf(input.OrderDateTo.HasValue, x => x.OrderDate <= input.OrderDateTo!.Value);

        var totalCount = query.Count();

        var items = query
            .OrderByDescending(x => x.OrderDate)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        return new PagedResultDto<SalesOrderDto>(
            totalCount,
            ObjectMapper.Map<List<SalesOrder>, List<SalesOrderDto>>(items)
        );
    }

    public async Task<SalesOrderDto> CreateAsync(CreateUpdateSalesOrderDto input)
    {
        var entity = new SalesOrder(
            GuidGenerator.Create(),
            CurrentTenant.Id,
            input.OrderNo,
            input.CustomerId,
            input.OrderDate
        );

        entity.DeliveryDate = input.DeliveryDate;
        entity.TotalAmount = input.TotalAmount;
        entity.DiscountAmount = input.DiscountAmount;
        entity.NetAmount = input.TotalAmount - input.DiscountAmount;
        entity.PaidAmount = 0;
        entity.DebtAmount = entity.NetAmount;
        entity.Note = input.Note;

        await _repository.InsertAsync(entity);

        return ObjectMapper.Map<SalesOrder, SalesOrderDto>(entity);
    }

    public async Task<SalesOrderDto> UpdateAsync(Guid id, CreateUpdateSalesOrderDto input)
    {
        var entity = await _repository.GetAsync(id);

        if (entity.Status != SalesOrderStatus.Draft)
            throw new UserFriendlyException("Chỉ có thể chỉnh sửa đơn hàng ở trạng thái Draft.");

        entity.OrderNo = input.OrderNo;
        entity.CustomerId = input.CustomerId;
        entity.OrderDate = input.OrderDate;
        entity.DeliveryDate = input.DeliveryDate;
        entity.TotalAmount = input.TotalAmount;
        entity.DiscountAmount = input.DiscountAmount;
        entity.NetAmount = input.TotalAmount - input.DiscountAmount;
        entity.DebtAmount = entity.NetAmount - entity.PaidAmount;
        entity.Note = input.Note;

        await _repository.UpdateAsync(entity);

        return ObjectMapper.Map<SalesOrder, SalesOrderDto>(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);

        if (entity.Status != SalesOrderStatus.Draft)
            throw new UserFriendlyException("Chỉ có thể xóa đơn hàng ở trạng thái Draft.");

        await _repository.DeleteAsync(id);
    }

    public async Task<SalesOrderDto> ConfirmAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);

        if (entity.Status != SalesOrderStatus.Draft)
            throw new UserFriendlyException("Chỉ có thể xác nhận đơn ở trạng thái Draft.");

        entity.Status = SalesOrderStatus.Confirmed;

        await _repository.UpdateAsync(entity);

        return ObjectMapper.Map<SalesOrder, SalesOrderDto>(entity);
    }

    public async Task<SalesOrderDto> DeliverAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);

        if (entity.Status != SalesOrderStatus.Confirmed)
            throw new UserFriendlyException("Chỉ có thể giao hàng đơn ở trạng thái Confirmed.");

        entity.Status = SalesOrderStatus.Delivered;

        await _repository.UpdateAsync(entity);

        return ObjectMapper.Map<SalesOrder, SalesOrderDto>(entity);
    }

    public async Task<SalesOrderDto> CompleteAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);

        if (entity.Status != SalesOrderStatus.Delivered)
            throw new UserFriendlyException("Chỉ có thể hoàn thành đơn ở trạng thái Delivered.");

        entity.Status = SalesOrderStatus.Completed;

        await _repository.UpdateAsync(entity);

        return ObjectMapper.Map<SalesOrder, SalesOrderDto>(entity);
    }

    public async Task<SalesOrderDto> CancelAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);

        if (entity.Status != SalesOrderStatus.Draft && entity.Status != SalesOrderStatus.Confirmed)
            throw new UserFriendlyException("Chỉ có thể hủy đơn ở trạng thái Draft hoặc Confirmed.");

        entity.Status = SalesOrderStatus.Cancelled;

        await _repository.UpdateAsync(entity);

        return ObjectMapper.Map<SalesOrder, SalesOrderDto>(entity);
    }
}

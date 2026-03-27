using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Sales.CustomerDebtLedgers;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Sales;

[RemoteService(IsEnabled = false)]
public class CustomerDebtLedgerAppService : ArilyAppService, ICustomerDebtLedgerAppService
{
    private readonly IRepository<CustomerDebtLedger, Guid> _repository;

    public CustomerDebtLedgerAppService(IRepository<CustomerDebtLedger, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<CustomerDebtLedgerDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return ObjectMapper.Map<CustomerDebtLedger, CustomerDebtLedgerDto>(entity);
    }

    public async Task<PagedResultDto<CustomerDebtLedgerDto>> GetListAsync(GetCustomerDebtLedgerListInput input)
    {
        var query = await _repository.GetQueryableAsync();

        query = query
            .WhereIf(input.CustomerId.HasValue, x => x.CustomerId == input.CustomerId!.Value)
            .WhereIf(input.LedgerType.HasValue, x => x.LedgerType == input.LedgerType!.Value)
            .WhereIf(input.DateFrom.HasValue, x => x.TransactionDate >= input.DateFrom!.Value)
            .WhereIf(input.DateTo.HasValue, x => x.TransactionDate <= input.DateTo!.Value);

        var totalCount = query.Count();

        var items = query
            .OrderByDescending(x => x.TransactionDate)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        return new PagedResultDto<CustomerDebtLedgerDto>(
            totalCount,
            ObjectMapper.Map<List<CustomerDebtLedger>, List<CustomerDebtLedgerDto>>(items)
        );
    }
}

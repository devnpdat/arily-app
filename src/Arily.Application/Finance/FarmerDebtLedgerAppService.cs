using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Finance.FarmerDebtLedgers;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Finance;

[RemoteService(IsEnabled = false)]
public class FarmerDebtLedgerAppService : ArilyAppService, IFarmerDebtLedgerAppService
{
    private readonly IRepository<FarmerDebtLedger, Guid> _repository;

    public FarmerDebtLedgerAppService(IRepository<FarmerDebtLedger, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<FarmerDebtLedgerDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return ObjectMapper.Map<FarmerDebtLedger, FarmerDebtLedgerDto>(entity);
    }

    public async Task<PagedResultDto<FarmerDebtLedgerDto>> GetListAsync(GetFarmerDebtLedgerListInput input)
    {
        var query = await _repository.GetQueryableAsync();

        query = query
            .WhereIf(input.FarmerId.HasValue, x => x.FarmerId == input.FarmerId!.Value)
            .WhereIf(input.LedgerType.HasValue, x => x.LedgerType == input.LedgerType!.Value)
            .WhereIf(input.DateFrom.HasValue, x => x.TransactionDate >= input.DateFrom!.Value)
            .WhereIf(input.DateTo.HasValue, x => x.TransactionDate <= input.DateTo!.Value);

        var totalCount = query.Count();

        var items = query
            .OrderByDescending(x => x.TransactionDate)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        return new PagedResultDto<FarmerDebtLedgerDto>(
            totalCount,
            ObjectMapper.Map<List<FarmerDebtLedger>, List<FarmerDebtLedgerDto>>(items)
        );
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Catalog.UnitOfMeasures;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Catalog;

[RemoteService(IsEnabled = false)]
public class UnitOfMeasureAppService : ArilyAppService, IUnitOfMeasureAppService
{
    private readonly IRepository<UnitOfMeasure, Guid> _unitOfMeasureRepository;

    public UnitOfMeasureAppService(IRepository<UnitOfMeasure, Guid> unitOfMeasureRepository)
    {
        _unitOfMeasureRepository = unitOfMeasureRepository;
    }

    public async Task<UnitOfMeasureDto> GetAsync(Guid id)
    {
        var unitOfMeasure = await _unitOfMeasureRepository.GetAsync(id);
        return ObjectMapper.Map<UnitOfMeasure, UnitOfMeasureDto>(unitOfMeasure);
    }

    public async Task<PagedResultDto<UnitOfMeasureDto>> GetListAsync(GetUnitOfMeasureListInput input)
    {
        var query = await _unitOfMeasureRepository.GetQueryableAsync();

        query = query
            .WhereIf(
                !input.Filter.IsNullOrWhiteSpace(),
                x => x.Name.Contains(input.Filter!) || x.Code.Contains(input.Filter!)
            )
            .WhereIf(input.Status.HasValue, x => x.Status == input.Status!.Value);

        var totalCount = query.Count();

        var unitOfMeasures = query
            .OrderBy(x => x.Name)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        return new PagedResultDto<UnitOfMeasureDto>(
            totalCount,
            ObjectMapper.Map<List<UnitOfMeasure>, List<UnitOfMeasureDto>>(unitOfMeasures)
        );
    }

    public async Task<UnitOfMeasureDto> CreateAsync(CreateUpdateUnitOfMeasureDto input)
    {
        var unitOfMeasure = new UnitOfMeasure(
            GuidGenerator.Create(),
            CurrentTenant.Id,
            input.Code,
            input.Name
        );

        unitOfMeasure.Status = input.Status;

        await _unitOfMeasureRepository.InsertAsync(unitOfMeasure);

        return ObjectMapper.Map<UnitOfMeasure, UnitOfMeasureDto>(unitOfMeasure);
    }

    public async Task<UnitOfMeasureDto> UpdateAsync(Guid id, CreateUpdateUnitOfMeasureDto input)
    {
        var unitOfMeasure = await _unitOfMeasureRepository.GetAsync(id);

        unitOfMeasure.Code = input.Code;
        unitOfMeasure.Name = input.Name;
        unitOfMeasure.Status = input.Status;

        await _unitOfMeasureRepository.UpdateAsync(unitOfMeasure);

        return ObjectMapper.Map<UnitOfMeasure, UnitOfMeasureDto>(unitOfMeasure);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _unitOfMeasureRepository.DeleteAsync(id);
    }
}

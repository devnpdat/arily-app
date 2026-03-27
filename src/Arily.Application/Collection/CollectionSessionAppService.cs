using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Collection.CollectionSessions;
using Arily.Enums;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Collection;

[RemoteService(IsEnabled = false)]
public class CollectionSessionAppService : ArilyAppService, ICollectionSessionAppService
{
    private readonly IRepository<CollectionSession, Guid> _sessionRepository;

    public CollectionSessionAppService(IRepository<CollectionSession, Guid> sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<CollectionSessionDto> GetAsync(Guid id)
    {
        var session = await _sessionRepository.GetAsync(id);
        return ObjectMapper.Map<CollectionSession, CollectionSessionDto>(session);
    }

    public async Task<PagedResultDto<CollectionSessionDto>> GetListAsync(GetCollectionSessionListInput input)
    {
        var query = await _sessionRepository.GetQueryableAsync();

        query = query
            .WhereIf(
                !input.Filter.IsNullOrWhiteSpace(),
                x => x.Name.Contains(input.Filter!) || x.Code.Contains(input.Filter!)
            )
            .WhereIf(input.Status.HasValue, x => x.Status == input.Status!.Value)
            .WhereIf(input.ProductCategoryId.HasValue, x => x.ProductCategoryId == input.ProductCategoryId!.Value)
            .WhereIf(!input.RegionProvinceCode.IsNullOrWhiteSpace(), x => x.RegionProvinceCode == input.RegionProvinceCode)
            .WhereIf(input.SessionDateFrom.HasValue, x => x.SessionDate >= input.SessionDateFrom!.Value)
            .WhereIf(input.SessionDateTo.HasValue, x => x.SessionDate <= input.SessionDateTo!.Value);

        var totalCount = query.Count();

        var sessions = query
            .OrderByDescending(x => x.SessionDate)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        return new PagedResultDto<CollectionSessionDto>(
            totalCount,
            ObjectMapper.Map<List<CollectionSession>, List<CollectionSessionDto>>(sessions)
        );
    }

    public async Task<CollectionSessionDto> CreateAsync(CreateUpdateCollectionSessionDto input)
    {
        var session = new CollectionSession(
            GuidGenerator.Create(),
            CurrentTenant.Id,
            input.Code,
            input.Name,
            input.SessionDate,
            input.ProductCategoryId
        );

        session.RegionProvinceCode = input.RegionProvinceCode;
        session.RegionDistrictCode = input.RegionDistrictCode;
        session.BuyerUserId = input.BuyerUserId;
        session.Note = input.Note;

        await _sessionRepository.InsertAsync(session);

        return ObjectMapper.Map<CollectionSession, CollectionSessionDto>(session);
    }

    public async Task<CollectionSessionDto> UpdateAsync(Guid id, CreateUpdateCollectionSessionDto input)
    {
        var session = await _sessionRepository.GetAsync(id);

        if (session.Status != CollectionSessionStatus.Draft)
            throw new UserFriendlyException("Chỉ có thể chỉnh sửa phiên ở trạng thái Draft.");

        session.Code = input.Code;
        session.Name = input.Name;
        session.SessionDate = input.SessionDate;
        session.ProductCategoryId = input.ProductCategoryId;
        session.RegionProvinceCode = input.RegionProvinceCode;
        session.RegionDistrictCode = input.RegionDistrictCode;
        session.BuyerUserId = input.BuyerUserId;
        session.Note = input.Note;

        await _sessionRepository.UpdateAsync(session);

        return ObjectMapper.Map<CollectionSession, CollectionSessionDto>(session);
    }

    public async Task DeleteAsync(Guid id)
    {
        var session = await _sessionRepository.GetAsync(id);

        if (session.Status == CollectionSessionStatus.Open)
            throw new UserFriendlyException("Không thể xóa phiên đang mở.");

        await _sessionRepository.DeleteAsync(id);
    }

    public async Task<CollectionSessionDto> OpenAsync(Guid id)
    {
        var session = await _sessionRepository.GetAsync(id);

        if (session.Status != CollectionSessionStatus.Draft)
            throw new UserFriendlyException("Chỉ có thể mở phiên ở trạng thái Draft.");

        session.Status = CollectionSessionStatus.Open;
        session.StartedAt = Clock.Now;

        await _sessionRepository.UpdateAsync(session);

        return ObjectMapper.Map<CollectionSession, CollectionSessionDto>(session);
    }

    public async Task<CollectionSessionDto> CloseAsync(Guid id)
    {
        var session = await _sessionRepository.GetAsync(id);

        if (session.Status != CollectionSessionStatus.Open)
            throw new UserFriendlyException("Chỉ có thể đóng phiên đang mở.");

        session.Status = CollectionSessionStatus.Closed;
        session.ClosedAt = Clock.Now;

        await _sessionRepository.UpdateAsync(session);

        return ObjectMapper.Map<CollectionSession, CollectionSessionDto>(session);
    }
}

using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Arily.Collection.CollectionSessions;

public interface ICollectionSessionAppService : IApplicationService
{
    Task<CollectionSessionDto> GetAsync(Guid id);
    Task<PagedResultDto<CollectionSessionDto>> GetListAsync(GetCollectionSessionListInput input);
    Task<CollectionSessionDto> CreateAsync(CreateUpdateCollectionSessionDto input);
    Task<CollectionSessionDto> UpdateAsync(Guid id, CreateUpdateCollectionSessionDto input);
    Task DeleteAsync(Guid id);
    Task<CollectionSessionDto> OpenAsync(Guid id);
    Task<CollectionSessionDto> CloseAsync(Guid id);
}

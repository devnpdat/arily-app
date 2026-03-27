using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Arily.Catalog.ProductGrades;

public interface IProductGradeAppService : IApplicationService
{
    Task<ProductGradeDto> GetAsync(Guid productId, Guid id);
    Task<ListResultDto<ProductGradeDto>> GetListAsync(Guid productId);
    Task<ProductGradeDto> CreateAsync(Guid productId, CreateUpdateProductGradeDto input);
    Task<ProductGradeDto> UpdateAsync(Guid productId, Guid id, CreateUpdateProductGradeDto input);
    Task DeleteAsync(Guid productId, Guid id);
}

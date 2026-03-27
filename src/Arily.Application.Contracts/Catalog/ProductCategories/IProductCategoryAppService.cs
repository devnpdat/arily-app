using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Arily.Catalog.ProductCategories;

public interface IProductCategoryAppService : IApplicationService
{
    Task<ProductCategoryDto> GetAsync(Guid id);
    Task<PagedResultDto<ProductCategoryDto>> GetListAsync(GetProductCategoryListInput input);
    Task<ProductCategoryDto> CreateAsync(CreateUpdateProductCategoryDto input);
    Task<ProductCategoryDto> UpdateAsync(Guid id, CreateUpdateProductCategoryDto input);
    Task DeleteAsync(Guid id);
}

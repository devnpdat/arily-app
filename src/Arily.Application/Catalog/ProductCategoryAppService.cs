using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Catalog.ProductCategories;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Catalog;

[RemoteService(IsEnabled = false)]
public class ProductCategoryAppService : ArilyAppService, IProductCategoryAppService
{
    private readonly IRepository<ProductCategory, Guid> _productCategoryRepository;

    public ProductCategoryAppService(IRepository<ProductCategory, Guid> productCategoryRepository)
    {
        _productCategoryRepository = productCategoryRepository;
    }

    public async Task<ProductCategoryDto> GetAsync(Guid id)
    {
        var productCategory = await _productCategoryRepository.GetAsync(id);
        return ObjectMapper.Map<ProductCategory, ProductCategoryDto>(productCategory);
    }

    public async Task<PagedResultDto<ProductCategoryDto>> GetListAsync(GetProductCategoryListInput input)
    {
        var query = await _productCategoryRepository.GetQueryableAsync();

        query = query
            .WhereIf(
                !input.Filter.IsNullOrWhiteSpace(),
                x => x.Name.Contains(input.Filter!) || x.Code.Contains(input.Filter!)
            )
            .WhereIf(input.Status.HasValue, x => x.Status == input.Status!.Value);

        var totalCount = query.Count();

        var productCategories = query
            .OrderBy(x => x.Name)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        return new PagedResultDto<ProductCategoryDto>(
            totalCount,
            ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryDto>>(productCategories)
        );
    }

    public async Task<ProductCategoryDto> CreateAsync(CreateUpdateProductCategoryDto input)
    {
        var productCategory = new ProductCategory(
            GuidGenerator.Create(),
            CurrentTenant.Id,
            input.Code,
            input.Name
        );

        productCategory.Description = input.Description;
        productCategory.Status = input.Status;

        await _productCategoryRepository.InsertAsync(productCategory);

        return ObjectMapper.Map<ProductCategory, ProductCategoryDto>(productCategory);
    }

    public async Task<ProductCategoryDto> UpdateAsync(Guid id, CreateUpdateProductCategoryDto input)
    {
        var productCategory = await _productCategoryRepository.GetAsync(id);

        productCategory.Code = input.Code;
        productCategory.Name = input.Name;
        productCategory.Description = input.Description;
        productCategory.Status = input.Status;

        await _productCategoryRepository.UpdateAsync(productCategory);

        return ObjectMapper.Map<ProductCategory, ProductCategoryDto>(productCategory);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _productCategoryRepository.DeleteAsync(id);
    }
}

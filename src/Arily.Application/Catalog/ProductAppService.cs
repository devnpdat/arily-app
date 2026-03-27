using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Catalog.Products;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Catalog;

[RemoteService(IsEnabled = false)]
public class ProductAppService : ArilyAppService, IProductAppService
{
    private readonly IRepository<Product, Guid> _productRepository;

    public ProductAppService(IRepository<Product, Guid> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> GetAsync(Guid id)
    {
        var product = await _productRepository.GetAsync(id);
        return ObjectMapper.Map<Product, ProductDto>(product);
    }

    public async Task<PagedResultDto<ProductDto>> GetListAsync(GetProductListInput input)
    {
        var query = await _productRepository.GetQueryableAsync();

        query = query
            .WhereIf(
                !input.Filter.IsNullOrWhiteSpace(),
                x => x.Name.Contains(input.Filter!) || x.Code.Contains(input.Filter!)
            )
            .WhereIf(input.Status.HasValue, x => x.Status == input.Status!.Value)
            .WhereIf(input.ProductCategoryId.HasValue, x => x.ProductCategoryId == input.ProductCategoryId!.Value);

        var totalCount = query.Count();

        var products = query
            .OrderBy(x => x.Name)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        return new PagedResultDto<ProductDto>(
            totalCount,
            ObjectMapper.Map<List<Product>, List<ProductDto>>(products)
        );
    }

    public async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
    {
        var product = new Product(
            GuidGenerator.Create(),
            CurrentTenant.Id,
            input.ProductCategoryId,
            input.UnitOfMeasureId,
            input.Code,
            input.Name
        );

        product.DefaultLossRate = input.DefaultLossRate;
        product.Note = input.Note;
        product.Status = input.Status;

        await _productRepository.InsertAsync(product);

        return ObjectMapper.Map<Product, ProductDto>(product);
    }

    public async Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input)
    {
        var product = await _productRepository.GetAsync(id);

        product.ProductCategoryId = input.ProductCategoryId;
        product.UnitOfMeasureId = input.UnitOfMeasureId;
        product.Code = input.Code;
        product.Name = input.Name;
        product.DefaultLossRate = input.DefaultLossRate;
        product.Note = input.Note;
        product.Status = input.Status;

        await _productRepository.UpdateAsync(product);

        return ObjectMapper.Map<Product, ProductDto>(product);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _productRepository.DeleteAsync(id);
    }
}

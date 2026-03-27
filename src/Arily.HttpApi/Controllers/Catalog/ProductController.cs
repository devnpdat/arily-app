using System;
using System.Threading.Tasks;
using Arily.Catalog.Products;
using Arily.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;

namespace Arily.Controllers.Catalog;

[ApiController]
[Route("api/app/products")]
[Authorize(ArilyPermissions.Products.Default)]
public class ProductController : ArilyController
{
    private readonly IProductAppService _productAppService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(
        IProductAppService productAppService,
        ILogger<ProductController> logger)
    {
        _productAppService = productAppService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    [Authorize(ArilyPermissions.Products.Default)]
    public async Task<ProductDto> GetAsync(Guid id)
    {
        _logger.LogInformation("GetProduct: id={Id}", id);
        return await _productAppService.GetAsync(id);
    }

    [HttpGet]
    [Authorize(ArilyPermissions.Products.Default)]
    public async Task<PagedResultDto<ProductDto>> GetListAsync([FromQuery] GetProductListInput input)
    {
        _logger.LogInformation("GetProductList: filter={Filter} status={Status} categoryId={CategoryId} skip={Skip} max={Max}",
            input.Filter, input.Status, input.ProductCategoryId, input.SkipCount, input.MaxResultCount);
        return await _productAppService.GetListAsync(input);
    }

    [HttpPost]
    [Authorize(ArilyPermissions.Products.Create)]
    public async Task<ProductDto> CreateAsync([FromBody] CreateUpdateProductDto input)
    {
        _logger.LogInformation("CreateProduct: code={Code} name={Name}", input.Code, input.Name);
        return await _productAppService.CreateAsync(input);
    }

    [HttpPut("{id:guid}")]
    [Authorize(ArilyPermissions.Products.Edit)]
    public async Task<ProductDto> UpdateAsync(Guid id, [FromBody] CreateUpdateProductDto input)
    {
        _logger.LogInformation("UpdateProduct: id={Id} code={Code} name={Name}", id, input.Code, input.Name);
        return await _productAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(ArilyPermissions.Products.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("DeleteProduct: id={Id}", id);
        await _productAppService.DeleteAsync(id);
    }
}

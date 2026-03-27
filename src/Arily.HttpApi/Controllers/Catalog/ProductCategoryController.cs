using System;
using System.Threading.Tasks;
using Arily.Catalog.ProductCategories;
using Arily.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;

namespace Arily.Controllers.Catalog;

[ApiController]
[Route("api/app/product-categories")]
[Authorize(ArilyPermissions.ProductCategories.Default)]
public class ProductCategoryController : ArilyController
{
    private readonly IProductCategoryAppService _productCategoryAppService;
    private readonly ILogger<ProductCategoryController> _logger;

    public ProductCategoryController(
        IProductCategoryAppService productCategoryAppService,
        ILogger<ProductCategoryController> logger)
    {
        _productCategoryAppService = productCategoryAppService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    [Authorize(ArilyPermissions.ProductCategories.Default)]
    public async Task<ProductCategoryDto> GetAsync(Guid id)
    {
        _logger.LogInformation("GetProductCategory: id={Id}", id);
        return await _productCategoryAppService.GetAsync(id);
    }

    [HttpGet]
    [Authorize(ArilyPermissions.ProductCategories.Default)]
    public async Task<PagedResultDto<ProductCategoryDto>> GetListAsync([FromQuery] GetProductCategoryListInput input)
    {
        _logger.LogInformation("GetProductCategoryList: filter={Filter} status={Status} skip={Skip} max={Max}",
            input.Filter, input.Status, input.SkipCount, input.MaxResultCount);
        return await _productCategoryAppService.GetListAsync(input);
    }

    [HttpPost]
    [Authorize(ArilyPermissions.ProductCategories.Create)]
    public async Task<ProductCategoryDto> CreateAsync([FromBody] CreateUpdateProductCategoryDto input)
    {
        _logger.LogInformation("CreateProductCategory: code={Code} name={Name}", input.Code, input.Name);
        return await _productCategoryAppService.CreateAsync(input);
    }

    [HttpPut("{id:guid}")]
    [Authorize(ArilyPermissions.ProductCategories.Edit)]
    public async Task<ProductCategoryDto> UpdateAsync(Guid id, [FromBody] CreateUpdateProductCategoryDto input)
    {
        _logger.LogInformation("UpdateProductCategory: id={Id} code={Code} name={Name}", id, input.Code, input.Name);
        return await _productCategoryAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(ArilyPermissions.ProductCategories.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("DeleteProductCategory: id={Id}", id);
        await _productCategoryAppService.DeleteAsync(id);
    }
}

using System;
using System.Threading.Tasks;
using Arily.Catalog.ProductGrades;
using Arily.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;

namespace Arily.Controllers.Catalog;

[ApiController]
[Route("api/app/products/{productId:guid}/grades")]
[Authorize(ArilyPermissions.ProductGrades.Default)]
public class ProductGradeController : ArilyController
{
    private readonly IProductGradeAppService _productGradeAppService;
    private readonly ILogger<ProductGradeController> _logger;

    public ProductGradeController(
        IProductGradeAppService productGradeAppService,
        ILogger<ProductGradeController> logger)
    {
        _productGradeAppService = productGradeAppService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    [Authorize(ArilyPermissions.ProductGrades.Default)]
    public async Task<ProductGradeDto> GetAsync(Guid productId, Guid id)
    {
        _logger.LogInformation("GetProductGrade: productId={ProductId} id={Id}", productId, id);
        return await _productGradeAppService.GetAsync(productId, id);
    }

    [HttpGet]
    [Authorize(ArilyPermissions.ProductGrades.Default)]
    public async Task<ListResultDto<ProductGradeDto>> GetListAsync(Guid productId)
    {
        _logger.LogInformation("GetProductGradeList: productId={ProductId}", productId);
        return await _productGradeAppService.GetListAsync(productId);
    }

    [HttpPost]
    [Authorize(ArilyPermissions.ProductGrades.Create)]
    public async Task<ProductGradeDto> CreateAsync(Guid productId, [FromBody] CreateUpdateProductGradeDto input)
    {
        _logger.LogInformation("CreateProductGrade: productId={ProductId} code={Code} name={Name}", productId, input.Code, input.Name);
        return await _productGradeAppService.CreateAsync(productId, input);
    }

    [HttpPut("{id:guid}")]
    [Authorize(ArilyPermissions.ProductGrades.Edit)]
    public async Task<ProductGradeDto> UpdateAsync(Guid productId, Guid id, [FromBody] CreateUpdateProductGradeDto input)
    {
        _logger.LogInformation("UpdateProductGrade: productId={ProductId} id={Id}", productId, id);
        return await _productGradeAppService.UpdateAsync(productId, id, input);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(ArilyPermissions.ProductGrades.Delete)]
    public async Task DeleteAsync(Guid productId, Guid id)
    {
        _logger.LogInformation("DeleteProductGrade: productId={ProductId} id={Id}", productId, id);
        await _productGradeAppService.DeleteAsync(productId, id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Catalog.ProductGrades;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Catalog;

[RemoteService(IsEnabled = false)]
public class ProductGradeAppService : ArilyAppService, IProductGradeAppService
{
    private readonly IRepository<ProductGrade, Guid> _productGradeRepository;

    public ProductGradeAppService(IRepository<ProductGrade, Guid> productGradeRepository)
    {
        _productGradeRepository = productGradeRepository;
    }

    public async Task<ProductGradeDto> GetAsync(Guid productId, Guid id)
    {
        var productGrade = await _productGradeRepository.GetAsync(x => x.Id == id && x.ProductId == productId);
        return ObjectMapper.Map<ProductGrade, ProductGradeDto>(productGrade);
    }

    public async Task<ListResultDto<ProductGradeDto>> GetListAsync(Guid productId)
    {
        var query = await _productGradeRepository.GetQueryableAsync();

        var productGrades = query
            .Where(x => x.ProductId == productId)
            .OrderBy(x => x.SortOrder)
            .ToList();

        return new ListResultDto<ProductGradeDto>(
            ObjectMapper.Map<List<ProductGrade>, List<ProductGradeDto>>(productGrades)
        );
    }

    public async Task<ProductGradeDto> CreateAsync(Guid productId, CreateUpdateProductGradeDto input)
    {
        var productGrade = new ProductGrade(
            GuidGenerator.Create(),
            CurrentTenant.Id,
            productId,
            input.Code,
            input.Name
        );

        productGrade.SortOrder = input.SortOrder;
        productGrade.Status = input.Status;

        await _productGradeRepository.InsertAsync(productGrade);

        return ObjectMapper.Map<ProductGrade, ProductGradeDto>(productGrade);
    }

    public async Task<ProductGradeDto> UpdateAsync(Guid productId, Guid id, CreateUpdateProductGradeDto input)
    {
        var productGrade = await _productGradeRepository.GetAsync(x => x.Id == id && x.ProductId == productId);

        productGrade.Code = input.Code;
        productGrade.Name = input.Name;
        productGrade.SortOrder = input.SortOrder;
        productGrade.Status = input.Status;

        await _productGradeRepository.UpdateAsync(productGrade);

        return ObjectMapper.Map<ProductGrade, ProductGradeDto>(productGrade);
    }

    public async Task DeleteAsync(Guid productId, Guid id)
    {
        await _productGradeRepository.DeleteAsync(x => x.Id == id && x.ProductId == productId);
    }
}

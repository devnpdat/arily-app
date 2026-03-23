using System;
using System.Linq;
using System.Threading.Tasks;
using Arily.Crm.Farmers;
using Arily.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Arily.Crm;

[Authorize(ArilyPermissions.Farmers.Default)]
public class FarmerAppService :
    CrudAppService<
        Farmer,
        FarmerDto,
        Guid,
        GetFarmerListInput,
        CreateUpdateFarmerDto>,
    IFarmerAppService
{
    public FarmerAppService(IRepository<Farmer, Guid> repository)
        : base(repository)
    {
        GetPolicyName = ArilyPermissions.Farmers.Default;
        GetListPolicyName = ArilyPermissions.Farmers.Default;
        CreatePolicyName = ArilyPermissions.Farmers.Create;
        UpdatePolicyName = ArilyPermissions.Farmers.Edit;
        DeletePolicyName = ArilyPermissions.Farmers.Delete;
    }

    protected override async Task<IQueryable<Farmer>> CreateFilteredQueryAsync(GetFarmerListInput input)
    {
        var query = await Repository.GetQueryableAsync();

        query = query
            .WhereIf(
                !input.Filter.IsNullOrWhiteSpace(),
                x => x.FullName.Contains(input.Filter!) ||
                     x.Code.Contains(input.Filter!) ||
                     x.PhoneNumber.Contains(input.Filter!) ||
                     (x.NickName != null && x.NickName.Contains(input.Filter!))
            )
            .WhereIf(input.Status.HasValue, x => x.Status == input.Status!.Value)
            .WhereIf(!input.ProvinceCode.IsNullOrWhiteSpace(), x => x.ProvinceCode == input.ProvinceCode);

        return query;
    }

    protected override Task<Farmer> MapToEntityAsync(CreateUpdateFarmerDto createInput)
    {
        var entity = new Farmer(
            GuidGenerator.Create(),
            CurrentTenant.Id,
            createInput.Code,
            createInput.FullName,
            createInput.PhoneNumber
        );

        entity.NickName = createInput.NickName;
        entity.ProvinceCode = createInput.ProvinceCode;
        entity.DistrictCode = createInput.DistrictCode;
        entity.WardCode = createInput.WardCode;
        entity.Address = createInput.Address;
        entity.Latitude = createInput.Latitude;
        entity.Longitude = createInput.Longitude;
        entity.Note = createInput.Note;
        entity.Status = createInput.Status;

        return Task.FromResult(entity);
    }

    protected override Task MapToEntityAsync(CreateUpdateFarmerDto updateInput, Farmer entity)
    {
        entity.Code = updateInput.Code;
        entity.FullName = updateInput.FullName;
        entity.NickName = updateInput.NickName;
        entity.PhoneNumber = updateInput.PhoneNumber;
        entity.ProvinceCode = updateInput.ProvinceCode;
        entity.DistrictCode = updateInput.DistrictCode;
        entity.WardCode = updateInput.WardCode;
        entity.Address = updateInput.Address;
        entity.Latitude = updateInput.Latitude;
        entity.Longitude = updateInput.Longitude;
        entity.Note = updateInput.Note;
        entity.Status = updateInput.Status;

        return Task.CompletedTask;
    }
}

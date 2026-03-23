using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Arily.Crm.Farmers;

public interface IFarmerAppService :
    ICrudAppService<
        FarmerDto,
        Guid,
        GetFarmerListInput,
        CreateUpdateFarmerDto>
{
}

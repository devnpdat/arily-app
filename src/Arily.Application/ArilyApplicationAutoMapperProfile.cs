using Arily.Crm;
using Arily.Crm.Farmers;
using AutoMapper;
using Volo.Abp.AutoMapper;

namespace Arily;

public class ArilyApplicationAutoMapperProfile : Profile
{
    public ArilyApplicationAutoMapperProfile()
    {
        CreateMap<Farmer, FarmerDto>();
        CreateMap<CreateUpdateFarmerDto, Farmer>()
            .IgnoreFullAuditedObjectProperties()
            .Ignore(x => x.Id)
            .Ignore(x => x.ReputationScore);
    }
}

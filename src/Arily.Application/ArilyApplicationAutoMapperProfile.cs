using Arily.Collection;
using Arily.Collection.CollectionSessions;
using Arily.Collection.PurchaseOrders;
using Arily.Crm;
using Arily.Crm.Customers;
using Arily.Crm.FarmerGardens;
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

        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateUpdateCustomerDto, Customer>()
            .IgnoreFullAuditedObjectProperties()
            .Ignore(x => x.Id);

        CreateMap<FarmerGarden, FarmerGardenDto>();
        CreateMap<CreateUpdateFarmerGardenDto, FarmerGarden>()
            .IgnoreFullAuditedObjectProperties()
            .Ignore(x => x.Id)
            .Ignore(x => x.FarmerId);

        CreateMap<CollectionSession, CollectionSessionDto>();
        CreateMap<PurchaseOrder, PurchaseOrderDto>();
    }
}

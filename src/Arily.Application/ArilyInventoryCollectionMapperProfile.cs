using Arily.Collection;
using Arily.Collection.PurchaseAdvances;
using Arily.Collection.WeighingTickets;
using Arily.Inventory;
using Arily.Inventory.InventoryLots;
using Arily.Inventory.Lots;
using Arily.Inventory.Warehouses;
using AutoMapper;

namespace Arily;

public class ArilyInventoryCollectionMapperProfile : Profile
{
    public ArilyInventoryCollectionMapperProfile()
    {
        CreateMap<WeighingTicket, WeighingTicketDto>();
        CreateMap<PurchaseAdvance, PurchaseAdvanceDto>();

        CreateMap<Warehouse, WarehouseDto>();
        CreateMap<Lot, LotDto>();
        CreateMap<InventoryLot, InventoryLotDto>();
    }
}

using Arily.Finance;
using Arily.Finance.FarmerDebtLedgers;
using Arily.Finance.LossAdjustmentOrders;
using Arily.Sales;
using Arily.Sales.CustomerDebtLedgers;
using Arily.Sales.SalesOrders;
using AutoMapper;

namespace Arily;

public class ArilyFinanceSalesMapperProfile : Profile
{
    public ArilyFinanceSalesMapperProfile()
    {
        CreateMap<LossAdjustmentOrder, LossAdjustmentOrderDto>();
        CreateMap<FarmerDebtLedger, FarmerDebtLedgerDto>();

        CreateMap<SalesOrder, SalesOrderDto>();
        CreateMap<CustomerDebtLedger, CustomerDebtLedgerDto>();
    }
}

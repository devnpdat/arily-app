using Arily.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Arily.Permissions;

public class ArilyPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var g = context.AddGroup(ArilyPermissions.GroupName, L("Permission:Arily"));

        // CRM
        AddCrud(g, ArilyPermissions.Farmers.Default, "Farmers");
        AddCrud(g, ArilyPermissions.FarmerGardens.Default, "FarmerGardens");
        AddCrud(g, ArilyPermissions.Customers.Default, "Customers");

        // Catalog
        AddCrud(g, ArilyPermissions.ProductCategories.Default, "ProductCategories");
        AddCrud(g, ArilyPermissions.UnitOfMeasures.Default, "UnitOfMeasures");
        AddCrud(g, ArilyPermissions.Products.Default, "Products");
        AddCrud(g, ArilyPermissions.ProductGrades.Default, "ProductGrades");

        // Collection
        AddCrud(g, ArilyPermissions.CollectionSessions.Default, "CollectionSessions");
        AddCrud(g, ArilyPermissions.PurchaseOrders.Default, "PurchaseOrders");

        var weighing = g.AddPermission(ArilyPermissions.WeighingTickets.Default, L("Permission:WeighingTickets"));
        weighing.AddChild(ArilyPermissions.WeighingTickets.Create, L("Permission:WeighingTickets.Create"));
        weighing.AddChild(ArilyPermissions.WeighingTickets.Delete, L("Permission:WeighingTickets.Delete"));

        var advances = g.AddPermission(ArilyPermissions.PurchaseAdvances.Default, L("Permission:PurchaseAdvances"));
        advances.AddChild(ArilyPermissions.PurchaseAdvances.Create, L("Permission:PurchaseAdvances.Create"));
        advances.AddChild(ArilyPermissions.PurchaseAdvances.Delete, L("Permission:PurchaseAdvances.Delete"));

        // Inventory
        AddCrud(g, ArilyPermissions.Warehouses.Default, "Warehouses");

        var lots = g.AddPermission(ArilyPermissions.Lots.Default, L("Permission:Lots"));
        lots.AddChild(ArilyPermissions.Lots.Create, L("Permission:Lots.Create"));
        lots.AddChild(ArilyPermissions.Lots.Delete, L("Permission:Lots.Delete"));

        // Finance
        var lossAdj = g.AddPermission(ArilyPermissions.LossAdjustmentOrders.Default, L("Permission:LossAdjustmentOrders"));
        lossAdj.AddChild(ArilyPermissions.LossAdjustmentOrders.Create, L("Permission:LossAdjustmentOrders.Create"));
        lossAdj.AddChild(ArilyPermissions.LossAdjustmentOrders.Delete, L("Permission:LossAdjustmentOrders.Delete"));

        g.AddPermission(ArilyPermissions.FarmerDebtLedgers.Default, L("Permission:FarmerDebtLedgers"));

        // Sales
        AddCrud(g, ArilyPermissions.SalesOrders.Default, "SalesOrders");
        g.AddPermission(ArilyPermissions.CustomerDebtLedgers.Default, L("Permission:CustomerDebtLedgers"));
    }

    private static void AddCrud(PermissionGroupDefinition g, string defaultPerm, string name)
    {
        var p = g.AddPermission(defaultPerm, L($"Permission:{name}"));
        p.AddChild(defaultPerm + ".Create", L($"Permission:{name}.Create"));
        p.AddChild(defaultPerm + ".Edit",   L($"Permission:{name}.Edit"));
        p.AddChild(defaultPerm + ".Delete", L($"Permission:{name}.Delete"));
    }

    private static LocalizableString L(string name)
        => LocalizableString.Create<ArilyResource>(name);
}

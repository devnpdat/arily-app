using Arily.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Arily.Permissions;

public class ArilyPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ArilyPermissions.GroupName, L("Permission:Arily"));

        var farmersPermission = myGroup.AddPermission(ArilyPermissions.Farmers.Default, L("Permission:Farmers"));
        farmersPermission.AddChild(ArilyPermissions.Farmers.Create, L("Permission:Farmers.Create"));
        farmersPermission.AddChild(ArilyPermissions.Farmers.Edit, L("Permission:Farmers.Edit"));
        farmersPermission.AddChild(ArilyPermissions.Farmers.Delete, L("Permission:Farmers.Delete"));

        var customersPermission = myGroup.AddPermission(ArilyPermissions.Customers.Default, L("Permission:Customers"));
        customersPermission.AddChild(ArilyPermissions.Customers.Create, L("Permission:Customers.Create"));
        customersPermission.AddChild(ArilyPermissions.Customers.Edit, L("Permission:Customers.Edit"));
        customersPermission.AddChild(ArilyPermissions.Customers.Delete, L("Permission:Customers.Delete"));

        var sessionsPermission = myGroup.AddPermission(ArilyPermissions.CollectionSessions.Default, L("Permission:CollectionSessions"));
        sessionsPermission.AddChild(ArilyPermissions.CollectionSessions.Create, L("Permission:CollectionSessions.Create"));
        sessionsPermission.AddChild(ArilyPermissions.CollectionSessions.Edit, L("Permission:CollectionSessions.Edit"));
        sessionsPermission.AddChild(ArilyPermissions.CollectionSessions.Delete, L("Permission:CollectionSessions.Delete"));

        var purchasePermission = myGroup.AddPermission(ArilyPermissions.PurchaseOrders.Default, L("Permission:PurchaseOrders"));
        purchasePermission.AddChild(ArilyPermissions.PurchaseOrders.Create, L("Permission:PurchaseOrders.Create"));
        purchasePermission.AddChild(ArilyPermissions.PurchaseOrders.Edit, L("Permission:PurchaseOrders.Edit"));
        purchasePermission.AddChild(ArilyPermissions.PurchaseOrders.Delete, L("Permission:PurchaseOrders.Delete"));

        var salesPermission = myGroup.AddPermission(ArilyPermissions.SalesOrders.Default, L("Permission:SalesOrders"));
        salesPermission.AddChild(ArilyPermissions.SalesOrders.Create, L("Permission:SalesOrders.Create"));
        salesPermission.AddChild(ArilyPermissions.SalesOrders.Edit, L("Permission:SalesOrders.Edit"));
        salesPermission.AddChild(ArilyPermissions.SalesOrders.Delete, L("Permission:SalesOrders.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ArilyResource>(name);
    }
}

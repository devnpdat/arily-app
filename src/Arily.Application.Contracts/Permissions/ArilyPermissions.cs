namespace Arily.Permissions;

public static class ArilyPermissions
{
    public const string GroupName = "Arily";

    // CRM
    public static class Farmers
    {
        public const string Default = GroupName + ".Farmers";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class FarmerGardens
    {
        public const string Default = GroupName + ".FarmerGardens";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class Customers
    {
        public const string Default = GroupName + ".Customers";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    // Catalog
    public static class ProductCategories
    {
        public const string Default = GroupName + ".ProductCategories";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class UnitOfMeasures
    {
        public const string Default = GroupName + ".UnitOfMeasures";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class Products
    {
        public const string Default = GroupName + ".Products";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class ProductGrades
    {
        public const string Default = GroupName + ".ProductGrades";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    // Collection
    public static class CollectionSessions
    {
        public const string Default = GroupName + ".CollectionSessions";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class PurchaseOrders
    {
        public const string Default = GroupName + ".PurchaseOrders";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class WeighingTickets
    {
        public const string Default = GroupName + ".WeighingTickets";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class PurchaseAdvances
    {
        public const string Default = GroupName + ".PurchaseAdvances";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    // Inventory
    public static class Warehouses
    {
        public const string Default = GroupName + ".Warehouses";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class Lots
    {
        public const string Default = GroupName + ".Lots";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    // Finance
    public static class LossAdjustmentOrders
    {
        public const string Default = GroupName + ".LossAdjustmentOrders";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class FarmerDebtLedgers
    {
        public const string Default = GroupName + ".FarmerDebtLedgers";
    }

    // Sales
    public static class SalesOrders
    {
        public const string Default = GroupName + ".SalesOrders";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class CustomerDebtLedgers
    {
        public const string Default = GroupName + ".CustomerDebtLedgers";
    }
}

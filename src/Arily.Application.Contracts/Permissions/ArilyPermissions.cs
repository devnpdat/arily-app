namespace Arily.Permissions;

public static class ArilyPermissions
{
    public const string GroupName = "Arily";

    public static class Farmers
    {
        public const string Default = GroupName + ".Farmers";
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

    public static class SalesOrders
    {
        public const string Default = GroupName + ".SalesOrders";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}

namespace BU.OnlineShop.OrderingService.API.Permissions
{
    public class OrderingServicePermissions
    {
        public const string GroupName = "OrderingService";

        public static class OrderManagement
        {
            public const string Permission = GroupName + ".OrderManagement";
        }

        public static class OrderManagementAdmin
        {
            public const string Permission = GroupName + ".Admin.OrderManagement";
            public const string Update = Permission + ".Update";
        }
    }
}

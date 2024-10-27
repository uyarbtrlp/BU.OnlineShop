namespace BU.OnlineShop.BasketService.API.Permissions
{
    public class BasketServicePermissions
    {
        public const string GroupName = "BasketService";

        public static class BasketManagement
        {
            public const string Permission = GroupName + ".BasketManagement";
            public const string Update = Permission + ".Update";
            public const string Checkout = Permission + ".Checkout";
        }
    }
}

namespace BU.OnlineShop.CatalogService.API.Permissions
{
    public static class CatalogServicePermissions
    {
        public const string GroupName = "CatalogService";

        public static class CategoryMaagement
        {
            public const string Permission = GroupName + ".CategoryManagement";
            public const string Create = Permission + ".Create";
            public const string Update = Permission + ".Update";
            public const string Delete = Permission + ".Delete";
        }
        public static class ProductManagement
        {
            public const string Permission = GroupName + ".ProductManagement";
            public const string Create = Permission + ".Create";
            public const string Update = Permission + ".Update";
            public const string Delete = Permission + ".Delete";
        }
    }
}

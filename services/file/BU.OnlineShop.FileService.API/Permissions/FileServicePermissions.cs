namespace BU.OnlineShop.FileService.API.Permissions
{
    public class FileServicePermissions
    {
        public const string GroupName = "FileService";

        public static class FileInformationManagement
        {
            public const string Permission = GroupName + ".FileInformationManagement";
            public const string Create = Permission + ".Create";
            public const string Update = Permission + ".Update";
            public const string Delete = Permission + ".Delete";
        }
    }
}

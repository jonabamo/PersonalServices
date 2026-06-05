using TaskAPI.Configurations;

public static class RolePermissionMatrix
{
    public static readonly Dictionary<string, List<string>> Roles = new()
    {
        { AppRoles.User, new List<string> { AppPermissions.ViewData } },
        { AppRoles.Admin, new List<string> { AppPermissions.ViewData, AppPermissions.CreateData } },
        { AppRoles.Manager, new List<string> { AppPermissions.ViewData, AppPermissions.CreateData, AppPermissions.EditData, AppPermissions.DeleteData } },
        { AppRoles.SuperAdmin, new List<string> { AppPermissions.ViewData, AppPermissions.CreateData, AppPermissions.EditData, AppPermissions.DeleteData } }
    };
}
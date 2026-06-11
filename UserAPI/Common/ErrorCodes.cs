namespace UserAPI.Common
{
    public static class ErrorCodes
    {
        public const string UserNotCreated = "USER_CREATION_ERROR";
        public const string UserNotUpdated = "USER_UPDATE_ERROR";
        public const string UserAlreadyExists = "USER_ALREADY_EXISTS";
        public const string UserNotFound = "USER_NOT_FOUND";
        public const string PermissionNotCreated = "PERMISSION_CREATION_ERROR";
        public const string PermissionNotFound = "PERMISSION_NOT_FOUND";
        public const string InvalidCredentials = "INVALID_CREDENTIALS";
        public const string Unauthorized = "UNAUTHORIZED";
    }
}
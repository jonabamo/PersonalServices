using UserAPI.Common;

namespace UserAPI.Application.DTOs
{
    public class GetPermisionResponse : BaseResponse
    {
        public int PermissionId { get; set; }
        public string Name { get; set; } = string.Empty;

        public GetPermisionResponse(bool success, int permissionId, string name, string message, int statusCode)
        {
            Success = success;
            PermissionId = permissionId;
            Name = name;
            Message = message;
            StatusCode = statusCode;
        }

        public static GetPermisionResponse ToResponse(int permissionId, string name) => new GetPermisionResponse(
            true,
            permissionId,
            name,
            "Permission Found",
            200
        );

        public static GetPermisionResponse ToNotFoundResponse() => new GetPermisionResponse(
            false, 
            0,
            string.Empty,
            ErrorCodes.PermissionNotFound, 
            404
        );
    }
}

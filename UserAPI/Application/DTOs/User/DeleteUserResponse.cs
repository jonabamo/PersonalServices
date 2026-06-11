namespace UserAPI.Application.DTOs
{
    public class DeleteUserResponse : BaseResponse
    {
        public DeleteUserResponse(bool success, string message)
        {
            Success = success;
            Message = message;
            StatusCode = StatusCodes.Status200OK;
        }

        public DeleteUserResponse(bool success, string message, int statusCode){
            Success = success;
            Message = message;
            StatusCode = statusCode;
        }

        public static DeleteUserResponse ToSuccessDeleteResponse() => new DeleteUserResponse(
            true, "User deleted successfully!"
        );

        public static DeleteUserResponse ToSuccessDeleteResponse(string name) => new DeleteUserResponse(
           true, "User deleted successfully! Name: " + name, 201
        );

        public static DeleteUserResponse ToSuccessDeleteAllResponse() => new DeleteUserResponse(
            true, "All users deleted successfully!", 201
        );

        public static DeleteUserResponse ToNotFoundDeleteResponse(Guid id) => new DeleteUserResponse(
            false, "User with id " + id + " not found!", 404
        );
    }
}

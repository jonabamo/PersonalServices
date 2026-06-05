namespace UserAPI.Application.DTOs.Responses
{
    public class DeleteUserResponse
    {
        public string Message { get; set; } = string.Empty;

        public DeleteUserResponse(string message)
        {
            Message = message;
        }

        public static DeleteUserResponse ToSuccessDeleteResponse() => new DeleteUserResponse(
            "User deleted successfully!"
        );

        public static DeleteUserResponse ToSuccessDeleteResponse(string name) => new DeleteUserResponse(
           "User deleted successfully! Name: " + name
        );

        public static DeleteUserResponse ToSuccessDeleteAllResponse() => new DeleteUserResponse(
            "All users deleted successfully!"
        );

        public static DeleteUserResponse ToNotFoundDeleteResponse(Guid id) => new DeleteUserResponse(
            "User with id " + id + " not found!"
        );
    }
}

namespace UserAPI.Application.DTOs.Responses;

public class CreateUserResponse
{
    public bool IsSuccess { get; set; }

    public int StatusCode { get; set; }

    public string Message { get; set; } = string.Empty;

    public CreateUserResponse(bool isSuccess, string message, int statusCode){
        IsSuccess = isSuccess;
        Message = message;
        StatusCode = statusCode;
    }

    public static CreateUserResponse ToSuccessResponse() => new CreateUserResponse(
        true, "User created successfully!", 201
    );

    public static CreateUserResponse ToErrorResponse() => new CreateUserResponse(
        false, "Error creating user(s)!", 400
    );

    public static CreateUserResponse ToSuccessResponse(string name) => new CreateUserResponse(
       true, "User created successfully! Name: " + name, 201
   );

    public static CreateUserResponse ToSuccessResponse(string name, string email) => new CreateUserResponse(
        true, "User created successfully! Name: " + name + ", Email: " + email, 201
    );

    public static CreateUserResponse ToTruncateResponse(string field) => new CreateUserResponse(
        false, "The field " + field + " is too long!", 400
    );

    public static CreateUserResponse ToDuplicateResponse() => new CreateUserResponse(
        false, "Duplicated Information!", 409
    );
}
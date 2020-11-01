using API.Utils.Models.ResponseModels;

namespace API.Utils.Interfaces
{
    public interface IApiResponseFormatter
    {
        ApiResponse<T> GetSuccessResponseBody<T>(T data);
        ApiErrorResponse GetErrorResponseBody(ApiErrorDetails errorDetails);
        ApiResponse<T> GetValidationErrorResponseBody<T>(T data);
        ApiResponse<T> GetSuccessCreateResponseBody<T>(T data);
        ApiResponse<T> GetSuccessUpdateResponseBody<T>(T data);
        ApiResponse<T> GetSuccessDeleteResponseBody<T>(T data);
        ApiResponse<T> GetResponseWithResultBody<T>(string status, T data);
    }
}

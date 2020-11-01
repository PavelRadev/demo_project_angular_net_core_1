using API.Utils.Interfaces;
using API.Utils.Models.ResponseModels;

namespace API.Classes
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseFormatter : IApiResponseFormatter
    {
        private string ResponseErrorStatus => "error";
        private string ResponseValidationErrorStatus => "validation_error";
        private string ResponseSuccessStatus => "success";
        private string ResponseDeletedStatus => "deleted";
        private string ResponseUpdatedStatus => "updated";
        private string ResponseCreatedStatus => "created";

        public ApiResponse<T> GetSuccessResponseBody<T>(T data)
        {
            return GetResponseWithResultBody(ResponseSuccessStatus, data);
        }

        public ApiErrorResponse GetErrorResponseBody(ApiErrorDetails errorDetails)
        {
            return new ApiErrorResponse() {ErrorDetails = errorDetails, Status = ResponseErrorStatus};
        }

        public ApiResponse<T> GetValidationErrorResponseBody<T>(T data)
        {
            return GetResponseWithResultBody(ResponseValidationErrorStatus, data);
        }

        public ApiResponse<T> GetSuccessCreateResponseBody<T>(T data)
        {
            return GetResponseWithResultBody(ResponseCreatedStatus, data);
        }

        public ApiResponse<T> GetSuccessUpdateResponseBody<T>(T data)
        {
            return GetResponseWithResultBody(ResponseUpdatedStatus, data);
        }

        public ApiResponse<T> GetSuccessDeleteResponseBody<T>(T data)
        {
            return GetResponseWithResultBody(ResponseDeletedStatus, data);
        }

        public ApiResponse<T> GetResponseWithResultBody<T>(string status, T data)
        {
            return new ApiResponse<T>() {Data = data, Status = status};
        }
    }
}

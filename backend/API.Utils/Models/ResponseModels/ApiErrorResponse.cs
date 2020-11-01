namespace API.Utils.Models.ResponseModels
{
    public class ApiErrorResponse
    {
        public string Status { get; set; }
        public ApiErrorDetails ErrorDetails { get; set; }
    }
}

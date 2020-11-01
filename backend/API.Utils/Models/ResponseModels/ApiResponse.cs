namespace API.Utils.Models.ResponseModels
{
    public class ApiResponse<T>
    {
        public string Status { get; set; }
        public T Data { get; set; }
    }
}

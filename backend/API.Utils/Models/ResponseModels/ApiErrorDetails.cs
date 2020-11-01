using System.Collections.Generic;

namespace API.Utils.Models.ResponseModels
{
    public class ApiErrorDetails
    {
        public string Message { get; set; }
        public Dictionary<string, string> FieldSpecificMessages { get; set; }
    }
}
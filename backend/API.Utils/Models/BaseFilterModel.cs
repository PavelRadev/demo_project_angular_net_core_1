using System.Collections.Generic;

namespace API.Utils.Models
{
    public class BaseFilterModel
    {
        public IEnumerable<string> IncludeProperties { get; set; }
    }
}
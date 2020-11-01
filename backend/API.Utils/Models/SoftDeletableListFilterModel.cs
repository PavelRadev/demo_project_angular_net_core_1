using System;
using System.Collections.Generic;

namespace API.Utils.Models
{
    public class SoftDeletableListFilterModel : BaseListFilterModel
    {
        public bool WithTrashed { get; set; } = false;
        public IEnumerable<Guid> DeletedBy { get; set; }
        public DateTime? DeletedFrom { get; set; }
        public DateTime? DeletedTo { get; set; }
    }
}
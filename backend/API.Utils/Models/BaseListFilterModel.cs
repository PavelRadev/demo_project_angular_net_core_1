using API.Utils.Interfaces;

namespace API.Utils.Models
{
    public class BaseListFilterModel : BaseFilterModel, IListFilterModel
    {
        public int? PageSize { get; set; } = 0;
        public int? Page { get; set; } = 0;
        public string OrderedBy { get; set; }
        public bool OrderReversed { get; set; }
    }
}
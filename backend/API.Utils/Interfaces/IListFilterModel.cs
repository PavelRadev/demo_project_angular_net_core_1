namespace API.Utils.Interfaces
{
    public interface IListFilterModel
    {
        int? PageSize { get; set; }
        int? Page { get; set; }
    }
}
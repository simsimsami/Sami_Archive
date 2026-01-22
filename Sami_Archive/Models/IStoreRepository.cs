namespace Sami_Archive.Models
{
    public interface IStoreRepository
    {
        IQueryable<Product> Products { get; }
    }
}

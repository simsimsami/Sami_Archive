namespace Sami_Archive.Models
{
    public class EFBookRepository : IBookRepository
    {
        private StoreDbContext context;

        public EFBookRepository(StoreDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Book> Books => context.Books;
    }
}

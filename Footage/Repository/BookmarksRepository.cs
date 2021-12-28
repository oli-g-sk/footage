namespace Footage.Repository
{
    using System.Threading.Tasks;
    using Footage.Model;

    public class BookmarksRepository : RepositoryBase
    {
        public async Task AddBookmark(Bookmark bookmark)
        {
            await Dao.Insert(bookmark);
        }
    }
}
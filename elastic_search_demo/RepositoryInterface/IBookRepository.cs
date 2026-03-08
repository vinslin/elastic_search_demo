using elastic_search_demo.Models;

namespace elastic_search_demo.RepositoryInterface
{
    public interface IBookRepository
    {
        Task<bool> IndexBook(Book book);
        Task<List<Book>> SearchBook(string keyword);
        Task<List<Book>> SearchRanking(string keyword);
        Task<List<Book>> FuzzySearch(string keyword);
        Task<List<Object>> HighlightSearch(string keyword);
        Task<bool> DeleteBook(string id);
        Task<List<Book>> GetAll();

     }
}

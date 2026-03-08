using elastic_search_demo.Models;

namespace elastic_search_demo.ServiceRepository
{
    public interface IBookService
    {
        Task<(Book, bool)> AddBook(BookRequest book);
        Task<List<Book>> Search(string keyword);

        Task<List<Book>> RankingSearch(string keyword);

        Task<List<Book>> FuzzySearch(string keyword);

        Task<List<object>> HighlightSearch(string keyword);

        Task<bool> DeleteBook(string id);

        Task<List<Book>> GetAll();
    }
}

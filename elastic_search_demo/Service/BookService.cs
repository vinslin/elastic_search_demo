using elastic_search_demo.Models;
using elastic_search_demo.RepositoryInterface;
using elastic_search_demo.ServiceRepository;

namespace elastic_search_demo.Service
{
    public class BookService: IBookService 
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<(Book,bool)> AddBook(BookRequest book) 
        {
            var books = new Book
            {
                Id = Guid.NewGuid().ToString(),
                BookName = book.BookName,   
                BookText = book.BookText
            };
            var result =await _bookRepository.IndexBook(books);
            return (books,result);
        }

        public async Task<List<Book>> Search(string keyword)
        {
            return await _bookRepository.SearchBook(keyword);
        }

        public async Task<List<Book>> RankingSearch(string keyword)
        {
            return await _bookRepository.SearchRanking(keyword);
        }

        public async Task<List<Book>> FuzzySearch(string keyword)
        {
            return await _bookRepository.FuzzySearch(keyword);
        }

        public async Task<List<object>> HighlightSearch(string keyword)
        {
            return await _bookRepository.HighlightSearch(keyword);
        }

        public async Task<bool> DeleteBook(string id)
        {
            return await _bookRepository.DeleteBook(id);
        }

        public async Task<List<Book>> UnifiedSearch(string keyword)
        {
            return await _bookRepository.UnifiedSearch(keyword);
        }

        public async Task<List<Book>> GetAll()
        {
            return await _bookRepository.GetAll();
        }

    }
}

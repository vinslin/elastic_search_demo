using elastic_search_demo.Models;
using elastic_search_demo.RepositoryInterface;
using Nest;

namespace elastic_search_demo.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly IElasticClient elasticClient;

        public BookRepository(IElasticClient elasticClient) 
        {
            this.elasticClient = elasticClient;
        }

        public async Task<bool> IndexBook(Book book) 
        {
            var response = await elasticClient.IndexDocumentAsync(book);

            return response.IsValid;
        }

        //exact match words  gandi != gandhi
        public async Task<List<Book>> SearchBook(string keyword)
        {
            var response = await elasticClient.SearchAsync<Book>(s => s.Query(q => q.MultiMatch(m => m.Fields(f => f.Field(p=>p.BookName).Field(p => p.BookText)).Query(keyword))));
            return response.Documents.ToList();
        }

        //get all documents
        public async Task<List<Book>> GetAll()
        {
            var response = await elasticClient.SearchAsync<Book>(s=>s.MatchAll());
            return response.Documents.ToList();
        }

        //delete document 
        public async Task<bool> DeleteBook(string id)
        {
            var response = await elasticClient.DeleteAsync<Book>(id);
            if (response.Result == Result.Deleted) 
            {
                return true;
            }
            return false;
        }

        //ranking search 
        public async Task<List<Book>> SearchRanking(string keyword) 
        {
            var response = await elasticClient.SearchAsync<Book>(s => s.Query(q => q.MultiMatch(m=>m.Fields(f => f.Field(p => p.BookName).Field(p=>p.BookText, boost: 3)).Query(keyword))));
            return response.Documents.ToList();        
        }

        //fuzzy search
        public async Task<List<Book>> FuzzySearch(string keyword)
        {
            var response = await elasticClient.SearchAsync<Book>(s => s.Query(q => q.Match(m => m.Field(f => f.BookText).Query(keyword).Fuzziness(Fuzziness.Auto))));
            return response.Documents.ToList();
        }

        //highlight search 
        public async Task<List<object>> HighlightSearch(string keyword)
        {
            var response = await elasticClient.SearchAsync<Book>(s => s
            .Query(q => q
                .Match(m => m
                    .Field(f => f.BookText)
                    .Query(keyword)
                )
            )
            .Highlight(h => h
                .Fields(
                    f => f
                        .Field(p => p.BookText)
                        .PreTags("<b>")
                        .PostTags("</b>")
                )
            )
        );

            var results = response.Hits.Select(hit => new
            {
                hit.Source.BookName,
                Highlight = hit.Highlight
            });
            return results.Cast<object>().ToList();

        }

        public async Task<List<Book>> UnifiedSearch(string keyword)
        {
            var response = await elasticClient.SearchAsync<Book>(s => s
                .Query(q => q
                    .MultiMatch(m => m
                        .Query(keyword)
                        .Fields(f => f
                            .Field(p => p.BookName, boost: 3) 
                            .Field(p => p.BookText)
                        )
                        .Type(TextQueryType.BestFields)
                        .Fuzziness(Fuzziness.Auto)
                    )
                )
            );

            return response.Documents.ToList();
        }


    }
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnalyzerDatabase.Models;

namespace AnalyzerDatabase.Interfaces
{
    public interface IScopusRestService
    {
        Task<List<SearchQueryModel>> GetSearchQuery(string query, CancellationTokenSource cts = null);
    }
}
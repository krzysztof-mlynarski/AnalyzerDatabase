using System.Threading;
using System.Threading.Tasks;
using AnalyzerDatabase.Models.ScienceDirect;
using AnalyzerDatabase.Models.Scopus;
using AnalyzerDatabase.Models.Springer;

namespace AnalyzerDatabase.Interfaces
{
    public interface IRestService
    {
    #region ScienceDirect methods

        Task<ScienceDirectSearchQuery> GetSearchQueryScienceDirect(string query, CancellationTokenSource cts = null);

    #endregion

    #region Scopus

        Task<ScopusSearchQuery> GetSearchQueryScopus(string query, CancellationTokenSource cts = null);

    #endregion

    #region Springer

        Task<SpringerSearchQuery> GetSearchQuerySpringer(string query, CancellationTokenSource cts = null);

    #endregion

    #region IEEE Xplore

        //Task<IeeeXploreSearchQuery> GetSearchQueryIeeeXplore(string query, CancellationTokenSource cts = null);

    #endregion

    }
}
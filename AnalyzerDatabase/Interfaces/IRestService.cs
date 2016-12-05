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
        Task<ScienceDirectSearchQuery> GetPreviousOrNextResultScienceDirect(string query, int start, CancellationTokenSource cts = null);

        #endregion

        #region Scopus

        Task<ScopusSearchQuery> GetSearchQueryScopus(string query, CancellationTokenSource cts = null);
        Task<ScopusSearchQuery> GetPreviousOrNextResultScopus(string query, int start, CancellationTokenSource cts = null);

        #endregion

        #region Springer

        Task<SpringerSearchQuery> GetSearchQuerySpringer(string query, CancellationTokenSource cts = null);
        Task<SpringerSearchQuery> GetPreviousOrNextResultSpringer(string query, int start, CancellationTokenSource cts = null);

        #endregion

        #region IEEE Xplore

        //Task<IeeeXploreSearchQuery> GetSearchQueryIeeeXplore(string query, CancellationTokenSource cts = null);

        #endregion

        #region Other methods

        void GetArticle(string doi, string title, CancellationTokenSource cts = null);
        void GetArticleDocx(string doi, string title, CancellationTokenSource cts = null);

        #endregion
    }
}
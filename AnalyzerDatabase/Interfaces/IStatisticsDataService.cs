using AnalyzerDatabase.Models;

namespace AnalyzerDatabase.Interfaces
{
    public interface IStatisticsDataService
    {
        void IncrementScienceDirect();
        void IncrementScopus();
        void IncrementSpringer();
        void IncrementWebOfScience();
        void IncrementIeeeXplore();
        void IncrementWileyOnlineLibrary();
        void IncrementDuplicate();
        void IncrementPublicationsDownload();
    }
}
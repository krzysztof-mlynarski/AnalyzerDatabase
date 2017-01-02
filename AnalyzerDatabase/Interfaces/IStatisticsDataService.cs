namespace AnalyzerDatabase.Interfaces
{
    public interface IStatisticsDataService
    {
        void IncrementScienceDirect();
        void IncrementScopus();
        void IncrementSpringer();
        void IncrementIeeeXplore();
        void IncrementDuplicate();
        void IncrementPublicationsDownload();
    }
}
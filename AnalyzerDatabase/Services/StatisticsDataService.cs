using System;
using AnalyzerDatabase.Interfaces;
using AnalyzerDatabase.Models;

namespace AnalyzerDatabase.Services
{
    public class StatisticsDataService : IStatisticsDataService
    {
        private AnalyzerDatabaseStatistics _analyzerDatabaseStatistics;

        private static StatisticsDataService _instance;

        public static StatisticsDataService Instance
        {
            get
            {
                return _instance ?? (_instance = new StatisticsDataService());
            }
        }

        public StatisticsDataService()
        {
        }

        public void SaveStatistics()
        {
            XmlSerialize<AnalyzerDatabaseStatistics>.Serialize(GetStatistics, LocalizationStatisticsService.AnalyzerDatabaseStatisticsPath);
        }

        public AnalyzerDatabaseStatistics GetStatistics
        {
            get
            {
                return _analyzerDatabaseStatistics ?? (_analyzerDatabaseStatistics = LoadStatistics());
            }
        }

        private AnalyzerDatabaseStatistics LoadStatistics()
        {
            try
            {
                AnalyzerDatabaseStatistics statistics;
                if (ZetaLongPaths.ZlpIOHelper.FileExists(LocalizationStatisticsService.AnalyzerDatabaseStatisticsPath))
                {
                    statistics = XmlSerialize<AnalyzerDatabaseStatistics>.Deserialize(
                        LocalizationStatisticsService.AnalyzerDatabaseStatisticsPath);
                }
                else
                {
                    statistics = EmptyStatistics();
                }

                return statistics;
            }
            catch (Exception)
            {
                return this.EmptyStatistics();
            }
        }

        private AnalyzerDatabaseStatistics EmptyStatistics()
        {
            AnalyzerDatabaseStatistics statistics = new AnalyzerDatabaseStatistics();
            XmlSerialize<AnalyzerDatabaseStatistics>.InitEmptyProperties(statistics);

            statistics.ScienceDirectCount = 0;
            statistics.ScopusCount = 0;
            statistics.SpringerCount = 0;
            statistics.WebOfScienceCount = 0;
            statistics.IeeeXploreCount = 0;
            statistics.WileyOnlineLibraryCount = 0;
            statistics.DuplicateCount = 0;
            statistics.PublicationsDownloadCount = 0;
            statistics.SumCount = 0;

            if (!ZetaLongPaths.ZlpIOHelper.DirectoryExists(LocalizationStatisticsService.ProgramDataApplicationDirectory))
            {
                ZetaLongPaths.ZlpIOHelper.CreateDirectory(LocalizationStatisticsService.ProgramDataApplicationDirectory);
            }

            XmlSerialize<AnalyzerDatabaseStatistics>.Serialize(statistics, LocalizationStatisticsService.AnalyzerDatabaseStatisticsPath);

            return statistics;
        }

        public void IncrementScienceDirect()
        {
            AnalyzerDatabaseStatistics statistics = GetStatistics;
            statistics.ScienceDirectCount++;
            SaveStatistics();
        }

        public void IncrementScopus()
        {
            AnalyzerDatabaseStatistics statistics = GetStatistics;
            statistics.ScopusCount++;
            SaveStatistics();
        }

        public void IncrementSpringer()
        {
            AnalyzerDatabaseStatistics statistics = GetStatistics;
            statistics.SpringerCount++;
            SaveStatistics();
        }

        public void IncrementWebOfScience()
        {
            AnalyzerDatabaseStatistics statistics = GetStatistics;
            statistics.WebOfScienceCount++;
            SaveStatistics();
        }

        public void IncrementIeeeXplore()
        {
            AnalyzerDatabaseStatistics statistics = GetStatistics;
            statistics.IeeeXploreCount++;
            SaveStatistics();
        }

        public void IncrementWileyOnlineLibrary()
        {
            AnalyzerDatabaseStatistics statistics = GetStatistics;
            statistics.WileyOnlineLibraryCount++;
            SaveStatistics();
        }

        public void IncrementDuplicate()
        {
            AnalyzerDatabaseStatistics statistics = GetStatistics;
            statistics.DuplicateCount++;
            SaveStatistics();
        }

        public void IncrementPublicationsDownload()
        {
            AnalyzerDatabaseStatistics statistics = GetStatistics;
            statistics.PublicationsDownloadCount++;
            SaveStatistics();
        }
    }
}
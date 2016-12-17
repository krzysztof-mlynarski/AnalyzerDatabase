using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using AnalyzerDatabase.Interfaces;
using AnalyzerDatabase.Models;
using LiveCharts.Helpers;

namespace AnalyzerDatabase.Services
{
    public class StatisticsDataService : IStatisticsDataService
    {
        private AnalyzerDatabaseStatistics _analyzerDatabaseStatistics;
        public readonly List<string> ListYear = new List<string>();
        public readonly List<int> ListYearAmount = new List<int>();

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
            statistics.IeeeXploreCount = 0;
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

        public void PublicationDateFromDatabasesLabels(ObservableCollection<ISearchResultsToDisplay> model)
        {
            string year = "";

            model.ForEach(x =>
            {
                year = x.Year;
                if (!ListYear.Any())
                {
                    ListYear.Add(year);
                    ListYearAmount.Add(1);
                }
                else
                {
                    bool found = false;

                    for (int i = 0; i < ListYear.Count; i++)
                    {
                        if (ListYear[i] == year)
                        {
                            ListYearAmount[i]++;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        ListYear.Add(year);
                        ListYearAmount.Add(1);
                    }
                }
            });
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

        public void IncrementIeeeXplore()
        {
            AnalyzerDatabaseStatistics statistics = GetStatistics;
            statistics.IeeeXploreCount++;
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
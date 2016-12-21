using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
                return EmptyStatistics();
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
            int year;
            List<int> listYear1 = new List<int>();
            List<int> listYearAmount1 = new List<int>();

            model.ForEach(x =>
            {
                year = Int32.Parse(x.Year);
                if (!listYear1.Any())
                {
                    listYear1.Add(year);
                    listYearAmount1.Add(1);
                }
                else
                {
                    bool found = false;

                    for (int i = 0; i < listYear1.Count; i++)
                    {
                        if (listYear1[i] == year)
                        {
                            listYearAmount1[i]++;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        listYear1.Add(year);
                        listYearAmount1.Add(1);
                    }
                }
            });

            List<int> indices = new List<int>(Enumerable.Range(0, listYear1.Count));

            indices.Sort((x, y) => listYear1[x] - listYear1[y]);

            for (int i = 0; i < indices.Count; i++)
            {
                var index = indices[i];
                var item1 = listYear1[index];
                var item2 = listYearAmount1[index];
                ListYear.Add(item1.ToString());
                ListYearAmount.Add(item2);
                //Console.WriteLine($"{listYear1[index]} = {listYearAmount1[index]}");
            }
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
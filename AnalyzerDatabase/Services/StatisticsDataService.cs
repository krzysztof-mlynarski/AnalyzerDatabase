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
        public readonly List<string> ListYearFull = new List<string>();
        public readonly List<int> ListYearAmount = new List<int>();
        public readonly List<int> ListYearAmountFull = new List<int>();
        public readonly List<string> ListMagazine = new List<string>();
        public readonly List<int> ListMagazineAmount = new List<int>();
        public readonly List<string> ListMagazineFull = new List<string>();
        public readonly List<int> ListMagazineAmountFull = new List<int>();

        private List<int> _listYear1 = new List<int>();
        private List<int> _listYearAmount1 = new List<int>();

        private List<int> _listYearFull = new List<int>();
        private List<int> _listYearAmountFull = new List<int>();

        public readonly List<string> ListAuthor = new List<string>();
        public readonly List<int> ListAuthorAmount = new List<int>();
        public readonly List<string> ListAuthorFull = new List<string>();
        public readonly List<int> ListAuthorAmountFull = new List<int>();

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
            XmlSerialize<AnalyzerDatabaseStatistics>.Serialize(_analyzerDatabaseStatistics, LocalizationStatisticsService.AnalyzerDatabaseStatisticsPath);
        }

        public AnalyzerDatabaseStatistics GetStatistics
        {
            get
            {
                return _analyzerDatabaseStatistics = LoadStatistics();
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

        public void PublicationDateFromDatabasesLabels(ObservableCollection<ISearchResultsToDisplay> model, bool isNewQuery, bool isNextPage, bool isPrevPage)
        {
            if (isNewQuery)
            {
                ListYear.Clear();
                ListYearAmount.Clear();
                
                _listYear1.Clear();
                _listYearAmount1.Clear();

                _listYearFull.Clear();
                _listYearAmountFull.Clear();

                ListYearFull.Clear();
                ListYearAmountFull.Clear();

                DataByYearHelper(model, true, false, false);
            }
            if(isNextPage)
            {
                ListYear.Clear();
                ListYearAmount.Clear();

                ListYearFull.Clear();
                ListYearAmountFull.Clear();

                _listYear1.Clear();
                _listYearAmount1.Clear();

                DataByYearHelper(model, false, true, false);
            }
            if (isPrevPage)
            {
                ListYear.Clear();
                ListYearAmount.Clear();

                _listYear1.Clear();
                _listYearAmount1.Clear();

                DataByYearHelper(model, false, false, true);
            }
        }

        public void PublicationByMagazines(ObservableCollection<ISearchResultsToDisplay> model, bool isNewQuery, bool isNextPage, bool isPrevPage)
        {
            if (isNewQuery)
            {
                ListMagazine.Clear();
                ListMagazineAmount.Clear();

                ListMagazineFull.Clear();
                ListMagazineAmountFull.Clear();

                DataByMagazineHelper(model, true, false, false);
            }
            if (isNextPage)
            {
                ListMagazine.Clear();
                ListMagazineAmount.Clear();

                DataByMagazineHelper(model, false, true, false);
            }
            if (isPrevPage)
            {
                ListMagazine.Clear();
                ListMagazineAmount.Clear();

                DataByMagazineHelper(model, false, false, true);
            }
        }

        public void PublicationsAuthors(ObservableCollection<ISearchResultsToDisplay> model, bool isNewQuery, bool isNextPage, bool isPrevPage)
        {
            if (isNewQuery)
            {
                ListAuthor.Clear();
                ListAuthorAmount.Clear();

                ListAuthorFull.Clear();
                ListAuthorAmountFull.Clear();

                DataByAuthorHelper(model, true, false, false);
            }
            if (isNextPage)
            {
                ListAuthor.Clear();
                ListAuthorAmount.Clear();

                DataByAuthorHelper(model, false, true, false);
            }
            if (isPrevPage)
            {
                ListAuthor.Clear();
                ListAuthorAmount.Clear();

                DataByAuthorHelper(model, false, false, true);
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

        #region Helpers
        private void DataByYearHelper(ObservableCollection<ISearchResultsToDisplay> model, bool isNewQuery, bool isNextPage, bool isPrevPage)
        {
            int year;

            model.ForEach(x =>
            {
                year = Int32.Parse(x.Year);
                if (!_listYear1.Any())
                {
                    _listYear1.Add(year);
                    _listYearAmount1.Add(1);

                    if (!_listYearFull.Any())
                    {
                        _listYearFull.Add(year);
                        _listYearAmountFull.Add(1);
                    }
                }
                else
                {
                    bool found = false;
                    bool found2 = false;

                    if (isNewQuery || isNextPage || isPrevPage)
                    {
                        for (int i = 0; i < _listYear1.Count; i++)
                        {
                            if (_listYear1[i] == year)
                            {
                                _listYearAmount1[i]++;
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            _listYear1.Add(year);
                            _listYearAmount1.Add(1);
                        }
                    }
                    if (isNewQuery || isNextPage)
                    {
                        for (int i = 0; i < _listYearFull.Count; i++)
                        {
                            if (_listYearFull[i] == year)
                            {
                                _listYearAmountFull[i]++;
                                found2 = true;
                                break;
                            }
                        }

                        if (!found2)
                        {
                            _listYearFull.Add(year);
                            _listYearAmountFull.Add(1);
                        }
                    }
                }
            });

            if (isNewQuery || isNextPage || isPrevPage)
            {
                List<int> indices = new List<int>(Enumerable.Range(0, _listYear1.Count));
                indices.Sort((x, y) => _listYear1[x] - _listYear1[y]);


                for (int i = 0; i < indices.Count; i++)
                {
                    var index = indices[i];
                    var item1 = _listYear1[index];
                    var item2 = _listYearAmount1[index];

                    ListYear.Add(item1.ToString());
                    ListYearAmount.Add(item2);
                    //Console.WriteLine($"{listYear1[index]} = {listYearAmount1[index]}");
                }

                if (isNewQuery || isNextPage)
                {
                    List<int> indices2 = new List<int>(Enumerable.Range(0, _listYearFull.Count));
                    indices2.Sort((x, y) => _listYearFull[x] - _listYearFull[y]);

                    for (int i = 0; i < indices2.Count; i++)
                    {
                        var index = indices2[i];
                        var item1 = _listYearFull[index];
                        var item2 = _listYearAmountFull[index];

                        ListYearFull.Add(item1.ToString());
                        ListYearAmountFull.Add(item2);
                    }
                }
            }
        }

        private void DataByMagazineHelper(ObservableCollection<ISearchResultsToDisplay> model, bool isNewQuery, bool isNextPage, bool isPrevPage)
        {
            string magazine;

            model.ForEach(x =>
            {
                magazine = x.PublicationName;
                if (!ListMagazine.Any())
                {
                    ListMagazine.Add(magazine);
                    ListMagazineAmount.Add(1);

                    if (!ListMagazineFull.Any())
                    {
                        ListMagazineFull.Add(magazine);
                        ListMagazineAmountFull.Add(1);
                    }
                }
                else
                {
                    bool found = false;
                    bool found2 = false;

                    if (isNewQuery || isNextPage || isPrevPage)
                    {
                        for (int i = 0; i < ListMagazine.Count; i++)
                        {
                            if (ListMagazine[i] == magazine)
                            {
                                ListMagazineAmount[i]++;
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            ListMagazine.Add(magazine);
                            ListMagazineAmount.Add(1);
                        }
                    }
                    if (isNewQuery || isNextPage)
                    {
                        for (int i = 0; i < ListMagazineFull.Count; i++)
                        {
                            if (ListMagazineFull[i] == magazine)
                            {
                                ListMagazineAmountFull[i]++;
                                found2 = true;
                                break;
                            }
                        }

                        if (!found2)
                        {
                            ListMagazineFull.Add(magazine);
                            ListMagazineAmountFull.Add(1);
                        }
                    }
                }
            });
        }

        private void DataByAuthorHelper(ObservableCollection<ISearchResultsToDisplay> model, bool isNewQuery, bool isNextPage, bool isPrevPage)
        {
            char[] stringSeparators = { ';' };
            string[] authors;
            string author;

            model.ForEach(x =>
            {
                x.GetCreator().ForEach(y =>
                {
                    authors = y.Split(stringSeparators, StringSplitOptions.None);
                    foreach (var item in authors)
                    {
                        author = item.TrimStart();

                        if (!ListAuthor.Any())
                        {
                            ListAuthor.Add(author);
                            ListAuthorAmount.Add(1);
                            if (!ListAuthorFull.Any())
                            {
                                ListAuthorFull.Add(author);
                                ListAuthorAmountFull.Add(1);
                            }
                        }
                        else
                        {
                            bool found = false;
                            bool found2 = false;

                            if (isNewQuery || isNextPage || isPrevPage)
                            {
                                for (int i = 0; i < ListAuthor.Count; i++)
                                {
                                    if (ListAuthor[i] == author)
                                    {
                                        ListAuthorAmount[i]++;
                                        found = true;
                                        break;
                                    }
                                }

                                if (!found)
                                {
                                    ListAuthor.Add(author);
                                    ListAuthorAmount.Add(1);
                                }
                            }

                            if (isNewQuery || isNextPage)
                            {
                                for (int i = 0; i < ListAuthorFull.Count; i++)
                                {
                                    if (ListAuthorFull[i] == author)
                                    {
                                        ListAuthorAmountFull[i]++;
                                        found2 = true;
                                        break;
                                    }
                                }

                                if (!found2)
                                {
                                    ListAuthorFull.Add(author);
                                    ListAuthorAmountFull.Add(1);
                                }
                            }
                        }
                    }
                });
            });
        }
        #endregion
    }
}
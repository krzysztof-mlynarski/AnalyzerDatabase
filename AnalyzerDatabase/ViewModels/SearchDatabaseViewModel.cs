using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;
using AnalyzerDatabase.Enums;
using AnalyzerDatabase.Interfaces;
using AnalyzerDatabase.Services;
using AnalyzerDatabase.View;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace AnalyzerDatabase.ViewModels
{
    public class SearchDatabaseViewModel : ExtendedViewModelBase
    {
        #region Variables

        private readonly IInternetConnectionService _internetConnectionService;
        private readonly IRestService _restService;
        private readonly IStatisticsDataService _statisticsDataService;

        public ICollectionView CollectionView { get; set; }
        public ICollectionView CollectionViewAll { get; set; }
        public ISearchResultsToDisplay DoiAndTitleAndAbstract { get; set; }

        private ObservableCollection<ISearchResultsToDisplay> _searchResultsToDisplay;
        private ObservableCollection<ISearchResultsToDisplay> _searchResultsToDisplayAll;
        private ObservableCollection<ITotalResultsToDisplay> _totalResultsToDisplay;
        //private ObservableCollection<ICreatorDataToDisplay> _creatorDataToDisplay;

        private List<string> _doiList = new List<string>();
        private readonly List<string> _doiListCopy = new List<string>();

        private RelayCommand _searchCommand;
        private RelayCommand _fullScreenDataGrid;
        private RelayCommand _nextResultPage;
        private RelayCommand _prevResultPage;
        private RelayCommand _downloadArticleToPdf;
        private RelayCommand _dataGridToCsvExport;
        private RelayCommand _fullResultsListCommand;
        private RelayCommand _openPageWithDoi;
        private RelayCommand _openPageWithPublication;

        private string _queryTextBox;
        private string _executionTime;
        private string _totalResults;

        private bool _isDataLoading;
        private bool _isDownloadFile;
        private bool _isGroupDescriptions = true;
        private bool _fullResultsList;
        private bool _dataGridResults = true;

        private bool _checkBoxScopus = true;
        private bool _checkBoxSpringer = true;
        private bool _checkBoxScienceDirect = true;
        private bool _checkBoxIeeeXplore = true;

        private bool _isDataEmpty = true;

        private static int _startUpScienceDirect;
        private static int _startUpScopus;
        private static int _startUpSpringer = 1;
        private static int _startUpIeeeXplore = 1;
        private static int _startDownScienceDirect = _startUpScienceDirect;
        private static int _startDownScopus = _startUpScopus;
        private static int _startDownSpringer = _startUpSpringer;
        private static int _startDownIeeeXplore = _startUpIeeeXplore;

        private MetroWindow _windowFullDataGrid;
        #endregion

        #region Constructors

        public SearchDatabaseViewModel(IRestService restService, IInternetConnectionService internetConnectionService, IStatisticsDataService statisticsDataService)
        {
            _restService = restService;
            _internetConnectionService = internetConnectionService;
            _statisticsDataService = statisticsDataService;
            SearchResultsToDisplay = new ObservableCollection<ISearchResultsToDisplay>();
            SearchResultsToDisplayAll = new ObservableCollection<ISearchResultsToDisplay>();
            TotalResultsToDisplay = new ObservableCollection<ITotalResultsToDisplay>();
            CollectionViewAll = CollectionViewSource.GetDefaultView(_searchResultsToDisplayAll);
            CollectionView = CollectionViewSource.GetDefaultView(_searchResultsToDisplay);
        }

        #endregion

        #region RelayCommand
        public RelayCommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new RelayCommand(Search));
            }
        }

        public RelayCommand OpenFullScreenDataGrid
        {
            get
            {
                return _fullScreenDataGrid ?? (_fullScreenDataGrid = new RelayCommand(() =>
                {
                    _windowFullDataGrid = new FullDataGridView();
                    _windowFullDataGrid.ShowDialog();
                }));
            }
        }

        public RelayCommand NextResultPage
        {
            get
            {
                return _nextResultPage ?? (_nextResultPage = new RelayCommand(NextPage));
            }
        }

        public RelayCommand PrevResultPage
        {
            get
            {
                return _prevResultPage ?? (_prevResultPage = new RelayCommand(PrevPage));
            }
        }

        public RelayCommand DownloadArticleToPdf
        {
            get
            {
                return _downloadArticleToPdf ?? (_downloadArticleToPdf = new RelayCommand(DownloadArticlePdf));
            }
        }

        public RelayCommand DataGridToCsvExport
        {
            get
            {
                return _dataGridToCsvExport ?? (_dataGridToCsvExport = new RelayCommand(() =>
                {
                    ExportDataToCsv.Instance.ExportDataGridToCsv(SearchResultsToDisplay, QueryTextBox);
                }));
            }
        }

        public RelayCommand FullResultsListCommand
        {
            get
            {
                return _fullResultsListCommand ?? (_fullResultsListCommand = new RelayCommand(() =>
                {
                    if (FullResultsList)
                    {
                        FullResultsList = false;
                        DataGridResults = true;
                    }
                    else
                    {
                        DataGridResults = false;
                        FullResultsList = true;
                    }

                }));
            }
        }

        public RelayCommand OpenPageWithDoi
        {
            get
            {
                return _openPageWithDoi ?? (_openPageWithDoi = new RelayCommand(() =>
                {

                    Process.Start("http://dx.doi.org/" + DoiAndTitleAndAbstract.Doi);
                }));
            }
        }

        public RelayCommand OpenPageWithPublication
        {
            get
            {
                return _openPageWithPublication ?? (_openPageWithPublication = new RelayCommand(() =>
                {
                    var source = DoiAndTitleAndAbstract.Source;

                    if (source == SourceDatabase.ScienceDirect)
                    {
                        Process.Start("http://www.sciencedirect.com/science/article/pii/" + DoiAndTitleAndAbstract.Pii);
                    }
                    else if (source == SourceDatabase.Scopus)
                    {
                        var scopusId = DoiAndTitleAndAbstract.Identifier;

                        var pattern = "[0-9]{11}";
                        Regex re1 = new Regex(pattern, RegexOptions.IgnoreCase);
                        Match m1 = re1.Match(scopusId);
                        scopusId = m1.ToString();

                        Process.Start("https://www.scopus.com/inward/record.uri?partnerID=HzOxMe3b&scp=" + scopusId + "&origin=inward");
                    }
                    else if (source == SourceDatabase.Springer)
                    {
                        Process.Start("http://dx.doi.org/" + DoiAndTitleAndAbstract.Doi);
                    }
                    else if(source == SourceDatabase.IeeeXplore)
                    {
                        Process.Start("http://ieeexplore.ieee.org/document/" + DoiAndTitleAndAbstract.Arnumber);
                    }
                }));
            }
        }
        #endregion

        #region Getters/Setters
        public string QueryTextBox
        {
            get
            {
                return _queryTextBox;
            }
            set
            {
                if (_queryTextBox != value)
                {
                    _queryTextBox = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string TotalResults
        {
            get
            {
                return _totalResults;
            }
            set
            {
                if (_totalResults != value)
                {
                    _totalResults = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsDataLoading
        {
            get
            {
                return _isDataLoading;
            }
            set
            {
                if (_isDataLoading != value)
                {
                    _isDataLoading = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsDownloadFile
        {
            get
            {
                return _isDownloadFile;
            }
            set
            {
                _isDownloadFile = value;
                RaisePropertyChanged();
            }
        }

        public bool CheckBoxScopus
        {
            get
            {
                return _checkBoxScopus;
            }
            set
            {
                _checkBoxScopus = value;
                RaisePropertyChanged();
            }
        }

        public bool CheckBoxSpringer
        {
            get
            {
                return _checkBoxSpringer;
            }
            set
            {
                _checkBoxSpringer = value;
                RaisePropertyChanged();
            }
        }

        public bool CheckBoxScienceDirect
        {
            get
            {
                return _checkBoxScienceDirect;
            }
            set
            {
                _checkBoxScienceDirect = value;
                RaisePropertyChanged();
            }
        }

        public bool CheckBoxIeeeXplore
        {
            get
            {
                return _checkBoxIeeeXplore;
            }
            set
            {
                _checkBoxIeeeXplore = value;
                RaisePropertyChanged();
            }
        }

        public bool IsGroupDescriptions
        {
            get
            {
                return _isGroupDescriptions;
            }
            set
            {
                _isGroupDescriptions = value;
                RaisePropertyChanged();
            }
        }

        public bool FullResultsList
        {
            get
            {
                return _fullResultsList;
            }
            set
            {
                _fullResultsList = value;
                RaisePropertyChanged();
            }
        }

        public bool DataGridResults
        {
            get
            {
                return _dataGridResults;
            }
            set
            {
                _dataGridResults = value;
                RaisePropertyChanged();
            }
        }

        public bool IsDataEmpty
        {
            get
            {
                return _isDataEmpty;
            }
            set
            {
                _isDataEmpty = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<ISearchResultsToDisplay> SearchResultsToDisplay
        {
            get
            {
                return _searchResultsToDisplay;
            }
            set
            {
                _searchResultsToDisplay = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<ISearchResultsToDisplay> SearchResultsToDisplayAll
        {
            get
            {
                return _searchResultsToDisplayAll;
            }
            set
            {
                _searchResultsToDisplayAll = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<ITotalResultsToDisplay> TotalResultsToDisplay
        {
            get
            {
                return _totalResultsToDisplay;
            }
            set
            {
                _totalResultsToDisplay = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Private methods
        private async void DownloadArticlePdf()
        {
            try
            {
                IsDownloadFile = true;
                await _restService.GetArticle(DoiAndTitleAndAbstract.Doi, DoiAndTitleAndAbstract.Title);
            }
            catch (Exception)
            {
                await _windowFullDataGrid.ShowMessageAsync(GetString("TitleDialogError"), GetString("ErrorDownloadPublication"));
            }
            finally
            {
                IsDownloadFile = false;
            }
        }

        private decimal DegreeOfCompliance(ISearchResultsToDisplay model)
        {
            int result = 0;
            int word = 0;
            decimal percentComplete = 0;

            var stringAbstract = model.Abstract;
            var stringTitle = model.Title;
            var stringPublicationName = model.PublicationName;

            char[] stringSeparators = 
            {
                ' ', ',', '.', '-', '/', '\\', '[', ']', '+', '*', '(', ')', ':', ';', '_', '^', '>', '<', '?', '!', '\"', '\'', '}', '{', '#', '$', '&', '@', '`', '~'
            };

            string[] queryWord = QueryTextBox.Split(stringSeparators, StringSplitOptions.None);
            string[] wordsAbstract = stringAbstract?.Split(stringSeparators, StringSplitOptions.None);
            string[] wordsTitle = stringTitle?.Split(stringSeparators, StringSplitOptions.None);
            string[] wordsPublicationName = stringPublicationName?.Split(stringSeparators, StringSplitOptions.None);


            if (wordsAbstract != null)
            {
                foreach (var source in wordsAbstract)
                {
                    word += 1;
                    foreach (var querys in queryWord)
                    {
                        if (String.Equals(source, querys, StringComparison.InvariantCultureIgnoreCase))
                            result += 1;
                    }
                }

                percentComplete = (decimal)(100 * result) / word;
            }

            if (wordsTitle != null)
            {
                foreach (var source in wordsTitle)
                {
                    word += 1;
                    foreach (var querys in queryWord)
                    {
                        if (String.Equals(source, querys, StringComparison.InvariantCultureIgnoreCase))
                            result += 1;
                    }
                }

                percentComplete += (decimal)(100 * result) / word;
            }

            if (wordsPublicationName != null)
            {
                foreach (var source in wordsPublicationName)
                {
                    word += 1;
                    foreach (var querys in queryWord)
                    {
                        if (String.Equals(source, querys, StringComparison.InvariantCultureIgnoreCase))
                            result += 1;
                    }
                }

                percentComplete += (decimal)(100 * result) / word;
            }

            return percentComplete;
        }

        private bool CompareDoi(ISearchResultsToDisplay model)
        {
            bool isDuplicate = false;

            if (!_doiList.Any())
            {
                _doiList.Add(model.Doi);
            }
            else
            {
                _doiListCopy.Add(model.Doi);

                foreach (var item1 in _doiList)
                {
                    foreach (var item2 in _doiListCopy)
                    {
                        if (item1 == item2)
                        {
                            isDuplicate = true;
                            _statisticsDataService.IncrementDuplicate();
                        }
                    }
                }
            }

            if (_doiListCopy != null)
            {
                foreach (var list in _doiListCopy)
                {
                    _doiList.Add(list);
                }

                _doiListCopy.Clear();
            }

            return isDuplicate;
        }

        private string RegexYear(ISearchResultsToDisplay model)
        {
            var regex = model.PublicationDate;
            var pattern = "[0-9]{4}";
            Regex re1 = new Regex(pattern, RegexOptions.IgnoreCase);
            Match m1 = re1.Match(regex);
            var year = m1.ToString();

            return year;
        }

        private async void NextPage()
        {
            bool isInternet = _internetConnectionService.CheckConnectedToInternet();
            bool isInternetVpn = _internetConnectionService.CheckConnectedToInternetVpn();

            if (isInternet || isInternetVpn)
            {
                SearchResultsToDisplay.Clear();
                _doiList.Clear();
                CollectionView?.GroupDescriptions.Clear();
                CollectionViewAll?.GroupDescriptions.Clear();

                if (!string.IsNullOrEmpty(QueryTextBox))
                {
                    try
                    {
                        IsDataLoading = true;

                        if (CheckBoxScienceDirect)
                        {
                            _startUpScienceDirect += 25;
                            _startDownScienceDirect += 25;

                            var obj = await _restService.GetPreviousOrNextResultScienceDirect(QueryTextBox, _startUpScienceDirect);
                            if (obj != null)
                            {
                                obj.SearchResults.Entry.ToList().ForEach(SearchHelper);
                            }
                            else
                            {
                                _startUpScienceDirect -= 25;
                            }
                        }

                        if (CheckBoxScopus)
                        {
                            _startUpScopus += 25;
                            _startDownScopus += 25;

                            var obj = await _restService.GetPreviousOrNextResultScopus(QueryTextBox, _startUpScopus);
                            if (obj != null)
                            {
                                obj.SearchResults.Entry.ToList().ForEach(SearchHelper);
                            }
                            else
                            {
                                _startUpScopus -= 25;
                            }

                        }

                        if (CheckBoxSpringer)
                        {
                            _startUpSpringer += 25;
                            _startDownSpringer += 25;

                            var obj = await _restService.GetPreviousOrNextResultSpringer(QueryTextBox, _startUpSpringer);
                            if (obj != null)
                            {
                                obj.Records.ToList().ForEach(element =>
                                {
                                    var x = CompareDoi(element);

                                    if (!x)
                                    {
                                        SearchResultsToDisplay.Add(element);
                                        SearchResultsToDisplay.Last().Year = RegexYear(SearchResultsToDisplay.Last());
                                        SearchResultsToDisplay.Last().IsDuplicate = false;
                                        SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(SearchResultsToDisplay.Last());
                                    }
                                });
                            }
                            else
                            {
                                _startUpSpringer -= 25;
                            }

                        }

                        if (CheckBoxIeeeXplore)
                        {
                            _startUpIeeeXplore += 25;
                            _startDownIeeeXplore += 25;

                            var obj = await _restService.GetPreviousOrNextResultIeeeXplore(QueryTextBox, _startUpIeeeXplore);
                            if (obj != null)
                            {
                                obj.Document.ToList().ForEach(element =>
                                {
                                    var x = CompareDoi(element);

                                    if (!x)
                                    {
                                        SearchResultsToDisplay.Add(element);
                                        SearchResultsToDisplay.Last().Year = element.PublicationDate;
                                        SearchResultsToDisplay.Last().IsDuplicate = false;
                                        SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(SearchResultsToDisplay.Last());
                                        SearchResultsToDisplay.Last().Source = SourceDatabase.IeeeXplore;
                                    }
                                });
                            }
                            else
                            {
                                _startUpIeeeXplore -= 25;
                            }

                        }

                        StatisticsDataService.Instance.PublicationDateFromDatabasesLabels(SearchResultsToDisplay, false, true, false);
                        StatisticsDataService.Instance.PublicationByMagazines(SearchResultsToDisplay, false, true, false);
                        StatisticsDataService.Instance.PublicationsAuthors(SearchResultsToDisplay, false, true, false);

                        CollectionView?.GroupDescriptions.Add(new PropertyGroupDescription("Source"));
                    }
                    finally
                    {
                        IsDataLoading = false;

                        if (SearchResultsToDisplay != null)
                        {
                            foreach (var item in SearchResultsToDisplay)
                            {
                                SearchResultsToDisplayAll.Add(item);
                            }

                            //CollectionViewAll?.GroupDescriptions.Add(new PropertyGroupDescription("Source"));
                        }
                    }
                }
            }
        }

        private async void PrevPage()
        {
            bool isInternet = _internetConnectionService.CheckConnectedToInternet();
            bool isInternetVpn = _internetConnectionService.CheckConnectedToInternetVpn();

            if (isInternet || isInternetVpn)
            {
                SearchResultsToDisplay.Clear();
                _doiList.Clear();
                CollectionView?.GroupDescriptions.Clear();

                if (!string.IsNullOrEmpty(QueryTextBox))
                {
                    try
                    {
                        IsDataLoading = true;

                        if (CheckBoxScienceDirect)
                        {
                            _startDownScienceDirect -= 25;
                            _startUpScienceDirect -= 25;

                            if (_startDownScienceDirect >= 0)
                            {
                                var obj = await _restService.GetPreviousOrNextResultScienceDirect(QueryTextBox, _startDownScienceDirect);
                                obj?.SearchResults.Entry.ToList().ForEach(SearchHelper);
                            }
                        }

                        if (CheckBoxScopus)
                        {
                            _startDownScopus -= 25;
                            _startUpScopus -= 25;

                            if (_startDownScopus >= 0)
                            {
                                var obj = await _restService.GetPreviousOrNextResultScopus(QueryTextBox, _startDownScopus);
                                obj?.SearchResults.Entry.ToList().ForEach(SearchHelper);
                            }
                        }

                        if (CheckBoxSpringer)
                        {
                            _startDownSpringer -= 25;
                            _startUpSpringer -= 25;

                            if (_startDownSpringer >= 1)
                            {
                                var obj = await _restService.GetPreviousOrNextResultSpringer(QueryTextBox, _startDownSpringer);
                                obj?.Records.ToList().ForEach(SearchHelper);
                            }
                        }

                        if (CheckBoxIeeeXplore)
                        {
                            _startDownIeeeXplore -= 25;
                            _startUpIeeeXplore -= 25;

                            if (_startDownIeeeXplore >= 1)
                            {
                                var obj = await _restService.GetPreviousOrNextResultIeeeXplore(QueryTextBox, _startDownIeeeXplore);
                                obj?.Document.ToList().ForEach(element =>
                                {
                                    SearchResultsToDisplay.Add(element);
                                    SearchHelper(null);
                                    SearchResultsToDisplay.Last().Source = SourceDatabase.IeeeXplore;
                                });
                            }
                        }

                        StatisticsDataService.Instance.PublicationDateFromDatabasesLabels(SearchResultsToDisplay, false, false, true);
                        StatisticsDataService.Instance.PublicationByMagazines(SearchResultsToDisplay, false, false, true);
                        StatisticsDataService.Instance.PublicationsAuthors(SearchResultsToDisplay, false, false, true);

                        CollectionView?.GroupDescriptions.Add(new PropertyGroupDescription("Source"));
                    }
                    finally
                    {
                        IsDataLoading = false;
                    }
                }
            }
        }

        private async void Search()
        {
            bool isInternet = _internetConnectionService.CheckConnectedToInternet();
            bool isInternetVpn = _internetConnectionService.CheckConnectedToInternetVpn();

            if (isInternet || isInternetVpn)
            {
                SearchResultsToDisplay.Clear();
                SearchResultsToDisplayAll.Clear();
                TotalResultsToDisplay.Clear();
                _doiList.Clear();
                CollectionView?.GroupDescriptions.Clear();
                CollectionViewAll?.GroupDescriptions.Clear();

                if (!string.IsNullOrEmpty(QueryTextBox))
                {
                    DateTime startTime = new DateTime();
                    try
                    {
                        startTime = DateTime.Now;
                        IsDataLoading = true;
                        IsDataEmpty = false;

                        #region ScienceDirect/Scopus/Springer/IeeeXplore

                        if (CheckBoxScienceDirect && CheckBoxScopus && CheckBoxSpringer && CheckBoxIeeeXplore)
                        {
                            var obj = await _restService.GetSearchQueryScienceDirect(QueryTextBox);
                            var obj1 = await _restService.GetSearchQueryScopus(QueryTextBox);
                            var obj2 = await _restService.GetSearchQuerySpringer(QueryTextBox);
                            var obj3 = await _restService.GetSearchQueryIeeeXplore(QueryTextBox);

                            obj?.SearchResults.Entry.ToList().ForEach(SearchHelper);

                            if (obj != null)
                                TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1?.SearchResults.Entry.ToList().ForEach(SearchHelper);

                            if (obj1 != null)
                                TotalResultsToDisplay.Add(obj1.SearchResults);

                            obj2?.Records.ToList().ForEach(SearchHelper);

                            obj2?.Result.ToList().ForEach(element =>
                            {
                                TotalResultsToDisplay.Add(element);
                            });

                            obj3?.Document.ToList().ForEach(element =>
                            {
                                SearchResultsToDisplay.Add(element);
                                SearchHelper(null);
                                SearchResultsToDisplay.Last().Source = SourceDatabase.IeeeXplore;
                            });

                            if (obj3 != null)
                                TotalResultsToDisplay.Add(obj3);

                            _statisticsDataService.IncrementScienceDirect();
                            _statisticsDataService.IncrementScopus();
                            _statisticsDataService.IncrementSpringer();
                            _statisticsDataService.IncrementIeeeXplore();
                        }
                        #endregion

                        #region ScienceDirect/Scopus/Springer
                        else if (CheckBoxScienceDirect && CheckBoxScopus && CheckBoxSpringer)
                        {
                            var obj = await _restService.GetSearchQueryScienceDirect(QueryTextBox);
                            var obj1 = await _restService.GetSearchQueryScopus(QueryTextBox);
                            var obj2 = await _restService.GetSearchQuerySpringer(QueryTextBox);

                            obj?.SearchResults.Entry.ToList().ForEach(SearchHelper);

                            if (obj != null)
                                TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1?.SearchResults.Entry.ToList().ForEach(SearchHelper);

                            if (obj1 != null)
                                TotalResultsToDisplay.Add(obj1.SearchResults);

                            obj2?.Records.ToList().ForEach(SearchHelper);

                            obj2?.Result.ToList().ForEach(element =>
                            {
                                TotalResultsToDisplay.Add(element);
                            });

                            _statisticsDataService.IncrementScienceDirect();
                            _statisticsDataService.IncrementScopus();
                            _statisticsDataService.IncrementSpringer();
                        }
                        #endregion

                        #region ScienceDirect/Scopus/IeeeXplore

                        else if (CheckBoxScienceDirect && CheckBoxScopus && CheckBoxIeeeXplore)
                        {
                            var obj = await _restService.GetSearchQueryScienceDirect(QueryTextBox);
                            var obj1 = await _restService.GetSearchQueryScopus(QueryTextBox);
                            var obj2 = await _restService.GetSearchQueryIeeeXplore(QueryTextBox);

                            obj?.SearchResults.Entry.ToList().ForEach(SearchHelper);

                            if (obj != null)
                                TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1?.SearchResults.Entry.ToList().ForEach(SearchHelper);

                            if (obj1 != null)
                                TotalResultsToDisplay.Add(obj1.SearchResults);

                            obj2?.Document.ToList().ForEach(element =>
                            {
                                SearchResultsToDisplay.Add(element);
                                SearchHelper(null);
                                SearchResultsToDisplay.Last().Source = SourceDatabase.IeeeXplore;
                            });

                            if (obj2 != null)
                                TotalResultsToDisplay.Add(obj2);

                            _statisticsDataService.IncrementScienceDirect();
                            _statisticsDataService.IncrementScopus();
                            _statisticsDataService.IncrementIeeeXplore();
                        }
                        #endregion

                        #region ScienceDirect/Springer/IeeeXplore

                        else if (CheckBoxScienceDirect && CheckBoxSpringer && CheckBoxIeeeXplore)
                        {
                            var obj = await _restService.GetSearchQueryScienceDirect(QueryTextBox);
                            var obj1 = await _restService.GetSearchQuerySpringer(QueryTextBox);
                            var obj2 = await _restService.GetSearchQueryIeeeXplore(QueryTextBox);

                            obj?.SearchResults.Entry.ToList().ForEach(SearchHelper);

                            if (obj != null)
                                TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1?.Records.ToList().ForEach(SearchHelper);

                            obj1?.Result.ToList().ForEach(element =>
                            {
                                TotalResultsToDisplay.Add(element);
                            });

                            obj2?.Document.ToList().ForEach(element =>
                            {
                                SearchResultsToDisplay.Add(element);
                                SearchHelper(null);
                                SearchResultsToDisplay.Last().Source = SourceDatabase.IeeeXplore;
                            });

                            if (obj2 != null)
                                TotalResultsToDisplay.Add(obj2);

                            _statisticsDataService.IncrementScienceDirect();
                            _statisticsDataService.IncrementSpringer();
                            _statisticsDataService.IncrementIeeeXplore();
                        }
                        #endregion

                        #region Scopus/Springer/IeeeXplore
                        else if (CheckBoxScopus && CheckBoxSpringer && CheckBoxIeeeXplore)
                        {
                            var obj = await _restService.GetSearchQueryScopus(QueryTextBox);
                            var obj1 = await _restService.GetSearchQuerySpringer(QueryTextBox);
                            var obj2 = await _restService.GetSearchQueryIeeeXplore(QueryTextBox);

                            obj?.SearchResults.Entry.ToList().ForEach(SearchHelper);

                            if (obj != null)
                                TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1?.Records.ToList().ForEach(SearchHelper);

                            obj1?.Result.ToList().ForEach(element =>
                            {
                                TotalResultsToDisplay.Add(element);
                            });

                            obj2?.Document.ToList().ForEach(element =>
                            {
                                SearchResultsToDisplay.Add(element);
                                SearchHelper(null);
                                SearchResultsToDisplay.Last().Source = SourceDatabase.IeeeXplore;
                            });

                            if (obj2 != null)
                                TotalResultsToDisplay.Add(obj2);

                            _statisticsDataService.IncrementScienceDirect();
                            _statisticsDataService.IncrementSpringer();
                            _statisticsDataService.IncrementIeeeXplore();
                        }
                        #endregion

                        #region ScienceDirect/Scopus

                        else if (CheckBoxScienceDirect && CheckBoxScopus)
                        {
                            var obj = await _restService.GetSearchQueryScienceDirect(QueryTextBox);
                            var obj1 = await _restService.GetSearchQueryScopus(QueryTextBox);

                            obj?.SearchResults.Entry.ToList().ForEach(SearchHelper);

                            if (obj != null)
                                TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1?.SearchResults.Entry.ToList().ForEach(SearchHelper);

                            if (obj1 != null)
                                TotalResultsToDisplay.Add(obj1.SearchResults);

                            _statisticsDataService.IncrementScienceDirect();
                            _statisticsDataService.IncrementScopus();
                        }
                        #endregion

                        #region ScienceDirect/Springer

                        else if (CheckBoxScienceDirect && CheckBoxSpringer)
                        {
                            var obj = await _restService.GetSearchQueryScienceDirect(QueryTextBox);
                            var obj1 = await _restService.GetSearchQuerySpringer(QueryTextBox);

                            obj?.SearchResults.Entry.ToList().ForEach(SearchHelper);

                            if (obj != null)
                                TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1?.Records.ToList().ForEach(SearchHelper);

                            obj1?.Result.ToList().ForEach(element =>
                            {
                                TotalResultsToDisplay.Add(element);
                            });

                            _statisticsDataService.IncrementScienceDirect();
                            _statisticsDataService.IncrementSpringer();
                        }
                        #endregion

                        #region ScienceDirect/IeeeXplore

                        else if (CheckBoxScienceDirect && CheckBoxIeeeXplore)
                        {
                            var obj = await _restService.GetSearchQueryScienceDirect(QueryTextBox);
                            var obj1 = await _restService.GetSearchQueryIeeeXplore(QueryTextBox);

                            obj?.SearchResults.Entry.ToList().ForEach(SearchHelper);

                            if (obj != null)
                                TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1?.Document.ToList().ForEach(element =>
                            {
                                SearchResultsToDisplay.Add(element);
                                SearchHelper(null);
                                SearchResultsToDisplay.Last().Source = SourceDatabase.IeeeXplore;
                            });

                            if (obj1 != null)
                                TotalResultsToDisplay.Add(obj1);

                            _statisticsDataService.IncrementScienceDirect();
                            _statisticsDataService.IncrementIeeeXplore();
                        }
                        #endregion

                        #region Scopus/Springer

                        else if (CheckBoxScopus && CheckBoxSpringer)
                        {
                            var obj = await _restService.GetSearchQueryScopus(QueryTextBox);
                            var obj1 = await _restService.GetSearchQuerySpringer(QueryTextBox);

                            obj?.SearchResults.Entry.ToList().ForEach(SearchHelper);

                            if (obj != null)
                                TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1?.Records.ToList().ForEach(SearchHelper);

                            obj1?.Result.ToList().ForEach(element =>
                            {
                                TotalResultsToDisplay.Add(element);
                            });

                            _statisticsDataService.IncrementScopus();
                            _statisticsDataService.IncrementSpringer();
                        }
                        #endregion

                        #region Scopus/IeeeXplore

                        else if (CheckBoxScopus && CheckBoxIeeeXplore)
                        {
                            var obj = await _restService.GetSearchQueryScopus(QueryTextBox);
                            var obj1 = await _restService.GetSearchQueryIeeeXplore(QueryTextBox);

                            obj?.SearchResults.Entry.ToList().ForEach(SearchHelper);

                            if (obj != null)
                                TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1?.Document.ToList().ForEach(element =>
                            {
                                SearchResultsToDisplay.Add(element);
                                SearchHelper(null);
                                SearchResultsToDisplay.Last().Source = SourceDatabase.IeeeXplore;
                            });

                            if (obj1 != null)
                                TotalResultsToDisplay.Add(obj1);

                            _statisticsDataService.IncrementScopus();
                            _statisticsDataService.IncrementIeeeXplore();
                        }
                        #endregion

                        #region Springer/IeeeXplore

                        else if (CheckBoxSpringer && CheckBoxIeeeXplore)
                        {
                            var obj = await _restService.GetSearchQuerySpringer(QueryTextBox);
                            var obj1 = await _restService.GetSearchQueryIeeeXplore(QueryTextBox);

                            obj?.Records.ToList().ForEach(SearchHelper);

                            obj?.Result.ToList().ForEach(element =>
                            {
                                TotalResultsToDisplay.Add(element);
                            });

                            obj1?.Document.ToList().ForEach(element =>
                            {
                                SearchResultsToDisplay.Add(element);
                                SearchHelper(null);
                                SearchResultsToDisplay.Last().Source = SourceDatabase.IeeeXplore;
                            });

                            if (obj1 != null)
                                TotalResultsToDisplay.Add(obj1);

                            _statisticsDataService.IncrementSpringer();
                            _statisticsDataService.IncrementIeeeXplore();
                        }
                        #endregion

                        #region ScienceDirect

                        else if (CheckBoxScienceDirect)
                        {
                            var obj = await _restService.GetSearchQueryScienceDirect(QueryTextBox);

                            obj?.SearchResults.Entry.ToList().ForEach(SearchHelper);

                            if (obj != null)
                                TotalResultsToDisplay.Add(obj.SearchResults);

                            _statisticsDataService.IncrementScienceDirect();
                        }
                        #endregion

                        #region Scopus

                        else if (CheckBoxScopus)
                        {
                            var obj = await _restService.GetSearchQueryScopus(QueryTextBox);

                            obj?.SearchResults.Entry.ToList().ForEach(SearchHelper);

                            if (obj != null)
                                TotalResultsToDisplay.Add(obj.SearchResults);

                            _statisticsDataService.IncrementScopus();
                        }
                        #endregion

                        #region Springer

                        else if (CheckBoxSpringer)
                        {
                            var obj = await _restService.GetSearchQuerySpringer(QueryTextBox);

                            obj?.Records.ToList().ForEach(SearchHelper);

                            obj?.Result.ToList().ForEach(e =>
                            {
                                TotalResultsToDisplay.Add(e);
                            });

                            _statisticsDataService.IncrementSpringer();
                        }
                        #endregion

                        #region IeeeXplore

                        else if (CheckBoxIeeeXplore)
                        {
                            var obj = await _restService.GetSearchQueryIeeeXplore(QueryTextBox);

                            obj?.Document.ToList().ForEach(element =>
                            {
                                SearchResultsToDisplay.Add(element);
                                SearchResultsToDisplay.Last().Year = element.PublicationDate;
                                SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(SearchResultsToDisplay.Last());
                                SearchResultsToDisplay.Last().PercentComplete =
                                    DegreeOfCompliance(SearchResultsToDisplay.Last());
                                SearchResultsToDisplay.Last().Source = SourceDatabase.IeeeXplore;
                            });

                            if (obj != null)
                                TotalResultsToDisplay.Add(obj);

                            _statisticsDataService.IncrementIeeeXplore();
                        }
                        else
                        {
                            ShowDialog(GetString("TitleDialogError"), GetString("NotSelectedDatabase"));
                        }

                        #endregion

                        StatisticsDataService.Instance.PublicationDateFromDatabasesLabels(SearchResultsToDisplay, true, false, false);
                        StatisticsDataService.Instance.PublicationByMagazines(SearchResultsToDisplay, true, false, false);
                        StatisticsDataService.Instance.PublicationsAuthors(SearchResultsToDisplay, true, false, false);

                        if (IsGroupDescriptions)
                        {
                            CollectionView?.GroupDescriptions.Add(new PropertyGroupDescription("Source"));
                        }
                    }
                    //catch
                    //{
                        
                    //}
                    finally
                    {
                        IsDataLoading = false;
                        
                        var stopTime = DateTime.Now;
                        TimeSpan executionTime = stopTime - startTime;
                        _executionTime = "(" + executionTime.TotalSeconds + GetString("Seconds") + ")";

                        var allTotalResults = 0;

                        TotalResultsToDisplay?.ToList().ForEach(e => allTotalResults += Int32.Parse(e.OpensearchTotalResults));

                        TotalResults = (GetString("AboutResult") + " " + allTotalResults + " " + GetString("Results") + " " + _executionTime);

                        if(SearchResultsToDisplay == null)
                            IsDataEmpty = true;

                        if (SearchResultsToDisplay != null)
                        {
                            foreach (var item in SearchResultsToDisplay)
                            {
                                SearchResultsToDisplayAll.Add(item);
                            }
                            //CollectionViewAll?.GroupDescriptions.Add(new PropertyGroupDescription("Source"));
                        }
                    }
                }
                else
                {
                    ShowDialog(GetString("TitleDialogError"), GetString("NotCriteria"));
                }
            }
            else
            {
                ShowDialog(GetString("TitleDialogError"), GetString("NoInternet"));
            }
        }

        #endregion

        #region Helper
        private void SearchHelper(ISearchResultsToDisplay element)
        {
            if (element != null)
            {
                SearchResultsToDisplay.Add(element);
                SearchResultsToDisplay.Last().Year = RegexYear(SearchResultsToDisplay.Last());
                SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(SearchResultsToDisplay.Last());
                SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(SearchResultsToDisplay.Last());
            }
            else
            {
                SearchResultsToDisplay.Last().Year = RegexYear(SearchResultsToDisplay.Last());
                SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(SearchResultsToDisplay.Last());
                SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(SearchResultsToDisplay.Last());
            }
        }
        #endregion
    }
}
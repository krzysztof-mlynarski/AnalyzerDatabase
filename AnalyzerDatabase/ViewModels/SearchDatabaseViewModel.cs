using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;
using AnalyzerDatabase.Enums;
using AnalyzerDatabase.Interfaces;
using AnalyzerDatabase.Models.ScienceDirect;
using AnalyzerDatabase.Services;
using AnalyzerDatabase.View;
using CsvHelper;
using GalaSoft.MvvmLight.Command;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace AnalyzerDatabase.ViewModels
{
    public class SearchDatabaseViewModel : ExtendedViewModelBase
    {
        #region Variables

        private readonly IInternetConnectionService _internetConnectionService;
        private readonly IRestService _restService;
        private readonly IStatisticsDataService _statisticsDataService;

        public ICollectionView CollectionView { get; set; }
        public ISearchResultsToDisplay DoiAndTitleAndAbstract { get; set; }

        private ObservableCollection<ISearchResultsToDisplay> _searchResultsToDisplay;
        private ObservableCollection<ITotalResultsToDisplay> _totalResultsToDisplay;

        private List<string> _doiList = new List<string>();
        private readonly List<string> _doiListCopy = new List<string>();

        private readonly string _currentPublicationSavingPath;

        private RelayCommand _searchCommand;
        private RelayCommand _fullScreenDataGrid;
        private RelayCommand _nextResultPage;
        private RelayCommand _prevResultPage;
        private RelayCommand _downloadArticleToPdf;
        private RelayCommand _downloadArticleToDocx;
        private RelayCommand _dataGridToCsvExport;

        private string _queryTextBox;
        private string _executionTime;
        private string _totalResults;

        private bool _isDataLoading;
        private bool _isDownloadFile = false;
        private bool _isGroupDescriptions = true;

        private bool _checkBoxScopus = true;
        private bool _checkBoxSpringer = true;
        private bool _checkBoxScienceDirect = true;
        private bool _checkBoxIeeeXplore = true;

        private static int _startUpScienceDirect = 0;
        private static int _startUpScopus = 0;
        private static int _startUpSpringer = 1;
        private static int _startUpIeeeXplore = 1;
        private static int _startDownScienceDirect = _startUpScienceDirect;
        private static int _startDownScopus = _startUpScopus;
        private static int _startDownSpringer = _startUpSpringer;
        private static int _startDownIeeeXplore = _startUpIeeeXplore;

        #endregion

        #region Constructors

        public SearchDatabaseViewModel(IRestService restService, IInternetConnectionService internetConnectionService, IStatisticsDataService statisticsDataService)
        {
            _restService = restService;
            _internetConnectionService = internetConnectionService;
            _statisticsDataService = statisticsDataService;
            _currentPublicationSavingPath = SettingsService.Instance.Settings.SavingPublicationPath;
            SearchResultsToDisplay = new ObservableCollection<ISearchResultsToDisplay>();
            TotalResultsToDisplay = new ObservableCollection<ITotalResultsToDisplay>();
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
                    var windowFullDataGrid = new FullDataGridView();
                    windowFullDataGrid.ShowDialog();
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

        public RelayCommand DownloadArticleToDocx
        {
            get
            {
                return _downloadArticleToDocx ?? (_downloadArticleToDocx = new RelayCommand(DownloadArticleDocx));
            }
        }

        public RelayCommand DataGridToCsvExport
        {
            get
            {
                return _dataGridToCsvExport ?? (_dataGridToCsvExport = new RelayCommand(ExportDataGridToCsv));
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
                    RaisePropertyChanged("QueryTextBox");
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

        public string ExecutionTime
        {
            get
            {
                return _executionTime;
            }
            set
            {
                if (_executionTime != value)
                {
                    _executionTime = value;
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
                if (_isDownloadFile != value)
                {
                    _isDownloadFile = value;
                    RaisePropertyChanged();
                }
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
                //if(_checkBoxScopus) test.Add("CheckBoxSc");
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

        public ObservableCollection<ISearchResultsToDisplay> SearchResultsToDisplay
        {
            get
            {
                return _searchResultsToDisplay;
            }
            set
            {
                _searchResultsToDisplay = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<ITotalResultsToDisplay> TotalResultsToDisplay
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
        private void DownloadArticlePdf()
        {
            try
            {
                IsDownloadFile = true;
                _restService.GetArticle(DoiAndTitleAndAbstract.Doi, DoiAndTitleAndAbstract.Title);
            }
            catch (Exception)
            {
                ShowDialog(GetString("TitleDialogError"), GetString("ErrorDownloadPublication"));
            }
            finally
            {
                IsDownloadFile = false;
            }
        }

        private void DownloadArticleDocx()
        {
            try
            {
                IsDataLoading = true;
                _restService.GetArticleDocx(DoiAndTitleAndAbstract.Doi, DoiAndTitleAndAbstract.Title);
            }
            catch (Exception)
            {
                ShowDialog(GetString("TitleDialogError"), GetString("ErrorDownloadPublication"));
            }
            finally
            {
                IsDataLoading = false;
            }
        }

        private void ExportDataGridToCsv()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = "DataGrid_" + QueryTextBox + "_" + DateTime.Today.DayOfWeek,
                InitialDirectory = _currentPublicationSavingPath
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var streamWriter = File.CreateText(saveFileDialog.FileName))
                {
                    var writer = new CsvWriter(streamWriter);
                    writer.Configuration.Delimiter = ";";
                    //writer.WriteRecords(SearchResultsToDisplay);

                    foreach (var item in SearchResultsToDisplay)
                    {
                        writer.WriteField(item.PercentComplete);
                        writer.WriteField(item.Creator);
                        writer.WriteField(item.Title);
                        writer.WriteField(item.Year);
                        writer.WriteField(item.Doi);
                        writer.WriteField(item.Abstract);
                        writer.NextRecord();
                    }
                }
            }

            //TODO: dialog pytajacy czy otworzyc plik
            System.Diagnostics.Process.Start(saveFileDialog.FileName);
        }

        //private void ImportCsvToDataGrid()
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog
        //    {
        //        Filter = "CSV (*.csv)|*.csv",
        //        InitialDirectory = _currentPublicationSavingPath,
        //        RestoreDirectory = true
        //    };

        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        string fileName = openFileDialog.FileName;
        //        using (var streamReader = File.OpenText(fileName))
        //        {
        //            var reader = new CsvReader(streamReader);

        //            Map(m => m.Entry).ConvertUsing(row =>
        //            {
        //                var oc = new ObservableCollection<ISearchResultsToDisplay>();
        //                var item = row.GetField<>(1);
        //                oc.Add(item);
        //            });
        //        }
        //    }
        //}

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
            string year = "";

            var regex = model.PublicationDate;
            var pattern = "[0-9]{4}";
            Regex re1 = new Regex(pattern, RegexOptions.IgnoreCase);
            Match m1 = re1.Match(regex);
            year = m1.ToString();

            return year;
        }

        private async void NextPage()
        {
            bool isInternet = _internetConnectionService.CheckConnectedToInternet();
            bool isInternetVpn = _internetConnectionService.CheckConnectedToInternetVpn();

            if (isInternet || isInternetVpn)
            {
                SearchResultsToDisplay.Clear();
                CollectionView?.GroupDescriptions.Clear();

                if (!string.IsNullOrEmpty(QueryTextBox))
                {
                    try
                    {
                        IsDataLoading = true;

                        if (CheckBoxScienceDirect)
                        {
                            _startUpScienceDirect += 25;
                            var obj = await _restService.GetPreviousOrNextResultScienceDirect(QueryTextBox, _startUpScienceDirect);
                            obj.SearchResults.Entry.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = RegexYear(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());

                            });
                        }

                        if (CheckBoxScopus)
                        {
                            _startUpScopus += 25;
                            var obj = await _restService.GetPreviousOrNextResultScopus(QueryTextBox, _startUpScopus);
                            obj.SearchResults.Entry.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = RegexYear(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                            });
                        }

                        if (CheckBoxSpringer)
                        {
                            _startUpSpringer += 1;
                            var obj = await _restService.GetPreviousOrNextResultSpringer(QueryTextBox, _startUpSpringer);
                            obj.Records.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = RegexYear(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());

                            });
                        }

                        if (CheckBoxIeeeXplore)
                        {
                            _startUpIeeeXplore += 1;
                            var obj = await _restService.GetPreviousOrNextResultIeeeXplore(QueryTextBox, _startUpIeeeXplore);
                            obj.Document.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = element.PublicationDate;
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().Source = SourceDatabase.IeeeXplore;
                            });
                        }

                        CollectionView?.GroupDescriptions.Add(new PropertyGroupDescription("Source"));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        IsDataLoading = false;
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
                CollectionView?.GroupDescriptions.Clear();

                if (!string.IsNullOrEmpty(QueryTextBox))
                {
                    try
                    {
                        IsDataLoading = true;

                        if (CheckBoxScienceDirect)
                        {
                            _startDownScienceDirect -= 25;
                            var obj = await _restService.GetPreviousOrNextResultScienceDirect(QueryTextBox, _startDownScienceDirect);
                            obj.SearchResults.Entry.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = RegexYear(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());

                            });
                        }

                        if (CheckBoxScopus)
                        {
                            _startDownScopus -= 25;
                            var obj = await _restService.GetPreviousOrNextResultScopus(QueryTextBox, _startDownScopus);
                            obj.SearchResults.Entry.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = RegexYear(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());

                            });
                        }

                        if (CheckBoxSpringer)
                        {
                            _startDownSpringer -= 1;
                            var obj = await _restService.GetPreviousOrNextResultSpringer(QueryTextBox, _startDownSpringer);
                            obj.Records.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = RegexYear(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());

                            });
                        }

                        if (CheckBoxIeeeXplore)
                        {
                            _startDownIeeeXplore -= 1;
                            var obj =
                                await _restService.GetPreviousOrNextResultIeeeXplore(QueryTextBox, _startDownIeeeXplore);
                            obj.Document.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = element.PublicationDate;
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().Source = SourceDatabase.IeeeXplore;
                            });
                        }

                        CollectionView?.GroupDescriptions.Add(new PropertyGroupDescription("Source"));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
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
                CollectionView?.GroupDescriptions.Clear();

                if (!string.IsNullOrEmpty(QueryTextBox))
                {
                    DateTime startTime = new DateTime();
                    try
                    {
                        startTime = DateTime.Now;
                        IsDataLoading = true;

                        #region ScienceDirect/Scopus/Springer/IeeeXplore
                        if (CheckBoxScienceDirect && CheckBoxScopus && CheckBoxSpringer && CheckBoxIeeeXplore)
                        {
                            var obj = await _restService.GetSearchQueryScienceDirect(QueryTextBox);
                            var obj1 = await _restService.GetSearchQueryScopus(QueryTextBox);
                            var obj2 = await _restService.GetSearchQuerySpringer(QueryTextBox);
                            var obj3 = await _restService.GetSearchQueryIeeeXplore(QueryTextBox);

                            obj.SearchResults.Entry.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = RegexYear(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                            });

                            this.TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1.SearchResults.Entry.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = RegexYear(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                            });

                            this.TotalResultsToDisplay.Add(obj1.SearchResults);

                            obj2.Records.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = RegexYear(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                            });

                            obj2.Result.ToList().ForEach(element =>
                            {
                                this.TotalResultsToDisplay.Add(element);
                            });

                            obj3.Document.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = RegexYear(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().Source = SourceDatabase.IeeeXplore;
                            });

                            //TODO: łączna ilość wyników
                            //this.TotalResultsToDisplay.Add(obj3.OpensearchTotalResults);

                            _statisticsDataService.IncrementScienceDirect();
                            _statisticsDataService.IncrementScopus();
                            _statisticsDataService.IncrementSpringer();
                            _statisticsDataService.IncrementIeeeXplore();
                        }
                        #endregion
                        #region ScienceDirect/Scopus
                        else if (CheckBoxScienceDirect && CheckBoxScopus)
                        {
                            var obj = await _restService.GetSearchQueryScienceDirect(QueryTextBox);
                            var obj1 = await _restService.GetSearchQueryScopus(QueryTextBox);

                            obj.SearchResults.Entry.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = RegexYear(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                            });

                            this.TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1.SearchResults.Entry.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = RegexYear(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                            });

                            this.TotalResultsToDisplay.Add(obj1.SearchResults);

                            _statisticsDataService.IncrementScienceDirect();
                            _statisticsDataService.IncrementScopus();
                        }
                        #endregion
                        #region ScienceDirect/Springer
                        else if (CheckBoxScienceDirect && CheckBoxSpringer)
                        {
                            var obj = await _restService.GetSearchQueryScienceDirect(QueryTextBox);
                            var obj1 = await _restService.GetSearchQuerySpringer(QueryTextBox);

                            obj.SearchResults.Entry.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = RegexYear(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                            });

                            this.TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1.Records.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = RegexYear(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                            });

                            obj1.Result.ToList().ForEach(element =>
                            {
                                this.TotalResultsToDisplay.Add(element);
                            });

                            _statisticsDataService.IncrementScienceDirect();
                            _statisticsDataService.IncrementSpringer();
                        }
                        #endregion
                        #region Scopus/Springer
                        else if (CheckBoxScopus && CheckBoxSpringer)
                        {
                            var obj = await _restService.GetSearchQueryScopus(QueryTextBox);
                            var obj1 = await _restService.GetSearchQuerySpringer(QueryTextBox);

                            obj.SearchResults.Entry.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = RegexYear(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                            });

                            this.TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1.Records.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = RegexYear(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                            });

                            obj1.Result.ToList().ForEach(element =>
                            {
                                this.TotalResultsToDisplay.Add(element);
                            });

                            _statisticsDataService.IncrementScopus();
                            _statisticsDataService.IncrementSpringer();
                        }
                        #endregion
                        #region ScienceDirect
                        else if (CheckBoxScienceDirect)
                        {
                            var obj = await _restService.GetSearchQueryScienceDirect(QueryTextBox);

                            obj.SearchResults.Entry.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = RegexYear(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                            });

                            this.TotalResultsToDisplay.Add(obj.SearchResults);

                            _statisticsDataService.IncrementScienceDirect();
                        }
                        #endregion
                        #region Scopus
                        else if (CheckBoxScopus)
                        {
                            var obj = await _restService.GetSearchQueryScopus(QueryTextBox);

                            obj.SearchResults.Entry.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = RegexYear(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                            });

                            this.TotalResultsToDisplay.Add(obj.SearchResults);

                            _statisticsDataService.IncrementScopus();
                        }
                        #endregion
                        #region Springer
                        else if (CheckBoxSpringer)
                        {
                            var obj = await _restService.GetSearchQuerySpringer(QueryTextBox);

                            obj.Records.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = RegexYear(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                            });

                            obj.Result.ToList().ForEach(e =>
                            {
                                this.TotalResultsToDisplay.Add(e);
                            });

                            _statisticsDataService.IncrementSpringer();
                        }
                        #endregion
                        #region IeeeXplore
                        else if (CheckBoxIeeeXplore)
                        {
                            var obj = await _restService.GetSearchQueryIeeeXplore(QueryTextBox);

                            obj.Document.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().Year = element.PublicationDate;
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().Source = SourceDatabase.IeeeXplore;
                            });

                            //TODO: łączna ilość wyników

                            _statisticsDataService.IncrementIeeeXplore();
                        }
                        else
                        {
                            ShowDialog(GetString("TitleDialogError"), GetString("NotSelectedDatabase"));
                        }
                        #endregion

                        StatisticsDataService.Instance.PublicationDateFromDatabasesLabels(this.SearchResultsToDisplay);

                        if (IsGroupDescriptions)
                        {
                            CollectionView?.GroupDescriptions.Add(new PropertyGroupDescription("Source"));
                        }
                    }
                    finally
                    {
                        IsDataLoading = false;

                        var stopTime = DateTime.Now;
                        TimeSpan executionTime = stopTime - startTime;
                        ExecutionTime = "(" + executionTime.TotalSeconds + GetString("Seconds") + ")";

                        var allTotalResults = 0;
                        TotalResultsToDisplay.ToList().ForEach(e => allTotalResults += Int32.Parse(e.OpensearchTotalResults));

                        TotalResults = (GetString("AboutResult") + " " + allTotalResults + " " + GetString("Results") + " " + ExecutionTime);
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
    }
}
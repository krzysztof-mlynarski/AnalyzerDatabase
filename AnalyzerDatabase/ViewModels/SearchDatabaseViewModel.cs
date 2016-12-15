using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Forms;
using AnalyzerDatabase.Interfaces;
using AnalyzerDatabase.Messages;
using AnalyzerDatabase.Models;
using AnalyzerDatabase.Models.ScienceDirect;
using AnalyzerDatabase.View;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LiveCharts.Helpers;
using KBCsv;
using DataGrid = System.Windows.Controls.DataGrid;
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
        private string _scienceDirectTotal;
        private string _scopusTotal;
        private string _springerTotal;

        private bool _isDataLoading;
        private bool _isDownloadFile = false;

        private bool _checkBoxScopus = true;
        private bool _checkBoxSpringer = true;
        private bool _checkBoxWiley /*= true*/;
        private bool _checkBoxScienceDirect = true;
        private bool _checkBoxWebOfScience /*= true*/;
        private bool _checkBoxIeeeXplore /*= true*/;

        private static int _startUpScienceDirect = 0;
        private static int _startUpScopus = 0;
        private static int _startUpSpringer = 1;
        private static int _startDownScienceDirect = _startUpScienceDirect;
        private static int _startDownScopus = _startUpScopus;
        private static int _startDownSpringer = _startUpSpringer;

        #endregion

        #region Constructors

        public SearchDatabaseViewModel(IRestService restService, IInternetConnectionService internetConnectionService, IStatisticsDataService statisticsDataService)
        {
            _restService = restService;
            _internetConnectionService = internetConnectionService;
            _statisticsDataService = statisticsDataService;
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

        public string ScienceDirectTotal
        {
            get
            {
                return _scienceDirectTotal;
            }
            set
            {
                if (_scienceDirectTotal != value)
                {
                    _scienceDirectTotal = value;
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

        public bool CheckBoxWiley
        {
            get
            {
                return _checkBoxWiley;
            }
            set
            {
                _checkBoxWiley = value;
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

        public bool CheckBoxWebOfScience
        {
            get
            {
                return _checkBoxWebOfScience;
            }
            set
            {
                _checkBoxWebOfScience = value;
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
            //var csv = new CsvWriter();
            //csv.WriteRecords(records);
            WriteDataTableToCSV();
        }

        private static void WriteDataTableToCSV()
        {
            #region WriteDataTableToCSV

            //var table = new DataTable();
            //table.Columns.Add("Name");
            //table.Columns.Add("Age");
            //table.Rows.Add("Kent", 33);
            //table.Rows.Add("Belinda", 34);
            //table.Rows.Add("Tempany", 8);
            //table.Rows.Add("Xak", 0);

            //using (var stringWriter = new StringWriter())
            //{
            //    using (var writer = new CsvWriter(stringWriter))
            //    {
            //        using (var streamWriter = new StreamWriter("Output.csv"))
            //        {
            //            table.WriteCsv(writer);
            //        }

            //    }

            //}


            using (var streamWriter = new StreamWriter("TESTOWY.csv"))
            using (var writer = new CsvWriter(streamWriter))
            {
                writer.ForceDelimit = true;

                writer.WriteRecord("Name", "Age");
                writer.WriteRecord("Kent", "33");
                writer.WriteRecord("Belinda", "34");
                writer.WriteRecord("Tempany", "8");

                Console.WriteLine("{0} records written", writer.RecordNumber);
            }

            #endregion
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
                            });
                        }

                        if (CheckBoxScopus)
                        {
                            _startUpScopus += 25;
                            var obj = await _restService.GetPreviousOrNextResultScopus(QueryTextBox, _startUpScopus);
                            obj.SearchResults.Entry.ToList().ForEach(e =>
                            {
                                this.SearchResultsToDisplay.Add(e);
                            });
                        }

                        if (CheckBoxSpringer)
                        {
                            _startUpSpringer += 1;
                            var obj = await _restService.GetPreviousOrNextResultSpringer(QueryTextBox, _startUpSpringer);
                            obj.Records.ToList().ForEach(e =>
                            {
                                this.SearchResultsToDisplay.Add(e);
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
                            });
                        }

                        if (CheckBoxScopus)
                        {
                            _startDownScopus -= 25;
                            var obj = await _restService.GetPreviousOrNextResultScopus(QueryTextBox, _startDownScopus);
                            obj.SearchResults.Entry.ToList().ForEach(e =>
                            {
                                this.SearchResultsToDisplay.Add(e);
                            });
                        }

                        if (CheckBoxSpringer)
                        {
                            _startDownSpringer -= 1;
                            var obj = await _restService.GetPreviousOrNextResultSpringer(QueryTextBox, _startDownSpringer);
                            obj.Records.ToList().ForEach(e =>
                            {
                                this.SearchResultsToDisplay.Add(e);
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

                        #region ScienceDirect/Scopus/Springer
                        if (CheckBoxScienceDirect && CheckBoxScopus && CheckBoxSpringer)
                        {
                            var obj = await _restService.GetSearchQueryScienceDirect(QueryTextBox);
                            var obj1 = await _restService.GetSearchQueryScopus(QueryTextBox);
                            var obj2 = await _restService.GetSearchQuerySpringer(QueryTextBox);

                            obj.SearchResults.Entry.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                            });

                            this.TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1.SearchResults.Entry.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                            });

                            this.TotalResultsToDisplay.Add(obj1.SearchResults);

                            obj2.Records.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                            });

                            obj2.Result.ToList().ForEach(element =>
                            {
                                this.TotalResultsToDisplay.Add(element);
                            });

                            _statisticsDataService.IncrementScienceDirect();
                            _statisticsDataService.IncrementScopus();
                            _statisticsDataService.IncrementSpringer();
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
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                            });

                            this.TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1.SearchResults.Entry.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
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
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                            });

                            this.TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1.Records.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
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
                                this.SearchResultsToDisplay.Last().IsDuplicate = CompareDoi(this.SearchResultsToDisplay.Last());
                                this.SearchResultsToDisplay.Last().PercentComplete = DegreeOfCompliance(this.SearchResultsToDisplay.Last());
                            });

                            this.TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1.Records.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
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
                        else
                        {
                            ShowDialog(GetString("TitleDialogError"), GetString("NotSelectedDatabase"));
                        }

//                        PublicationDateFromDatabasesLabels(SearchResultsToDisplay);

                        //if (CheckBoxIeeeXplore)
                        //{
                        //    //TODO:
                        //}

                        //if (CheckBoxWebOfScience)
                        //{
                        //    //TODO:
                        //}

                        //if (CheckBoxWiley)
                        //{
                        //    //TODO:
                        //}

                        CollectionView?.GroupDescriptions.Add(new PropertyGroupDescription("Source"));
                        //Messenger.Default.Send<PublicationDateMessageToChart>(new PublicationDateMessageToChart
                        //{
                        //    SearchResultsToDisplaysMessage = DoiAndTitleAndAbstract.PublicationDate
                        //});
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
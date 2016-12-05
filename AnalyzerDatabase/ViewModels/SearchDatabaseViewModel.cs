using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using AnalyzerDatabase.Interfaces;
using AnalyzerDatabase.View;
using GalaSoft.MvvmLight.Command;

namespace AnalyzerDatabase.ViewModels
{
    public class SearchDatabaseViewModel : ExtendedViewModelBase
    {
        #region Variables

        private readonly IInternetConnectionService _internetConnectionService;
        private readonly IRestService _restService;

        public ICollectionView CollectionView { get; set; }
        public ISearchResultsToDisplay DoiAndTitle { get; set; }

        private ObservableCollection<ISearchResultsToDisplay> _searchResultsToDisplay;
        private ObservableCollection<ITotalResultsToDisplay> _totalResultsToDisplay;

        private RelayCommand _searchCommand;
        private RelayCommand _fullScreenDataGrid;
        private RelayCommand _nextResultPage;
        private RelayCommand _prevResultPage;
        private RelayCommand _downloadArticleToPdf;
        private RelayCommand _downloadArticleToDocx;

        private string _queryTextBox;
        private string _executionTime;
        private string _totalResults;

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

        public SearchDatabaseViewModel(IRestService restService, IInternetConnectionService internetConnectionService)
        {
            _restService = restService;
            _internetConnectionService = internetConnectionService;
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
                _restService.GetArticle(DoiAndTitle.Doi, DoiAndTitle.Title);
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
                _restService.GetArticleDocx(DoiAndTitle.Doi, DoiAndTitle.Title);
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

                        if (CheckBoxScienceDirect && CheckBoxScopus && CheckBoxSpringer)
                        {
                            var obj = await _restService.GetSearchQueryScienceDirect(QueryTextBox);
                            var obj1 = await _restService.GetSearchQueryScopus(QueryTextBox);
                            var obj2 = await _restService.GetSearchQuerySpringer(QueryTextBox);

                            obj.SearchResults.Entry.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                            });

                            this.TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1.SearchResults.Entry.ToList().ForEach(e =>
                            {
                                this.SearchResultsToDisplay.Add(e);
                            });

                            this.TotalResultsToDisplay.Add(obj1.SearchResults);

                            obj2.Records.ToList().ForEach(e =>
                            {
                                this.SearchResultsToDisplay.Add(e);
                            });

                            obj2.Result.ToList().ForEach(e =>
                            {
                                this.TotalResultsToDisplay.Add(e);
                            });
                        }
                        else if (CheckBoxScienceDirect && CheckBoxScopus)
                        {
                            var obj = await _restService.GetSearchQueryScienceDirect(QueryTextBox);
                            var obj1 = await _restService.GetSearchQueryScopus(QueryTextBox);

                            obj.SearchResults.Entry.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                            });

                            this.TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1.SearchResults.Entry.ToList().ForEach(e =>
                            {
                                this.SearchResultsToDisplay.Add(e);
                            });

                            this.TotalResultsToDisplay.Add(obj1.SearchResults);
                        }
                        else if (CheckBoxScienceDirect && CheckBoxSpringer)
                        {
                            var obj = await _restService.GetSearchQueryScienceDirect(QueryTextBox);
                            var obj1 = await _restService.GetSearchQuerySpringer(QueryTextBox);

                            obj.SearchResults.Entry.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                            });

                            this.TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1.Records.ToList().ForEach(e =>
                            {
                                this.SearchResultsToDisplay.Add(e);
                            });

                            obj1.Result.ToList().ForEach(e =>
                            {
                                this.TotalResultsToDisplay.Add(e);
                            });
                        }
                        else if (CheckBoxScopus && CheckBoxSpringer)
                        {
                            var obj = await _restService.GetSearchQueryScopus(QueryTextBox);
                            var obj1 = await _restService.GetSearchQuerySpringer(QueryTextBox);

                            obj.SearchResults.Entry.ToList().ForEach(e =>
                            {
                                this.SearchResultsToDisplay.Add(e);
                            });

                            this.TotalResultsToDisplay.Add(obj.SearchResults);

                            obj1.Records.ToList().ForEach(e =>
                            {
                                this.SearchResultsToDisplay.Add(e);
                            });

                            obj1.Result.ToList().ForEach(e =>
                            {
                                this.TotalResultsToDisplay.Add(e);
                            });
                        }
                        else if (CheckBoxScienceDirect)
                        {
                            var obj = await _restService.GetSearchQueryScienceDirect(QueryTextBox);

                            obj.SearchResults.Entry.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                            });

                            this.TotalResultsToDisplay.Add(obj.SearchResults);
                        }
                        else if (CheckBoxScopus)
                        {
                            var obj = await _restService.GetSearchQueryScopus(QueryTextBox);

                            obj.SearchResults.Entry.ToList().ForEach(e =>
                            {
                                this.SearchResultsToDisplay.Add(e);
                            });

                            this.TotalResultsToDisplay.Add(obj.SearchResults);
                        }
                        else if (CheckBoxSpringer)
                        {
                            var obj = await _restService.GetSearchQuerySpringer(QueryTextBox);

                            obj.Records.ToList().ForEach(e =>
                            {
                                this.SearchResultsToDisplay.Add(e);
                            });

                            obj.Result.ToList().ForEach(e =>
                            {
                                this.TotalResultsToDisplay.Add(e);
                            });
                        }
                        else
                        {
                            ShowDialog(GetString("TitleDialogError"), GetString("NotSelectedDatabase"));
                        }

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
                    }
                    catch (Exception)
                    {
                        throw;
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
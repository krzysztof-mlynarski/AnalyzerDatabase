using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using AnalyzerDatabase.Interfaces;
using AnalyzerDatabase.Models;
using AnalyzerDatabase.Models.ScienceDirect;
using AnalyzerDatabase.Models.Scopus;
using AnalyzerDatabase.Models.Springer;
using AnalyzerDatabase.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json.Linq;
using MahApps.Metro.Controls.Dialogs;

namespace AnalyzerDatabase.ViewModels
{
    public class SearchDatabaseViewModel : ExtendedViewModelBase
    {
        private readonly IInternetConnectionService _internetConnectionService;
        private readonly IRestService _restService;
        private ScienceDirectSearchQuery _scienceDirectSearchCollection;
        private ScopusSearchQuery _scopusSearchCollection;
        private SpringerSearchQuery _springerSearchCollection;

        private ObservableCollection<ISearchResultsToDisplay> _searchResultsToDisplay;

        public ICollectionView CollectionView { get; set; }
        public ISearchResultsToDisplay DoiAndTitle { get; set; }

        private RelayCommand _searchCommand;
        private RelayCommand _fullScreenDataGrid;
        private RelayCommand _nextResultPage;
        private RelayCommand _prevResultPage;
        private RelayCommand _downloadArticleToPdf;

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

        #region Constructors

        public SearchDatabaseViewModel(IRestService restService, IInternetConnectionService internetConnectionService)
        {
            _restService = restService;
            _internetConnectionService = internetConnectionService;
            SearchResultsToDisplay = new ObservableCollection<ISearchResultsToDisplay>();
            CollectionView = CollectionViewSource.GetDefaultView(_searchResultsToDisplay);
        }

        #endregion

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
                    windowFullDataGrid.Show();
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
                return _downloadArticleToPdf ?? (_downloadArticleToPdf = new RelayCommand(DownloadArticle));
            }
        }

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

        public ScienceDirectSearchQuery ScienceDirectSearchCollection
        {
            get
            {
                return _scienceDirectSearchCollection;
            }
            set
            {
                if (value != null)
                {
                    _scienceDirectSearchCollection = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ScopusSearchQuery ScopusSearchCollection
        {
            get
            {
                return _scopusSearchCollection;
            }
            set
            {
                if (value != null)
                {
                    _scopusSearchCollection = value;
                    RaisePropertyChanged();
                }
            }
        }

        public SpringerSearchQuery SpringerSearchCollection
        {
            get
            {
                return _springerSearchCollection;
            }
            set
            {
                if (value != null)
                {
                    _springerSearchCollection = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void DownloadArticle()
        {
            try
            {
                IsDataLoading = true;
                _restService.GetArticle(DoiAndTitle.Doi, DoiAndTitle.Title);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsDataLoading = false;
            }
        }

        private async void NextPage()
        {
            bool isInternet = _internetConnectionService.CheckConnectedToInternet();

            if (isInternet)
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

            if (isInternet)
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

            if (isInternet)
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

                        if (CheckBoxScienceDirect)
                        {
                            var obj = await _restService.GetSearchQueryScienceDirect(QueryTextBox);
                            obj.SearchResults.Entry.ToList().ForEach(element =>
                            {
                                this.SearchResultsToDisplay.Add(element);
                            });                          
                        }
                        
                        if (CheckBoxScopus)
                        {
                            var obj = await _restService.GetSearchQueryScopus(QueryTextBox);
                            obj.SearchResults.Entry.ToList().ForEach(e =>
                            {
                                this.SearchResultsToDisplay.Add(e);
                            });
                        }

                        if (CheckBoxSpringer)
                        {
                            var obj = await _restService.GetSearchQuerySpringer(QueryTextBox);
                            obj.Records.ToList().ForEach(e =>
                            {
                                this.SearchResultsToDisplay.Add(e);
                            });
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
                    catch (Exception ex)
                    {
                        throw ex;
                        //TODO: "nie wybrales zadnej bazy danych do przeszukania"
                    }
                    finally
                    {
                        IsDataLoading = false;

                        var stopTime = DateTime.Now;
                        TimeSpan executionTime = stopTime - startTime;
                        ExecutionTime = "(" + executionTime.TotalSeconds + "s)";

                        var allTotalResults = SearchResultsToDisplay.Count;
                        TotalResults = "Około " + allTotalResults + " wyników w " + ExecutionTime;
                    }
                }
                else
                {
                    //TODO: stworzyc dialog do informowania o bledach
                    //TODO: brak wpisanego slowa do wyszukania

                    //await this.ShowMessageAsync("Nie wprowadziłeś kryteria wyszukiwania");
                }
            }
            else
            {
                //TODO: brak polaczenia z internetem
            }
        }
    }
}
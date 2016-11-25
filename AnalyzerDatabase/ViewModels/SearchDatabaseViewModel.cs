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

        private ObservableCollection<IScienceDirectAndScopus> _scienceDirects;

        public ICollectionView collectionView { get; set; }

        private RelayCommand _searchCommand;
        private RelayCommand _fullScreenDataGrid;

        private string _queryTextBox;
        private string _executionTime;
        private string _totalResults;

        private bool _isDataLoading;

        private bool _checkBoxScopus = true;
        private bool _checkBoxSpringer = true;
        private bool _checkBoxWiley /*= true*/;
        private bool _checkBoxScienceDirect = true;
        private bool _checkBoxWebOfScience /*= true*/;
        private bool _checkBoxIeeeXplore /*= true*/;

        #region Constructors

        public SearchDatabaseViewModel(IRestService restService, IInternetConnectionService internetConnectionService)
        {
            _restService = restService;
            _internetConnectionService = internetConnectionService;
            ScienceDirectAndScopus = new ObservableCollection<IScienceDirectAndScopus>();
            collectionView = CollectionViewSource.GetDefaultView(_scienceDirects);

        }

        #endregion

        public RelayCommand SearchCommand
        {
            get { return _searchCommand ?? (_searchCommand = new RelayCommand(Search)); }
        }

        public RelayCommand OpenFullScreenDataGrid
        {
            get { return _fullScreenDataGrid ?? (_fullScreenDataGrid = new RelayCommand(FullDataGridMethod)); }
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

        public ObservableCollection<IScienceDirectAndScopus> ScienceDirectAndScopus
        {
            get
            {
                return _scienceDirects;
            }
            set
            {
                _scienceDirects = value;
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

        private void FullDataGridMethod()
        {
            var windowFullDataGrid = new FullDataGridView();
            windowFullDataGrid.Show();
        }

        private async void Search()
        {
            bool isInternet = _internetConnectionService.CheckConnectedToInternet();

            if (isInternet)
            {
                ScienceDirectAndScopus.Clear();
                collectionView?.GroupDescriptions.Clear();

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
                                this.ScienceDirectAndScopus.Add(element);
                            });                          
                        }
                        
                        if (CheckBoxScopus)
                        {
                            var obj = await _restService.GetSearchQueryScopus(QueryTextBox);
                            obj.SearchResults.Entry.ToList().ForEach(e =>
                            {
                                this.ScienceDirectAndScopus.Add(e);
                            });
                        }

                        if (CheckBoxSpringer)
                        {
                            var obj = await _restService.GetSearchQuerySpringer(QueryTextBox);
                            obj.Records.ToList().ForEach(e =>
                            {
                                this.ScienceDirectAndScopus.Add(e);
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

                        collectionView?.GroupDescriptions.Add(new PropertyGroupDescription("Source"));
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

                        //int allTotalResults = Int32.Parse(SearchResults.OpensearchTotalResults);
                        //allTotalResults += Int32.Parse(ScopusSearchCollection.SearchResults.OpensearchTotalResults);
                        //TotalResults = "Około " + allTotalResults + " wyników w";
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
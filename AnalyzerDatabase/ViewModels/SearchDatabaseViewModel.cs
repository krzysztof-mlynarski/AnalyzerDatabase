using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        private RelayCommand _searchCommand;
        private RelayCommand _fullScreenDataGrid;

        private bool _isDataLoading;
        private string _executionTime;

        private List<String> test = new List<string>();

        private string _queryTextBox = "";

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
        }

        #endregion

        public RelayCommand OpenFullScreenDataGrid
        {
            get { return _fullScreenDataGrid ?? (_fullScreenDataGrid = new RelayCommand(FullDataGridMethod)); }
        }

        public string ExecutionTime
        {
            get
            {
                return _executionTime;
            }
            set
            {
                if (_executionTime != null)
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

        public RelayCommand SearchCommand
        {
            get { return _searchCommand ?? (_searchCommand = new RelayCommand(Search)); }
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
                if (!string.IsNullOrEmpty(QueryTextBox))
                {
                    DateTime startTime = new DateTime();
                    try
                    {
                        startTime = DateTime.Now;
                        IsDataLoading = true;

                        //Wszystkie bazy
                        if (CheckBoxScopus && CheckBoxScienceDirect && CheckBoxSpringer && CheckBoxWebOfScience &&
                            CheckBoxWiley && CheckBoxIeeeXplore)
                        {
                            //TODO: Get dla innych baz
                        }
                        //Brak IEEE Xplore
                        else if (CheckBoxScopus && CheckBoxScienceDirect && CheckBoxSpringer && CheckBoxWebOfScience &&
                                 CheckBoxWiley)
                        {
                        }
                        //Brak IEEE Xplore i Wiley
                        else if (CheckBoxScopus && CheckBoxScienceDirect && CheckBoxSpringer && CheckBoxWebOfScience)
                        {
                        }
                        //Brak IEEE Xplore, Wiley, Web of Science
                        else if (CheckBoxScopus && CheckBoxScienceDirect && CheckBoxSpringer)
                        {
                            ScopusSearchCollection = await _restService.GetSearchQueryScopus(QueryTextBox);
                            ScienceDirectSearchCollection = await _restService.GetSearchQueryScienceDirect(QueryTextBox);
                            SpringerSearchCollection = await _restService.GetSearchQuerySpringer(QueryTextBox);
                        }
                        else if (CheckBoxScopus)
                        {
                            ScopusSearchCollection = await _restService.GetSearchQueryScopus(QueryTextBox);
                        }
                        else if (CheckBoxScienceDirect)
                        {
                            ScienceDirectSearchCollection = await _restService.GetSearchQueryScienceDirect(QueryTextBox);
                            //_entryScienceDirects = ScienceDirectSearchCollection.SearchResults.Entry;

                        }
                        else if (CheckBoxSpringer)
                        {
                            SpringerSearchCollection = await _restService.GetSearchQuerySpringer(QueryTextBox);
                        }
                        else if (CheckBoxWebOfScience)
                        {
                            //TODO:

                            
                        }
                        else if (CheckBoxWiley)
                        {
                            //TODO:
                        }
                        else if (CheckBoxIeeeXplore)
                        {
                            //TODO:
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                        //TODO: "nie wybrales zadnej bazy danych do przeszukania"
                    }
                    finally
                    {
                        IsDataLoading = false;

                        var stopTime = DateTime.Now;
                        TimeSpan executionTime = stopTime - startTime;
                        ExecutionTime = (executionTime.TotalSeconds).ToString();
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
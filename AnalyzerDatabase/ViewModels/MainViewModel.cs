using AnalyzerDatabase.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace AnalyzerDatabase.ViewModels
{
    public class MainViewModel : ExtendedViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>

        private readonly IInternetConnectionService _internetConnectionService;
        private bool _isInternetConnection;
        private bool _isVpnConnection;

        private RelayCommand _openSearchDatabaseCommand;
        private RelayCommand _openStatisticsCommand;
        private RelayCommand _openSettingsCommand;
        private RelayCommand _openAboutCommand;

        public MainViewModel(IInternetConnectionService internetConnectionService)
        {
            _internetConnectionService = internetConnectionService;
            CurrentViewModel = ViewModelLocator.Instance.SearchDatabase;
            //CurrentViewModel = ViewModelLocator.Instance.Statistics;
            //CurrentViewModel = ViewModelLocator.Instance.Settings;
            //CurrentViewModel = ViewModelLocator.Instance.About;
            CurrentViewModel = null;

            CheckInternetConnection();
        }

        private void CheckInternetConnection()
        {
            IsInternetConnection = _internetConnectionService.CheckConnectedToInternet();
            IsVpnConnection = _internetConnectionService.CheckConnectedToInternetVpn();
        }

        #region Getters setters

        public bool IsInternetConnection
        {
            get
            {
                return _isInternetConnection;               
            }
            set
            {
                if (_isInternetConnection != value)
                {
                    _isInternetConnection = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsVpnConnection
        {
            get
            {
                return _isVpnConnection;                 
            }
            set
            {
                if (_isVpnConnection != value)
                {
                    _isVpnConnection = value;
                    RaisePropertyChanged();
                }
            }
        }



        public RelayCommand OpenSearchDatabaseCommand
        {
            get
            {
                return _openSearchDatabaseCommand ?? (_openSearchDatabaseCommand = new RelayCommand(OpenSearchDatabase));
            }
        }

        public RelayCommand OpenStatisticsCommand
        {
            get
            {
                return _openStatisticsCommand ?? (_openStatisticsCommand = new RelayCommand(OpenStatistics));
            }
        }

        public RelayCommand OpenSettingsCommand
        {
            get
            {
                return _openSettingsCommand ?? (_openSettingsCommand = new RelayCommand(OpenSettings));
            }
        }

        public RelayCommand OpenAboutCommand
        {
            get
            {
                return _openAboutCommand ?? (_openAboutCommand = new RelayCommand(OpenAbout));
            }
        }

        #endregion

        private void OpenSearchDatabase()
        {
            NavigateTo(ViewModelLocator.Instance.SearchDatabase);
        }

        private void OpenStatistics()
        {
            //NavigateTo(ViewModelLocator.Instance.Statistics);
        }

        private void OpenSettings()
        {
            //NavigateTo(ViewModelLocator.Instance.Settings);
        }

        private void OpenAbout()
        {
            //NavigateTo(ViewModelLocator.Instance.About);
        }
    }
}

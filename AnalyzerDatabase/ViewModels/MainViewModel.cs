using AnalyzerDatabase.Interfaces;
using AnalyzerDatabase.Messengers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace AnalyzerDatabase.ViewModels
{
    public class MainViewModel : ExtendedViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        #region Variables
        private ViewModelBase Page { get; set; }

        private readonly IInternetConnectionService _internetConnectionService;
        private bool _isInternetConnection;
        private bool _isVpnConnection;

        private RelayCommand _openSearchDatabaseCommand;
        private RelayCommand _openStatisticsCommand;
        private RelayCommand _openSettingsCommand;
        private RelayCommand _openAboutCommand;
        private RelayCommand _goToSettingsViewCommand;
        private RelayCommand _openHomeWindowCommand;

        #endregion

        #region Constructor
        public MainViewModel(IInternetConnectionService internetConnectionService)
        {
            _internetConnectionService = internetConnectionService;
            CurrentViewModel = ViewModelLocator.Instance.SearchDatabase;
            CurrentViewModel = ViewModelLocator.Instance.Statistics;
            CurrentViewModel = ViewModelLocator.Instance.Settings;
            //CurrentViewModel = ViewModelLocator.Instance.About;
            CurrentViewModel = null;

            CheckInternetConnection();

            Messenger.Default.Register<ExceptionToSettingsMessage>(this, HandleMessage);
        }

        #endregion

        #region Message

        private async void HandleMessage(ExceptionToSettingsMessage message)
        {
            Page = message.Exception;
            var source = message.Source;

            if (await ExceptionDialog("ERROR - " + source, "Wystąpił problem komunikacji z bazą.\nSprawdź w ustawieniach czy posiadasz wpisany klucz API dla tej bazy!"))
                NavigateTo(Page);
        }

        #endregion

        #region Getters/setters

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

        #endregion

        #region RelayCommand
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

        public RelayCommand GoToSettingsViewCommand
        {
            get
            {
                return _goToSettingsViewCommand ?? (_goToSettingsViewCommand = new RelayCommand(OpenSettings));
            }
        }

        public RelayCommand OpenHomeWindowCommand
        {
            get { return _openHomeWindowCommand ?? (_openHomeWindowCommand = new RelayCommand(OpenHomeWindow)); }
        }
        #endregion

        #region Private methods
        private void OpenHomeWindow()
        {
            NavigateTo(ViewModelLocator.Instance.Main);
        }

        private void OpenSearchDatabase()
        {
            NavigateTo(ViewModelLocator.Instance.SearchDatabase);
        }

        private void OpenStatistics()
        {
            NavigateTo(ViewModelLocator.Instance.Statistics);
        }

        private void OpenSettings()
        {
            NavigateTo(ViewModelLocator.Instance.Settings);
        }

        private void OpenAbout()
        {
            //NavigateTo(ViewModelLocator.Instance.About);
        }

        private void CheckInternetConnection()
        {
            IsInternetConnection = _internetConnectionService.CheckConnectedToInternet();
            IsVpnConnection = _internetConnectionService.CheckConnectedToInternetVpn();
        }
        #endregion
    }
}

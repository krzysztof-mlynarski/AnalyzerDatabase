using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalyzerDatabase.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace AnalyzerDatabase.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>

        private ViewModelBase _currentViewModel;
        private RelayCommand _openSearchDatabaseCommand;
        private RelayCommand _openStatisticsCommand;
        private RelayCommand _openSettingsCommand;
        private RelayCommand _openAboutCommand;

        public MainViewModel()
        {
            CurrentViewModel = (new ViewModelLocator()).SearchDatabase;
            CurrentViewModel = null;
        }

        #region Getters setters
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }

            set
            {
                _currentViewModel = value;
                RaisePropertyChanged("CurrentViewModel");
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
            CurrentViewModel = (new ViewModelLocator()).SearchDatabase;
        }

        private void OpenStatistics()
        {
        }

        private void OpenSettings()
        {

        }

        private void OpenAbout()
        {

        }
    }
}

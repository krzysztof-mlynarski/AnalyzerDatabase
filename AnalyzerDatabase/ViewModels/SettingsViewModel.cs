using System;
using System.Diagnostics;
using System.Windows.Forms;
using AnalyzerDatabase.Services;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;

namespace AnalyzerDatabase.ViewModels
{
    public class SettingsViewModel : ExtendedViewModelBase
    {
        #region Variables
        private string _currentPublicationSavingPath;
        private string _currentScienceDirectAndScopusApiKey;
        private string _currentSpringerApiKey;
        private bool _startOnLogin;

        private RelayCommand _openDirectoryFilePicker;
        private RelayCommand _openPublicationsDirectory;
        private RelayCommand _openPageDevElsevier;
        private RelayCommand _openPageDevSpringer;
        #endregion

        #region Constructors
        public SettingsViewModel()
        {
            CurrentPublicationSavingPath = SettingsService.Instance.Settings.SavingPublicationPath;
            CurrentScienceDirectAndScopusApiKey = SettingsService.Instance.Settings.ScienceDirectAndScopusApiKey;
            CurrentSpringerApiKey = SettingsService.Instance.Settings.SpringerApiKey;
            StartOnLogin = SettingsService.Instance.Settings.StartOnLogin;
        }

        #endregion

        #region Getters/Setters
        public string CurrentPublicationSavingPath
        {
            get
            {
                return _currentPublicationSavingPath;
            }
            set
            {
                if (_currentPublicationSavingPath == value)
                    return;

                _currentPublicationSavingPath = value;

                if (ZetaLongPaths.ZlpIOHelper.DirectoryExists(_currentPublicationSavingPath))
                {
                    SettingsService.Instance.Settings.SavingPublicationPath = _currentPublicationSavingPath;
                }
                SettingsService.Instance.Save();

                RaisePropertyChanged();
            }
        }

        public string CurrentScienceDirectAndScopusApiKey
        {
            get
            {
                return _currentScienceDirectAndScopusApiKey;
            }
            set
            {
                if (_currentScienceDirectAndScopusApiKey == value)
                    return;

                _currentScienceDirectAndScopusApiKey = value;

                SettingsService.Instance.Settings.ScienceDirectAndScopusApiKey = _currentScienceDirectAndScopusApiKey;
                SettingsService.Instance.Save();

                RaisePropertyChanged();
            }
        }

        public string CurrentSpringerApiKey
        {
            get
            {
                return _currentSpringerApiKey;
            }
            set
            {
                if (_currentSpringerApiKey == value)
                    return;

                _currentSpringerApiKey = value;

                SettingsService.Instance.Settings.SpringerApiKey = _currentSpringerApiKey;
                SettingsService.Instance.Save();

                RaisePropertyChanged();
            }
        }

        public bool StartOnLogin
        {
            get
            {
                return _startOnLogin;
            }
            set
            {
                if (_startOnLogin == value)
                    return;
                _startOnLogin = value;

                SettingsService.Instance.Settings.StartOnLogin = _startOnLogin;
                SettingsService.Instance.Save();

                RegisterInStartup(_startOnLogin);

                RaisePropertyChanged();
            }
        }

        #endregion

        #region RelayCommand
        public RelayCommand OpenPublicationsDirectory
        {
            get
            {
                return _openPublicationsDirectory ?? (_openPublicationsDirectory = new RelayCommand(() =>
                {
                    Process.Start(SettingsService.Instance.Settings.SavingPublicationPath);
                }));
            }
        }

        public RelayCommand OpenDirectoryFilePicker
        {
            get
            {
                return _openDirectoryFilePicker ?? (_openDirectoryFilePicker = new RelayCommand(() =>
                {
                    FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
                    {
                        SelectedPath = AppDomain.CurrentDomain.BaseDirectory,
                        Description = GetString("PickFolderWithPublication")
                    };

                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        CurrentPublicationSavingPath = folderBrowserDialog.SelectedPath;
                    }
                }));
            }
        }

        public RelayCommand OpenPageDevElsevier
        {
            get
            {
                return _openPageDevElsevier ?? (_openPageDevElsevier = new RelayCommand(() =>
                {
                    Process.Start("https://dev.elsevier.com/user/login");
                }));
            }
        }

        public RelayCommand OpenPageDevSpringer
        {
            get
            {
                return _openPageDevSpringer ?? (_openPageDevSpringer = new RelayCommand(() =>
                {
                    Process.Start("https://dev.springer.com/login");
                }));
            }
        }
        #endregion

        #region Private methods
        private void RegisterInStartup(bool isChecked)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (isChecked)
            {
                registryKey?.SetValue("Analyzer Database", Application.ExecutablePath);
            }
            else
            {
                registryKey?.DeleteValue("Analyzer Database", false);
            }
        }

        #endregion
    }
}


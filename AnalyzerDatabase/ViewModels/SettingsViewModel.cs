using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using AnalyzerDatabase.Services;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;

namespace AnalyzerDatabase.ViewModels
{
    public class SettingsViewModel : ExtendedViewModelBase
    {
        private string _selectedLanguage;
        private string _selectedAppStyle;
        private string _currentPublicationSavingPath;
        private string _currentScienceDirectAndScopusApiKey;
        private string _currentSpringerApiKey;
        private bool _startOnLogin;

        private RelayCommand _openDirectoryFilePicker;
        private RelayCommand _openPublicationsDirectory;
        private RelayCommand _openPageDevElsevier;
        private RelayCommand _openPageDevSpringer;

        public SettingsViewModel()
        {
            if (SettingsService.Instance.Settings.CurrentLanguage == "pl-PL")
                SelectedLanguage = GetString("Polish");
            if (SettingsService.Instance.Settings.CurrentLanguage == "en-EN")
                SelectedLanguage = GetString("English");

            if (SettingsService.Instance.Settings.CurrentStyle == "cobalt.xaml")
                SelectedAppStyle = GetString("Cobalt");
            if (SettingsService.Instance.Settings.CurrentStyle == "green.xaml")
                SelectedAppStyle = GetString("Green");
            if (SettingsService.Instance.Settings.CurrentStyle == "indigo.xaml")
                SelectedAppStyle = GetString("Indygo");

            CurrentPublicationSavingPath = SettingsService.Instance.Settings.SavingPublicationPath;
            CurrentScienceDirectAndScopusApiKey = SettingsService.Instance.Settings.ScienceDirectAndScopusApiKey;
            CurrentSpringerApiKey = SettingsService.Instance.Settings.SpringerApiKey;
            StartOnLogin = SettingsService.Instance.Settings.StartOnLogin;
        }

        public string SelectedLanguage
        {
            get
            {
                return _selectedLanguage;
            }
            set
            {
                if(_selectedLanguage != null && _selectedLanguage == value)
                    return;

                _selectedLanguage = value;

                SelectedLanguageItem(_selectedLanguage);

                RaisePropertyChanged();
            }
        }

        private void SelectedLanguageItem(string name)
        {
            if (name == this.GetString("Polish"))
                SettingsService.Instance.Settings.CurrentLanguage = "pl-PL";
            if (name == this.GetString("English"))
                SettingsService.Instance.Settings.CurrentLanguage = "en-EN";

            SettingsService.Instance.Save();

            //System.Windows.Forms.Application.Restart();
            //System.Windows.Application.Current.Shutdown();
        }

        public string SelectedAppStyle
        {
            get
            {
                return _selectedAppStyle;
            }
            set
            {
                if (_selectedAppStyle != null && _selectedAppStyle == value)
                    return;

                _selectedAppStyle = value;

                SelectedAppStyleItem(_selectedAppStyle);

                RaisePropertyChanged();
            }
        }

        public void SelectedAppStyleItem(string style)
        {
            //System.Windows.Application.Current.Resources.MergedDictionaries.Add(style);
            // get the current app style (theme and accent) from the application
            // you can then use the current theme and custom accent instead set a new theme
            //   Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(MediaTypeNames.Application.Current);

            // now set the Green accent and dark theme
            //     ThemeManager.ChangeAppStyle(MediaTypeNames.Application.Current,
            //                            ThemeManager.GetAccent("Cobalt"),
            //                            ThemeManager.GetAppTheme("BaseLight")); // or appStyle.Item1
        }

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
                    FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                    folderBrowserDialog.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;
                    folderBrowserDialog.Description = this.GetString("PickFolderWithPublication");

                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        this.CurrentPublicationSavingPath = folderBrowserDialog.SelectedPath;
                    }
                }));
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

        public RelayCommand OpenPageDevElsevier
        {
            get
            {
                return _openPageDevElsevier ?? (_openPageDevElsevier = new RelayCommand(() =>
                {
                    try
                    {
                        System.Diagnostics.Process.Start("https://dev.elsevier.com/user/login");
                    }
                    catch
                    {
                        throw;
                    }
                }));
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

        public RelayCommand OpenPageDevSpringer
        {
            get
            {
                return _openPageDevSpringer ?? (_openPageDevSpringer = new RelayCommand(() =>
                {
                    try
                    {
                        System.Diagnostics.Process.Start("https://dev.springer.com/login");
                    }
                    catch
                    {
                        throw;
                    }
                }));
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

        private void RegisterInStartup(bool isChecked)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (isChecked)
            {
                registryKey?.SetValue("Analyzer Database", Assembly.GetEntryAssembly());
            }
            else
            {
                registryKey?.DeleteValue("Analyzer Database");
            }
        }
    }
}


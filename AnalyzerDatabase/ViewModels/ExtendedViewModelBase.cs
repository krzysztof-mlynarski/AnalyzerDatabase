using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using System.Windows;
using AnalyzerDatabase.Services;
using GalaSoft.MvvmLight;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace AnalyzerDatabase.ViewModels
{
    public class ExtendedViewModelBase : ViewModelBase
    {
        private ViewModelBase _currentViewModel;
        private readonly ResourceManager _resourceManager;
        private readonly MetroWindow _metroWindow = Application.Current.MainWindow as MetroWindow;

        protected ExtendedViewModelBase()
        {
            _resourceManager = new ResourceManager("AnalyzerDatabase.Properties.Resources", Assembly.GetExecutingAssembly());
        }

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                if(_currentViewModel == value)
                    return;
                _currentViewModel = value;
                RaisePropertyChanged();
            }
        }

        private void OnLoad()
        {
            CurrentViewModel = null;
        }

        protected void NavigateTo(ViewModelBase viewModelBase)
        {
            CurrentViewModel = viewModelBase;
            var extendedViewModel = viewModelBase as ExtendedViewModelBase;
            extendedViewModel?.OnLoad();
        }

        protected string GetString(string name)
        {
            return _resourceManager.GetString(name, SettingsService.Instance.Culture);
        }

        protected async void ShowDialog(string title, string discription)
        {
            await _metroWindow.ShowMessageAsync(title, discription);
        }

        public async void ShowDialogColor(string title, string discription)
        {
            if (_metroWindow != null)
                _metroWindow.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;

            await _metroWindow.ShowMessageAsync(title, discription);
        }

        protected async Task<bool> ConfirmationDialog(string title, string description)
        {
            MetroDialogSettings settings = new MetroDialogSettings()
            {
                //TODO: ustawic pobiernaie dla danego jezyka
                AffirmativeButtonText = "Tak",
                NegativeButtonText = "Nie",
                AnimateShow = true,
                ColorScheme = MetroDialogColorScheme.Theme
            };

            MessageDialogResult result = await _metroWindow.ShowMessageAsync(title, description, MessageDialogStyle.AffirmativeAndNegative, settings);
            return result == MessageDialogResult.Affirmative;
        }

        protected async Task<bool> ExceptionDialog(string title, string description)
        {
            MetroDialogSettings settings = new MetroDialogSettings()
            {
                //TODO: ustawic pobiernaie dla danego jezyka
                AffirmativeButtonText = "Settings",
                NegativeButtonText = "OK",
                AnimateShow = true,
                ColorScheme = MetroDialogColorScheme.Theme
            };

            MessageDialogResult result = await _metroWindow.ShowMessageAsync(title, description, MessageDialogStyle.AffirmativeAndNegative, settings);
            return result == MessageDialogResult.Affirmative;
        }
    }
}
using System.Reflection;
using System.Resources;
using AnalyzerDatabase.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;

namespace AnalyzerDatabase.ViewModels
{
    public class ExtendedViewModelBase : ViewModelBase
    {
        private ViewModelBase _currentViewModel;
        private readonly ResourceManager _resourceManager;

        public ExtendedViewModelBase()
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
                RaisePropertyChanged("CurrentViewModel");
            }
        }

        public virtual void OnLoad()
        {
            CurrentViewModel = null;
        }

        public virtual void NavigateTo(ViewModelBase viewModelBase)
        {
            CurrentViewModel = viewModelBase;
            var extendedViewModel = viewModelBase as ExtendedViewModelBase;
            if (extendedViewModel != null)
            {
                extendedViewModel.OnLoad();
            }
        }

        public string GetString(string name)
        {
            return _resourceManager.GetString(name, SettingsService.Instance.Culture);
        }
    }
}
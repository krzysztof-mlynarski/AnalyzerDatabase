using AnalyzerDatabase.Interfaces;
using AnalyzerDatabase.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.ServiceLocation;

namespace AnalyzerDatabase.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>

        private static ViewModelLocator _instance;

        public static ViewModelLocator Instance
        {
            get
            {
                return _instance ?? (_instance = new ViewModelLocator());
            }
        }

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IDialogCoordinator, DialogCoordinator>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SearchDatabaseViewModel>();
            SimpleIoc.Default.Register<StatisticsViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<AboutViewModel>();

            SimpleIoc.Default.Register<FullDataGridViewModel>();

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDeserializeJsonService, DeserializeJsonService>();
                SimpleIoc.Default.Register<IInternetConnectionService, InternetConnectionService>();
                SimpleIoc.Default.Register<IRestService, RestService>();
                SimpleIoc.Default.Register<IStatisticsDataService, StatisticsDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDeserializeJsonService, DeserializeJsonService>();
                SimpleIoc.Default.Register<IInternetConnectionService, InternetConnectionService>();
                SimpleIoc.Default.Register<IRestService, RestService>();
                SimpleIoc.Default.Register<IStatisticsDataService, StatisticsDataService>();
            }
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public SearchDatabaseViewModel SearchDatabase
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SearchDatabaseViewModel>();
            }
        }

        public StatisticsViewModel Statistics
        {
            get
            {
                return ServiceLocator.Current.GetInstance<StatisticsViewModel>();
            }
        }

        public SettingsViewModel Settings
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingsViewModel>();
            }
        }

        public AboutViewModel About
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AboutViewModel>();
            }
        }

        public FullDataGridViewModel FullDataGrid
        {
            get
            {
                return ServiceLocator.Current.GetInstance<FullDataGridViewModel>();               
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
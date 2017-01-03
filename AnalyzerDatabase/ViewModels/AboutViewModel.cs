using System.Diagnostics;
using GalaSoft.MvvmLight.Command;

namespace AnalyzerDatabase.ViewModels
{
    public class AboutViewModel : ExtendedViewModelBase
    {
        #region Variables
        private RelayCommand _openGitHubPageCommand;
        private RelayCommand _openEmailClientCommand;

        private string _versionNumber = System.Reflection.Assembly.GetExecutingAssembly()
                                                                  .GetName()
                                                                  .Version
                                                                  .ToString();
        #endregion

        #region RelayCommand
        public RelayCommand OpenGitHubPageCommand
        {
            get
            {
                return _openGitHubPageCommand ?? (_openGitHubPageCommand = new RelayCommand(() =>
                {
                    Process.Start("https://github.com/krzysztof-mlynarski");
                }));
            }
        }

        public RelayCommand OpenEmailClientCommand
        {
            get
            {
                return _openEmailClientCommand ?? (_openEmailClientCommand = new RelayCommand(() =>
                {
                    Process.Start("mailto:krzysztof-mlynarski@o2.pl?subject=&body=");
                }));
            }
        }
        #endregion

        #region Getters/Setters

        public string VersionNumber
        {
            get
            {
                return _versionNumber;
            }
            set
            {
                _versionNumber = value;
                RaisePropertyChanged();
            }
        }

        #endregion
    }
}

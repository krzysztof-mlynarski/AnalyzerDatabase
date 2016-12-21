using AnalyzerDatabase.Enums;
using AnalyzerDatabase.ViewModels;
using GalaSoft.MvvmLight.Messaging;

namespace AnalyzerDatabase.Messengers
{
    public class ExceptionToSettingsMessage
    {
        public ExtendedViewModelBase Exception { get; set; }
        public string Source { get; set; }
    }
}
using AnalyzerDatabase.ViewModels;

namespace AnalyzerDatabase.Messengers
{
    public class ExceptionToSettingsMessage
    {
        public ExtendedViewModelBase Exception { get; set; }
        public string Source { get; set; }
    }
}
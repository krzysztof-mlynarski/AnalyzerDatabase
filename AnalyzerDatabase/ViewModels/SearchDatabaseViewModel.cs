using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace AnalyzerDatabase.ViewModels
{
    public class SearchDatabaseViewModel :ViewModelBase
    {
        private string _queryTextBox = "";

        public string QueryTextBox
        {
            get
            {
                return _queryTextBox;
            }
            set
            {
                if (_queryTextBox != value)
                {
                    _queryTextBox = value;
                    RaisePropertyChanged("QueryTextBox");
                    //IsSearchButtonEnabled = true;
                }
            }
        }
    }
}

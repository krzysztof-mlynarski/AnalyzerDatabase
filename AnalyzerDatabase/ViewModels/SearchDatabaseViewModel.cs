using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace AnalyzerDatabase.ViewModels
{
    public class SearchDatabaseViewModel : ExtendedViewModelBase
    {
        private RelayCommand _searchAction;
        private string _queryTextBox = "";

        public RelayCommand SearchAction
        {
            get
            {
                return _searchAction ?? (_searchAction = new RelayCommand(Search));               
            }
        }
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

        public async void Search()
        {
            if (!string.IsNullOrEmpty(QueryTextBox))
            {
                try
                {

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


    }
}

using AnalyzerDatabase.Services;
using GalaSoft.MvvmLight.Command;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace AnalyzerDatabase.ViewModels
{
    public class StatisticsViewModel : ExtendedViewModelBase
    {
        #region Variables

        private int _currentScienceDirectCount;
        private int _currentScopusCount;
        private int _currentSpringerCount;
        private int _currentIeeeXploreCount;
        private int _currentDuplicateCount;
        private int _currentPublicationsDownloadCount;
        private int _currentSumCount;

        private bool _isVisibility = false;

        private RelayCommand _refreshData;

        public SeriesCollection SeriesCollectionSearchCount { get; set; }
        public SeriesCollection SeriesCollectionDuplicateAndDownloadCount { get; set; }
        public SeriesCollection SeriesCollectionByYear { get; set; }

        public string[] Labels { get; set; }

        #endregion

        #region Constructor

        public StatisticsViewModel()
        {
            CurrentScienceDirectCount = StatisticsDataService.Instance.GetStatistics.ScienceDirectCount;
            CurrentScopusCount = StatisticsDataService.Instance.GetStatistics.ScopusCount;
            CurrentSpringerCount = StatisticsDataService.Instance.GetStatistics.SpringerCount;
            CurrentIeeeXploreCount = StatisticsDataService.Instance.GetStatistics.IeeeXploreCount;
            CurrentDuplicateCount = StatisticsDataService.Instance.GetStatistics.DuplicateCount;
            CurrentPublicationsDownloadCount = StatisticsDataService.Instance.GetStatistics.PublicationsDownloadCount;
            CurrentSumCount = StatisticsDataService.Instance.GetStatistics.SumCount;

            #region SeriesCollectionSearchCount
            SeriesCollectionSearchCount = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "ScienceDirect",
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(CurrentScienceDirectCount)
                    },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Scopus",
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(CurrentScopusCount)
                    },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Springer",
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(CurrentSpringerCount)
                    },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "IEEE Xplore",
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(CurrentIeeeXploreCount)
                    },
                    DataLabels = true
                }
            };
            #endregion
            #region SeriesCollectionDuplicateAndDownloadCount
            SeriesCollectionDuplicateAndDownloadCount = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "Publikacje",
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(CurrentPublicationsDownloadCount)
                    },
                    DataLabels = true
                },
                new RowSeries
                {
                    Title = "Duplikaty",
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(CurrentDuplicateCount)
                    },
                    DataLabels = true
                }
            };

            Labels = new[] {"Publikacje", "Duplikaty"};
            #endregion

            SeriesCollectionByYear = new SeriesCollection();
        }

        #endregion

        private void SeriesCollectionYear()
        {
            IsVisibility = true;

            int size1 = StatisticsDataService.Instance.ListYearAmount.Count;

            for (int i = 0; i < size1; i++)
            {
                SeriesCollectionByYear.Add(new ColumnSeries
                {
                    Title = StatisticsDataService.Instance.ListYear[i],
                    DataLabels = true
                });
                SeriesCollectionByYear[i].Values = new ChartValues<ObservableValue>();
                var val = StatisticsDataService.Instance.ListYearAmount[i];
                SeriesCollectionByYear[i].Values.Add(new ObservableValue(val));
            }
        }

        public RelayCommand RefreshData
        {
            get
            {
                return _refreshData ?? (_refreshData = new RelayCommand(SeriesCollectionYear));
            }
        }

        #region Getters/Setters
        public int CurrentScienceDirectCount
        {
            get
            {
                return _currentScienceDirectCount;
            }
            set
            {
                if (_currentScienceDirectCount == value)
                    return;

                _currentScienceDirectCount = value;

                StatisticsDataService.Instance.GetStatistics.ScienceDirectCount = _currentScienceDirectCount;
                StatisticsDataService.Instance.SaveStatistics();

                RaisePropertyChanged();
            }
        }

        public int CurrentScopusCount
        {
            get
            {
                return _currentScopusCount;
            }
            set
            {
                if (_currentScopusCount == value)
                    return;

                _currentScopusCount = value;

                StatisticsDataService.Instance.GetStatistics.ScopusCount = _currentScopusCount;
                StatisticsDataService.Instance.SaveStatistics();

                RaisePropertyChanged();
            }
        }

        public int CurrentSpringerCount
        {
            get
            {
                return _currentSpringerCount;
            }
            set
            {
                if (_currentSpringerCount == value)
                    return;

                _currentSpringerCount = value;

                StatisticsDataService.Instance.GetStatistics.SpringerCount = _currentSpringerCount;
                StatisticsDataService.Instance.SaveStatistics();

                RaisePropertyChanged();
            }
        }

        public int CurrentIeeeXploreCount
        {
            get
            {
                return _currentIeeeXploreCount;
            }
            set
            {
                if (_currentIeeeXploreCount == value)
                    return;

                _currentIeeeXploreCount = value;

                StatisticsDataService.Instance.GetStatistics.IeeeXploreCount = _currentIeeeXploreCount;
                StatisticsDataService.Instance.SaveStatistics();

                RaisePropertyChanged();
            }
        }

        public int CurrentDuplicateCount
        {
            get
            {
                return _currentDuplicateCount;
            }
            set
            {
                if (_currentDuplicateCount == value)
                    return;

                _currentDuplicateCount = value;

                StatisticsDataService.Instance.GetStatistics.DuplicateCount = _currentDuplicateCount;
                StatisticsDataService.Instance.SaveStatistics();

                RaisePropertyChanged();
            }
        }

        public int CurrentPublicationsDownloadCount
        {
            get
            {
                return _currentPublicationsDownloadCount;
            }
            set
            {
                if (_currentPublicationsDownloadCount == value)
                    return;

                _currentPublicationsDownloadCount = value;

                StatisticsDataService.Instance.GetStatistics.PublicationsDownloadCount = _currentPublicationsDownloadCount;
                StatisticsDataService.Instance.SaveStatistics();

                RaisePropertyChanged();
            }
        }

        public int CurrentSumCount
        {
            get
            {
                return _currentSumCount;
            }
            set
            {
                //if(_currentSumCount == value)
                //    return;

                _currentSumCount = value;

                StatisticsDataService.Instance.GetStatistics.SumCount = _currentSumCount;
                StatisticsDataService.Instance.SaveStatistics();

                SumCount();
                RaisePropertyChanged();
            }
        }

        public bool IsVisibility
        {
            get
            {
                return _isVisibility;
            }
            set
            {
                _isVisibility = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Private methods
        private void SumCount()
        {
            _currentSumCount = CurrentScienceDirectCount + CurrentScopusCount + CurrentSpringerCount + CurrentIeeeXploreCount;

            StatisticsDataService.Instance.GetStatistics.SumCount = _currentSumCount;
            StatisticsDataService.Instance.SaveStatistics();
        }
        #endregion
    }
}

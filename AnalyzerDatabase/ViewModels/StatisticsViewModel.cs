using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using AnalyzerDatabase.Services;
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
        private int _currentWebOfScienceCount;
        private int _currentIeeeXploreCount;
        private int _currentWileyOnlineLibraryCount;
        private int _currentDuplicateCount;
        private int _currentPublicationsDownloadCount;
        private int _currentSumCount;

        public SeriesCollection SeriesCollectionSearchCount { get; set; }
        public SeriesCollection SeriesCollectionDuplicateAndDownloadCount { get; set; }

        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        public Func<ChartPoint, string> PointLabel { get; set; }

        #endregion

        #region Constructor

        public StatisticsViewModel()
        {
            CurrentScienceDirectCount = StatisticsDataService.Instance.GetStatistics.ScienceDirectCount;
            CurrentScopusCount = StatisticsDataService.Instance.GetStatistics.ScopusCount;
            CurrentSpringerCount = StatisticsDataService.Instance.GetStatistics.SpringerCount;
            CurrentWebOfScienceCount = StatisticsDataService.Instance.GetStatistics.WebOfScienceCount;
            CurrentIeeeXploreCount = StatisticsDataService.Instance.GetStatistics.IeeeXploreCount;
            CurrentWileyOnlineLibraryCount = StatisticsDataService.Instance.GetStatistics.WileyOnlineLibraryCount;
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
                    Title = "Web Of Science",
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(CurrentWebOfScienceCount)
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
                },
                new PieSeries
                {
                    Title = "Wiley Online Library",
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(CurrentWileyOnlineLibraryCount)
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
                    Values = new ChartValues<double> { CurrentPublicationsDownloadCount }
                },
                new RowSeries
                {
                    Title = "Duplikaty",
                    Values = new ChartValues<double> { CurrentDuplicateCount }
                }
            };
            #endregion
        }

        #endregion

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

        public int CurrentWebOfScienceCount
        {
            get
            {
                return _currentWebOfScienceCount;
            }
            set
            {
                if (_currentWebOfScienceCount == value)
                    return;

                _currentWebOfScienceCount = value;

                StatisticsDataService.Instance.GetStatistics.WebOfScienceCount = _currentWebOfScienceCount;
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

        public int CurrentWileyOnlineLibraryCount
        {
            get
            {
                return _currentWileyOnlineLibraryCount;
            }
            set
            {
                if (_currentWileyOnlineLibraryCount == value)
                    return;

                _currentWileyOnlineLibraryCount = value;

                StatisticsDataService.Instance.GetStatistics.WileyOnlineLibraryCount = _currentWileyOnlineLibraryCount;
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
        #endregion

        #region Private methods
        private void SumCount()
        {
            _currentSumCount = CurrentScienceDirectCount + CurrentScopusCount + CurrentSpringerCount +
                               CurrentWebOfScienceCount + CurrentIeeeXploreCount + CurrentWileyOnlineLibraryCount;

            StatisticsDataService.Instance.GetStatistics.SumCount = _currentSumCount;
            StatisticsDataService.Instance.SaveStatistics();
        }
        #endregion
    }
}

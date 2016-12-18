﻿using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AnalyzerDatabase.Services;
using GalaSoft.MvvmLight.Command;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using Microsoft.Win32;

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

        private string _queryTextBox;

        private bool _isVisibility = false;

        private RelayCommand _refreshData;
        private RelayCommand _refreshOverallData;
        private RelayCommand<object> _exportChartToImageCommand;
        private RelayCommand _searchCommand;

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

        #region RelayCommand
        public RelayCommand RefreshData
        {
            get
            {
                return _refreshData ?? (_refreshData = new RelayCommand(SeriesCollectionYear));
            }
        }

        public RelayCommand RefreshOverallData
        {
            get
            {
                return _refreshOverallData ?? (_refreshOverallData = new RelayCommand(SeriesCollectionSearchCount1));
            }
        }

        public RelayCommand<object> ExportChartToImageCommand
        {
            get
            {
                return _exportChartToImageCommand ?? (_exportChartToImageCommand = new RelayCommand<object>(ExportChartToImage));
            }
        }

        public RelayCommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new RelayCommand(TextBoxSearch));
            }
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
                    RaisePropertyChanged();
                }
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

        private void ExportChartToImage(object param)
        {
            var ChartByYear = param as CartesianChart;
            if (ChartByYear == null)
            {
                ShowDialog("ERROR", "There is nothing to export");
            }
            else
            {
                Rect bounds = VisualTreeHelper.GetDescendantBounds(ChartByYear);

                RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
                    (int)bounds.Width, (int)bounds.Height, 96, 96, PixelFormats.Pbgra32);

                DrawingVisual isolatedVisual = new DrawingVisual();
                using (DrawingContext drawing = isolatedVisual.RenderOpen())
                {
                    drawing.DrawRectangle(Brushes.White, null, new Rect(new Point(), bounds.Size));
                    drawing.DrawRectangle(new VisualBrush(ChartByYear), null, new Rect(
                        new Point(), bounds.Size));
                }

                renderBitmap.Render(isolatedVisual);

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = "Chart by year_" + DateTime.Today.DayOfWeek,
                    DefaultExt = "png"
                };

                bool? result = saveFileDialog.ShowDialog();
                if (result == true)
                {
                    string obrCesta = saveFileDialog.FileName;

                    using (FileStream outStream = new FileStream(obrCesta, FileMode.Create))
                    {
                        PngBitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                        encoder.Save(outStream);
                    }
                }
            }
        }

        private void TextBoxSearch()
        {
            if (!String.IsNullOrEmpty(QueryTextBox))
            {
                string records = null;
                double val = 0;

                for (int i = 0; i < StatisticsDataService.Instance.ListYear.Count; i++)
                {
                    for (int j = 0; j < StatisticsDataService.Instance.ListYearAmount.Count; j++)
                    {
                        var item1 = StatisticsDataService.Instance.ListYear[j];
                        var item2 = StatisticsDataService.Instance.ListYearAmount[j];
                        if (QueryTextBox.Equals(item1))
                        {
                            records = item1;
                            val = item2;
                            break;
                        }
                    }
                }

                SeriesCollectionByYear.Clear();

                SeriesCollectionByYear.Add(new ColumnSeries
                {
                    Title = records,
                    DataLabels = true
                });
                SeriesCollectionByYear[0].Values = new ChartValues<ObservableValue>
                {
                    new ObservableValue(val)
                };
            }
        }

        private void SeriesCollectionSearchCount1()
        {
            SeriesCollectionSearchCount.Clear();
            SeriesCollectionDuplicateAndDownloadCount.Clear();

            SeriesCollectionSearchCount.Add(new PieSeries
            {
                Title = "ScienceDirect",
                Values = new ChartValues<ObservableValue>
                {
                    new ObservableValue(CurrentScienceDirectCount)
                },
                DataLabels = true
            });

            SeriesCollectionSearchCount.Add(new PieSeries
            {
                Title = "Scopus",
                Values = new ChartValues<ObservableValue>
                {
                    new ObservableValue(CurrentScopusCount)
                },
                DataLabels = true
            });

            SeriesCollectionSearchCount.Add(new PieSeries
            {
                Title = "Springer",
                Values = new ChartValues<ObservableValue>
                {
                    new ObservableValue(CurrentSpringerCount)
                },
                DataLabels = true
            });

            SeriesCollectionSearchCount.Add(new PieSeries
            {
                Title = "IEEE Xplore",
                Values = new ChartValues<ObservableValue>
                {
                    new ObservableValue(CurrentIeeeXploreCount)
                },
                DataLabels = true
            });

            SeriesCollectionDuplicateAndDownloadCount.Add(new RowSeries
            {
                Title = "Publikacje",
                Values = new ChartValues<ObservableValue>
                {
                    new ObservableValue(CurrentPublicationsDownloadCount)
                },
                DataLabels = true
            });

            SeriesCollectionDuplicateAndDownloadCount.Add(new RowSeries
            {
                Title = "Duplikaty",
                Values = new ChartValues<ObservableValue>
                {
                    new ObservableValue(CurrentDuplicateCount)
                },
                DataLabels = true
            });
        }

        private void SeriesCollectionYear()
        {
            QueryTextBox = null;
            SeriesCollectionByYear.Clear();
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
        #endregion
    }
}

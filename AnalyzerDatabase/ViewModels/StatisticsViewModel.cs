using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AnalyzerDatabase.Models;
using AnalyzerDatabase.Services;
using GalaSoft.MvvmLight.Command;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
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
        private readonly string _currentPublicationSavingPath;

        private bool _isDataOverallEmpty = true;
        private bool _isDataByYearEmpty = true;
        private bool _isDataByAuthorEmpty = true;
        private bool _isDataByMagazineEmpty = true;

        private bool _chartDataOverall1Loading;
        private bool _dataGridOverall1Loading;

        private bool _chartDataOverall2Loading;
        private bool _dataGridOverall2Loading;

        private bool _chartDataByYearLoading;
        private bool _dataGridByYearLoading;
        private bool _chartDataByYearFullLoading;
        private bool _dataGridByYearFullLoading;

        private bool _chartDataByAuthorLoading;
        private bool _dataGridByAuthorLoading;

        private bool _chartDataLoading; //Magazine
        private bool _dataGridLoading;
        private bool _chartDataByMagazineFullLoading;
        private bool _dataGridByMagazineFullLoading;

        private string _queryTextBox;

        private int _valueLabelRotation;
        private int _valueFontSize;

        private RelayCommand _refreshDataOverall1;
        private RelayCommand _refreshDataGridOverall1;

        private RelayCommand _refreshDataOverall2;
        private RelayCommand _refreshDataGridOverall2;

        private RelayCommand _refreshDataByYear;
        private RelayCommand _refreshDataByYearFull;
        private RelayCommand _refreshDataGridByYear;
        private RelayCommand _refreshDataGridByYearFull;

        private RelayCommand _refreshDataByAuthor;
        private RelayCommand _refreshDataGridByAuthor;

        private RelayCommand _refreshDataByMagazine;
        private RelayCommand _refreshDataGridByMagazine;
        private RelayCommand _refreshDataByMagazineFull;
        private RelayCommand _refreshDataGridByMagazineFull;
       
        private RelayCommand<object> _exportChartToImageCommand;
        private RelayCommand _searchCommand;
        private RelayCommand _exportChartDataToCsvCommand1;
        private RelayCommand _exportChartDataToCsvCommand2;
        private RelayCommand _exportChartDataToCsvCommand3;
        private RelayCommand _exportChartDataToCsvCommand3_1;
        private RelayCommand _exportChartDataToCsvCommand4;
        private RelayCommand _exportChartDataToCsvCommand4_1;

        private SeriesCollection _seriesCollectionSearchCount;
        private SeriesCollection _seriesCollectionDuplicateAndDownloadCount;
        private SeriesCollection _seriesCollectionByYear;
        private SeriesCollection _seriesCollectionByYearFull;
        private SeriesCollection _seriesCollectionByAuthor;
        private SeriesCollection _seriesCollectionByMagazine;
        private SeriesCollection _seriesCollectionByMagazineFull;

        private ObservableCollection<OverallStatistics> _overallStatisticsObservableCollection;
        private ObservableCollection<OverallStatistics2> _overallStatistics2ObservableCollection;
        private ObservableCollection<ByYear> _dataByYearObservableCollection;
        private ObservableCollection<ByYear> _dataByYearObservableCollectionFull;
        private ObservableCollection<ByAuthor> _dataByAuthorObservableCollection;
        private ObservableCollection<ByMagazine> _dataByMagazineObservableCollection;
        private ObservableCollection<ByMagazine> _dataByMagazineObservableCollectionFull;

        private string[] _labelsYear;
        private string[] _labelsYearFull;
        private string[] _labelsMagazine;
        private string[] _labelsAuthor;

        public Func<double, string> Formatter { get; set; }

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
            _currentPublicationSavingPath = SettingsService.Instance.Settings.SavingPublicationPath;

            SeriesCollectionSearchCount = new SeriesCollection();
            #region SeriesCollectionDuplicateAndDownload
            SeriesCollectionDuplicateAndDownloadCount = new SeriesCollection
            {
                new PieSeries
                {
                    //TODO: jezyki
                    Title = "Publikacje",
                    Fill = Brushes.Green,
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(CurrentPublicationsDownloadCount)
                    },
                    DataLabels = true
                },
                new PieSeries
                {
                    //TODO: jezyki
                    Title = "Duplikaty",
                    Fill = Brushes.OrangeRed,
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(CurrentDuplicateCount)
                    },
                    DataLabels = true
                }
            };
            #endregion
            SeriesCollectionByYear = new SeriesCollection();
            SeriesCollectionByYearFull = new SeriesCollection();
            SeriesCollectionByAuthor = new SeriesCollection();
            SeriesCollectionByMagazine = new SeriesCollection();
            SeriesCollectionByMagazineFull = new SeriesCollection();

            OverallStatisticsObservableCollection = new ObservableCollection<OverallStatistics>();
            OverallStatistics2ObservableCollection = new ObservableCollection<OverallStatistics2>();
            DataByYearObservableCollection = new ObservableCollection<ByYear>();
            DataByYearObservableCollectionFull = new ObservableCollection<ByYear>();
            DataByAuthorObservableCollection = new ObservableCollection<ByAuthor>();
            DataByMagazineObservableCollection = new ObservableCollection<ByMagazine>();
            DataByMagazineObservableCollectionFull = new ObservableCollection<ByMagazine>();
        }

        #endregion

        #region RelayCommand

        public RelayCommand RefreshDataOverall1
        {
            get
            {
                return _refreshDataOverall1 ?? (_refreshDataOverall1 = new RelayCommand(FillSeriesCollectionSearchCount));
            }
        }
        public RelayCommand RefreshDataGridOverall1
        {
            get
            {
                return _refreshDataGridOverall1 ?? (_refreshDataGridOverall1 = new RelayCommand(FillDataGridSearchCount));
            }
        }
        public RelayCommand RefreshDataOverall2
        {
            get
            {
                return _refreshDataOverall2 ?? (_refreshDataOverall2 = new RelayCommand(FillSeriesCollectionDuplicateAndDownloadCount));
            }
        }
        public RelayCommand RefreshDataGridOverall2
        {
            get
            {
                return _refreshDataGridOverall2 ?? (_refreshDataGridOverall2 = new RelayCommand(FillDataGridDuplicateAndDownloadCount));
            }
        }
        public RelayCommand RefreshDataByYear
        {
            get
            {
                return _refreshDataByYear ?? (_refreshDataByYear = new RelayCommand(FillSeriesCollectionYear));
            }
        }
        public RelayCommand RefreshDataByYearFull
        {
            get
            {
                return _refreshDataByYearFull ?? (_refreshDataByYearFull = new RelayCommand(FillSeriesCollectionYearFull));
            }
        }    
        public RelayCommand RefreshDataGridByYear
        {
            get
            {
                return _refreshDataGridByYear ?? (_refreshDataGridByYear = new RelayCommand(FillDataGridByYear));
            }
        }
        public RelayCommand RefreshDataGridByYearFull
        {
            get
            {
                return _refreshDataGridByYearFull ?? (_refreshDataGridByYearFull = new RelayCommand(FillDataGridByYearFull));
            }
        }
        public RelayCommand RefreshDataByAuthor
        {
            get
            {
                return _refreshDataByAuthor ?? (_refreshDataByAuthor = new RelayCommand(FillSeriesCollectionByAuthor));
            }
        }
        public RelayCommand RefreshDataGridByAuthor
        {
            get
            {
                return _refreshDataGridByAuthor ?? (_refreshDataGridByAuthor = new RelayCommand(FillDataGridByAuthor));
            }
        }
        public RelayCommand RefreshDataByMagazineChart
        {
            get
            {
                return _refreshDataByMagazine ?? (_refreshDataByMagazine = new RelayCommand(FillSeriesCollectionMagazine));
            }
        }
        public RelayCommand RefreshDataByMagazineChartFull
        {
            get
            {
                return _refreshDataByMagazineFull ?? (_refreshDataByMagazineFull = new RelayCommand(FillSeriesCollectionByMagazineFull));
            }
        }
        public RelayCommand RefreshDataByMagazineDataGrid
        {
            get
            {
                return _refreshDataGridByMagazine ?? (_refreshDataGridByMagazine = new RelayCommand(FillDataGridByMagazine));
            }
        }
        public RelayCommand RefreshDataByMagazineDataGridFull
        {
            get
            {
                return _refreshDataGridByMagazineFull ?? (_refreshDataGridByMagazineFull = new RelayCommand(FillDataGridByMagazineFull));
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
        public RelayCommand ExportChartDataToCsvCommand1
        {
            get
            {
                return _exportChartDataToCsvCommand1 ?? (_exportChartDataToCsvCommand1 = new RelayCommand(() =>
                {
                    ExportDataToCsv.Instance.ExportChartDataToCsv1();
                }));
            }
        }
        public RelayCommand ExportChartDataToCsvCommand2
        {
            get
            {
                return _exportChartDataToCsvCommand2 ?? (_exportChartDataToCsvCommand2 = new RelayCommand(() =>
                {
                    ExportDataToCsv.Instance.ExportChartDataToCsv2();
                }));
            }
        }
        public RelayCommand ExportChartDataToCsvCommand3
        {
            get
            {
                return _exportChartDataToCsvCommand3 ?? (_exportChartDataToCsvCommand3 = new RelayCommand(() =>
                {
                    ExportDataToCsv.Instance.ExportChartDataToCsv3();      
                }));
            }
        }
        public RelayCommand ExportChartDataToCsvCommand3_1
        {
            get
            {
                return _exportChartDataToCsvCommand3_1 ?? (_exportChartDataToCsvCommand3_1 = new RelayCommand(() =>
                {
                    ExportDataToCsv.Instance.ExportChartDataToCsv3_1();
                }));
            }
        }
        public RelayCommand ExportChartDataToCsvCommand4
        {
            get
            {
                return _exportChartDataToCsvCommand4 ?? (_exportChartDataToCsvCommand4 = new RelayCommand(() =>
                {
                    ExportDataToCsv.Instance.ExportChartDataToCsv4();
                }));
            }
        }
        public RelayCommand ExportChartDataToCsvCommand4_1
        {
            get
            {
                return _exportChartDataToCsvCommand4_1 ?? (_exportChartDataToCsvCommand4_1 = new RelayCommand(() =>
                {
                    ExportDataToCsv.Instance.ExportChartDataToCsv4_1();
                }));
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

        public bool IsDataOverallEmpty
        {
            get
            {
                return _isDataOverallEmpty;
            }
            set
            {
                _isDataOverallEmpty = value;
                RaisePropertyChanged();
            }
        }

        public bool IsDataByYearEmpty
        {
            get
            {
                return _isDataByYearEmpty;
            }
            set
            {
                _isDataByYearEmpty = value;
                RaisePropertyChanged();
            }
        }

        public bool IsDataByAuthorEmpty
        {
            get
            {
                return _isDataByAuthorEmpty;
            }
            set
            {
                _isDataByAuthorEmpty = value;
                RaisePropertyChanged();
            }
        }

        public bool IsDataByMagazineEmpty
        {
            get
            {
                return _isDataByMagazineEmpty;
            }
            set
            {
                _isDataByMagazineEmpty = value;
                RaisePropertyChanged();
            }
        }

        public bool ChartDataOverall1Loading
        {
            get
            {
                return _chartDataOverall1Loading;
            }
            set
            {
                _chartDataOverall1Loading = value;
                RaisePropertyChanged();
            }
        }

        public bool DataGridOverall1Loading
        {
            get
            {
                return _dataGridOverall1Loading;
            }
            set
            {
                _dataGridOverall1Loading = value;
                RaisePropertyChanged();
            }
        }

        public bool ChartDataOverall2Loading
        {
            get
            {
                return _chartDataOverall2Loading;
            }
            set
            {
                _chartDataOverall2Loading = value;
                RaisePropertyChanged();
            }
        }

        public bool DataGridOverall2Loading
        {
            get
            {
                return _dataGridOverall2Loading;
            }
            set
            {
                _dataGridOverall2Loading = value;
                RaisePropertyChanged();
            }
        }

        public bool ChartDataByYearLoading
        {
            get
            {
                return _chartDataByYearLoading;
            }
            set
            {
                _chartDataByYearLoading = value;
                RaisePropertyChanged();
            }
        }

        public bool ChartDataByYearFullLoading
        {
            get
            {
                return _chartDataByYearFullLoading;
            }
            set
            {
                _chartDataByYearFullLoading = value;
                RaisePropertyChanged();
            }
        }

        public bool DataGridByYearLoading
        {
            get
            {
                return _dataGridByYearLoading;
            }
            set
            {
                _dataGridByYearLoading = value;
                RaisePropertyChanged();
            }
        }

        public bool DataGridByYearFullLoading
        {
            get
            {
                return _dataGridByYearFullLoading;
            }
            set
            {
                _dataGridByYearFullLoading = value;
                RaisePropertyChanged();
            }
        }

        public bool ChartDataByAuthorLoading
        {
            get
            {
                return _chartDataByAuthorLoading;
            }
            set
            {
                _chartDataByAuthorLoading = value;
                RaisePropertyChanged();
            }
        }

        public bool DataGridByAuthorLoading
        {
            get
            {
                return _dataGridByAuthorLoading;
            }
            set
            {
                _dataGridByAuthorLoading = value;
                RaisePropertyChanged();
            }
        }

        public bool ChartDataLoading
        {
            get
            {
                return _chartDataLoading;
            }
            set
            {
                _chartDataLoading = value;
                RaisePropertyChanged();
            }
        }

        public bool DataGridLoading
        {
            get
            {
                return _dataGridLoading;
            }
            set
            {
                _dataGridLoading = value;
                RaisePropertyChanged();
            }
        }

        public bool ChartDataByMagazineFullLoading
        {
            get
            {
                return _chartDataByMagazineFullLoading;
            }
            set
            {
                _chartDataByMagazineFullLoading = value;
                RaisePropertyChanged();
            }
        }

        public bool DataGridByMagazineFullLoading
        {
            get
            {
                return _dataGridByMagazineFullLoading;
            }
            set
            {
                _dataGridByMagazineFullLoading = value;
                RaisePropertyChanged();
            }
        }

        public string[] LabelsYear
        {
            get
            {
                return _labelsYear;
            }
            set
            {
                _labelsYear = value;
                RaisePropertyChanged();
            }
        }

        public string[] LabelsYearFull
        {
            get
            {
                return _labelsYearFull;
            }
            set
            {
                _labelsYearFull = value;
                RaisePropertyChanged();
            }
        }

        public string[] LabelsMagazine
        {
            get
            {
                return _labelsMagazine;
            }
            set
            {
                _labelsMagazine = value;
                RaisePropertyChanged();
            }
        }

        public string[] LabelsAuthor
        {
            get
            {
                return _labelsAuthor;
            }
            set
            {
                _labelsAuthor = value;
                RaisePropertyChanged();
            }
        }

        public SeriesCollection SeriesCollectionSearchCount
        {
            get
            {
                return _seriesCollectionSearchCount;
            }
            set
            {
                _seriesCollectionSearchCount = value;
                RaisePropertyChanged();
            }
        }

        public SeriesCollection SeriesCollectionDuplicateAndDownloadCount
        {
            get
            {
                return _seriesCollectionDuplicateAndDownloadCount;
            }
            set
            {
                _seriesCollectionDuplicateAndDownloadCount = value;
                RaisePropertyChanged();
            }
        }

        public SeriesCollection SeriesCollectionByYear
        {
            get
            {
                return _seriesCollectionByYear;
            }
            set
            {
                _seriesCollectionByYear = value;
                RaisePropertyChanged();
            }
        }

        public SeriesCollection SeriesCollectionByYearFull
        {
            get
            {
                return _seriesCollectionByYearFull;
            }
            set
            {
                _seriesCollectionByYearFull = value;
                RaisePropertyChanged();
            }
        }

        public SeriesCollection SeriesCollectionByAuthor
        {
            get
            {
                return _seriesCollectionByAuthor;
            }
            set
            {
                _seriesCollectionByAuthor = value;
                RaisePropertyChanged();
            }
        }

        public SeriesCollection SeriesCollectionByMagazine
        {
            get
            {
                return _seriesCollectionByMagazine;
            }
            set
            {
                _seriesCollectionByMagazine = value;
                RaisePropertyChanged();
            }
        }

        public SeriesCollection SeriesCollectionByMagazineFull
        {
            get
            {
                return _seriesCollectionByMagazineFull;
            }
            set
            {
                _seriesCollectionByMagazineFull = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<OverallStatistics> OverallStatisticsObservableCollection
        {
            get
            {
                return _overallStatisticsObservableCollection;
            }
            set
            {
                _overallStatisticsObservableCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<OverallStatistics2> OverallStatistics2ObservableCollection
        {
            get
            {
                return _overallStatistics2ObservableCollection;
            }
            set
            {
                _overallStatistics2ObservableCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ByYear> DataByYearObservableCollection
        {
            get
            {
                return _dataByYearObservableCollection;
            }
            set
            {
                _dataByYearObservableCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ByYear> DataByYearObservableCollectionFull
        {
            get
            {
                return _dataByYearObservableCollectionFull;
            }
            set
            {
                _dataByYearObservableCollectionFull = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ByAuthor> DataByAuthorObservableCollection
        {
            get
            {
                return _dataByAuthorObservableCollection;
            }
            set
            {
                _dataByAuthorObservableCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ByMagazine> DataByMagazineObservableCollection
        {
            get
            {
                return _dataByMagazineObservableCollection;
            }
            set
            {
                _dataByMagazineObservableCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ByMagazine> DataByMagazineObservableCollectionFull
        {
            get
            {
                return _dataByMagazineObservableCollectionFull;
            }
            set
            {
                _dataByMagazineObservableCollectionFull = value;
                RaisePropertyChanged();
            }
        }

        public int ValueLabelRotation
        {
            get
            {
                return _valueLabelRotation;
            }
            set
            {
                _valueLabelRotation = value;
                RaisePropertyChanged();
            }
        }

        public int ValueFontSize
        {
            get
            {
                return _valueFontSize;
            }
            set
            {
                _valueFontSize = value;
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

        private void ExportChartToImage(object param)
        {
            var chartOverall = param as PieChart;
            var chartOverallOrYear = param as CartesianChart;

            if (chartOverall != null || chartOverallOrYear != null)
            {
                ExportToImage(chartOverallOrYear, chartOverall);
                chartOverall = null;
                chartOverallOrYear = null;
            }
            else
                //TODO: jezyki
                ShowDialog("ERROR", "There is nothing to export");
        }

        private async void ExportToImage(CartesianChart valueCartesianChart, PieChart valuePieChart)
        {
            var chartOverall = valuePieChart;
            var chartOverallOrYear = valueCartesianChart;
            Visual chart;

            if (chartOverall == null)
            {
                chart = chartOverallOrYear;
            }
            else
            {
                chart = chartOverall;
            }

            Rect bounds = VisualTreeHelper.GetDescendantBounds(chart);

            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
                (int)bounds.Width, (int)bounds.Height, 96, 96, PixelFormats.Pbgra32);

            DrawingVisual isolatedVisual = new DrawingVisual();
            using (DrawingContext drawing = isolatedVisual.RenderOpen())
            {
                drawing.DrawRectangle(Brushes.White, null, new Rect(new Point(), bounds.Size));
                drawing.DrawRectangle(new VisualBrush(chart), null, new Rect(new Point(), bounds.Size));
            }

            renderBitmap.Render(isolatedVisual);

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PNG (*.png)|*.png",
                FileName = "ChartData_" + DateTime.Now.ToString("yyyy_hh_mm_ss"),
                InitialDirectory = _currentPublicationSavingPath
            };

            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                string fileName = saveFileDialog.FileName;

                using (FileStream outStream = new FileStream(fileName, FileMode.Create))
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                    encoder.Save(outStream);
                }

                //TODO: jezyki
                if (await ConfirmationDialog("Potwierdź", "Czy otworzyc wyeksportowany plik?"))
                    Process.Start(saveFileDialog.FileName);
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

        private void FillSeriesCollectionSearchCount()
        {
            IsDataOverallEmpty = false;
            ChartDataOverall1Loading = true;
            DataGridOverall1Loading = false;

            SeriesCollectionSearchCount.Clear();
            
            SeriesCollectionSearchCount.Add(new PieSeries
            {
                Title = "ScienceDirect",
                DataLabels = true
            });
            SeriesCollectionSearchCount[0].Values = new ChartValues<ObservableValue>
            {
                new ObservableValue(CurrentScienceDirectCount)
            };

            SeriesCollectionSearchCount.Add(new PieSeries
            {
                Title = "Scopus",
                DataLabels = true
            });
            SeriesCollectionSearchCount[1].Values = new ChartValues<ObservableValue>
            {
                new ObservableValue(CurrentScopusCount)
            };

            SeriesCollectionSearchCount.Add(new PieSeries
            {
                Title = "Springer",
                DataLabels = true
            });
            SeriesCollectionSearchCount[2].Values = new ChartValues<ObservableValue>
            {
                new ObservableValue(CurrentSpringerCount)
            };

            SeriesCollectionSearchCount.Add(new PieSeries
            {
                Title = "IEEE Xplore",
                DataLabels = true
            });
            SeriesCollectionSearchCount[3].Values = new ChartValues<ObservableValue>
            {
                new ObservableValue(CurrentIeeeXploreCount)
            };
        }

        private void FillDataGridSearchCount()
        {
            IsDataOverallEmpty = false;
            ChartDataOverall1Loading = false;
            DataGridOverall1Loading = true;

            OverallStatisticsObservableCollection.Clear();

            OverallStatisticsObservableCollection.Add(new OverallStatistics("Science Direct", StatisticsDataService.Instance.GetStatistics.ScienceDirectCount));
            OverallStatisticsObservableCollection.Add(new OverallStatistics("Scopus", StatisticsDataService.Instance.GetStatistics.ScopusCount));
            OverallStatisticsObservableCollection.Add(new OverallStatistics("Springer", StatisticsDataService.Instance.GetStatistics.SpringerCount));
            OverallStatisticsObservableCollection.Add(new OverallStatistics("IEEE Xplore", StatisticsDataService.Instance.GetStatistics.IeeeXploreCount));
        }

        private void FillSeriesCollectionDuplicateAndDownloadCount()
        {
            IsDataOverallEmpty = false;
            ChartDataOverall2Loading = true;
            DataGridOverall2Loading = false;

            //SeriesCollectionDuplicateAndDownloadCount.Clear();

            //SeriesCollectionDuplicateAndDownloadCount.Add(new RowSeries
            //{
            //    //TODO: jezyki
            //    Title = "Publikacje",
            //    Fill = Brushes.Green,
            //    DataLabels = true
            //});
            //SeriesCollectionDuplicateAndDownloadCount[0].Values = new ChartValues<ObservableValue>
            //{
            //    new ObservableValue(CurrentPublicationsDownloadCount)
            //};

            //SeriesCollectionDuplicateAndDownloadCount.Add(new RowSeries
            //{
            //    //TODO: jezyki
            //    Title = "Duplikaty",
            //    Fill = Brushes.OrangeRed,
            //    DataLabels = true
            //});
            //SeriesCollectionDuplicateAndDownloadCount[1].Values = new ChartValues<ObservableValue>
            //{
            //    new ObservableValue(CurrentDuplicateCount)
            //}; 
        }

        private void FillDataGridDuplicateAndDownloadCount()
        {
            IsDataOverallEmpty = false;
            ChartDataOverall2Loading = false;
            DataGridOverall2Loading = true;

            OverallStatistics2ObservableCollection.Clear();

            //TODO: jezyki
            OverallStatistics2ObservableCollection.Add(new OverallStatistics2("Publikacje", StatisticsDataService.Instance.GetStatistics.PublicationsDownloadCount));
            OverallStatistics2ObservableCollection.Add(new OverallStatistics2("Duplikaty", StatisticsDataService.Instance.GetStatistics.DuplicateCount));
        }

        private void FillSeriesCollectionYear()
        {
            QueryTextBox = null;
            IsDataByYearEmpty = false;
            ChartDataByYearLoading = true;
            ChartDataByYearFullLoading = false;
            DataGridByYearLoading = false;
            DataGridByYearFullLoading = false;

            LabelsYear = new string[StatisticsDataService.Instance.ListYear.Count];

            var columnSeries = new ColumnSeries
            {
                //TODO: 
                Title = "Wystąpił",
                Values = new ChartValues<int>(),
                DataLabels = true
            };

            for (int i = 0; i < StatisticsDataService.Instance.ListYear.Count; i++)
            {
                columnSeries.Values.Add(StatisticsDataService.Instance.ListYearAmount[i]);
                LabelsYear[i] = StatisticsDataService.Instance.ListYear[i];
            }

            SeriesCollectionByYear = new SeriesCollection {columnSeries};
        }

        private void FillSeriesCollectionYearFull()
        {
            IsDataByYearEmpty = false;
            ChartDataByYearLoading = false;
            ChartDataByYearFullLoading = true;
            DataGridByYearLoading = false;
            DataGridByYearFullLoading = false;

            LabelsYearFull = new string[StatisticsDataService.Instance.ListYearFull.Count];

            var columnSeries2 = new ColumnSeries
            {
                //TODO: 
                Title = "Wystąpił",
                Values = new ChartValues<int>(),
                DataLabels = true
            };

            for (int i = 0; i < StatisticsDataService.Instance.ListYearFull.Count; i++)
            {
                columnSeries2.Values.Add(StatisticsDataService.Instance.ListYearAmountFull[i]);
                LabelsYearFull[i] = StatisticsDataService.Instance.ListYearFull[i];
            }

            if (StatisticsDataService.Instance.ListYearFull.Count >= 30)
                ValueLabelRotation = 20;
            if (StatisticsDataService.Instance.ListYearFull.Count >= 34)
                ValueFontSize = 9;

            SeriesCollectionByYearFull = new SeriesCollection { columnSeries2 };
        }

        private void FillDataGridByYear()
        {
            IsDataByYearEmpty = false;
            ChartDataByYearLoading = false;
            ChartDataByYearFullLoading = false;
            DataGridByYearLoading = true;
            DataGridByYearFullLoading = false;

            DataByYearObservableCollection.Clear();

            for (int i = 0; i < StatisticsDataService.Instance.ListYear.Count; i++)
            {
                var year = StatisticsDataService.Instance.ListYear[i];
                var amount = StatisticsDataService.Instance.ListYearAmount[i];
                DataByYearObservableCollection.Add(new ByYear(year, amount));
            }
        }

        private void FillDataGridByYearFull()
        {
            IsDataByYearEmpty = false;
            ChartDataByYearLoading = false;
            ChartDataByYearFullLoading = false;
            DataGridByYearLoading = false;
            DataGridByYearFullLoading = true;

            DataByYearObservableCollectionFull.Clear();

            for (int i = 0; i < StatisticsDataService.Instance.ListYearFull.Count; i++)
            {
                var year = StatisticsDataService.Instance.ListYearFull[i];
                var amount = StatisticsDataService.Instance.ListYearAmountFull[i];
                DataByYearObservableCollectionFull.Add(new ByYear(year, amount));
            }
        }

        private void FillSeriesCollectionByAuthor()
        {
            IsDataByAuthorEmpty = false;
            ChartDataByAuthorLoading = true;
            DataGridByAuthorLoading = false;
        }

        private void FillDataGridByAuthor()
        {
            IsDataByAuthorEmpty = false;
            ChartDataByAuthorLoading = false;
            DataGridByAuthorLoading = true;
        }

        private void FillSeriesCollectionMagazine()
        {
            IsDataByMagazineEmpty = false;
            ChartDataLoading = true;
            DataGridLoading = false;
            ChartDataByMagazineFullLoading = false;
            DataGridByMagazineFullLoading = false; 
                  
            SeriesCollectionByMagazine.Clear();
            //LabelsMagazine = new string[StatisticsDataService.Instance.ListMagazine.Count];

            for (int i = 0; i < StatisticsDataService.Instance.ListMagazineAmount.Count; i++)
            {
                SeriesCollectionByMagazine.Add(new ColumnSeries
                {
                        Title = StatisticsDataService.Instance.ListMagazine[i],
                        Fill = Brushes.CornflowerBlue,
                        DataLabels =  true
                });
                SeriesCollectionByMagazine[i].Values = new ChartValues<ObservableValue>();
                var val = StatisticsDataService.Instance.ListMagazineAmount[i];
                SeriesCollectionByMagazine[i].Values.Add(new ObservableValue(val));
                //LabelsMagazine[i] = StatisticsDataService.Instance.ListMagazine[i];
            }
        }

        private void FillSeriesCollectionByMagazineFull()
        {
            IsDataByMagazineEmpty = false;
            ChartDataByMagazineFullLoading = true;
            ChartDataLoading = false;
            DataGridByMagazineFullLoading = false;
            DataGridLoading = false;


        }

        private void FillDataGridByMagazine()
        {
            IsDataByMagazineEmpty = false;
            ChartDataLoading = false;
            DataGridLoading = true;
            ChartDataByMagazineFullLoading = false;
            DataGridByMagazineFullLoading = false;

            DataByMagazineObservableCollection.Clear();
            
            for (int i = 0; i < StatisticsDataService.Instance.ListMagazine.Count; i++)
            {
                var name = StatisticsDataService.Instance.ListMagazine[i];
                var amount = StatisticsDataService.Instance.ListMagazineAmount[i];
                DataByMagazineObservableCollection.Add(new ByMagazine(name, amount));
            }
        }

        private void FillDataGridByMagazineFull()
        {
            IsDataByMagazineEmpty = false;
            ChartDataByMagazineFullLoading = false;
            ChartDataLoading = false;
            DataGridByMagazineFullLoading = true;
            DataGridLoading = false;

            DataByMagazineObservableCollectionFull.Clear();

            for (int i = 0; i < StatisticsDataService.Instance.ListMagazineFull.Count; i++)
            {
                var name = StatisticsDataService.Instance.ListMagazineFull[i];
                var amount = StatisticsDataService.Instance.ListMagazineAmountFull[i];
                DataByMagazineObservableCollectionFull.Add(new ByMagazine(name, amount));
            }
        }
        #endregion
    }
}

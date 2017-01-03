using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using AnalyzerDatabase.Interfaces;
using AnalyzerDatabase.ViewModels;
using CsvHelper;
using Microsoft.Win32;

namespace AnalyzerDatabase.Services
{
    public class ExportDataToCsv : ExtendedViewModelBase
    {
        #region Variables
        private readonly string _currentPublicationSavingPath;
        private readonly int _currentScienceDirectCount;
        private readonly int _currentScopusCount;
        private readonly int _currentSpringerCount;
        private readonly int _currentIeeeXploreCount;
        private readonly int _currentDuplicateCount;
        private readonly int _currentPublicationsDownloadCount;
        #endregion

        #region Singleton
        private static ExportDataToCsv _instance;

        public static ExportDataToCsv Instance
        {
            get
            {
                return _instance ?? (_instance = new ExportDataToCsv());
            }
        }

        public ExportDataToCsv()
        {
            _currentScienceDirectCount = StatisticsDataService.Instance.GetStatistics.ScienceDirectCount;
            _currentScopusCount = StatisticsDataService.Instance.GetStatistics.ScopusCount;
            _currentSpringerCount = StatisticsDataService.Instance.GetStatistics.SpringerCount;
            _currentIeeeXploreCount = StatisticsDataService.Instance.GetStatistics.IeeeXploreCount;
            _currentDuplicateCount = StatisticsDataService.Instance.GetStatistics.DuplicateCount;
            _currentPublicationsDownloadCount = StatisticsDataService.Instance.GetStatistics.PublicationsDownloadCount;
            _currentPublicationSavingPath = SettingsService.Instance.Settings.SavingPublicationPath;
        }
        #endregion

        #region Public Methods
        public async void ExportChartDataToCsv1()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = "ChartData_" + DateTime.Now.ToString("yyyy_hh_mm_ss"),
                InitialDirectory = _currentPublicationSavingPath
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var streamWriter = File.CreateText(saveFileDialog.FileName))
                {
                    var writer = new CsvWriter(streamWriter);
                    writer.Configuration.Delimiter = ";";
                    writer.WriteField("Science Direct");
                    writer.WriteField(_currentScienceDirectCount);
                    writer.NextRecord();
                    writer.WriteField("Scopus");
                    writer.WriteField(_currentScopusCount);
                    writer.NextRecord();
                    writer.WriteField("Springer");
                    writer.WriteField(_currentSpringerCount);
                    writer.NextRecord();
                    writer.WriteField("IEEE Xplore");
                    writer.WriteField(_currentIeeeXploreCount);
                    writer.NextRecord();
                }

                if (await ConfirmationDialog(GetString("Confirm"), GetString("OpenExportFile")))
                    Process.Start(saveFileDialog.FileName);
            }
        }

        public async void ExportChartDataToCsv2()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = "ChartData_" + DateTime.Now.ToString("yyyy_hh_mm_ss"),
                InitialDirectory = _currentPublicationSavingPath
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var streamWriter = File.CreateText(saveFileDialog.FileName))
                {
                    var writer = new CsvWriter(streamWriter);
                    writer.Configuration.Delimiter = ";";
                    writer.WriteField(GetString("Downloads"));
                    writer.WriteField(_currentPublicationsDownloadCount);
                    writer.NextRecord();
                    writer.WriteField(GetString("Duplicates"));
                    writer.WriteField(_currentDuplicateCount);
                    writer.NextRecord();
                }

                if (await ConfirmationDialog(GetString("Confirm"), GetString("OpenExportFile")))
                    Process.Start(saveFileDialog.FileName);
            }
        }

        public async void ExportChartDataToCsv3()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = "ChartData_" + DateTime.Now.ToString("yyyy_hh_mm_ss"),
                InitialDirectory = _currentPublicationSavingPath
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var streamWriter = File.CreateText(saveFileDialog.FileName))
                {
                    var writer = new CsvWriter(streamWriter);
                    writer.Configuration.Delimiter = ";";

                    for (int i = 0; i < StatisticsDataService.Instance.ListYear.Count; i++)
                    {
                        writer.WriteField(StatisticsDataService.Instance.ListYear[i]);
                        writer.WriteField(StatisticsDataService.Instance.ListYearAmount[i]);
                        writer.NextRecord();
                    }
                }

                if (await ConfirmationDialog(GetString("Confirm"), GetString("OpenExportFile")))
                    Process.Start(saveFileDialog.FileName);
            }
        }

        public async void ExportChartDataToCsv3_1()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = "ChartData_" + DateTime.Now.ToString("yyyy_hh_mm_ss"),
                InitialDirectory = _currentPublicationSavingPath
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var streamWriter = File.CreateText(saveFileDialog.FileName))
                {
                    var writer = new CsvWriter(streamWriter);
                    writer.Configuration.Delimiter = ";";

                    for (int i = 0; i < StatisticsDataService.Instance.ListYearFull.Count; i++)
                    {
                        writer.WriteField(StatisticsDataService.Instance.ListYearFull[i]);
                        writer.WriteField(StatisticsDataService.Instance.ListYearAmountFull[i]);
                        writer.NextRecord();
                    }
                }

                if (await ConfirmationDialog(GetString("Confirm"), GetString("OpenExportFile")))
                    Process.Start(saveFileDialog.FileName);
            }
        }

        public async void ExportChartDataToCsv4()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = "ChartDataMagazine_" + DateTime.Now.ToString("yyyy_hh_mm_ss"),
                InitialDirectory = _currentPublicationSavingPath
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var streamWriter = File.CreateText(saveFileDialog.FileName))
                {
                    var writer = new CsvWriter(streamWriter);
                    writer.Configuration.Delimiter = ";";

                    for (int i = 0; i < StatisticsDataService.Instance.ListMagazine.Count; i++)
                    {
                        writer.WriteField(StatisticsDataService.Instance.ListMagazine[i]);
                        writer.WriteField(StatisticsDataService.Instance.ListMagazineAmount[i]);
                        writer.NextRecord();
                    }
                }

                if (await ConfirmationDialog(GetString("Confirm"), GetString("OpenExportFile")))
                    Process.Start(saveFileDialog.FileName);
            }
        }

        public async void ExportChartDataToCsv4_1()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = "ChartDataMagazine_" + DateTime.Now.ToString("yyyy_hh_mm_ss"),
                InitialDirectory = _currentPublicationSavingPath
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var streamWriter = File.CreateText(saveFileDialog.FileName))
                {
                    var writer = new CsvWriter(streamWriter);
                    writer.Configuration.Delimiter = ";";

                    for (int i = 0; i < StatisticsDataService.Instance.ListMagazineFull.Count; i++)
                    {
                        writer.WriteField(StatisticsDataService.Instance.ListMagazineFull[i]);
                        writer.WriteField(StatisticsDataService.Instance.ListMagazineAmountFull[i]);
                        writer.NextRecord();
                    }
                }

                if (await ConfirmationDialog(GetString("Confirm"), GetString("OpenExportFile")))
                    Process.Start(saveFileDialog.FileName);
            }
        }
         
        public void ExportDataGridToCsv(ObservableCollection<ISearchResultsToDisplay> model, string query)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = "DataGrid_" + query + "_" + DateTime.Now.ToString("yyyy_hh_mm_ss"),
                InitialDirectory = _currentPublicationSavingPath
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var streamWriter = File.CreateText(saveFileDialog.FileName))
                {
                    var writer = new CsvWriter(streamWriter);
                    writer.Configuration.Delimiter = ";";

                    foreach (var item in model)
                    {
                        //writer.WriteField(item.PercentComplete);
                        writer.WriteField(item.Creator);
                        writer.WriteField(item.Title);
                        writer.WriteField(item.Year);
                        writer.WriteField(item.Doi);
                        writer.WriteField(item.Abstract);
                        writer.NextRecord();
                    }
                }

                //if (await ConfirmationDialog(GetString("Confirm"), GetString("OpenExportFile")))
                Process.Start(saveFileDialog.FileName);
            }
        }
        #endregion
    }
}
﻿using System;
using System.Diagnostics;
using System.IO;
using AnalyzerDatabase.ViewModels;
using CsvHelper;
using Microsoft.Win32;

namespace AnalyzerDatabase.Services
{
    public class ExportDataToCsv : ExtendedViewModelBase
    {
        private readonly string _currentPublicationSavingPath;
        private readonly int _currentScienceDirectCount;
        private readonly int _currentScopusCount;
        private readonly int _currentSpringerCount;
        private readonly int _currentIeeeXploreCount;
        private readonly int _currentDuplicateCount;
        private readonly int _currentPublicationsDownloadCount;

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

                //TODO: jezyki
                if (await ConfirmationDialog("Potwierdź", "Czy otworzyc wyeksportowany plik?"))
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
                    //TODO: jezyki
                    writer.WriteField("Download");
                    writer.WriteField(_currentPublicationsDownloadCount);
                    writer.NextRecord();
                    writer.WriteField("Duplicate");
                    writer.WriteField(_currentDuplicateCount);
                    writer.NextRecord();
                }

                //TODO: jezyki
                if (await ConfirmationDialog("Potwierdź", "Czy otworzyc wyeksportowany plik?"))
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
                        //TODO: jezyki
                        writer.WriteField(StatisticsDataService.Instance.ListYear[i]);
                        writer.WriteField(StatisticsDataService.Instance.ListYearAmount[i]);
                        writer.NextRecord();
                    }
                }

                //TODO: jezyki
                if (await ConfirmationDialog("Potwierdź", "Czy otworzyc wyeksportowany plik?"))
                    Process.Start(saveFileDialog.FileName);
            }
        }
    }
}
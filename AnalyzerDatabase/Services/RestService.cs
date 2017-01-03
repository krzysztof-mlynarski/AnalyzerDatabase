using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using AnalyzerDatabase.Interfaces;
using AnalyzerDatabase.Messengers;
using AnalyzerDatabase.Models.IeeeXplore;
using AnalyzerDatabase.Models.ScienceDirect;
using AnalyzerDatabase.Models.Scopus;
using AnalyzerDatabase.Models.Springer;
using AnalyzerDatabase.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;

namespace AnalyzerDatabase.Services
{
    public class RestService : IRestService
    {
        #region Variables
        private readonly ResourceDictionary _resources = Application.Current.Resources;
        private readonly IDeserializeJsonService _deserializeJsonService;
        private readonly IStatisticsDataService _statisticsDataService;
        private readonly string _currentPublicationSavingPath;
        private readonly string _currentScienceDirectAndScopusApiKey;
        private readonly string _currentSpringerApiKey;
        #endregion

        #region Constructor
        public RestService(IDeserializeJsonService deserializeJsonService, IStatisticsDataService statisticsDataService)
        {
            _deserializeJsonService = deserializeJsonService;
            _statisticsDataService = statisticsDataService;
            _currentPublicationSavingPath = SettingsService.Instance.Settings.SavingPublicationPath;
            _currentScienceDirectAndScopusApiKey = SettingsService.Instance.Settings.ScienceDirectAndScopusApiKey;
            _currentSpringerApiKey = SettingsService.Instance.Settings.SpringerApiKey;
        }
        #endregion

        #region Public methods
        public async Task<ScienceDirectSearchQuery> GetSearchQueryScienceDirect(string query, CancellationTokenSource cts = null)
        {
            try
            {
                string url = String.Format(_resources["SearchQueryScienceDirect"].ToString(), query,
                    _currentScienceDirectAndScopusApiKey);
                string webPageSource = await GetWebPageSource(url, cts);

                return _deserializeJsonService.GetObjectFromJson<ScienceDirectSearchQuery>(webPageSource);
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                Messenger.Default.Send(new ExceptionToSettingsMessage {
                    Exception = ViewModelLocator.Instance.Settings,
                    Source = "Science Direct",
                });
                return null;
            }
        }

        public async Task<ScienceDirectSearchQuery> GetPreviousOrNextResultScienceDirect(string query, int start, CancellationTokenSource cts = null)
        {
            try
            {
                string url = String.Format(_resources["ScienceDirectPreviousOrNextPageResult"].ToString(), start, query,
                    _currentScienceDirectAndScopusApiKey);
                string webPageSource = await GetWebPageSource(url, cts);

                return _deserializeJsonService.GetObjectFromJson<ScienceDirectSearchQuery>(webPageSource);
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ScopusSearchQuery> GetSearchQueryScopus(string query, CancellationTokenSource cts = null)
        {
            try
            {
                string url = String.Format(_resources["SearchQueryScopus"].ToString(), query, _currentScienceDirectAndScopusApiKey);
                string webPageSource = await GetWebPageSource(url, cts);

                return _deserializeJsonService.GetObjectFromJson<ScopusSearchQuery>(webPageSource);
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                Messenger.Default.Send(new ExceptionToSettingsMessage
                {
                    Exception = ViewModelLocator.Instance.Settings,
                    Source = "Scopus",
                });
                return null;
            }
        }

        public async Task<ScopusSearchQuery> GetPreviousOrNextResultScopus(string query, int start, CancellationTokenSource cts = null)
        {
            try
            {
                string url = String.Format(_resources["ScopusPreviousOrNextPageResult"].ToString(), start, query, _currentScienceDirectAndScopusApiKey);
                string webPageSource = await GetWebPageSource(url, cts);

                return _deserializeJsonService.GetObjectFromJson<ScopusSearchQuery>(webPageSource);
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<SpringerSearchQuery> GetSearchQuerySpringer(string query, CancellationTokenSource cts = null)
        {
            try
            {
                string url = String.Format(_resources["SearchQuerySpringer"].ToString(), _currentSpringerApiKey, query);
                string webPageSource = await GetWebPageSource(url, cts);

                return _deserializeJsonService.GetObjectFromJson<SpringerSearchQuery>(webPageSource);
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                Messenger.Default.Send(new ExceptionToSettingsMessage
                {
                    Exception = ViewModelLocator.Instance.Settings,
                    Source = "Springer",
                });
                return null;
            }
        }

        public async Task<SpringerSearchQuery> GetPreviousOrNextResultSpringer(string query, int start, CancellationTokenSource cts = null)
        {
            try
            {
                string url = String.Format(_resources["SpringerPreviousOrNextPageResult"].ToString(), _currentSpringerApiKey, query, start);
                string webPageSource = await GetWebPageSource(url, cts);

                return _deserializeJsonService.GetObjectFromJson<SpringerSearchQuery>(webPageSource);
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IeeeXploreSearchQuery> GetSearchQueryIeeeXplore(string query, CancellationTokenSource cts = null)
        {
            try
            {
                string url = String.Format(_resources["SearchQueryIeeeXplore"].ToString(), query);
                string webPageSource = await GetWebPageSource(url, cts);

                return XmlSerialize<IeeeXploreSearchQuery>.DeserializeXml(webPageSource);
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                Messenger.Default.Send(new ExceptionToSettingsMessage
                {
                    Exception = ViewModelLocator.Instance.Settings,
                    Source = "IEEE Xplore",
                });
                return null;
            }
        }

        public async Task<IeeeXploreSearchQuery> GetPreviousOrNextResultIeeeXplore(string query, int start, CancellationTokenSource cts = null)
        {
            try
            {
                string url = String.Format(_resources["IeeeXplorePreviousOrNextPageResult"].ToString(), query, start);
                string webPageSource = await GetWebPageSource(url, cts);

                return XmlSerialize<IeeeXploreSearchQuery>.DeserializeXml(webPageSource);
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<string> GetArticle(string doi, string title, CancellationTokenSource cts = null)
        {
            try
            {
                string url = String.Format(_resources["DownloadArticleFromScienceDirectToPdf"].ToString(), doi, _currentScienceDirectAndScopusApiKey);

                return await GetWebPageSourcePdf(url, title, cts);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Private methods
        private async Task<string> GetWebPageSource(string url, CancellationTokenSource cts)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response;
                if (cts == null)
                {
                    response = await httpClient.GetAsync(url);
                }
                else
                {
                    response = await httpClient.GetAsync(url, cts.Token);
                }

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new HttpRequestException();
                }

                return await DecodeResponseContent(response);
            }
        }

        private async Task<string> DecodeResponseContent(HttpResponseMessage response)
        {
            string jsonString;
            byte[] byteContent = await response.Content.ReadAsByteArrayAsync();

            try
            {
                string encoding = response.Content.Headers.ContentType.CharSet;
                jsonString = Encoding.GetEncoding(encoding).GetString(byteContent);
            }
            catch
            {
                jsonString = Encoding.UTF8.GetString(byteContent);
            }
            return jsonString;
        }

        private async Task<string> GetWebPageSourcePdf(string url, string title, CancellationTokenSource cts = null)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response;
                if (cts == null)
                {
                    response = await httpClient.GetAsync(url);
                }
                else
                {
                    response = await httpClient.GetAsync(url, cts.Token);
                }

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new HttpRequestException();
                }

                return await DecodeResponseContentAndSaveToPdf(response, title);
            }
        }

        private async Task<string> DecodeResponseContentAndSaveToPdf(HttpResponseMessage response, string title)
        {
            string pdfString = "";
            byte[] byteContent = await response.Content.ReadAsByteArrayAsync();

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF (*.pdf)|*.pdf",
                    FileName = title,
                    InitialDirectory = _currentPublicationSavingPath
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    System.IO.File.WriteAllBytes(saveFileDialog.FileName, byteContent);
                    _statisticsDataService.IncrementPublicationsDownload();
                }

                System.Diagnostics.Process.Start(saveFileDialog.FileName);
            }
            catch
            {
                pdfString = Encoding.UTF8.GetString(byteContent);
            }

            return pdfString;
        }
        #endregion
    }
}

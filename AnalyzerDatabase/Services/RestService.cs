using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using AnalyzerDatabase.Interfaces;
using AnalyzerDatabase.Models.IeeeXplore;
using AnalyzerDatabase.Models.ScienceDirect;
using AnalyzerDatabase.Models.Scopus;
using AnalyzerDatabase.Models.Springer;
using Microsoft.Win32;

namespace AnalyzerDatabase.Services
{
    public class RestService : IRestService
    {
        #region Private fields
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
                string url = String.Format(_resources["SearchQueryScienceDirect"].ToString(), query, _currentScienceDirectAndScopusApiKey);
                string webPageSource = await GetWebPageSource(url, cts);
                
                return _deserializeJsonService.GetObjectFromJson<ScienceDirectSearchQuery>(webPageSource);
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
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
            catch (TaskCanceledException ex)
            {
                throw ex;
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
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
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
            catch (TaskCanceledException ex)
            {
                throw ex;
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
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
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
            catch (TaskCanceledException ex)
            {
                throw ex;
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
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
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
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async void GetArticle(string doi, string title, CancellationTokenSource cts = null)
        {
            try
            {
                string url = String.Format(_resources["DownloadArticleFromScienceDirectToPdf"].ToString(), doi,
                    _currentScienceDirectAndScopusApiKey);
                await GetWebPageSourcePdf(url, title, cts);
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
        }

        public async void GetArticleDocx(string doi, string title, CancellationTokenSource cts = null)
        {
            try
            {
                string url = String.Format(_resources["DownloadArticleFromScienceDirectToPdf"].ToString(), doi,
                    _currentScienceDirectAndScopusApiKey);
                await GetWebPageSourceDocx(url, title, cts);
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
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

        private async Task<string> GetWebPageSourceDocx(string url, string title, CancellationTokenSource cts = null)
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

                return await DecodeResponseContentAndSaveToDocx(response, title);
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

        private async Task<string> DecodeResponseContentAndSaveToDocx(HttpResponseMessage response, string title)
        {
            string pdfString = "";
            byte[] byteContent = await response.Content.ReadAsByteArrayAsync();

            System.IO.File.WriteAllBytes("ERSN-OpenMC1", byteContent);

            //PDDocument doc = null;
            //doc = PDDocument.load(@"C:\Users\Krzysztof Młynarski\Documents\Publications\ERSN-OpenMC1.pdf");
            //PDFTextStripper textStripper = new PDFTextStripper();
            //string strPDFText = textStripper.getText(doc);
            //doc.close();

            //string fn = @"C:\Users\Krzysztof Młynarski\Documents\Publications\ERSN-OpenMC1.docx";
            //var wordDoc = DocX.Create(fn);

            //wordDoc.InsertParagraph(strPDFText);

            //wordDoc.Save();

            //System.Diagnostics.Process.Start(fn);



            //try
            //{
            //    SaveFileDialog saveFileDialog = new SaveFileDialog
            //    {
            //        Filter = "PDF (*.pdf)|*.pdf",
            //        FileName = title,
            //        InitialDirectory = _currentPublicationSavingPath
            //    };

            //    if (saveFileDialog.ShowDialog() == true)
            //    {
            //        System.IO.File.WriteAllBytes(saveFileDialog.FileName, byteContent);
            //    }

            //    System.Diagnostics.Process.Start(saveFileDialog.FileName);
            //}
            //catch
            //{
            //    pdfString = Encoding.UTF8.GetString(byteContent);
            //}

            return pdfString;
        }

        #endregion
    }
}

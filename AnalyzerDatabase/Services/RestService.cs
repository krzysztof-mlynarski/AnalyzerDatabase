﻿using System;
using System.CodeDom;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using AnalyzerDatabase.Interfaces;
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
        #endregion

        #region Constructor
        public RestService(IDeserializeJsonService deserializeJsonService)
        {
            _deserializeJsonService = deserializeJsonService;
        }
        #endregion

        #region Public methods
        public async Task<ScienceDirectSearchQuery> GetSearchQueryScienceDirect(string query, CancellationTokenSource cts = null)
        {
            try
            {
                string url = String.Format(_resources["SearchQueryScienceDirect"].ToString(), query, _resources["X-ELS-APIKey"]);
                //string url = String.Format(_resources["ScienceDirectAllResult"].ToString(), query, _resources["X-ELS-APIKey"]);
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
                    _resources["X-ELS-APIKey"]);
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
                string url = String.Format(_resources["SearchQueryScopus"].ToString(), query, _resources["X-ELS-APIKey"]);
                //string url = String.Format(_resources["ScopusAllResult"].ToString(), query, _resources["X-ELS-APIKey"]);
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
                string url = String.Format(_resources["ScopusPreviousOrNextPageResult"].ToString(), start, query, _resources["X-ELS-APIKey"]);
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
                string url = String.Format(_resources["SearchQuerySpringer"].ToString(), query);
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
                string url = String.Format(_resources["SpringerPreviousOrNextPageResult"].ToString(), query, start);
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

        public async void GetArticle(string doi, string title, CancellationTokenSource cts = null)
        {
            try
            {
                string url = String.Format(_resources["DownloadArticleFromScienceDirectToPdf"].ToString(), doi,
                    _resources["X-ELS-APIKey"]);
                string webPageSourcePdf = await GetWebPageSourcePdf(url, title, cts);
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
            string jsonString = "";
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
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    System.IO.File.WriteAllBytes(saveFileDialog.FileName, byteContent);
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

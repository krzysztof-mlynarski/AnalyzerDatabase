using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using AnalyzerDatabase.Interfaces;
using AnalyzerDatabase.Models;

namespace AnalyzerDatabase.Services
{
    public class ScopusRestService : IScopusRestService
    {
        private readonly ResourceDictionary _resources = Application.Current.Resources;
        private readonly IDeserializeJsonService _deserializeJsonService;

        public ScopusRestService(IDeserializeJsonService deserializeJsonService)
        {
            _deserializeJsonService = deserializeJsonService;
        }
        public async Task<List<SearchQueryModel>> GetSearchQuery(string query, CancellationTokenSource cts = null)
        {
            try
            {
                string url = String.Format(_resources["SearchQuery"].ToString(), query);
                string webPageSource = await GetWebPageSource(url, cts);

                return _deserializeJsonService.GetObjectFromJson<List<SearchQueryModel>>(webPageSource);
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

        private async Task<string> GetWebPageSource(string url, CancellationTokenSource cts, bool authorize = false)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                //if (authorize)
                //{
                //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(String.Format("{0}:{1}", _accountService.GetUserEmail(), _accountService.GetUserPassword()))));
                //}

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
    }
}

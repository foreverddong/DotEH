using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DotEH.Model;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace DotEH.Services
{
    public class EhSearchingService
    {
        private readonly OptionsStorageService optionsStorage;
        private readonly HttpClient client;
        private readonly ILogger<EhSearchingService> logger;

        private string nextId { get; set; }
        public EhSearchingService(OptionsStorageService _optionsStorage, HttpClient _httpClient, ILogger<EhSearchingService> _logger)
        {
            this.optionsStorage = _optionsStorage;
            this.client = _httpClient;
            this.logger = _logger;
        }

        public async Task<IEnumerable<GalleryMetadata>> DoSearch(SearchParameter param)
        {
            var querystr = param.Query + " " + param.Tags.Aggregate((a, b) => $"{a}$ {b}$");
            return await DoSearch(querystr, param.categoryCode);
        }

        public void ClearSearch()
        {
            this.nextId = "";
        }

        public async Task<IEnumerable<GalleryMetadata>> DoSearch(string query, uint category)
        {
            var queryParameters = new Dictionary<string, string>
            {
                {"f_cats",  category.ToString()},
                {"f_search", query },
            };
            if (!string.IsNullOrEmpty(nextId))
            {
                queryParameters.Add("next", nextId);
            }
            var queryString = await new FormUrlEncodedContent(queryParameters).ReadAsStringAsync();
            var message = PrepareMessage($"{optionsStorage.EhBaseAddress}?{queryString}", HttpMethod.Get);
            var response = await client.SendAsync(message);
            var rawMetadataResult = await this.ParseGalleryEntries(await response.Content.ReadAsStringAsync());

            return rawMetadataResult;
        }

        private async Task<IEnumerable<GalleryMetadata>> ParseGalleryEntries(string htmlString)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlString);
            var entryRows = doc.DocumentNode.SelectNodes(@"//table[@class='itg gltc'][1]/tr").Skip(1);
            var metadataRequestArray = entryRows.Where(r => r.ChildNodes.Count >= 3).Select(r =>
            {
                var entry = new GalleryEntry(r.ChildNodes[2].ChildNodes[0].ChildNodes[0].InnerText, r.ChildNodes[2].ChildNodes[0].Attributes["href"].Value);
                return entry;
            }).Select(r =>
            {
                return $"[{r.GalleryId}, \"{r.GalleryToken}\"]";
            }).Aggregate((a, b) => $"{a}, {b}");
            var metadataBody = $"{{ \"method\": \"gdata\", \"gidlist\" : [{metadataRequestArray}] }}";
            var message = PrepareMessage($"{optionsStorage.EhBaseAddress}api.php", HttpMethod.Post);
            message.Content = new StringContent(metadataBody);
            var rawMetadataResponse = await (await client.SendAsync(message)).Content.ReadAsStringAsync();
            var jsondoc = JsonDocument.Parse(rawMetadataResponse);
            var metaNodes = jsondoc.RootElement.GetProperty("gmetadata").EnumerateArray();
            var result = metaNodes.Select(n =>
            {
                return JsonSerializer.Deserialize<GalleryMetadata>(n.GetRawText());
            });
            var nextHref = doc.DocumentNode.SelectSingleNode("//a[@id='dnext']").Attributes["href"].Value;
            var regex = new Regex(@"(.*)next[=]([0-9]+)");
            var groups = regex.Match(nextHref).Groups;
            this.nextId = groups[2].Value;
            return result;
        }

        private HttpRequestMessage PrepareMessage(string uri, HttpMethod method)
        {
            var message = new HttpRequestMessage
            {
                RequestUri = new Uri(uri),
                Method = method,
            };
            if (optionsStorage.UseEx)
            {
                /*
                   $"ipb_member_id={this.EhMemberId};",
                    $"ipb_pass_hash={this.EhPassHash};",
                    $"igneous={this.EhIgneous};"
                 */
                message.Headers.Add("Cookie", optionsStorage.Cookies);
            }
            return message;
        }
    }
}

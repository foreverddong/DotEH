using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DotEH.Model;
using HtmlAgilityPack;

namespace DotEH.Services
{
    public class EhSearchingService
    {
        private readonly OptionsStorageService optionsStorage;
        private readonly HttpClient client;

        private string nextId { get; set; }
        public EhSearchingService(OptionsStorageService _optionsStorage, HttpClient _httpClient)
        {
            this.optionsStorage = _optionsStorage;
            this.client = _httpClient;
        }

        public async Task<IEnumerable<ImageGalleryMetadata>> DoSearch(string query)
        {
            var queryParameters = new Dictionary<string, string>
            {
                {"F_search", query }
            };
            if (!string.IsNullOrEmpty(nextId))
            {
                queryParameters.Add("next", nextId);
            }
            var queryString = await new FormUrlEncodedContent(queryParameters).ReadAsStringAsync();
            var response = await client.GetAsync($"/?{queryString}");
            var rawMetadataResult = await this.ParseGalleryEntries(await response.Content.ReadAsStringAsync());
            var result = rawMetadataResult.Select(async (m) => 
            { 
                using var imgClient = new HttpClient();
                if (optionsStorage.UseEx)
                {
                    imgClient.DefaultRequestHeaders.Add("Cookie", optionsStorage.Cookies);
                }
                return new ImageGalleryMetadata
                {
                    Metadata = m,
                    base64Image = await imgClient.GetByteArrayAsync(m.thumb),
                };
            });
            return (await Task.WhenAll(result)).ToList();
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
            var rawMetadataResponse = await (await client.PostAsync(@"/api.php", new StringContent(metadataBody))).Content.ReadAsStringAsync();
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


    }
}

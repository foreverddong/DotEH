using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotEH.Services
{
    public class OptionsStorageService
    {
        public bool UseEx { get; set; }
        public string EhUsername { get; set; }
        public string EhPassword { get; set; }
        public bool UseAdvancedOptions { get; set; } = false;

        public string EhSessionId { get; set; }
        public string EhMemberId { get; set; }
        public string EhPassHash { get; set; }

        public Uri EhBaseAddress
        {
            get
            {
                return UseEx switch
                {
                    true => new Uri(@"https://exhentai.org"),
                    false => new Uri(@"https://e-hentai.org")
                };
            }
        }

        public async Task UpdateFromStorageAsync()
        {
            this.UseEx = await SecureStorage.Default.GetAsync("UseEx") switch
            {
                "True" => true,
                "False" => false,
                _ => false
            };
            this.EhUsername = await SecureStorage.Default.GetAsync("EhUsername");
            this.EhPassword = await SecureStorage.Default.GetAsync("EhPassword");
            this.UseAdvancedOptions = await SecureStorage.Default.GetAsync("UseAdvancedOptions") switch
            {
                "True" => true,
                "False" => false,
                _ => false
            };
            this.EhSessionId = await SecureStorage.Default.GetAsync("EhSessionId");
            this.EhMemberId = await SecureStorage.Default.GetAsync("EhMemberId");
            this.EhPassHash = await SecureStorage.Default.GetAsync("EhPassHash");
        }

        public async Task SaveChangesAsync()
        {
            
            await SecureStorage.Default.SetAsync("UseEx", UseEx.ToString());
            await SecureStorage.Default.SetAsync("EhUsername", this.EhUsername);
            await SecureStorage.Default.SetAsync("EhPassword", this.EhPassword);
            await SecureStorage.Default.SetAsync("UseAdvancedOptions", this.UseAdvancedOptions.ToString());
            await SecureStorage.Default.SetAsync("EhSessionId", this.EhSessionId);
            await SecureStorage.Default.SetAsync("EhMemberId", this.EhMemberId);
            await SecureStorage.Default.SetAsync("EhPassHash", this.EhPassHash);
        }

        public async Task AcquireEhTokenAsync()
        {
            using var client = new HttpClient();
            Dictionary<string, string> formData = new()
            {
                {"UserName", this.EhUsername },
                {"PassWord", this.EhPassword },
                {"CookieDate", "1" },
            };
            using var content = new FormUrlEncodedContent(formData);
            client.BaseAddress = new Uri(@"https://forums.e-hentai.org");
            var result = await client.PostAsync(@"/index.php?act=Login&CODE=01", content);
            var cookies = result.Headers
                .Where(h => h.Key == "Set-Cookie").First().Value;
            this.EhSessionId = (new Regex("ipb_session_id=([a-z0-9]+);"))
                .Match(cookies.Where(h => h.StartsWith("ipb_session_id")).First())
                .Groups[1]
                .Value;
            this.EhMemberId = (new Regex("ipb_member_id=([0-9]+);"))
                .Match(cookies.Where(h => h.StartsWith("ipb_member_id")).First())
                .Groups[1]
                .Value;
            this.EhPassHash = (new Regex("ipb_pass_hash=([a-z0-9]+);"))
                .Match(cookies.Where(h => h.StartsWith("ipb_pass_hash")).First())
                .Groups[1]
                .Value;
        }
    }
}

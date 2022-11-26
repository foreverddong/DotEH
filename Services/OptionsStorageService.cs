using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEH.Services
{
    public class OptionsStorageService
    {
        private bool initialized = false;
        public bool UseEx { get; set; }
        public string EhUsername { get; set; }
        public string EhPassword { get; set; }

        public Uri EhBaseAddress {
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
            this.initialized = true;
        }

        public async Task SaveChangesAsync()
        {
            var exStr = UseEx.ToString();
            await SecureStorage.Default.SetAsync("UseEx", exStr);
            await SecureStorage.Default.SetAsync("EhUsername", this.EhUsername);
            await SecureStorage.Default.SetAsync("EhPassword", this.EhPassword);
        }
    }
}

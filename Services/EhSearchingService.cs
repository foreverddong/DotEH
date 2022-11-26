using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEH.Services
{
    public class EhSearchingService
    {
        private readonly OptionsStorageService optionsStorage;
        public EhSearchingService(OptionsStorageService _optionsStorage)
        {
            this.optionsStorage = _optionsStorage;
        }

        public async Task DoSearch(string query)
        {
            
        }
    }
}

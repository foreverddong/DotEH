using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEH.Pages
{
    public partial class EhOptions
    {

        public async Task AcquireToken()
        {
            try
            {
                await optionsStorage.AcquireEhTokenAsync();
                snackBar.Add("It seems like tokens for exhentai are acquired successfully.");
            }
            catch (Exception)
            {
                snackBar.Add("Error- exhentai tokens were not acquired, check your username and password.");
            }
        }
    }
}

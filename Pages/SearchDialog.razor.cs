using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEH.Pages
{
    public partial class SearchDialog
    {
        [CascadingParameter]
        private MudDialogInstance Dialog { get; set; }

        public string queryStr { get; set; }
    }
}

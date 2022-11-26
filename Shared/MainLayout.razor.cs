using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEH.Shared
{
    public partial class MainLayout
    {
        private bool _drawerOpen = true;
        private void DrawerToggle()
        {
            this._drawerOpen = !this._drawerOpen;
        }
    }
}

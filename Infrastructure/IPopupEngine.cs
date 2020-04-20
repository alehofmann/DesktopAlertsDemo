using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tikisoft.DesktopAlertsDemo.Model;

namespace Tikisoft.DesktopAlertsDemo.Infrastructure
{
    interface IPopupEngine
    {
        void ShowPopup(PopupAlert popup);
        string LastError { get; set; }
    }
}

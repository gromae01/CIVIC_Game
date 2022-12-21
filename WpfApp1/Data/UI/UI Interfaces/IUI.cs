using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WinFormsApp1.Data.UI.UI_Interfaces
{
    internal interface IUI
    {
        public void PlaySound(string soundname);
        public void DoMethod(Control UIElement);
    }
}

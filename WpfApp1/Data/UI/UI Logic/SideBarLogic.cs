
using System.Windows.Controls;
using NAudio.Wave;


namespace WinFormsApp1.Data.UI.UI_Logic
{
    public class SideBarLogic:UI
    {
        WaveOut waveOut;
        public bool DoMethod(Grid UIElement,bool sidebarstate)
        {
            if(sidebarstate)
            {

                if (UIElement.Width - 9 <= UIElement.MinWidth)
                {
                    UIElement.Width = UIElement.MinWidth;
                    return false;
                }
                else
                    UIElement.Width -= 9;    
            }
            else
            {
                if (UIElement.Width + 9 >= UIElement.MaxWidth)
                {
                    UIElement.Width = UIElement.MaxWidth;
                    return true;
                }
                else
                    UIElement.Width += 9;
            }
            return sidebarstate;
        }
    }
}

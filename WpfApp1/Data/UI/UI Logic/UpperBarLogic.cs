using System.Windows.Controls;
using NAudio.Wave;

namespace WinFormsApp1.Data.UI.UI_Logic
{
    public class UpperBarLogic : UI
    {
        
        public bool DoMethod(Canvas UIElement, bool upperbarstate)
        {
            if (upperbarstate)
            {

                if (UIElement.Height - 5 <= UIElement.MinHeight)
                {
                    UIElement.Height = UIElement.MinHeight;
                    return false;
                }
                else
                    UIElement.Height -= 5;
            }
            else
            {
                if (UIElement.Height + 5 >= UIElement.MaxHeight)
                {
                    UIElement.Height = UIElement.MaxHeight;
                    return true;
                }
                else
                    UIElement.Height += 5;
            }
            return upperbarstate;

        }
        
    }
}

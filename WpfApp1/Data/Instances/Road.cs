using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WinFormsApp1.Data.Instaces;

namespace WpfApp1.Data.Instances
{
    public class Road :IDisposable
    {
        public City CityFrom;
        public City CityTo;
        public (int, int) Position1;
        public (int, int) Position2;
        SolidColorBrush TeamBrush;
        Line RoadInstanceLine;
        Canvas Parent;
        Сommun ParentCommun;
        public Road(City cityFrom, City cityTo, System.Drawing.Color color,Canvas parent, Сommun parentCommun)
        {
            CityFrom = cityFrom;
            CityTo = cityTo;
            Parent = parent;
            ParentCommun = parentCommun;

            switch (color.Name)
            {
                case "Blue":
                    TeamBrush = new SolidColorBrush(Colors.Blue);
                    break;
                case "Red":
                    TeamBrush = new SolidColorBrush(Colors.Red);
                    break;
                case "Orange":
                    TeamBrush = new SolidColorBrush(Colors.Orange);
                    break;
                case "DarkOrange":
                    TeamBrush = new SolidColorBrush(Colors.DarkOrange);
                    break;
                case "Green":
                    TeamBrush = new SolidColorBrush(Colors.Green);
                    break;
                case "Violet":
                    TeamBrush = new SolidColorBrush(Colors.Violet);
                    break;
                default:
                    TeamBrush = new SolidColorBrush(Colors.Gray);
                    break;
            }

            (int, int) p1;
            Position1.Item1 = CityFrom.Position.Item1;
            Position1.Item2 = CityFrom.Position.Item2;
            p1.Item1 = CityFrom.Position.Item1 + 25;
            p1.Item2 = CityFrom.Position.Item2 + 25;

            (int, int) p2;
            Position2.Item1 = CityTo.Position.Item1;
            Position2.Item2 = CityTo.Position.Item2;
            p2.Item1 = CityTo.Position.Item1 + 25;
            p2.Item2 = CityTo.Position.Item2 + 25;

            RoadInstanceLine = new Line();
            RoadInstanceLine.X1 = p1.Item1;
            RoadInstanceLine.Y1 = p1.Item2;

            RoadInstanceLine.X2 = p2.Item1;
            RoadInstanceLine.Y2 = p2.Item2;

            RoadInstanceLine.Stroke = TeamBrush;
            RoadInstanceLine.Opacity = 0.8;
            RoadInstanceLine.StrokeThickness = 1;

            parent.Children.Add(RoadInstanceLine);
            
        }

        public void Dispose()
        {

            Parent.Children.Remove(RoadInstanceLine);
            if(RoadInstanceLine!=null)
                RoadInstanceLine.Opacity = 0;
            ParentCommun.Roads.Remove(this);
            RoadInstanceLine = null;
        }
    }
}

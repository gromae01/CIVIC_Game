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
    public class Сommun : IDisposable
    {
        public List<(int, int)> Centers;
        public List<City> CitiesList;
        public List<Road> Roads;
        public List<Ellipse> RangeOfCom;
        private Canvas Parent;
        SolidColorBrush TeamBrush;
        private World WorldInstance;
        public System.Drawing.Color TeamColor;
        private int PrevCityCount = 0;
        private int PrevComCount = 0;
        public Сommun(Canvas parent, System.Drawing.Color color, List<City> citylist, World worldInstance, params (int, int)[] centers)
        {
            
            
            WorldInstance = worldInstance;
            Centers = new List<(int, int)>(100);
            Centers.AddRange(centers);
            Roads = new List<Road>(200);
            RangeOfCom = new List<Ellipse>(30);
            CitiesList = citylist;
            foreach (var city in CitiesList)
                city.InCommun = true;
            Parent = parent;
            TeamColor = color;
            switch (TeamColor.Name)
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
            TeamBrush.Opacity = 0.08;
        }
        public Сommun(Canvas parent, System.Drawing.Color color, List<City> citylist, World worldInstance, (int, int) center)
        {
            AudioPlayer player = new AudioPlayer();
            player.PlaySound("Sounds/comcreate.wav");
            WorldInstance = worldInstance;
            //Capital = capital;
            Centers = new List<(int, int)>(100);
            Roads = new List<Road>(200);
            Centers.Add(center);
            CitiesList = citylist;
            RangeOfCom = new List<Ellipse>(30);
            //CitiesList.Add(capital);
            foreach (var city in CitiesList)
                city.InCommun = true;
            Parent = parent;
            TeamColor = color;
            Ellipse elipse = new Ellipse();

            elipse.Width = 241;
            elipse.Height = 241;
            elipse.Margin = new System.Windows.Thickness((center.Item1 - (241 / 2)) + 25, (center.Item2 - (241 / 2)) + 25, 0, 0);
            TeamBrush = new SolidColorBrush(Colors.Gray);

            switch (TeamColor.Name)
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
            TeamBrush.Opacity = 0.08;

            elipse.Fill = TeamBrush;
            parent.Children.Add(elipse);
            RangeOfCom.Add(elipse);

            //----------------------------------

        }

        public void Live()
        {
            if (CitiesList != null && Centers != null && CitiesList.Count > 0 && Centers.Count>0)
            {
                //build roads
                if (PrevCityCount != CitiesList.Count)
                {
                    foreach (var city in CitiesList)
                    {
                        foreach (var city2 in CitiesList)
                        {
                            if (city != null & city2 != null && city.CitizenCount > 0 && city2.CitizenCount > 0 && city != city2 && Roads.Where(r => (r.CityFrom == city2 && r.CityTo == city) || (r.CityFrom == city && r.CityTo == city2)).Count() == 0 && Math.Sqrt(Math.Pow(((city.Position.Item1 + 25) - (city2.Position.Item1 + 25)), 2) + Math.Pow(((city.Position.Item2 + 25) - (city2.Position.Item2 + 25)), 2)) <= 141)
                                Roads.Add(new(city, city2, TeamColor, Parent, this));
                        }
                    }
                }
                PrevCityCount = CitiesList.Count;
            }
            else
                this.Dispose();
            //----------------------------------


            //Merge Communs
            if (PrevComCount != WorldInstance.Teams.Where(t => t.TeamColor == TeamColor).First().Communs.Count)
            {
                foreach (var com in WorldInstance.Teams.Where(t => t.TeamColor == TeamColor).First().Communs)
                {
                    bool merged = false;
                    if (com != this && Centers !=null && com.Centers!=null)
                    {
                        foreach (var center in Centers)
                        {
                            foreach (var center2 in com.Centers)
                            {
                                if (Math.Sqrt(Math.Pow(((center.Item1 + 25) - (center2.Item1 + 25)), 2) + Math.Pow(((center.Item2 + 25) - (center2.Item2 + 25)), 2)) <= 240)
                                {
                                    Сommun cnew = new Сommun(Parent, TeamColor, CitiesList, WorldInstance, Centers.ToArray());
                                    cnew.Centers.AddRange(com.Centers);
                                    foreach (var city in com.CitiesList)
                                        cnew.CitiesList.Add(city);
                                    foreach (var road in com.Roads)
                                        cnew.Roads.Add(road);
                                    foreach (var road in Roads)
                                        cnew.Roads.Add(road);
                                    foreach (var r in RangeOfCom)
                                        cnew.RangeOfCom.Add(r);
                                    foreach (var r in com.RangeOfCom)
                                        cnew.RangeOfCom.Add(r);
                                    WorldInstance.Teams.Where(t => t.TeamColor == TeamColor).First().Communs.Add(cnew);
                                    WorldInstance.Teams.Where(t => t.TeamColor == TeamColor).First().Communs.Remove(this);
                                    WorldInstance.Teams.Where(t => t.TeamColor == TeamColor).First().Communs.Remove(com);
                                    merged = true;
                                    break;
                                }
                            }
                            if (merged)
                                break;
                        }
                    }
                    if (merged)
                        break;
                }
            }
            PrevComCount = WorldInstance.Teams.Where(t => t.TeamColor == TeamColor).First().Communs.Count;
        //----------------------------------
    }

        public void Dispose()
        {
            WorldInstance.Teams.Where(t => t.TeamColor == TeamColor).First().Communs.Remove(this);
            foreach (var city in CitiesList)
                city.InCommun = false;
            CitiesList.Clear();
            CitiesList = null;
            foreach (var road in Roads.ToList())
                road.Dispose();
            Roads.Clear();
            Roads = null;
            foreach (var range in RangeOfCom)
            {
                range.Opacity = 0;
                Parent.Children.Remove(range);
            }
            Centers.Clear();
            Centers = null;
            GC.Collect();

        }

    }
}

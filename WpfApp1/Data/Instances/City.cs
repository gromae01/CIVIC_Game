using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfApp1.Data.Instances;
using Brush = System.Windows.Media.Brush;


namespace WinFormsApp1.Data.Instaces
{
    public class City :IDisposable
    {
        private System.Windows.Controls.Image CityInstance;
        private Label CitizenCountLabel;
        private (int, int) position;
        private Random CitizenIncrement = new Random();
        private int citizencount =1;
        private int PreviousCitizPeak = 0;
        private World WorldInstance;
        private System.Drawing.Color TeamColor;
        private Canvas Parent;
        private SoundPlayer player;
        private Label CitizenIncrementLabel;
        public System.Windows.Forms.Timer CitizenIncrementTimer;
        public bool InCommun = false;
        Random RandLabelIncrementUp = new Random();
       
        Brushes TeamBrush;


        public int CitizenCount { get => citizencount; }
        public (int,int) Position { get => position;}  
        public City(Canvas parent, System.Drawing.Color teamcolor, World worldInstance, (int,int) coordinates)
        {
            Parent = parent;
            TeamColor = teamcolor;
            WorldInstance = worldInstance;
            
            //Play Sound
            player = new SoundPlayer("Sounds/TyTyTyTy.wav");
            //----------------------------------
            CityInstance = new System.Windows.Controls.Image();
            Parent.Children.Add(CityInstance);
            CityInstance.Height = 50;
            CityInstance.Width = 50;
            CityInstance.Margin = new System.Windows.Thickness(coordinates.Item1, coordinates.Item2,0,0);
            position.Item1 = coordinates.Item1;
            position.Item2 = coordinates.Item2;
            CityInstance.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Towns/City_"+teamcolor.Name+".png"));
            //----------------------------------

            //Label CitizenCountLabel settings
            CitizenCountLabel = new Label();
            Parent.Children.Add(CitizenCountLabel);
            CitizenCountLabel.Margin = new System.Windows.Thickness(coordinates.Item1+15, coordinates.Item2+6,0,0);
            CitizenCountLabel.FontFamily = new System.Windows.Media.FontFamily("Comic Sans MS");
            CitizenCountLabel.FontSize = 16;
            CitizenCountLabel.FontWeight = FontWeights.Bold;
            CitizenCountLabel.Width = 48;
            CitizenCountLabel.Height = 27;
            CitizenCountLabel.Content = citizencount.ToString();
            //----------------------------------

            //Label CitizenIncrementLabel settings
            CitizenIncrementLabel = new Label();
            Parent.Children.Add(CitizenIncrementLabel);
            CitizenIncrementLabel.FontFamily = new System.Windows.Media.FontFamily("Comic Sans MS");
            CitizenIncrementLabel.FontSize = 10;
            switch(teamcolor.Name)
            {
                case "Blue":
                    CitizenIncrementLabel.Foreground = Brushes.Blue;
                    break;
                case "Red":
                    CitizenIncrementLabel.Foreground = Brushes.Red;
                    break;
                case "Orange":
                    CitizenIncrementLabel.Foreground = Brushes.Orange;
                    break;
                case "DarkOrange":
                    CitizenIncrementLabel.Foreground = Brushes.DarkOrange;
                    break;
                case "Green":
                    CitizenIncrementLabel.Foreground = Brushes.Green;
                    break;
                case "Violet":
                    CitizenIncrementLabel.Foreground = Brushes.Violet;
                    break;
                default:
                    CitizenIncrementLabel.Foreground = Brushes.DarkGray;
                    break;
            }
            CitizenIncrementLabel.FontWeight = FontWeights.Bold;
            CitizenIncrementLabel.Margin = new System.Windows.Thickness(coordinates.Item1+45, coordinates.Item2,0,0);
            CitizenIncrementLabel.Width = 45;
            CitizenIncrementLabel.Height = 25;
            //----------------------------------

            //Timer CitizenIncrementTimer settings
            CitizenIncrementTimer = new System.Windows.Forms.Timer();
            CitizenIncrementTimer.Interval = 50;
            CitizenIncrementTimer.Tick += new EventHandler(CitizenIncrementTimer_Tick);
            //----------------------------------
        }
        private void CityInstance_Click(object sender, EventArgs e)
        {
            player = new SoundPlayer("Sounds/City.wav");
        }
        public void RefreshCitizenCount()
        {
            CitizenCountLabel.Content = citizencount.ToString();
        }
        private void CitizenIncrementTimer_Tick(object sender, EventArgs e)
        {
            LabelIncrementUp();
        }
        private int up = 0;
        private void LabelIncrementUp()
        {
            CitizenIncrementLabel.Margin = new System.Windows.Thickness(CitizenIncrementLabel.Margin.Left, CitizenIncrementLabel.Margin.Top-up,0,0);
        }
        private void SpawnNewCitizen()
        {
                PreviousCitizPeak = citizencount / 10 + 5;
                foreach (var team in WorldInstance.Teams)
                {
                    if (team.TeamColor == TeamColor)
                    {
                        team.Citizens.Add(new Citizen(Parent, TeamColor, WorldInstance, Position));
                        citizencount--;
                        break;
                    }
                }
        }
        private int RandomEvent(int rnd)
        {
           string[] Events = {
            WorldInstance.Year + ":The "+TeamColor.Name+" team celebrates the day of the city",
            //WorldInstance.Year + ":Mystical ritual claims the lives of people in the " + TeamColor.Name+ " team",
            //WorldInstance.Year + ":Divine Grace for the " +TeamColor.Name+ " team",
            WorldInstance.Year + ":The " +TeamColor.Name+ " team finds a sacred artifact",
            WorldInstance.Year + ":Hungry Beast Attacks " +TeamColor.Name+ " team Settlement",
            WorldInstance.Year + ":METEORITE falls on the settlement of the " +TeamColor.Name+ " team",
            };
            switch (rnd)
            {
                case 0:
                    WorldInstance.Journal.AddNewEvent(Events[rnd]);
                    return 10;
                case 1:
                    WorldInstance.Journal.AddNewEvent(Events[rnd]);
                    return 3;
                case 2:
                    WorldInstance.Journal.AddNewEvent(Events[rnd]);
                    return -5;
                case 3:
                    WorldInstance.Journal.AddNewEvent(Events[rnd]);
                    return -10;
                //case 4:
                //    WorldInstance.Journal.AddNewEvent(Events[rnd]);
                //    return -5;
                //case 5:
                //    WorldInstance.Journal.AddNewEvent(Events[rnd]);
                //    return -10;
            }
            return 0;
        }
        public void Live()
        {
            if (citizencount > 0)
            {
                Random RandEventRnd = new Random();
                up = RandLabelIncrementUp.Next(1, 3);
                int rndevent = RandEventRnd.Next(0,1000);
                //int rndevent = 0;
                int Increment = rndevent < 4 ? RandomEvent(rndevent) : CitizenIncrement.Next(1, 7);
                if (InCommun)
                    Increment += WorldInstance.Teams.Where(t => t.TeamColor == TeamColor).First().Communs.Where(c => c.TeamColor == TeamColor).Where(c => c.CitiesList.Contains(this)).First().CitiesList.Count();
                citizencount += Increment;
                CitizenIncrementLabel.Margin = new System.Windows.Thickness(position.Item1 + 45, position.Item2,0,0);
                if (Increment>0)
                    CitizenIncrementLabel.Content = "+" + Increment;
                else
                    CitizenIncrementLabel.Content = ""+Increment;
                if (!CitizenIncrementTimer.Enabled)
                    CitizenIncrementTimer.Start();
                CitizenCountLabel.Content = citizencount.ToString();
                if (citizencount >= 10 && citizencount < 100)
                    CitizenCountLabel.Margin = new System.Windows.Thickness(position.Item1 + 10, position.Item2 + 6, 0, 0);
                else if (citizencount >= 100 && citizencount < 1000)
                    CitizenCountLabel.Margin = new System.Windows.Thickness(position.Item1+6, position.Item2 + 6, 0, 0);
                else if (citizencount >= 1000)
                {
                    CitizenCountLabel.Content = (citizencount / 1000).ToString() + "k";
                    CitizenCountLabel.Margin = new System.Windows.Thickness(position.Item1 + 10, position.Item2 + 6, 0, 0);
                }
                if (citizencount / 20 > PreviousCitizPeak)
                    SpawnNewCitizen();
                
                if(InCommun == false && AmIInCommun()==false)
                {
                    List<City> cities = new List<City>(20);
                    if (ScanNearestCities(out cities))
                    {
                        cities.Add(this);
                        WorldInstance.Teams.Where(c => c.TeamColor == TeamColor).First().Communs.Add(new Сommun(Parent,TeamColor, cities,WorldInstance, Position));
                        InCommun = true;
                    }
                    else
                        cities.Clear();
                }
            }
            else
              this.Dispose();
        }

        private bool AmIInCommun()
        {
            foreach(var com in WorldInstance.Teams.Where(c => c.TeamColor == TeamColor).First().Communs)
            {
                foreach (var center in com.Centers)
                {
                    if (Math.Sqrt(Math.Pow(((center.Item1 + 25) - (this.position.Item1 + 25)), 2) + Math.Pow(((center.Item2 + 25) - (this.position.Item2 + 25)), 2)) <= 126)
                    {
                        this.InCommun = true;
                        com.CitiesList.Add(this);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ScanNearestCities(out List<City> cities)
        {
            cities = new List<City>(20);
            if (InCommun == false)
            {
                foreach (var team in WorldInstance.Teams)
                {
                    foreach (var city in team.Cities)
                    {
                        if (city != this)
                        {
                            if (Math.Sqrt(Math.Pow(((city.Position.Item1+25) - (this.position.Item1 + 25)), 2) + Math.Pow(((city.Position.Item2 + 25) - (this.position.Item2 + 25)), 2)) <= 126)
                            {
                                if (city.TeamColor != this.TeamColor)
                                    return false;
                                else if (city.TeamColor == TeamColor && city.InCommun == false)
                                    cities.Add(city);
                            }
                        }
                    }
                }
                if (cities.Count >=2)
                    return true;
                cities.Clear();
            }
            return false;
        }

        public int Get10Percents()
        {
            double percent = (CitizenCount * 0.1);
            citizencount -= (int)percent;
            return (int)percent;
        }

        public void GetDamage(int warriorsataacking)
        {
            citizencount -= (int)(warriorsataacking+(CitizenCount*0.05));
        }

        public void Dispose()
        {
            WorldInstance.Journal.AddNewEvent(WorldInstance.Year + ":The city of the " + TeamColor.Name + " team was DESTROYED after the disaster");
            if(InCommun)
            {
                //WorldInstance.Teams.Where(t => t.TeamColor == TeamColor).First().Communs.Where(c => c.CitiesList.Contains(this)).First().CitiesList.Remove(this);
                foreach (var com in WorldInstance.Teams.Where(t=>t.TeamColor==TeamColor).First().Communs)
                {
                    if (com.CitiesList.Contains(this))
                    {
                        foreach (var r in com.Roads.Where(r => r.Position1 == Position || r.Position2 == Position).ToList())
                            r.Dispose();
                        com.CitiesList.Remove(this);
                    }
                    if (com.Centers.Contains(this.Position))
                        com.Centers.Remove(this.Position);
                }
            }
            WorldInstance.Teams.Where(t => t.TeamColor == TeamColor).First().Cities.Remove(this);
            CityInstance.Opacity = 0;
            CitizenCountLabel.Opacity = 0;
            CitizenIncrementLabel.Opacity = 0;
            Parent.Children.Remove(CityInstance);
            Parent.Children.Remove(CitizenCountLabel);
            Parent.Children.Remove(CitizenIncrementLabel);
            CityInstance = null;
            CitizenCountLabel = null;
            CitizenIncrementLabel = null;
            CitizenIncrement = null;
            player.Dispose();
            CitizenIncrementTimer.Dispose();
            RandLabelIncrementUp = null;
            //RandEventRnd = null;
            GC.Collect();
        }
    }
}

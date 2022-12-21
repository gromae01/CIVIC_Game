using NAudio.Wave;
using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Linq;

namespace WinFormsApp1.Data.Instaces
{
    public class Citizen : IDisposable
    {
        private (int, int) position;
        public  System.Windows.Forms.Timer Citizen_Timer_Direction_Choose;
        public System.Windows.Forms.Timer Citizen_Timer_Move;
        public System.Windows.Forms.Timer Citizen_Timer_Build;
        private System.Windows.Controls.Image GearsOfBuild;
        private ProgressBar BuildProgressBar;
        private System.Windows.Controls.Image CitizenInstance;
        private World WorldInstance;
        private System.Drawing.Color TeamColor;
        private Canvas Parent;
        private int BuildSpeed;
        private int YearOfSpawn;
        private SoundPlayer player;
        public bool startbuild = false;
        private (int, int) Position { get => position; set => position = value; }
       
        public Citizen(Canvas parent, System.Drawing.Color teamcolor, World worldInstance, (int,int)StartPosition)
        {
            YearOfSpawn = worldInstance.Year;
            TeamColor = teamcolor;
            Parent = parent;
            Random rnd = new Random();
            BuildSpeed = rnd.Next(10,20);
            

            //Play Sound
            player = new SoundPlayer("Sounds/oi.wav");
            //----------------------------------

            //Start position 
            position.Item1 = StartPosition.Item1;
            position.Item2 = StartPosition.Item2;
            //----------------------------------

            //Image Citizen Settings
            CitizenInstance = new System.Windows.Controls.Image();
            CitizenInstance.Width = 37;
            CitizenInstance.Height = 32;
            Parent.Children.Add(CitizenInstance);
            //CitizenInstance.BackColor = Color.Transparent;
            //CitizenInstance.SizeMode = PictureBoxSizeMode.StretchImage;
            CitizenInstance.Margin = new System.Windows.Thickness(position.Item1, position.Item2,0,0);
            CitizenInstance.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Citizens/Citizen_"+teamcolor.Name+".png"));
            //----------------------------------

            //PictureBox GearsOfBuild Settings
            GearsOfBuild = new System.Windows.Controls.Image();
            GearsOfBuild.Opacity = 0;
            GearsOfBuild.Width = 20;
            GearsOfBuild.Height = 25;
            Parent.Children.Add(GearsOfBuild);
            //GearsOfBuild.BackColor = Color.Transparent;
            //GearsOfBuild.SizeMode = PictureBoxSizeMode.StretchImage;
            //GearsOfBuild.Zindex
            GearsOfBuild.Source = new BitmapImage(new Uri("pack://application:,,,/Images/gears_animation.gif"));
            //----------------------------------

            //ProgressBar BuildProgressBar Settings
            BuildProgressBar = new ProgressBar();
            BuildProgressBar.Opacity = 0;
            BuildProgressBar.Width = 40;
            BuildProgressBar.Height = 5;
            BuildProgressBar.Maximum = 150;
            BuildProgressBar.Minimum = 0;
            Parent.Children.Add(BuildProgressBar);
            //BuildProgressBar.Style = ;
            //BuildProgressBar.ForeColor = teamcolor;
            //----------------------------------

            //Timers Settings
            //Citizen_Timer_Direction_Choose
            Citizen_Timer_Direction_Choose = new System.Windows.Forms.Timer();
            Citizen_Timer_Direction_Choose.Interval = 500;
            Citizen_Timer_Direction_Choose.Tick += new EventHandler(Citizen_Timer_Direction_Choose_Tick);

            //Citizen_Timer_Build
            Citizen_Timer_Build = new System.Windows.Forms.Timer();
            Citizen_Timer_Build.Interval = 100;
            Citizen_Timer_Build.Tick += new EventHandler(Citizen_Timer_Build_Tick);

            //Citizen_Timer_Move
            Citizen_Timer_Move = new System.Windows.Forms.Timer();
            Citizen_Timer_Move.Interval = 10;
            Citizen_Timer_Move.Tick += new EventHandler(Citizen_Timer_Move_Tick);
            //----------------------------------

            WorldInstance = worldInstance;
        }
        //Scan nearest objects
        private bool AmIFarEnough(World ourworld)
        {
            foreach (var team in ourworld.Teams)
            {
                foreach (var citiz in team.Citizens)
                {
                    if ((Math.Sqrt(Math.Pow(citiz.Position.Item1 - this.Position.Item1,2)+Math.Pow(citiz.Position.Item2 - this.Position.Item2, 2)) <= 80) && (citiz.position.Item1 != position.Item1 && citiz.position.Item2 != position.Item2))
                        return false;
                }
                foreach (var cities in team.Cities)
                {
                    if (Math.Sqrt(Math.Pow(cities.Position.Item1 - this.Position.Item1, 2) + Math.Pow(cities.Position.Item2 - this.Position.Item2, 2)) <= 80)
                        return false;
                }
            }
            return true;   
        }
        //---------------------

       
        private  (int, int)[] Directions =
        {
            (0,-3),
            (-3,-3),
            (-3,0),
            (3,-3),
            (3,0),
            (-3,3),
            (3,3),
            (0,3)
        };
        private void Move(int choosendirection) 
        {
            if (position.Item2 < Parent.Height - 50 && position.Item1 < Parent.Width - 50 && position.Item1 > 0 && position.Item2 > 0)
            {
                position.Item1 += Directions[choosendirection].Item1;
                position.Item2 += Directions[choosendirection].Item2;
            }
            else if (position.Item2 >= Parent.Height - 50)
            {
                position.Item1 += Directions[0].Item1;
                position.Item2 += Directions[0].Item2;
            }
            else if (position.Item1 >= Parent.Width - 50)
            {
                position.Item1 += Directions[2].Item1;
                position.Item2 += Directions[2].Item2;
            }
            else if (position.Item1 <= 40)
            {
                position.Item1 += Directions[4].Item1;
                position.Item2 += Directions[4].Item2;
            }
            else if (position.Item2 <= 40)
            {
                position.Item1 += Directions[7].Item1;
                position.Item2 += Directions[7].Item2;
            }
            CitizenInstance.Margin = new System.Windows.Thickness(position.Item1, position.Item2, 0, 0);
            //CitizenInstance.Location = new Point(position.Item1, position.Item2);
            //CitizenInstance.Refresh();
        }
        private void StartBuildACity() 
        {
            startbuild = true;
            BuildProgressBar.Opacity = 1;
            BuildProgressBar.Margin = new System.Windows.Thickness(position.Item1 - 10, position.Item2 + 35,0,0);
            //BuildProgressBar.Refresh();

            GearsOfBuild.Opacity = 1;
            GearsOfBuild.Margin = new System.Windows.Thickness(position.Item1 - 20, position.Item2 - 1, 0, 0);
            //GearsOfBuild.Refresh();
        }
        private void BuildingACity()
        {
            
            if (BuildProgressBar.Value < 150)
            {
                if (BuildProgressBar.Value + BuildSpeed < 150)
                    BuildProgressBar.Value += BuildSpeed;
                else
                    BuildProgressBar.Value = 150;
            }
            else
            {
                startbuild = false;
                Citizen_Timer_Build.Stop();
                WorldInstance.Teams.Where(t => t.TeamColor == TeamColor).First().Cities.Add(new City(Parent, TeamColor, WorldInstance, Position));
                this.Dispose();
            }
            }
        //---------------------

        int ChoosenDirection =0;

        //Methods for timers
        //Timers Tick Handlers
        private void Citizen_Timer_Direction_Choose_Tick(object sender, EventArgs e)
        {
            Random RandomDirection = new Random();
            ChoosenDirection = RandomDirection.Next(0, 7);
            Live(WorldInstance);
        }
        private void Citizen_Timer_Move_Tick(object sender, EventArgs e)
        {
            Move(ChoosenDirection);
        }
        private void Citizen_Timer_Build_Tick(object sender, EventArgs e)
        {
            BuildingACity();
        }
        //---------------------
        private void Live(World world)
        {
            if (world.Year - YearOfSpawn >= 3 && !Citizen_Timer_Build.Enabled)
            {
                //world.Journal.AddNewEvent(world.Year+":Citizen from team "+TeamColor.Name+" didn`t make it");
                this.Dispose();
            }
            else
            {
                if (!AmIFarEnough(world) && !Citizen_Timer_Build.Enabled)
                {
                    if (!Citizen_Timer_Move.Enabled)
                        Citizen_Timer_Move.Start();
                }
                else
                {
                    if (Citizen_Timer_Move.Enabled)
                        Citizen_Timer_Move.Stop();
                    if (Citizen_Timer_Direction_Choose.Enabled)
                        Citizen_Timer_Direction_Choose.Stop();
                    if (!Citizen_Timer_Build.Enabled)
                    {
                        StartBuildACity();
                        Citizen_Timer_Build.Start();
                    }
                }
            }
        }

        public void Dispose()
        {
            WorldInstance.Teams.Where(t => t.TeamColor == TeamColor).First().Citizens.Remove(this);
            //BuildProgressBar.Dispose();
            //CitizenInstance.Dispose();
            //GearsOfBuild.Dispose();
            BuildProgressBar.Opacity =0;
            CitizenInstance.Opacity = 0;
            GearsOfBuild.Opacity = 0;
            Parent.Children.Remove(CitizenInstance);
            Parent.Children.Remove(BuildProgressBar);
            Parent.Children.Remove(GearsOfBuild);
            BuildProgressBar = null;
            CitizenInstance = null;
            GearsOfBuild = null;
            Citizen_Timer_Build.Dispose();
            Citizen_Timer_Direction_Choose.Dispose();
            Citizen_Timer_Move.Dispose();
            Directions = null;
            player.Dispose();
            GC.Collect();
        }
    }
}

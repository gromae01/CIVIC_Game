using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WinFormsApp1.Data.Instaces;

namespace WpfApp1.Data.Instances
{
    public class Squad : IDisposable
    {
        int squadsize;
        private (double, double) Position;
        public System.Windows.Forms.Timer Squad_Timer_Move;
        private System.Windows.Controls.Image Attack;
        private ProgressBar AttackProgressBar;
        private System.Windows.Controls.Image SquadInstance;
        private Label SquadSize;
        public System.Windows.Forms.Timer Squad_Timer_Attack;
        private System.Drawing.Color TeamColor;
        private Canvas Parent;
        City citytarget = null;
        World WorldInstance;
        Team Opponent;
        
        public Squad (int size, Team opponentteam, (int,int) spawnpoint, World worldinstance, System.Drawing.Color color)
        {
            squadsize = size;
            TeamColor = color;
            Position = spawnpoint;
            Parent = worldinstance.playing_field;
            WorldInstance = worldinstance;
            Opponent = opponentteam;

            SelectTarget();
            //Start position
            Position.Item1 = spawnpoint.Item1;
            Position.Item2 = spawnpoint.Item2;
            //----------------------------------

            //Image Squad Settings
            SquadInstance = new System.Windows.Controls.Image();
            SquadInstance.Width = 37;
            SquadInstance.Height = 32;
            Parent.Children.Add(SquadInstance);
            //CitizenInstance.BackColor = Color.Transparent;
            //CitizenInstance.SizeMode = PictureBoxSizeMode.StretchImage;
            SquadInstance.Margin = new System.Windows.Thickness(Position.Item1, Position.Item2,0,0);
            SquadInstance.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Squads/Squad_" + color.Name + ".png"));
            //----------------------------------

            //Label SquadSize settings
            SquadSize = new Label();
            SquadSize.Width = 30;
            SquadSize.Height = 30;
            Parent.Children.Add(SquadSize);
            SquadSize.Margin = new System.Windows.Thickness(Position.Item1+7, Position.Item2+5, 0, 0);
            SquadSize.FontWeight = FontWeights.Bold;
            SquadSize.Content = squadsize.ToString();
            //----------------------------------

            //PictureBox Attack Settings
            Attack = new System.Windows.Controls.Image();
            Attack.Opacity = 0;
            Attack.Width = 20;
            Attack.Height = 25;
            Parent.Children.Add(Attack);
            //GearsOfBuild.BackColor = Color.Transparent;
            //GearsOfBuild.SizeMode = PictureBoxSizeMode.StretchImage;
            //GearsOfBuild.Zindex
            Attack.Source = new BitmapImage(new Uri("pack://application:,,,/Images/attack.png")); ;
            //----------------------------------

            //ProgressBar AttackProgressBar Settings
            AttackProgressBar = new ProgressBar();
            AttackProgressBar.Opacity = 0;
            AttackProgressBar.Width = 40;
            AttackProgressBar.Height = 5;
            AttackProgressBar.Maximum = squadsize;
            AttackProgressBar.Minimum = 0;
            Parent.Children.Add(AttackProgressBar);
            //BuildProgressBar.Style = ;
            //BuildProgressBar.ForeColor = teamcolor;
            //----------------------------------

            //Timers Settings
            //Squad_Timer_Move
            Squad_Timer_Move = new System.Windows.Forms.Timer();
            Squad_Timer_Move.Interval = 10;
            Squad_Timer_Move.Tick += new EventHandler(Squad_Timer_Move_Tick);

            //Squad_Timer_Attack
            Squad_Timer_Attack = new System.Windows.Forms.Timer();
            Squad_Timer_Attack.Interval = 100;
            Squad_Timer_Attack.Tick += new EventHandler(Squad_Timer_Attack_Tick);
            Squad_Timer_Move.Start();

        }
        public void Live()
        {
            if (squadsize <= 0 || Opponent == null || Opponent.Cities.Count <= 0)
                this.Dispose();
        }
        private void Squad_Timer_Move_Tick(object sender, EventArgs e)
        {
            if (citytarget != null && citytarget.CitizenCount>0)
            {
                double x = citytarget.Position.Item1 - Position.Item1;
                double y = citytarget.Position.Item2 - Position.Item2;
                Position.Item1 += x / 100;
                Position.Item2 += y / 100;
                SquadInstance.Margin = new System.Windows.Thickness(Position.Item1, Position.Item2, 0, 0);
                SquadSize.Margin = new System.Windows.Thickness(Position.Item1 + 5, Position.Item2 + 7, 0, 0);
                if (Math.Sqrt(Math.Pow(citytarget.Position.Item1 - Position.Item1, 2) + Math.Pow(citytarget.Position.Item2 - Position.Item2, 2)) < 30)
                {
                    StartAttack((((Position.Item1 + citytarget.Position.Item1) / 2), ((Position.Item2 + citytarget.Position.Item2) / 2)));
                    Squad_Timer_Move.Stop();
                    Squad_Timer_Attack.Start();
                }
            }
            else
            {
                SelectTarget();
                if (citytarget == null || citytarget.CitizenCount > 0)
                    this.Dispose();
            }
        }

        void StartAttack((double,double) AttackMarkerPos)
        {
            Attack.Opacity = 1;
            Attack.Margin = new System.Windows.Thickness(AttackMarkerPos.Item1, AttackMarkerPos.Item2, 0, 0);

            AttackProgressBar.Opacity = 1;
            AttackProgressBar.Margin = new System.Windows.Thickness(Position.Item1 - 10, Position.Item2 + 35, 0, 0);
        }
        private void Squad_Timer_Attack_Tick(object sender, EventArgs e)
        {
            if(citytarget!= null && AttackProgressBar.Value < AttackProgressBar.Maximum && citytarget.CitizenCount>0 && Opponent != null && Opponent.Cities.Count > 0)
            {
                Random RandomWarriorsToAttack = new Random();
                int WariorsToAttack = RandomWarriorsToAttack.Next(1, squadsize);
                if (AttackProgressBar.Value + WariorsToAttack < AttackProgressBar.Maximum)
                    AttackProgressBar.Value += WariorsToAttack;
                else
                    AttackProgressBar.Value = AttackProgressBar.Maximum;
                squadsize -= WariorsToAttack;
                SquadSize.Content = squadsize.ToString();
                citytarget.GetDamage(WariorsToAttack);
                citytarget.RefreshCitizenCount();
            }
            else if((citytarget == null || citytarget.CitizenCount <= 0) && AttackProgressBar.Value < AttackProgressBar.Maximum && Opponent!=null && Opponent.Cities.Count>0)
            {
                Attack.Opacity = 0;
                AttackProgressBar.Opacity = 0;
                SquadSize.Content = squadsize.ToString();
                Squad_Timer_Attack.Stop();
                Squad_Timer_Move.Start();
            }
            else if (AttackProgressBar.Value >= AttackProgressBar.Maximum)
            {
                Squad_Timer_Attack.Stop();
                this.Dispose();
            }
        }

        void SelectTarget()
        {
            if (Opponent != null)
            {
                double mindist = double.MaxValue;
                foreach (var city in Opponent.Cities)
                {
                    if (citytarget == null || citytarget.CitizenCount<=0)
                    {
                        citytarget = city;
                        mindist = Math.Sqrt(Math.Pow(Position.Item1 - city.Position.Item1, 2) + Math.Pow(Position.Item2 - city.Position.Item2, 2));
                    }
                    else
                    {
                        if (Math.Sqrt(Math.Pow(Position.Item1 - city.Position.Item1, 2) + Math.Pow(Position.Item2 - city.Position.Item2, 2)) < mindist)
                        {
                            citytarget = city;
                            mindist = Math.Sqrt(Math.Pow(Position.Item1 - city.Position.Item1, 2) + Math.Pow(Position.Item2 - city.Position.Item2, 2));
                        }
                    }

                }
            }
            
        }

        public void Dispose()
        {
            if(WorldInstance.Teams.Where(t => t.TeamColor == TeamColor).Any())
                WorldInstance.Teams.Where(t => t.TeamColor == TeamColor).First().Squads.Remove(this);
            AttackProgressBar.Opacity = 0;
            SquadInstance.Opacity = 0;
            Attack.Opacity = 0;
            Parent.Children.Remove(SquadInstance);
            Parent.Children.Remove(AttackProgressBar);
            Parent.Children.Remove(Attack);
            Parent.Children.Remove(SquadSize);
            SquadSize = null;
            AttackProgressBar = null;
            SquadInstance = null;
            Attack = null;
            Squad_Timer_Attack.Dispose();
            Squad_Timer_Move.Dispose();
            GC.Collect();
        }
    }
}

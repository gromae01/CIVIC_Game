using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WinFormsApp1.Data.Instaces;
using WinFormsApp1.Data.UI.UI_Logic;
using WpfApp1.Data;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer sidebar_timer = new DispatcherTimer();
        DispatcherTimer upper_timer = new DispatcherTimer();
        DispatcherTimer World_timer = new DispatcherTimer();
        World world;
        WaveOut waveOut;
        
        public MainWindow()
        {

            InitializeComponent();
            AudioPlayer audioPlayer = new AudioPlayer();
            //Timers Settings
            sidebar_timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            sidebar_timer.Tick += new EventHandler(sidebar_timer_Tick);

            upper_timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            upper_timer.Tick += new EventHandler(upper_timer_Tick);

            World_timer.Interval = new TimeSpan(0, 0, 0, 3);
            World_timer.Tick += new EventHandler(World_timer_Tick);
            //----------------------

            
            world = new World(playing_canvas, Journal_ListBox);
                     
            Year_Label.Content = "" + world.Year;
            audioPlayer.PlaySound("Sounds/GameStarted.wav");
        }
        private void World_timer_Tick(object sender, EventArgs e)
        {
            Year_Label.Content = "" + world.Year;
            world.WorldLives();
        }


        SideBarLogic sidebarL = new SideBarLogic();
        RotateTransform sidebar_rotateTransform = new RotateTransform();
        RotateTransform upperbar_rotateTransform = new RotateTransform();

        bool side_state = true;
      
        private void sidebar_timer_Tick(object sender, EventArgs e)
        {
            if (side_state != sidebarL.DoMethod(sidebar, side_state))
            {
                sidebar_timer.Stop();
                side_state = !side_state;
            }
        }
        private void sidebar_menu_pic_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
         
        }

        bool upper_state = true;
        UpperBarLogic upperbarL = new UpperBarLogic();
        private void upperbar_pic_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!sidebar_timer.IsEnabled)
            {
                upperbar_rotateTransform.Angle = upperbar_rotateTransform.Angle == 180 ? 0 : 180;
                upperbar_pic.RenderTransform = upperbar_rotateTransform;
                upperbarL.PlaySound("Sounds/sb.wav");
                upper_timer.Start();
            }
        }
        private void upper_timer_Tick(object sender, EventArgs e)
        {
            if (upper_state != upperbarL.DoMethod(upperbar_canvas, upper_state))
            {
                upper_timer.Stop();
                upper_state = !upper_state;
            }
        }

        private void AttackImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AudioPlayer audioPlayer = new AudioPlayer();
            audioPlayer.PlaySound("Sounds/sb.wav");
            if(world.AllowWars)
            {
                world.AllowWars = false;
                AttackImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/cancel_attack.png"));
            }
            else
            {
                world.AllowWars = true;
                AttackImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/attack.png"));
            }
        }

        private void PlayPause_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AudioPlayer audioPlayer = new AudioPlayer();
            audioPlayer.PlaySound("Sounds/sb.wav");
            if (world.Teams.Count > 0)
            {
                if (World_timer.IsEnabled)
                {
                    foreach (var team in world.Teams)
                    {
                        foreach (var citizen in team.Citizens)
                        {
                            if (citizen.startbuild)
                                citizen.Citizen_Timer_Build.Stop();
                            else
                            {
                                citizen.Citizen_Timer_Move.Stop();
                                citizen.Citizen_Timer_Direction_Choose.Stop();
                            }  
                        }
                        foreach (var squad in team.Squads)
                            squad.Squad_Timer_Move.Stop();
                        foreach (var city in team.Cities)
                            city.CitizenIncrementTimer.Stop();
                    }
                    World_timer.Stop();
                    PlayPause.Source = new BitmapImage(new Uri("pack://application:,,,/Images/play.png"));
                }
                else
                {
                    foreach (var team in world.Teams)
                    {
                        foreach (var citizen in team.Citizens)
                        {
                            if (citizen.startbuild)
                                citizen.Citizen_Timer_Build.Start();
                            else
                            {
                                citizen.Citizen_Timer_Move.Start();
                                citizen.Citizen_Timer_Direction_Choose.Start();
                            }  
                        }
                        foreach (var squad in team.Squads)
                            squad.Squad_Timer_Move.Start();
                        foreach (var city in team.Cities)
                            city.CitizenIncrementTimer.Start();
                    }
                    World_timer.Start();
                    PlayPause.Source = new BitmapImage(new Uri("pack://application:,,,/Images/pause.png"));
                }
            }
            else
                MessageBox.Show("No team selected!");
        }

        private void StartButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            AudioPlayer audioPlayer = new AudioPlayer();
            audioPlayer.PlaySound("Sounds/sb.wav");
            System.Drawing.Color color1 = System.Drawing.Color.Red;
            System.Drawing.Color color2 = System.Drawing.Color.Blue;
            System.Drawing.Color color3 = System.Drawing.Color.Green;
            System.Drawing.Color color4 = System.Drawing.Color.Orange;
            System.Drawing.Color color5 = System.Drawing.Color.DarkOrange;
            System.Drawing.Color color6 = System.Drawing.Color.Violet;
            if (RedCheck.IsChecked == true)
                world.AddTeam("abc1", color1);
            if (BlueCheck.IsChecked == true)
                world.AddTeam("abc2", color2);
            if (GreenCheck.IsChecked == true)
                world.AddTeam("abc3", color3);
            if (OrangeCheck.IsChecked == true)
                world.AddTeam("abc4", color4);
            if (DarkOrangeCheck.IsChecked == true)
                world.AddTeam("abc5", color5);
            if (VioletCheck.IsChecked == true)
                world.AddTeam("abc6", color6);
            if (world.Teams.Count > 0)
            {
                InitialLabel.Opacity = 0;
                sidebar.Children.Remove(InitialLabel);
                RedCheck.Opacity = 0;
                sidebar.Children.Remove(RedCheck);
                BlueCheck.Opacity = 0;
                sidebar.Children.Remove(BlueCheck);
                GreenCheck.Opacity = 0;
                sidebar.Children.Remove(GreenCheck);
                OrangeCheck.Opacity = 0;
                sidebar.Children.Remove(OrangeCheck);
                DarkOrangeCheck.Opacity = 0;
                sidebar.Children.Remove(DarkOrangeCheck);
                VioletCheck.Opacity = 0;
                sidebar.Children.Remove(VioletCheck);
                StartButton.Opacity = 0;
                sidebar.Children.Remove(StartButton);
                World_timer.Start();
            }
            else
                MessageBox.Show("No team selected!");
        }

        private void JournalImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AudioPlayer audioPlayer = new AudioPlayer();
            audioPlayer.PlaySound("Sounds/sb.wav");
            if (Journal_ListBox.Visibility == Visibility.Visible)
            {
                Journal_ListBox.Visibility = Visibility.Hidden;
                JournalImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/journal.png"));
            }
            else
            {
                Journal_ListBox.Visibility = Visibility.Visible;
                JournalImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/journal_cancel.png"));
            }
        }

        private void SpeedUpImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AudioPlayer audioPlayer = new AudioPlayer();
            audioPlayer.PlaySound("Sounds/sb.wav");
            TimeSpan interval = new TimeSpan(0, 0, 0, 3);
            if (World_timer.Interval== interval)
            {
                SpeedUpImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/speedup_cancel.png"));
                World_timer.Interval = new TimeSpan(0,0,0,1,50);
            }
            else
            {
                SpeedUpImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/speedup.png"));
                World_timer.Interval = new TimeSpan(0, 0, 0, 3);
            }
        }

        private void AboutImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AudioPlayer audioPlayer = new AudioPlayer();
            audioPlayer.PlaySound("Sounds/sb.wav");
            AboutWindow page = new AboutWindow();
            page.Show();
        }
    }
}

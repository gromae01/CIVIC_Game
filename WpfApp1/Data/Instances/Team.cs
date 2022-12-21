using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Controls;
using WpfApp1.Data.Instances;

namespace WinFormsApp1.Data.Instaces
{
    public class Team:IDisposable
    {
       
        public string TeamName { get; init; }
        public Color TeamColor { get; init; }
        private World WorldInstance;
        public List<City> Cities;
        public List<Citizen> Citizens;
        public List<Сommun> Communs;
        public List<Squad> Squads;
        public bool InWar = false;
        public Team(string teamname, Color teamcolor, Canvas playing_field,World world)
        {
            WorldInstance = world;
            Citizens = new List<Citizen>(100);
            Cities = new List<City>(50);
            Communs = new List<Сommun>(20);
            Squads = new List<Squad>(20);
            TeamName = teamname;
            TeamColor = teamcolor;
            Random startposition = new Random();
            Citizens.Add(new Citizen(playing_field,TeamColor, world, (startposition.Next(30, 750), startposition.Next(30,470))));
        }
        
        public void Live()
        {
            if (Cities.Count > 0 || Citizens.Count > 0)
            {
                foreach (var sq in Squads.ToList())
                    sq.Live();
                foreach (var citizen in Citizens.ToList())
                    if (!citizen.Citizen_Timer_Direction_Choose.Enabled)
                        citizen.Citizen_Timer_Direction_Choose.Start();
                foreach (var city in Cities.ToList())
                    city.Live();
                foreach (var com in Communs.ToList())
                    com.Live();
                if(InWar==false && WorldInstance.Teams.Count>1 && this.Cities.Count>0 && WorldInstance.AllowWars)
                {
                    Random RandomWarDeclaration = new Random();
                    if (RandomWarDeclaration.Next(1, 20) == 5)//Chance to Declare war
                    {
                        Team teamToWar = null;
                        List<int> TeamsToChoose = new List<int>(WorldInstance.Teams.Count);
                        for (int i = 0; i < WorldInstance.Teams.Count; i++)
                            TeamsToChoose.Add(i);
                        while(teamToWar==null)
                        {
                            TeamsToChoose.Sort();
                            Random Random = new Random();
                            int rnd = Random.Next(TeamsToChoose[0], TeamsToChoose[TeamsToChoose.Count-1]);
                            teamToWar = WorldInstance.Teams[rnd];
                            if (teamToWar == this)
                            {
                                teamToWar = null;
                                TeamsToChoose.Remove(rnd);
                            }
                        }
                        if (teamToWar != null && teamToWar.Cities.Count>=2)
                        {
                            WorldInstance.Wars.Add(new War(this, teamToWar, WorldInstance));
                            InWar = true;
                        }
                    }
                }
                
            }
            else
                this.Dispose();
        }

        public void Dispose()
        {
            WorldInstance.Journal.AddNewEvent(WorldInstance.Year + ":THE " + TeamColor.Name + " TEAM WAS DESTROYED");
            WorldInstance.Teams.Remove(this);
            foreach (var city in Cities)
                city.Dispose();
            Cities.Clear();
            Citizens.Clear();
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Data.Instaces;

namespace WpfApp1.Data.Instances
{
    public class War : IDisposable
    {
        //Team Team1;
        //Team Team2;
        World WorldInstance;
        List<Team> WarringTeams;
        public War(Team team1, Team team2, World worldinstance)
        {
            AudioPlayer player = new AudioPlayer();
            player.PlaySound("Sounds/warstart.wav");
            team1.InWar = true;
            team2.InWar = true;
            WarringTeams = new List<Team>(2);
            WarringTeams.Add(team1);
            WarringTeams.Add(team2);
            WorldInstance = worldinstance;
            WorldInstance.Journal.AddNewEvent(WorldInstance.Year+":" + team1.TeamColor.Name + " team declared WAR on "+team2.TeamColor.Name + " team");
        }

        public void Live()
        {
            if (WarringTeams.Where(t => t == null || t.Cities.Count <= 0).Count() <= 0)
            {
                foreach (var team in WarringTeams)
                {
                    if (team.Communs.Count == 0)
                    {
                        foreach (var city in team.Cities)
                        {
                            int size = city.Get10Percents();
                            if (size != 0)
                                team.Squads.Add(new Squad(size, WarringTeams.Where(t => t != team).First(), city.Position, WorldInstance, team.TeamColor));
                        }
                    }
                    else
                    {
                        int CountOfNewSquad = 0;
                        foreach (var com in team.Communs)
                        {
                            foreach (var city in com.CitiesList)
                                CountOfNewSquad += city.Get10Percents();
                            if (CountOfNewSquad != 0)
                                team.Squads.Add(new Squad(CountOfNewSquad, WarringTeams.Where(t => t != team).First(), com.Centers[0], WorldInstance, team.TeamColor));
                            CountOfNewSquad = 0;
                        }
                    }
                }
            }
            else
                this.Dispose();
        }
        public void Dispose()
        {
            WorldInstance.Wars.Remove(this);
            WarringTeams.Where(t => t != null && t.Cities.Count > 0).First().InWar = false;
            WarringTeams.Clear();
        }
    }
}

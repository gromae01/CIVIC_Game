using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfApp1.Data.Instances;

namespace WinFormsApp1.Data.Instaces
{
    public class World 
    {
        public Canvas playing_field;
        private int year;
        public EventJournal Journal;
        public List<Team> Teams;
        public List<War> Wars;
        public bool AllowWars = true;
        public World(Canvas worldInstance, ListBox journal)
        {
            Teams = new List<Team>(6);
            Wars = new List<War>(6);
            Journal = new EventJournal(journal);
            Year = 0;
            playing_field = worldInstance;
        }

        public void AddTeam(string TeamName, Color TeamColor)
        {
            Teams.Add(new Team(TeamName,TeamColor, playing_field,this));
        }
        public int Year { get => year; set => year = value; }

        public void WorldLives()
        {
            YearPassed();
            foreach (var war in this.Wars.ToList())
                war.Live();
            foreach (var team in this.Teams.ToList())
                team.Live();
        }
        private void YearPassed() => Year++;
    }
}

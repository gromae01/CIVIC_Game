using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WinFormsApp1.Data.Instaces
{
    public class EventJournal
    {
        private ListBox Journal;
        public EventJournal(ListBox journal)
        {
            Journal = journal;
        }
        public void AddNewEvent(string EventString)
        {
            Journal.Items.Add(EventString);
        }
    }
}

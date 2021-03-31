using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaSimulatie.SimulationObjects
{
    public class App
    {

        Person owner;
        LinkedList<(App,int)> contacts;
        HashSet<App> hascontacts;


        Dictionary<App, (int,int)> close;

        int step;

        public App()
        {
            owner = null;
            contacts = new LinkedList<(App,int)>();
            close = new Dictionary<App, (int,int)>();
            hascontacts = new HashSet<App>();

            step = 0;
        }

        public void Run()
        {
            if (owner == null)
                return;

            step++;

            RemoveContacts();
            AddContacts();
        }

        private void RemoveContacts()
        {
            if (contacts.Count == 0)
                return;
            (App app, int tijd) pair = contacts.First();
            if (step - pair.tijd >= Globals.connectiontime/ Globals.timestep)
            {
                contacts.RemoveFirst();
                hascontacts.Remove(pair.app);
                RemoveContacts();
            }
        }

        private void AddContacts()
        {
            List<List<App>> neighbours = owner.Tile.AppNeighbours();
            foreach(List<App> l in neighbours)
            {
                foreach(App app in l)
                {
                    if (hascontacts.Contains(app))
                        return;

                    if (!close.ContainsKey(app))
                        close.Add(app, (step, step));

                    (int last, int start) pair = close[app];

                    if (pair.last != step - 1)
                    {
                        close[app] = (step, step);
                        continue;
                    }

                    close[app] = (step, pair.start);

                    if (pair.last - pair.start >= Globals.getconnectedTime/ Globals.timestep)
                    {
                        contacts.AddLast((app, step));
                        hascontacts.Add(app);
                    }
                }
            }
        }

        public void RecieveMessage()
        {
            owner.CheckSickness();
        }

        public void SentMessage()
        {
            foreach((App, int) app in contacts)
            {
                app.Item1.RecieveMessage();
            }
        }

        public Person Owner
        {
            get { return owner; }
            set { owner = value; }
        }
    }
}

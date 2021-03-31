using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaSimulatie.SimulationObjects
{
    public class Tile
    {
        protected List<Person> passengers;
        protected List<Person> infectiousPassengers;
        protected List<App> appPassengers;

        protected int x, y;
        protected WorldGrid world;

        public Tile(int x, int y, WorldGrid world)
        {
            passengers = new List<Person>();
            infectiousPassengers = new List<Person>();
            appPassengers = new List<App>();

            this.x = x;
            this.y = y;

            this.world = world;
        }

        /// <summary>
        /// Move a person from one tile to another;
        /// </summary>
        /// <param name="p">the person that moves.</param>
        public void Move(Person p)
        {
            Tile t = world.GetTile(p.X, p.Y);
            if (t == this)
                return;

            passengers.Remove(p);
            if (p.HealthStatus == HealthStatus.Infectious)
                infectiousPassengers.Remove(p);
            if (p.App != null && p.HealthStatus != HealthStatus.Recovered)
                appPassengers.Remove(p.App);
            world.TilePerson(p);
        }

        public void UpdateStatus(Person p)
        {
            if (p.HealthStatus == HealthStatus.Infectious)
                infectiousPassengers.Add(p);
            if (p.HealthStatus == HealthStatus.Recovered)
            {
                infectiousPassengers.Remove(p);
                if (p.App != null)
                    appPassengers.Remove(p.App);
            }
        }

        public void AddPassenger(Person p)
        {
            passengers.Add(p);
            if (p.HealthStatus == HealthStatus.Infectious)
                infectiousPassengers.Add(p);
            if (p.App != null && p.HealthStatus != HealthStatus.Recovered)
                appPassengers.Add(p.App);
        }


        /// <summary>
        /// Get all neighbouring people.
        /// </summary>
        /// <returns>returns all neighbouring people.</returns>
        public List<List<Person>> Neighbours()
        {
            List<List<Person>> neighbours = new List<List<Person>>();

            for (int x = this.x-1; x <= this.x+1; x++)
            {
                for (int y = this.y-1; y <= this.y+1; y++)
                {
                    Tile neighbour = world[x, y];
                    if (neighbour != null)
                        neighbours.Add(neighbour.Passengers);
                }
            }
            return neighbours;
        }

        /// <summary>
        /// Get all ill neighbouring people.
        /// </summary>
        /// <returns>returns all neighbouring people.</returns>
        public List<List<Person>> InfectiousNeighbours()
        {
            List<List<Person>> neighbours = new List<List<Person>>();

            for (int x = this.x - 1; x <= this.x + 1; x++)
            {
                for (int y = this.y - 1; y <= this.y + 1; y++)
                {
                    Tile neighbour = world[x, y];
                    if (neighbour != null)
                        neighbours.Add(neighbour.InfectiousPassengers);
                }
            }
            return neighbours;
        }

        public List<List<App>> AppNeighbours()
        {
            List<List<App>> neighbours = new List<List<App>>();

            for (int x = this.x - 1; x <= this.x + 1; x++)
            {
                for (int y = this.y - 1; y <= this.y + 1; y++)
                {
                    Tile neighbour = world[x, y];
                    if (neighbour != null)
                        neighbours.Add(neighbour.AppPassengers);
                }
            }

            return neighbours;
        }


        public List<Person> Passengers
        {
            get { return passengers; }
        }

        public List<Person> InfectiousPassengers
        {
            get { return infectiousPassengers; }
        }

        public List<App> AppPassengers
        {
            get { return appPassengers; }
        }

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }
    }
}

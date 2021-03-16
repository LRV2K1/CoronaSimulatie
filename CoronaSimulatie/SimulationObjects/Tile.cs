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
        protected List<Person> illpassengers;

        protected int x, y;
        protected WorldGrid world;

        public Tile(int x, int y, WorldGrid world)
        {
            passengers = new List<Person>();
            illpassengers = new List<Person>();

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
            if (p.HealthStatus == HealthStatus.Ill)
                illpassengers.Remove(p);
            world.TilePerson(p);
        }

        public void UpdateStatus(Person p)
        {
            if (p.HealthStatus == HealthStatus.Ill)
                illpassengers.Add(p);
            if (p.HealthStatus == HealthStatus.Recovered)
                illpassengers.Remove(p);
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
        public List<List<Person>> IllNeighbours()
        {
            List<List<Person>> neighbours = new List<List<Person>>();

            for (int x = this.x - 1; x <= this.x + 1; x++)
            {
                for (int y = this.y - 1; y <= this.y + 1; y++)
                {
                    Tile neighbour = world[x, y];
                    if (neighbour != null)
                        neighbours.Add(neighbour.IllPassengers);
                }
            }
            return neighbours;
        }


        public List<Person> Passengers
        {
            get { return passengers; }
        }

        public List<Person> IllPassengers
        {
            get { return illpassengers; }
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

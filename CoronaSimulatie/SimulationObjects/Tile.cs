using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaSimulatie.SimulationObjects
{
    class Tile
    {
        List<Person> passengers;

        int x, y;
        WorldGrid world;

        public Tile(int x, int y, WorldGrid world)
        {
            passengers = new List<Person>();

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
            world.TilePerson(p);
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


        public List<Person> Passengers
        {
            get { return passengers; }
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

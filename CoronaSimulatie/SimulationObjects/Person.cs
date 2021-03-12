using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaSimulatie.SimulationObjects
{
    public class Person
    {
        protected float x, y;

        protected Tile tile;

        public Person(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public virtual void Move()
        {
            if (tile == null)
                return;

            x += 25f;
            y += 10f;

            tile.Move(this);
        }

        public void Sickness()
        {

        }

        public float X
        {
            get { return x; }
        }

        public float Y
        {
            get { return y; }
        }

        public virtual Tile Tile
        {
            get { return tile; }
            set { 
                tile = value;
                Console.WriteLine("Move person{pos: (" + x + ", " + y + "), tile: (" + tile.X + ", " + tile.Y + ")}");
            }
        }
    }
}

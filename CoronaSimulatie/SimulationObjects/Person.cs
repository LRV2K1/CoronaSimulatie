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
        protected Random random;

        public Person(int x, int y, Random random)
        {
            this.x = x;
            this.y = y;
            this.random = random;
        }

        public virtual void Move()
        {
            if (tile == null)
                return;

            x += ((float)random.NextDouble() - 0.5f) * 10;
            y += ((float)random.NextDouble() - 0.5f) * 10;
            
            tile.Move(this);
        }

        public void Sickness()
        {

        }

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
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

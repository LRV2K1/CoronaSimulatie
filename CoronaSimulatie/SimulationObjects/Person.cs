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

        float direction;

        public Person(float x, float y, Random random)
        {
            this.x = x;
            this.y = y;
            this.random = random;
        }

        public virtual void Move()
        {
            if (tile == null)
                return;

            direction += (float)((random.NextDouble() - 0.5f) * Math.PI * 0.5);
            float distance = (float)random.NextDouble() * 5f;

            x += (float)(distance * Math.Cos(direction));
            y += (float)(distance * Math.Sin(direction));


            //x += ((float)random.NextDouble() - 0.5f) * 10;
            //y += ((float)random.NextDouble() - 0.5f) * 10;
            
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

        public float Direction
        {
            get { return direction; }
            set { direction = value; }
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

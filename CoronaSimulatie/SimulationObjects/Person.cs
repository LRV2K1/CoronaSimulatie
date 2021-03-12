using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaSimulatie.SimulationObjects
{
    class Person
    {
        float x, y;

        Tile tile;

        public Person(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Move()
        {
            if (tile == null)
                return;

            tile.Move(this);
            x += 25f;
            y += 10f;
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

        public Tile Tile
        {
            get { return tile; }
            set { 
                tile = value;
                Console.WriteLine("Move person{pos: (" + x + ", " + y + "), tile: (" + tile.X + ", " + tile.Y + ")}");
            }
        }
    }
}

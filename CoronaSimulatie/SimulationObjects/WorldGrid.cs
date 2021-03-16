using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaSimulatie.SimulationObjects
{
    public class WorldGrid
    {
        protected int tilesize;

        protected Tile[,] grid;

        public WorldGrid(int gridsize, int tilesize, List<Person> people)
        {
            grid = new Tile[gridsize, gridsize];
            this.tilesize = tilesize;

            for(int x = 0; x < gridsize; x++)
            {
                for (int y = 0; y < gridsize; y++)
                {
                    grid[x, y] = new Tile(x,y,this);
                }
            }

            foreach(Person p in people)
            {
                int x = (int)(p.X / (float)this.tilesize);
                int y = (int)(p.Y / (float)tilesize);

                p.Tile = this[x, y];
                if (this[x,y] != null)
                    this[x, y].Passengers.Add(p);
            }
        }

        public Tile GetTile(float x, float y)
        {
            int ix = (int)(x / (float)tilesize);
            int iy = (int)(y / (float)tilesize);
            if ((x / (float)tilesize) < 0 || (y / (float)tilesize) < 0)
                return null;

            return this[ix, iy];
        }

        public void TilePerson(Person p)
        {
            if (p.X < 0)
            {
                p.GetTarget();
                p.X = 0;
            }
            if (p.Y < 0)
            {
                p.GetTarget();
                p.Y = 0;
            }
            if (p.X >= Globals.worldsize)
            {
                p.GetTarget();
                p.X = Globals.worldsize-1;
            }
            if (p.Y >= Globals.worldsize)
            {
                p.GetTarget();
                p.Y = Globals.worldsize-1;
            }

            int x = (int)(p.X / (float)tilesize);
            int y = (int)(p.Y / (float)tilesize);

            p.Tile = this[x, y];
            if (this[x, y] != null)
            {
                this[x, y].Passengers.Add(p);
                if (p.HealthStatus == HealthStatus.Ill)
                    this[x, y].IllPassengers.Add(p);
            }
        }


        public Tile this[int x, int y]
        {
            get
            {
                if (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
                    return grid[x, y];
                return null;
            }
            set
            {
                if (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
                    grid[x, y] = value;
            }
        }

        public int TileSize
        {
            get { return tilesize; }
        }

        public int Widht
        {
            get { return grid.GetLength(0); }
        }

        public int Height
        {
            get { return grid.GetLength(1); }
        }
    }
}

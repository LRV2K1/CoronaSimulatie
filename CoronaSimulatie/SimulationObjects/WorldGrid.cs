using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaSimulatie.SimulationObjects
{
    public class WorldGrid
    {
        protected int tilewidth, tileheight;

        protected Tile[,] grid;

        public WorldGrid(int width, int height, int tilewidth, int tileheight, List<Person> people)
        {
            grid = new Tile[width, height];
            this.tilewidth = tilewidth;
            this.tileheight = tileheight;

            for(int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = new Tile(x,y,this);
                }
            }

            foreach(Person p in people)
            {
                int x = (int)(p.X / (float)tilewidth);
                int y = (int)(p.Y / (float)tileheight);

                p.Tile = this[x, y];
                if (this[x,y] != null)
                    this[x, y].Passengers.Add(p);
            }
        }

        public Tile GetTile(float x, float y)
        {
            int ix = (int)(x / (float)tilewidth);
            int iy = (int)(y / (float)tileheight);
            if ((x / (float)tilewidth) < 0 || (y / (float)tileheight) < 0)
                return null;

            return this[ix, iy];
        }

        public void TilePerson(Person p)
        {
            if (p.X < 0)
            {
                p.X = 0;
                p.Direction -= (float)Math.PI;
            }
            if (p.Y < 0)
            {
                p.Y = 0;
                p.Direction -= (float)Math.PI;
            }
            if (p.X >= tilewidth * grid.GetLength(0))
            {
                p.X = (tilewidth * grid.GetLength(0) - 1);
                p.Direction -= (float)Math.PI;
            }
            if (p.Y >= tileheight * grid.GetLength(1))
            {
                p.Y = (tileheight * grid.GetLength(1) - 1);
                p.Direction -= (float)Math.PI;
            }

            int x = (int)(p.X / (float)tilewidth);
            int y = (int)(p.Y / (float)tileheight);

            p.Tile = this[x, y];
            if (this[x, y] != null)
                this[x, y].Passengers.Add(p);
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

        public int TileWidht
        {
            get { return tilewidth; }
        }

        public int TileHeight
        {
            get { return tileheight; }
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

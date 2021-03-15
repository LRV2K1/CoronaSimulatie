using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoronaSimulatie.SimulationObjects;
using Engine.GameManagment.GameObjects;
using Engine.GameManagment.Assets;
using Engine.GameManagment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VisualCoronaSimulatie.Simulation_Objects
{
    class DrawableWorld : WorldGrid
    {
        List<DrawableTile> tiles;

        public DrawableWorld(int gridsize, int tilesize, List<Person> people)
            : base(gridsize, tilesize, people)
        {
            tiles = new List<DrawableTile>();

            for (int x = 0; x < gridsize; x++)
            {
                for (int y = 0; y < gridsize; y++)
                {
                    DrawableTile t = new DrawableTile(x, y, this);
                    grid[x, y] = t;
                    tiles.Add(t);
                }
            }

            foreach (Person p in people)
            {
                int x = (int)(p.X / (float)tilesize);
                int y = (int)(p.Y / (float)tilesize);

                p.Tile = this[x, y];
                if (this[x, y] != null)
                    this[x, y].Passengers.Add(p);
            }
        }

        public List<DrawableTile> Tiles
        {
            get { return tiles; }
        }
    }
}

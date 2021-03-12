using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.GameManagment.GameObjects;
using EG = Engine.GameManagment.Assets;
using Microsoft.Xna.Framework;

namespace VisualCoronaSimulatie.Simulation_Objects
{
    class Tile<T> : DrawGameObject
    {
        List<T> passengers;
        Point gridpos;

        public Tile(EG.IDrawable drawable, Point gridpos)
            : base(drawable)
        {
            this.gridpos = gridpos;
            passengers = new List<T>();
        }

        public List<List<T>> getNeighbours()
        {
            List<List<T>> neighbours = new List<List<T>>();
            neighbours.Add(passengers);

            for (int x = gridpos.X-1; x <= gridpos.X+1; x++)
            {
                for(int y = gridpos.Y-1; y <= gridpos.Y+1; y++)
                {
                    if (x == gridpos.X && y == gridpos.Y)
                        continue;
                    neighbours.Add((Parent as GameObjectGrid<Tile<T>>).Find(x, y).passengers);
                }
            }
            return neighbours;
        }

        public List<T> Passengers
        {
            get { return passengers; }
        }
    }
}

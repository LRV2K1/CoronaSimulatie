using CoronaSimulatie.SimulationObjects;
using Engine.GameManagment.GameObjects;
using Engine.GameManagment.Assets;
using Engine.GameManagment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VisualCoronaSimulatie.Simulation_Objects
{
    class DrawableTile : Tile
    {
        DrawGameObject visual;

        public DrawableTile(int x, int y, WorldGrid world) 
            : base(x, y, world)
        {
            visual = new DrawGameObject(new SpriteSheet(GameEnvironment.DrawingHelper.GetTexture()));
            visual.Drawable.SpritePart = new Rectangle(0, 0, world.TileSize, world.TileSize);
            visual.Drawable.Origin = new Vector2(0, world.TileSize);

            visual.Position2 = new Vector2(x * world.TileSize, Y * world.TileSize);
            //if ((x + y) % 2 == 0)
            //    visual.Drawable.Color = Color.Black;
            //else
            //    visual.Drawable.Color = Color.White;
        }

        public DrawGameObject Visual
        {
            get { return visual; }
        }
    }
}

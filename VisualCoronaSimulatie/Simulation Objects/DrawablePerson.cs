using CoronaSimulatie.SimulationObjects;
using Engine.GameManagment.GameObjects;
using Engine.GameManagment.Assets;
using Engine.GameManagment;
using Microsoft.Xna.Framework;
using System;

namespace VisualCoronaSimulatie.Simulation_Objects
{
    class DrawablePerson : Person
    {
        DrawGameObject visual;

        public DrawablePerson(float x, float y, Random random)
            : base(x,y, random)
        {
            visual = new DrawGameObject(new SpriteSheet(GameEnvironment.DrawingHelper.GetTexture()), 1);
            visual.Drawable.SpritePart = new Rectangle(0, 0, 5, 5);
            visual.Drawable.Origin = visual.Drawable.Center;

            //visual.Drawable.Color = new Color((int)((x / 10000f) * 255), (int)((y / 10000f) * 255), 0);

            visual.Position2 = new Vector2(x, y);
            visual.Drawable.Color = Color.LightGreen;
        }

        public override void Move()
        {
            base.Move();

            visual.Position2 = new Vector2(x, y);
        }

        public override Tile Tile
        {
            get { return tile; }
            set
            {
                tile = value;
                //if (tile != null)
                //{
                //    if ((tile.X + tile.Y) % 2 == 0)
                //        visual.Drawable.Color = Color.Red;
                //    else
                //        visual.Drawable.Color = Color.Blue;
                //}
            }
        }

        public DrawGameObject Visual
        {
            get { return visual; }
        }

        public override HeathStatus Status
        {
            get { return base.Status; }
            set
            {
                base.Status = value;
                switch (value)
                {
                    case HeathStatus.Healthy:
                        visual.Drawable.Color = Color.LightGreen;
                        break;
                    case HeathStatus.Ill:
                        visual.Drawable.Color = Color.Red;
                        break;
                    case HeathStatus.Recovered:
                        visual.Drawable.Color = Color.Gray;
                        break;
                }
            }
        }
    }
}
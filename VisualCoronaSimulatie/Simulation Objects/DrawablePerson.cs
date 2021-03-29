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
            visual.Drawable.SpritePart = new Rectangle(0, 0, 15, 15);
            visual.Drawable.Origin = visual.Drawable.Center;

            //visual.Drawable.Color = new Color((int)((x / 10000f) * 255), (int)((y / 10000f) * 255), 0);

            visual.Position2 = new Vector2(x, y);
            visual.Drawable.Color = GetHeathColor(HealthStatus);
        }

        public override void Move()
        {
            base.Move();

            visual.Position2 = new Vector2(x, y);
        }

        public DrawGameObject Visual
        {
            get { return visual; }
        }

        public override HealthStatus HealthStatus
        {
            get { return base.HealthStatus; }
            set
            {
                base.HealthStatus = value;
                visual.Drawable.Color = GetHeathColor(HealthStatus);
            }
        }


        private Color GetHeathColor(HealthStatus health)
        {
            switch (health)
            {
                case HealthStatus.Susceptible:
                    return new Color(65,122,0);
                case HealthStatus.Exposed:
                    return new Color(195,174,0);
                case HealthStatus.Infectious:
                    return new Color(255,56,0);
                case HealthStatus.Recovered:
                    return new Color(1, 65, 255);
            }

            return Color.White;
        }
    }
}
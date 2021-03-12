using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Engine.GameManagment;
using Engine.GameManagment.GameObjects;
using Engine.GameManagment.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VisualCoronaSimulatie.Simulation_Objects;
using CoronaSimulatie.SimulationObjects;

namespace VisualCoronaSimulatie
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SimulatieStart : GameEnvironment
    {
        static void Main()
        {
            SimulatieStart simulatie = new SimulatieStart();
            simulatie.Run();
        }

        public SimulatieStart()
        {
            Content.RootDirectory = "Content";
        }

        DrawableWorld world;
        List<Person> people;

        protected override void LoadContent()
        {
            base.LoadContent();

            IsMouseVisible = true;
            Screen.Background = Color.Blue;
            Screen.WindowSize = new Point(1600, 900);

            GameObjectList<GameObject> state = new GameObjectList<GameObject>();


            people = new List<Person>();
            people.Add(new DrawablePerson(10, 10));
            state.Add((people[0] as DrawablePerson).Visual);

            world = new DrawableWorld(10, 10, 100, 100, people);
            foreach(DrawableTile t in world.Tiles)
            {
                state.Add(t.Visual);
            }

            state.Position2 = new Vector2(-500, -500);

            GameStateManager.AddGameState("start", state);
            GameStateManager.SwitchTo("start");

        }

        protected override void HandleInput()
        {
            base.HandleInput();

            if (inputHelper.KeyPressed(Keys.Space))
            {
                foreach (Person p in people)
                {
                    p.Move();
                }
                foreach (Person p in people)
                {
                    p.Sickness();
                }
            }
        }
    }
}

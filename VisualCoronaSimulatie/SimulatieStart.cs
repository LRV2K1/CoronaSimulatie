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

        TextGameObject frames;
        int frame;
        float time;

        protected override void LoadContent()
        {
            base.LoadContent();

            IsMouseVisible = true;
            Screen.Background = Color.Blue;
            Screen.WindowSize = new Point(1600, 900);

            GameObjectList<GameObject> state = new GameObjectList<GameObject>();
            GameObjectList<GameObject> simulatie = new GameObjectList<GameObject>();


            people = new List<Person>();
            for (int i = 0; i < 10000; i++)
            {
                DrawablePerson p = new DrawablePerson(random.Next(0, 9999), random.Next(0, 9999), random);
                people.Add(p);
                simulatie.Add((p as DrawablePerson).Visual);
            }
            world = new DrawableWorld(100, 100, 100, 100, people);
            foreach(DrawableTile t in world.Tiles)
            {
                simulatie.Add(t.Visual);
            }

            simulatie.Position2 = new Vector2(-500, -500);
            simulatie.Scale = 0.1f;


            frames = new TextGameObject("Hud", 2);
            frames.TextFont.Text = "start";
            frames.Position2 = new Vector2(-960, 500);

            state.Add(frames);
            state.Add(simulatie);
            GameStateManager.AddGameState("start", state);
            GameStateManager.SwitchTo("start");
        }

        //protected override void HandleInput()
        //{
        //    base.HandleInput();

        //    if (inputHelper.KeyPressed(Keys.Space))
        //    {
        //        foreach (Person p in people)
        //        {
        //            p.Move();
        //        }
        //        foreach (Person p in people)
        //        {
        //            p.Sickness();
        //        }
        //    }
        //}

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (Person p in people)
            {
                p.Move();
            }
            foreach (Person p in people)
            {
                p.Sickness();
            }

        }

        protected override void Draw(GameTime gameTime)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            frame++;
            if (time >= 1f)
            {
                time--;
                frames.TextFont.Text = frame.ToString();
                frame = 0;
            }
            base.Draw(gameTime);
        }
    }
}
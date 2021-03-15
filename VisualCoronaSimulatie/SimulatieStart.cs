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

        DataWriter dataWriter;

        DrawableWorld world;
        List<Person> people;

        TextGameObject frames;
        int frame;
        float time;

        TextGameObject healhy, ill, recovered;

        protected override void LoadContent()
        {
            base.LoadContent();

            IsMouseVisible = true;
            Screen.Background = Color.Black;
            Screen.WindowSize = new Point(1600, 900);

            dataWriter = new DataWriter();

            GameObjectList<GameObject> state = new GameObjectList<GameObject>();
            GameObjectList<GameObject> simulatie = new GameObjectList<GameObject>();

            people = new List<Person>();
            SaveData.Healthy = Globals.totalpopulation;
            for (int i = 0; i < Globals.totalpopulation; i++)
            {
                DrawablePerson p = new DrawablePerson(random.Next(0, Globals.worldsize), random.Next(0, Globals.worldsize), random);
                people.Add(p);
                simulatie.Add((p as DrawablePerson).Visual);
            }
            world = new DrawableWorld(Globals.gridsize, Globals.tilesize, people);
            foreach(DrawableTile t in world.Tiles)
            {
                simulatie.Add(t.Visual);
            }
            for (int i = 0; i < Globals.illpopulation; i++)
            {
                people[i].HealthStatus = HealthStatus.Ill;
            }

            dataWriter.Write(SaveData.Healthy, SaveData.Ill, SaveData.Recovered);

            simulatie.Position2 = new Vector2(-500, -500);
            simulatie.Scale = 1000f/ (float)Globals.worldsize;

            //simulation data
            frames = new TextGameObject("Hud", 2);
            frames.TextFont.Text = "start";
            frames.Position2 = new Vector2(-960, 500);

            healhy = new TextGameObject("Hud", 2);
            healhy.TextFont.Text = 0.ToString();
            healhy.Position2 = new Vector2(-960, 450);
            healhy.TextFont.Color = Color.LightGreen;
            ill = new TextGameObject("Hud", 2);
            ill.TextFont.Text = 0.ToString();
            ill.Position2 = new Vector2(-960, 400);
            ill.TextFont.Color = Color.Red;
            recovered = new TextGameObject("Hud", 2);
            recovered.TextFont.Text = 0.ToString();
            recovered.Position2 = new Vector2(-960, 350);
            recovered.TextFont.Color = Color.Gray;

            state.Add(frames);
            state.Add(healhy);
            state.Add(ill);
            state.Add(recovered);
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
            if (SaveData.Ill > 0)
            {
                for (int i = 0; i < 1; i++)
                {
                    foreach (Person p in people)
                    {
                        p.Move();
                    }
                    foreach (Person p in people)
                    {
                        p.Sickness();
                    }

                    dataWriter.Write(SaveData.Healthy, SaveData.Ill, SaveData.Recovered);
                }

                healhy.TextFont.Text = SaveData.Healthy.ToString();
                ill.TextFont.Text = SaveData.Ill.ToString();
                recovered.TextFont.Text = SaveData.Recovered.ToString();
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
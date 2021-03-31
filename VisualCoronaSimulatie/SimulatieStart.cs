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

        //DataWriter dataWriter;

        DrawableWorld world;
        List<Person> people;

        TextGameObject frames;
        int frame;
        float time;

        TextGameObject susceptible, exposed, infectious, recovered;
        int step;
        int hours;
        TextGameObject steps;

        protected override void LoadContent()
        {
            base.LoadContent();
            IsMouseVisible = true;
            Screen.Background = Color.Black;
            Screen.WindowSize = new Point(1600, 900);

            //dataWriter = new DataWriter();

            GameObjectList<GameObject> state = new GameObjectList<GameObject>();
            GameObjectList<GameObject> simulatie = new GameObjectList<GameObject>();

            people = new List<Person>();
            SaveData.Susceptible = Globals.totalpopulation;
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
                people[i].HealthStatus = HealthStatus.Infectious;
            }

            ShuffleClass.Shuffle(people);

            for (int i = 0; i < Globals.apppopulation; i++)
            {
                people[i].App = new App();
            }

            //dataWriter.Write(SaveData.Healthy, SaveData.Ill, SaveData.Recovered);

            simulatie.Position2 = new Vector2(-500, -500);
            simulatie.Scale = 1000f/ (float)Globals.worldsize;

            //simulation data
            frames = new TextGameObject("Hud", 2);
            frames.TextFont.Text = "start";
            frames.Position2 = new Vector2(-960, 500);

            susceptible = new TextGameObject("Hud", 2);
            susceptible.TextFont.Text = 0.ToString();
            susceptible.Position2 = new Vector2(-960, 450);
            susceptible.TextFont.Color = new Color(65, 122, 0);
            exposed = new TextGameObject("Hud", 2);
            exposed.TextFont.Text = 0.ToString();
            exposed.Position2 = new Vector2(-960, 400);
            exposed.TextFont.Color = new Color(195, 174, 0);
            infectious = new TextGameObject("Hud", 2);
            infectious.TextFont.Text = 0.ToString();
            infectious.Position2 = new Vector2(-960, 350);
            infectious.TextFont.Color = new Color(255, 56, 0);
            recovered = new TextGameObject("Hud", 2);
            recovered.TextFont.Text = 0.ToString();
            recovered.Position2 = new Vector2(-960, 300);
            recovered.TextFont.Color = new Color(1, 65, 255);

            step = 0;
            hours = 0;
            steps = new TextGameObject("Hud", 2);
            steps.TextFont.Text = step.ToString();
            steps.Position2 = new Vector2(-960, 250);

            state.Add(frames);
            state.Add(susceptible);
            state.Add(exposed);
            state.Add(infectious);
            state.Add(recovered);
            state.Add(steps);
            state.Add(simulatie);
            GameStateManager.AddGameState("start", state);
            GameStateManager.SwitchTo("start");
        }


        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (SaveData.Infectious > 0)
            {
                for (int i = 0; i < 1; i++)
                {
                    step++;
                    foreach (Person p in people)
                    {
                        p.Move();
                    }
                    foreach (Person p in people)
                    {
                        p.Sickness();
                    }
                }

                if ((float)step * Globals.timestep > hours)
                {
                    hours++;
                    //dataWriter.Write();
                }
            }

        }

        protected override void Draw(GameTime gameTime)
        {
            susceptible.TextFont.Text = SaveData.Susceptible.ToString();
            exposed.TextFont.Text = SaveData.Exposed.ToString();
            infectious.TextFont.Text = SaveData.Infectious.ToString();
            recovered.TextFont.Text = SaveData.Recovered.ToString();

            steps.TextFont.Text = ((float)step * Globals.timestep).ToString();

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
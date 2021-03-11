using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Game1.GameManagment;
using Game1.GameManagment.GameObjects;
using Game1.GameManagment.IO;
using Game1.GameManagment.Assets;
using Game1.GameManagment.StartUp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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

        protected override void LoadContent()
        {
            base.LoadContent();


            SpriteSheet sprite = new SpriteSheet(drawingHelper.GetTexture());
            sprite.Color = Color.Red;
            sprite.SpritePart = new Rectangle(0, 0, 20, 20);
            DrawGameObject draw = new DrawGameObject(sprite);
            GameObjectList<GameObject> state = new GameObjectList<GameObject>();
            state.Add(draw);

            GameStateManager.AddGameState("start", state);
            GameStateManager.SwitchTo("start");

        }


    }
}

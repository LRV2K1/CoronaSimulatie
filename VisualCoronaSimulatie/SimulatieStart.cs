using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Engine.GameManagment;
using Engine.GameManagment.GameObjects;
using Engine.GameManagment.IO;
using Engine.GameManagment.Assets;
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

            IsMouseVisible = true;
            Screen.Background = Color.Blue;
            Screen.WindowSize = new Point(1600, 900);

            //SpriteSheet sprite = new SpriteSheet(drawingHelper.GetTexture());
            //sprite.Color = Color.Red;
            //sprite.SpritePart = new Rectangle(0, 0, 400, 400);
            //sprite.Origin = sprite.Center;
            //DrawGameObject draw = new DrawGameObject(sprite);
            GameObjectList<GameObject> state = new GameObjectList<GameObject>();

            SpriteSheet blackTile = new SpriteSheet(drawingHelper.GetTexture());
            blackTile.Color = Color.Black;
            blackTile.SpritePart = new Rectangle(0, 0, 50, 50);
            blackTile.Origin = blackTile.Center;
            SpriteSheet whiteTile = new SpriteSheet(drawingHelper.GetTexture());
            whiteTile.Color = Color.White;
            whiteTile.SpritePart = new Rectangle(0, 0, 50, 50);
            whiteTile.Origin = whiteTile.Center;

            GameObjectGrid<GameObject> grid = new GameObjectGrid<GameObject>(16, 16);
            grid.CellSize = new Point(50, 50);
            grid.Position2 = new Vector2(-(grid.CellWidth * (grid.Columns-1)) / 2, -(grid.CellHeight * (grid.Rows-1)) / 2);
            for (int x = 0; x < grid.Columns; x++)
            {
                for (int y = 0; y < grid.Rows; y++)
                {
                    if ((x + y) % 2 == 0)
                        grid.Add(new DrawGameObject(whiteTile), x, y);
                    else
                        grid.Add(new DrawGameObject(blackTile), x, y);
                }
            }

            state.Add(grid);
            //state.Add(draw);

            GameStateManager.AddGameState("start", state);
            GameStateManager.SwitchTo("start");

        }


    }
}

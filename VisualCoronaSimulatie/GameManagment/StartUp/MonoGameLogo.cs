using Game1.GameManagment.Assets;
using Game1.GameManagment.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game1.GameManagment.StartUp
{
    public class MonoGameLogo : GameObjectList<SpriteGameObject>
    {
        string prevstate;
        Color prevcolor;
        float timer;
        int cicle;

        public MonoGameLogo()
            : base()
        {
            Texture2D texture = GameEnvironment.DrawingHelper.GetTexture();

            //make objects
            //logos
            SpriteGameObject monogamelogo = new SpriteGameObject("Credits/MonoGameLogo", 0, "mgl");
            monogamelogo.Position2 = new Vector2(0, 100);
            SpriteGameObject monogameextendedlogo = new SpriteGameObject("Credits/MonoGameExtendedLogo", 0, "mgel");
            monogameextendedlogo.Position2 = monogamelogo.Position2 + new Vector2(0, monogamelogo.Sprite.Size.Y);
            //text
            SpriteGameObject extended = new SpriteGameObject("Credits/Extended", 2, "e");
            extended.Position2 = new Vector2(0, -180);
            SpriteGameObject monogame = new SpriteGameObject("Credits/MonoGame", 4, "mg");
            monogame.Position2 = new Vector2(0, -180);
            SpriteGameObject poweredby = new SpriteGameObject("Credits/PoweredBy", 5);
            poweredby.Sprite.Origin = new Vector2(0, 0);
            poweredby.Position2 = new Vector2(-GameEnvironment.Screen.ScreenSize.X, GameEnvironment.Screen.ScreenSize.Y) / 2f;
            //black
            SpriteGameObject black1 = new SpriteGameObject(new SpriteSheet(texture), 1);
            black1.Position2 = monogameextendedlogo.Position2;
            black1.Scale = Math.Max(monogamelogo.Sprite.Size.Y, monogamelogo.Sprite.Size.X);
            black1.Sprite.Color = Color.Black;
            SpriteGameObject black2 = new SpriteGameObject(new SpriteSheet(texture), 1, "b");
            black2.Position2 = monogamelogo.Position2 - new Vector2(0, monogamelogo.Sprite.Size.Y);
            black2.Scale = Math.Max(monogamelogo.Sprite.Size.Y, monogamelogo.Sprite.Size.X);
            black2.Sprite.Color = Color.Black;
            SpriteGameObject black3 = new SpriteGameObject(new SpriteSheet(texture), 3);
            black3.Position2 = extended.Position2;
            black3.Sprite.SpritePart = new Rectangle((extended.Sprite.Size / 2).ToPoint(), extended.Sprite.Size.ToPoint());
            black3.Sprite.Origin = extended.Sprite.Size / 2;
            black3.Sprite.Color = Color.Black;

            //add objects
            Add(monogamelogo);
            Add(monogameextendedlogo);
            Add(black1);
            Add(black2);
            Add(extended);
            Add(black3);
            Add(monogame);
            Add(poweredby);

            timer = 0f;
            cicle = 0;

            //states
            prevcolor = GameEnvironment.Screen.Background;
            GameEnvironment.Screen.Background = Color.Black;
            prevstate = GameEnvironment.GameStateManager.CurrentID;
            GameEnvironment.GameStateManager.AddGameState("Logo", this);
            GameEnvironment.GameStateManager.SwitchTo("Logo");
        }

        public override void Load()
        {
            Find("mgl").Sprite.Color = Color.Black;
            Find("mgel").Sprite.Color = Color.Black;
            Find("e").Sprite.Color = Color.Black;
            Find("mg").Sprite.Color = Color.Black;

            timer = 0f;
            cicle = 0;
            base.Load();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            switch (cicle)
            {
                case 0:
                    if (timer >= 2)
                    {
                        cicle++;
                        timer = 0;
                    }
                    break;
                case 1:
                    Visible(gameTime);
                    break;
                case 2:
                    if (timer >= 1)
                    {
                        cicle++;
                        timer = 0;
                    }
                    break;
                case 3:
                    Move(gameTime);
                    break;
                case 4:
                    if (timer >= 2)
                    {
                        cicle++;
                        timer = 0;
                    }
                    break;
                case 5:
                    Finished();
                    break;
            }
        }

        private void Visible(GameTime gameTime)
        {
            float value = timer / 2.5f;
            Find("mgl").Sprite.Color = Color.White * (value * value);
            Find("mgel").Sprite.Color = Color.White * (value * value);
            Find("e").Sprite.Color = Color.White * (value * value);
            Find("mg").Sprite.Color = Color.White * (value * value);
            if (timer >= 2.5f)
            {
                cicle++;
                timer = 0;
            }
        }

        private void Move(GameTime gameTime)
        {
            if (Find("mgl").Position2.Y > Find("b").Position2.Y)
            {
                Find("mgl").Velocity2 = new Vector2(0, -300);
                Find("mgel").Velocity2 = new Vector2(0, -300);
            }
            else
            {
                Find("mgl").Velocity2 = Vector2.Zero;
                Find("mgl").Position2 = Find("b").Position2;
                Find("mgel").Velocity2 = Vector2.Zero;
                Find("mgel").Position2 = new Vector2(0, 100);
                if (Find("e").Position2.Y > Find("mg").Position2.Y - 100)
                {
                    Find("e").Velocity2 = new Vector2(0, -300);
                }
                else
                {
                    Find("e").Velocity2 = Vector2.Zero;
                    Find("e").Position2 = Find("mg").Position2 - new Vector2(0, 100);
                    cicle++;
                    timer = 0;
                }
            }
        }

        /// <summary>
        /// When the logo's are finished.
        /// </summary>
        public void Finished()
        {
            GameEnvironment.Screen.Background = prevcolor;
            GameEnvironment.GameStateManager.SwitchTo(prevstate);
            GameEnvironment.GameStateManager.RemoveGameState("Logo");
        }
    }
}

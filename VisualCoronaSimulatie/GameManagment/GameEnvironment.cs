using System;
using Engine.GameManagment.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.GameManagment.Assets;
using Engine.GameManagment.Command;

namespace Engine.GameManagment
{
    public class GameEnvironment : Game
    {
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;
        protected InputHelper inputHelper;

        protected static Random random;
        protected static AssetManager assetManager;
        protected static GameStateManager gameStateManager;
        protected static CommandManager commandManager;
        protected static DrawingHelper drawingHelper;
        protected static FileManager fileManager;

        static bool quitGame;
        static Screen screen;

        public GameEnvironment()
        {
            graphics = new GraphicsDeviceManager(this);
            inputHelper = new InputHelper();
            assetManager = new AssetManager(Content);
            gameStateManager = new GameStateManager();
            drawingHelper = new DrawingHelper();
            screen = new Screen(graphics);
            commandManager = new CommandManager();
            fileManager = new FileManager();

            random = new Random();

            Window.TextInput += inputHelper.TextInput;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            drawingHelper.Initialize(this.GraphicsDevice);
            AssetManager.Initialize(spriteBatch);
            screen.Initialize(this.GraphicsDevice);
            commandManager.Initialize();
        }

        protected override void UnloadContent()
        {
            assetManager.Unload();
            gameStateManager.UnLoad();
            gameStateManager.Clear();
            fileManager.UnLoad();
        }

        protected virtual void HandleInput()
        {
            inputHelper.Update();
            gameStateManager.HandleInput(inputHelper);
            commandManager.HandleInput(inputHelper);
        }

        protected override void Update(GameTime gameTime)
        {
            if (quitGame)
            {
                Exit();
                return;
            }
            HandleInput();
            gameStateManager.Update(gameTime);
            commandManager.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(screen.RenderTarget);
            GraphicsDevice.Clear(screen.Background);
            gameStateManager.Draw(gameTime, spriteBatch, screen.SpriteTransform);
            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, screen.SpriteScale);
            spriteBatch.Draw(screen.RenderTarget, Vector2.Zero, Color.White);
            spriteBatch.End();

            commandManager.Draw(gameTime, spriteBatch, Matrix.CreateScale(1, -1, 1));
        }

        /// <summary>
        /// AssetManager that loads all assets.
        /// </summary>
        public static AssetManager AssetManager
        {
            get { return assetManager; }
        }

        /// <summary>
        /// GamestateManager that stores all the gamestates.
        /// </summary>
        public static GameStateManager GameStateManager
        {
            get { return gameStateManager; }
        }

        /// <summary>
        /// Random number generator.
        /// </summary>
        public static Random Random
        {
            get { return random; }
        }

        /// <summary>
        /// The commandline.
        /// </summary>
        public static CommandManager CommandManager
        {
            get { return commandManager; }
        }

        /// <summary>
        /// The drawinghelper.
        /// </summary>
        public static DrawingHelper DrawingHelper
        {
            get { return drawingHelper; }
        }

        /// <summary>
        /// The screen.
        /// </summary>
        public static Screen Screen
        {
            get { return screen; }
        }

        /// <summary>
        /// The filemanager.
        /// </summary>
        public static FileManager FileManager
        {
            get { return fileManager; }
        }

        /// <summary>
        /// Variable to quit the game.
        /// </summary>
        public static bool QuitGame
        {
            set { quitGame = value; }
        }
    }
}
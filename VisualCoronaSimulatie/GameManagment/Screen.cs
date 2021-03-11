using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game1.GameManagment
{
    public class Screen
    {
        protected Point screenSize;         //Internal screensize
        protected Point windowSize;         //External windowsize
        protected Matrix spriteScale;
        protected Matrix spriteTransform;
        protected Vector2 scale, offset;
        protected bool fullscreen;
        protected float aspectRatio;
        protected Rectangle screenRectangle;
        protected Color background;

        protected GraphicsDeviceManager graphics;
        protected GraphicsDevice graphicsDevice;
        protected RenderTarget2D renderTarget;

        public Screen(GraphicsDeviceManager graphics)
        {
            background = Color.Black;
            screenSize = new Point(1920, 1080);
            windowSize = new Point(960, 540);
            spriteScale = Matrix.CreateScale(new Vector3(1));
            aspectRatio = screenSize.X / screenSize.Y;
            fullscreen = false;

            this.graphics = graphics;
        }

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            renderTarget = new RenderTarget2D(this.graphicsDevice, screenSize.X, screenSize.Y);
            ApplyResolutionSettings();
        }

        protected virtual void ApplyResolutionSettings()
        {
            //check fullscreen
            if (!fullscreen)
            {
                graphics.PreferredBackBufferWidth = windowSize.X;
                graphics.PreferredBackBufferHeight = windowSize.Y;
                graphics.IsFullScreen = false;
                graphics.ApplyChanges();
            }
            else
            {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                graphics.IsFullScreen = true;
                graphics.ApplyChanges();
            }

            //make screen aspect
            float targetAspectRatio = (float)screenSize.X / (float)screenSize.Y;
            int width = graphics.PreferredBackBufferWidth;
            int height = (int)(width / targetAspectRatio);
            if (height > graphics.PreferredBackBufferHeight)
            {
                height = graphics.PreferredBackBufferHeight;
                width = (int)(height * targetAspectRatio);
            }

            //set viewport
            Viewport viewport = new Viewport();
            viewport.X = (graphics.PreferredBackBufferWidth / 2) - (width / 2);
            viewport.Y = (graphics.PreferredBackBufferHeight / 2) - (height / 2);
            viewport.Width = width;
            viewport.Height = height;
            graphicsDevice.Viewport = viewport;

            scale = new Vector2((float)graphicsDevice.Viewport.Width / screenSize.X, (float)graphicsDevice.Viewport.Height / screenSize.Y);     //create scale
            offset = new Vector2(viewport.X, viewport.Y);                                                                                       //create offset
            spriteScale = Matrix.CreateScale(scale.X, scale.Y, 1) * Matrix.CreateTranslation(new Vector3(offset, 0));                           //create scale/offset matrix
            aspectRatio = (float)windowSize.X / (float)windowSize.Y;

            spriteTransform = Matrix.CreateScale(1,-1,1) * Matrix.CreateTranslation(new Vector3(screenSize.X/2f, screenSize.Y/2f, 0));                                       //sprite transformation matrix

            renderTarget = new RenderTarget2D(this.graphicsDevice, screenSize.X, screenSize.Y);                                                 //create rendertarget
            //GameEnvironment.PostProcessingManager.ResizeScreen();


            screenRectangle = new Rectangle(-screenSize.X / 2, -screenSize.Y / 2, screenSize.X, screenSize.Y);
        }

        /// <summary>
        /// Checks if a rectangle is on the screen.
        /// </summary>
        /// <param name="r">The rectangle to be checked.</param>
        /// <returns></returns>
        public bool OnScreen(Rectangle r)
        {
            return (screenRectangle.Intersects(r));
        }

        /// <summary>
        /// The internal size of the screen.
        /// </summary>
        public Point ScreenSize
        {
            get { return screenSize; }
            set { 
                screenSize = value;
                ApplyResolutionSettings();
            }
        }

        /// <summary>
        /// The size of the window on the computer screen.
        /// </summary>
        public Point WindowSize
        {
            get { return windowSize; }
            set 
            { 
                windowSize = value;
                ApplyResolutionSettings();
            }
        }

        /// <summary>
        /// Dictates if the screen is fullscreen.
        /// </summary>
        public bool FullScreen
        {
            get { return fullscreen; }
            set { 
                fullscreen = value;
                ApplyResolutionSettings();
            }
        }

        /// <summary>
        /// The aspectratio of the screen.
        /// </summary>
        public float AspectRatio
        {
            get { return aspectRatio; }
        }

        /// <summary>
        /// The scale the sprites need to be drawn in, in order to fit the window.
        /// </summary>
        public Matrix SpriteScale
        {
            get { return spriteScale; }
        }

        /// <summary>
        /// The transformation of the sprites to make (0,0) the center of the screen and the y up.
        /// </summary>
        public Matrix SpriteTransform
        {
            get { return spriteTransform; }
        }

        /// <summary>
        /// The render target.
        /// </summary>
        public RenderTarget2D RenderTarget
        {
            get { return renderTarget; }
        }

        /// <summary>
        /// The screensize in a rectangle, used to check if stuf is on the screen.
        /// </summary>
        public Rectangle ScreenRectangle
        {
            get { return screenRectangle; }
        }

        /// <summary>
        /// The background color.
        /// </summary>
        public Color Background
        {
            get { return background; }
            set { background = value; }
        }
    }
}
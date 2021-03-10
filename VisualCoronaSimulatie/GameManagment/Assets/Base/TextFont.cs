using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.GameManagment.Assets
{
    public class TextFont : Drawable
    {
        protected SpriteFont spriteFont;
        protected string assetname;
        protected string text;

        public TextFont(string assetname)
        {
            this.assetname = assetname;
            text = "";

            //load spriteFont.
            try { spriteFont = GameEnvironment.AssetManager.Content.Load<SpriteFont>(assetname); }
            catch { spriteFont = null; }
        }

        public TextFont(SpriteFont spriteFont)
        {
            assetname = "";
            this.spriteFont = spriteFont;

            text = "";

            spriteEffect = SpriteEffects.None;
        }

        /// <summary>
        /// Draw the string of text.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw the string.</param>
        /// <param name="transform">The transform of the sprite.</param>
        public override void Draw(SpriteBatch spriteBatch, Matrix transform, GameTime gameTime)
        {
            if (spriteFont != null || text == "")
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, shader, Matrix.CreateScale(1, -1, 1) * transform);
                spriteBatch.DrawString(spriteFont, text, Vector2.Zero, Color, 0, Origin, 1, SpriteEffects, 0);
                spriteBatch.End();
            }
        }

        /// <summary>
        /// Measures the size of the string.
        /// </summary>
        /// <param name="text">The string to be measured.</param>
        /// <returns></returns>
        public Vector2 MeasureString(string text)
        {
            if (spriteFont != null)
                return spriteFont.MeasureString(text);
            return Vector2.Zero;
        }

        /// <summary>
        /// The original spritefont.
        /// </summary>
        public SpriteFont SpriteFont
        {
            get { return spriteFont; }
        }

        /// <summary>
        /// The original assetname of the spritefont.
        /// </summary>
        public string AssetName
        {
            get { return assetname; }
        }

        /// <summary>
        /// The size of the string.
        /// </summary>
        public override Vector2 Size
        {
            get
            {
                if (spriteFont != null)
                    return spriteFont.MeasureString(text);
                return Vector2.Zero;
            }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Game1.GameManagment.Assets
{
    public class SpriteSheet : Drawable
    {
        protected Texture2D sprite;
        protected string assetname;

        public SpriteSheet(Texture2D sprite)
        {
            assetname = "";
            this.sprite = sprite;

            if (sprite == null)
                return;

            spritePart = new Rectangle(0, 0, (int)Size.X, (int)Size.Y);
            origin = Center;
        }

        public SpriteSheet(string assetname)
        {
            this.assetname = assetname;

            sprite = GameEnvironment.AssetManager.GetSprite(assetname);

            if (sprite == null)
                return;

            spritePart = new Rectangle(0, 0, (int)Size.X, (int)Size.Y);
            origin = Center;
        }
       
        /// <summary>
        /// Draw the sprite.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw the sprite.</param>
        /// <param name="transform">The transform of the sprite.</param>
        public override void Draw(SpriteBatch spriteBatch, Matrix transform, GameTime gameTime)
        {
            if (sprite != null)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, shader, Matrix.CreateScale(1,-1,1) * transform);
                spriteBatch.Draw(sprite, Vector2.Zero, SpritePart, Color, 0, Origin, 1, spriteEffect, 0);
                spriteBatch.End();
            }
        }

        /// <summary>
        /// The original texture of the sprite sheet.
        /// </summary>
        public Texture2D Sprite
        {
            get { return sprite; }
        }

        /// <summary>
        /// The original assetname of the sprite sheet.
        /// </summary>
        public string AssetName
        {
            get { return assetname; }
        }

        public override Vector2 Size
        {
            get 
            { 
                if (sprite != null)
                    return new Vector2(sprite.Width, sprite.Height);
                return Vector2.Zero;
            }
        }
    }
}
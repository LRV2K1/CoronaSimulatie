using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Engine.GameManagment.Assets
{
    public abstract class Drawable : IDrawable
    {
        protected Color color;
        protected Vector2 origin;
        protected Rectangle spritePart;
        protected SpriteEffects spriteEffect;
        protected Effect shader;

        public Drawable()
        {
            color = Color.White;
            origin = Vector2.Zero;
            spritePart = Rectangle.Empty;
            spriteEffect = SpriteEffects.None;
            shader = null;
        }

        public abstract void Draw(SpriteBatch spriteBatch, Matrix transform, GameTime gameTime);

        /// <summary>
        /// The size of the sprite.
        /// </summary>
        public virtual Vector2 Size
        {
            get { return Vector2.Zero; }
        }

        /// <summary>
        /// The center of the spritepart in the sprite sheet.
        /// </summary>
        public virtual Vector2 Center
        {
            get { return new Vector2(((float)(spritePart.Width))/2f,((float)(spritePart.Height))/2f); }
        }

        /// <summary>
        /// The origin of the sprite.
        /// </summary>
        public virtual Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        /// <summary>
        /// The overlay color for the sprite sheet.
        /// </summary>
        public virtual Color Color
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// The part of the drawable that will be drawn.
        /// </summary>
        public virtual Rectangle SpritePart
        {
            get { return spritePart; }
            set { spritePart = value; }
        }

        /// <summary>
        /// The spriteEffect of the drawable.
        /// </summary>
        public virtual SpriteEffects SpriteEffects
        {
            get { return spriteEffect; }
            set { spriteEffect = value; }
        }

        /// <summary>
        /// Shader used for drawing.
        /// </summary>
        public virtual Effect Shader
        {
            get { return shader; }
            set { shader = value; }
        }

        /// <summary>
        /// The Base of the drawable, drawable is itself the lowest base, therefore null.
        /// </summary>
        public virtual IDrawable Base
        {
            get { return null; }
            set { }
        }
    }
}
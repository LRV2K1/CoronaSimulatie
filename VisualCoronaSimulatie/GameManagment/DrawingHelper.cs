using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Engine.GameManagment
{
    public class DrawingHelper
    {
        protected Texture2D pixel;
        
        public DrawingHelper()
        {
            pixel = null;
        }

        public void Initialize(GraphicsDevice graphics)
        {
            pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="r">The rectangle to be drawn.</param>
        /// <param name="spriteBatch">Spritebatch needed to draw the rectangle.</param>
        /// <param name="color">The color of the rectangle.</param>
        /// <param name="thickniss">The thickniss of the lines of the rectangle.</param>
        public void DrawRectangle(Rectangle r, SpriteBatch spriteBatch, Color color, int thickniss)
        {
            if (pixel == null)
                return;
            spriteBatch.Draw(pixel, new Rectangle(r.Left, r.Top, thickniss, r.Height), color); // Left
            spriteBatch.Draw(pixel, new Rectangle(r.Right - thickniss, r.Top, thickniss, r.Height), color); // Right
            spriteBatch.Draw(pixel, new Rectangle(r.Left, r.Top, r.Width, thickniss), color); // Top
            spriteBatch.Draw(pixel, new Rectangle(r.Left, r.Bottom - thickniss, r.Width, thickniss), color); // Bottom
        }

        /// <summary>
        /// Draws a filled rectangle.
        /// </summary>
        /// <param name="r">The rectangle to be drawn.</param>
        /// <param name="spriteBatch">Spritebatch needed to draw the rectangle.</param>
        /// <param name="color">The color of the rectangle.</param>
        public void DrawFillRechtangle(Rectangle r, SpriteBatch spriteBatch, Color color)
        {
            if (pixel == null)
                return;
            spriteBatch.Draw(pixel, r, color);
        }

        /// <summary>
        /// Draw a line between two points
        /// </summary>
        /// <param name="begin">The begin point.</param>
        /// <param name="end">The end point.</param>
        /// <param name="spriteBatch">Spritebatch needed to draw the line.</param>
        /// <param name="color">The color of the line.</param>
        /// <param name="thickniss">The thickniss of the line.</param>
        public void DrawLine(Vector2 begin, Vector2 end, SpriteBatch spriteBatch, Color color, int thickniss)
        {
            if (pixel == null)
                return;
            float dy = end.Y - begin.Y;
            float dx = end.X - begin.X;

            int length = (int)((new Vector2(dx, dy)).Length());
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, length, thickniss);
            Vector2 origin = new Vector2(0, (float)thickniss / 2);

            float angle = 0;
            if (dx != 0)
                angle = (float)Math.Atan(dy / dx);
            else if (dy > 0)
                angle = (float)Math.PI/2;
            else
                angle = -(float)Math.PI / 2;
            if (dx < 0)
                angle += (float)Math.PI;

            spriteBatch.Draw(pixel, begin, new Rectangle(0, 0, length, thickniss), color, angle, origin, Vector2.One, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draw a line between two points.
        /// </summary>
        /// <param name="begin">The begin point.</param>
        /// <param name="end">The end point.</param>
        /// <param name="spriteBatch">Spritebatch needed to draw the line.</param>
        /// <param name="color">The color of the line.</param>
        /// <param name="thickniss">The thickniss of the line.</param>
        public void DrawLine(Point begin, Point end, SpriteBatch spriteBatch, Color color, int thickniss)
        {
            DrawLine(begin.ToVector2(), end.ToVector2(), spriteBatch, color, thickniss);
        }

        /// <summary>
        /// Gives the pixel texture.
        /// </summary>
        /// <returns>The pixel texture.</returns>
        public Texture2D GetTexture()
        {
            return pixel;
        }
    }
}
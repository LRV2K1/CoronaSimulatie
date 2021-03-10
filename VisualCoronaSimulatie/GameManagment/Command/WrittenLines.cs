using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game1.GameManagment.GameObjects;
using Game1.GameManagment.Assets;

namespace Game1.GameManagment.Command
{
    public class WrittenLines : TextGameObject
    {
        LinkedList<TextLine> lines;
        int linecap = 20;
        float timer = 3f;
        bool activecommand;
        bool up;

        public WrittenLines(string assetname)
            : base(assetname)
        {
            lines = new LinkedList<TextLine>();
            activecommand = false;
            up = true;
        }

        public void WriteLine(string text)
        {
            lines.AddFirst(new TextLine(timer, text));
            //no more than 20 lines.
            while (lines.Count > linecap)
                lines.RemoveLast();
        }

        public override void Update(GameTime gameTime)
        {
            //update times
            foreach (TextLine t in lines)
                t.Time -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix transform)
        {
            Vector2 offset = Vector2.Zero;
            foreach (TextLine t in lines)
            {
                if (t.Time < 0 && !activecommand)
                    continue;
                TextFont.Text = t.Text;
                if (!up)
                    offset.Y += TextFont.Size.Y;
                else
                    offset.Y -= TextFont.Size.Y;

                base.Draw(gameTime, spriteBatch, Matrix.CreateTranslation(new Vector3(offset, 0)) * transform);
            }
        }

        public override void Reset()
        {
            lines.Clear();
        }

        public float Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        public int lineCap
        {
            get { return lineCap; }
            set { lineCap = value; }
        }

        public bool ActiveCommand
        {
            get { return activecommand; }
            set { 
                activecommand = value;
                if (activecommand)
                    TextFont.Color = Color.White;
                else
                    TextFont.Color = Color.Gray;
            }
        }

        public bool Up
        {
            get { return up; }
            set { up = value; }
        }

        public LinkedList<TextLine> Lines
        {
            get { return lines; }
        }
    }
}
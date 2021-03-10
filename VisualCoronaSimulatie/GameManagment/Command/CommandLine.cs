using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game1.GameManagment.GameObjects;
using Game1.GameManagment.Assets;

namespace Game1.GameManagment.Command
{
    public class CommandLine : TextGameObject
    {
        class StackList<T> : List<T>
        {
            public void Push(T t)
            {
                Add(t);
            }

            public T Pop()
            {
                T t = this[Count - 1];
                RemoveAt(Count - 1);
                return t;
            }

            public T Peek()
            {
                return this[Count - 1];
            }
        }

        StackList<char> vc;
        StackList<char> ac;

        const char cursor = '|';
        const string startcursor = " > ";

        char acursor;
        float timer;


        public CommandLine(string assetname) :
            base(assetname)
        {
            if (TextFont != null)
                TextFont.Color = Color.White;

            vc = new StackList<char>();
            ac = new StackList<char>();

            acursor = cursor;
            timer = 0.5f;
        }

        /// <summary>
        /// Makes a string from a stacklist.
        /// </summary>
        /// <param name="c">The stacklist.</param>
        /// <param name="back">The direction the stacklist needs to be read.</param>
        /// <returns></returns>
        private string StackToString(StackList<char> c, bool back = false)
        {
            string s = "";
            for (int i = 0; i < c.Count; i++)
            {
                if (!back)
                    s = c[i] + s;
                else
                    s += c[i];
            }
            return s;
        }

        /// <summary>
        /// Transforms the commandLine into a string.
        /// </summary>
        /// <returns></returns>
        public string ToCommand()
        {
            string command = "";
            while (vc.Count > 0)
                command = vc.Pop() + command;
            while (ac.Count > 0)
                command += ac.Pop();
            return command;
        }
        
        /// <summary>
        /// Transforms a string to a line in the commandLine.
        /// </summary>
        /// <param name="command">The string to be transformed.</param>
        public void ToLine(string command)
        {
            ac.Clear();
            vc.Clear();
            for (int i = 0; i < command.Length; i++)
                vc.Push(command[i]);
        }

        /// <summary>
        /// Move the cursor left or right.
        /// </summary>
        /// <param name="left">Direction of the cursor.</param>
        private void MoveCursor(bool left)
        {
            if (left && vc.Count > 0)
                ac.Push(vc.Pop());
            else if (!left && ac.Count > 0)
                vc.Push(ac.Pop());
        }

        public override string ToString()
        {
            return startcursor + StackToString(vc, true) + acursor + StackToString(ac);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.KeyPressed(Keys.Left) || inputHelper.KeyPressed(Keys.Right))    //Move cursor
            {
                MoveCursor(inputHelper.KeyPressed(Keys.Left));
                return;
            }

            //write command
            char s = inputHelper.GetText();
            if ((int)s >= 32 && (int)s < 127)                                               //Valid character
                vc.Push(s);
            else if ((int)s == 8 && vc.Count > 0)                                           //BackSpace
                vc.Pop();
            else if (inputHelper.KeyPressed(Keys.Delete) && ac.Count > 0)                   //Delete
                ac.Pop();
        }

        public override void Update(GameTime gameTime)
        {
            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer <= 0)
            {
                timer = 0.5f;
                if (acursor == cursor)
                    acursor = ' ';
                else
                    acursor = cursor;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix transform)
        {
            TextFont.Text = ToString();
            base.Draw(gameTime, spriteBatch, transform);
        }

        public override void Reset()
        {
            vc.Clear();
            ac.Clear();
        }

        public string StartCursor
        {
            get { return startcursor; }
        }
    }
}

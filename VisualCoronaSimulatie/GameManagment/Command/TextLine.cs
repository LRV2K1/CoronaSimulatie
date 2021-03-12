using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.GameManagment.Command
{
    public class TextLine
    {
        string text;
        float time;

        public TextLine(float time, string text)
        {
            this.time = time;
            this.text = text;
        }

        public float Time
        {
            get { return time; }
            set { time = value; }
        }

        public string Text
        {
            get { return text; }
        }
    }
}
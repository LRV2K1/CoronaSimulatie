using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.GameManagment.Assets
{
    public class MultiAnimate : Animation, IAnimation
    {
        protected IDrawable[] drawables;

        protected int current;

        /// <summary>
        /// Animate multiple drawables by switching them.
        /// </summary>
        /// <param name="drawables">The drawables to animate.</param>
        /// <param name="frameTime">The time it takes to switch a frame.</param>
        public MultiAnimate(IDrawable[] drawables, float frameTime = 0.05f)
            : base(drawables[0], frameTime)
        {
            this.drawables = drawables;
            current = 0;
        }

        protected override void SwitchFrame(int index)
        {
            current = index;
            drawable = drawables[current];
        }

        protected override int AnimationIdex
        {
            get { return current; }
        }

        public override int NumberSheetElements
        {
            get { return drawables.Length; }
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.GameManagment.Assets
{
    public abstract class Animation : SingleDecorator, IAnimation
    {
        protected float frameTime;
        protected AnimationEffects animationEffect;
        protected bool paused;

        protected bool forward;
        protected float timer;

        public Animation(IDrawable drawable, float frameTime = 0.05f)
            : base(drawable)
        {
            this.frameTime = frameTime;
            paused = false;
            forward = true;
            timer = 0;
            animationEffect = AnimationEffects.None;
        }

        public override void Draw(SpriteBatch spriteBatch, Matrix transform, GameTime gameTime)
        {
            Animate(gameTime);

            base.Draw(spriteBatch, transform, gameTime);
        }

        protected virtual void Animate(GameTime gameTime)
        {
            if (AnimationEnded || paused)
                return;

            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer <= 0)
            {
                timer = frameTime;

                //next frame
                if (forward)
                    SheetIndex++;
                else
                    SheetIndex--;
            }
        }

        protected abstract void SwitchFrame(int index);

        public virtual bool AnimationEnded
        {
            get
            {
                if (animationEffect == AnimationEffects.Looping || animationEffect == AnimationEffects.BackAndForth)
                    return false;
                if (animationEffect == AnimationEffects.None && SheetIndex == NumberSheetElements - 1)
                    return true;
                if (animationEffect == AnimationEffects.BackWard && SheetIndex == 0)
                    return true;
                return false;
            }
        }

        public virtual float FrameTime
        {
            get { return frameTime; }
            set { frameTime = value; }
        }
        public virtual AnimationEffects AnimationEffect
        {
            get { return animationEffect; }
            set
            {
                animationEffect = value;
                if (animationEffect == AnimationEffects.BackWard)
                    forward = false;
                else
                    forward = true;
            }
        }

        protected abstract int AnimationIdex { get; }

        public virtual int SheetIndex
        {
            get { return AnimationIdex; }
            set
            {
                //If one time
                if ((value < NumberSheetElements && value >= 0) || animationEffect == AnimationEffects.None || animationEffect == AnimationEffects.BackWard)
                {
                    SwitchFrame(value);
                    return;
                }

                //If looping
                if (animationEffect == AnimationEffects.Looping)
                {
                    SwitchFrame(0);                               //set to first element
                    return;
                }

                //If back and forth
                if (animationEffect == AnimationEffects.BackAndForth)
                {
                    forward = !forward;                                 //switch direction
                    if (value < 0)
                        SwitchFrame(1);                         //set to second element
                    else
                        SwitchFrame(NumberSheetElements - 2);      //set to second last element
                }
            }
        }

        public abstract int NumberSheetElements { get; }

        public virtual bool Paused
        {
            get { return paused; }
            set { paused = value; }
        }
    }
}
namespace Game1.GameManagment.Assets
{
    public class SingleAnimation<T> : Animation where T : IDrawable, IDrawableSheet
    {
        new protected T drawable;

        /// <summary>
        /// Animate a single drawable using it's sheetIndex.
        /// </summary>
        /// <param name="drawable">The drawable to be anmiated.</param>
        /// <param name="frameTime">The time it takes to switch a frame.</param>
        public SingleAnimation (T drawable, float frameTime = 0.05f)
            : base (drawable, frameTime)
        {
            this.drawable = drawable;
        }

        protected override void SwitchFrame(int index)
        {
            drawable.SheetIndex = index;
        }

        protected override int AnimationIdex
        {
            get { return drawable.SheetIndex; }
        }

        public override int NumberSheetElements
        {
            get { return drawable.NumberSheetElements; }
        }
    }
}
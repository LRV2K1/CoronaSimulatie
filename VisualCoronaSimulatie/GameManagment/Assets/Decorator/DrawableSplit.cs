using Microsoft.Xna.Framework;

namespace Game1.GameManagment.Assets
{
    public class DrawableSplit : SingleDecorator ,IDrawableSheet
    {
        int columns;
        int rows;
        int sheetIndex;

        Rectangle spritePart;

        public DrawableSplit(IDrawable drawable, int columns = 1, int rows = 1)
            : base(drawable)
        {
            spritePart = Rectangle.Empty;

            this.drawable = drawable;
            this.rows = rows;
            this.columns = columns;

            if (drawable != null)
            {
                spritePart = new Rectangle(0, 0, (int)Size.X, (int)Size.Y);
                drawable.Origin = Center;
            }

            SheetIndex = 0;
        }

        public virtual int SheetIndex 
        {
            get { return sheetIndex; }
            set
            {
                if (value < NumberSheetElements && value >= 0)
                {
                    sheetIndex = value;
                    int columnIndex = sheetIndex % columns;
                    int rowIndex = sheetIndex / columns % rows;
                    drawable.SpritePart = new Rectangle(columnIndex * (int)Size.X + spritePart.X, rowIndex * (int)Size.Y + spritePart.Y, spritePart.Width, spritePart.Height);
                }
            }
        }

        public override Vector2 Size
        {
            get
            {
                if (drawable != null)
                    return new Vector2(drawable.Size.X / columns, drawable.Size.Y / rows);
                return Vector2.Zero;
            }
        }

        public override Vector2 Center
        {
            get { return new Vector2(((float)(spritePart.Width)) / 2f, ((float)(spritePart.Height)) / 2f); }
        }

        public override Rectangle SpritePart
        {
            get { return spritePart; }
            set
            {
                spritePart = value;
                SheetIndex = sheetIndex;
            }
        }

        public virtual int NumberSheetElements
        {
            get { return columns * rows; }
        }
    }
}
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Engine.GameManagment.Assets;

namespace Engine.GameManagment.GameObjects
{
    public class TextGameObject : DrawGameObject
    {
        public TextGameObject(string assetName, int layer = 0, string id = "")
            : base(new TextFont(assetName), layer, id)
        {
        }

        public TextGameObject(TextFont textFont, int layer = 0, string id = "")
            : base(textFont, layer, id)
        {
        }

        /// <summary>
        /// The textfont used to draw the text.
        /// </summary>
        public TextFont TextFont
        {
            get { return drawable as TextFont; }
            set { drawable = value; }
        }
    }
}
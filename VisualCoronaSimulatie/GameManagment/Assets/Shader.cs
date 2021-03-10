using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.GameManagment.Assets
{
    public class Shader
    {
        int layer;
        Effect effect;

        public Shader(string effect, int layer = 0)
        {
            this.effect = GameEnvironment.AssetManager.Content.Load<Effect>(effect).Clone();
            this.layer = layer;
        }

        /// <summary>
        /// Set the value of a bool.
        /// </summary>
        /// <param name="id">Name of the parameter.</param>
        /// <param name="value">Value.</param>
        public void SetBool(string id, bool value)
        {
            effect.Parameters[id].SetValue(value);
        }

        /// <summary>
        /// Set the value of a matrix.
        /// </summary>
        /// <param name="id">Name of the parameter.</param>
        /// <param name="value">Value.</param>
        public void SetMatrix(string id, Matrix value)
        {
            effect.Parameters[id].SetValue(value);
        }

        /// <summary>
        /// Set the value of a float.
        /// </summary>
        /// <param name="id">Name of the parameter.</param>
        /// <param name="value">Value.</param>
        public void SetFloat(string id, float value)
        {
            effect.Parameters[id].SetValue(value);
        }

        /// <summary>
        /// Set the value of a int.
        /// </summary>
        /// <param name="id">Name of the parameter.</param>
        /// <param name="value">Value.</param>
        public void SetInt(string id, int value)
        {
            effect.Parameters[id].SetValue(value);
        }

        /// <summary>
        /// Set the value of a vector2.
        /// </summary>
        /// <param name="id">Name of the parameter.</param>
        /// <param name="value">Value.</param>
        public void SetVector2(string id, Vector2 value)
        {
            effect.Parameters[id].SetValue(value);
        }

        /// <summary>
        /// Set the value of a vector3.
        /// </summary>
        /// <param name="id">Name of the parameter.</param>
        /// <param name="value">Value.</param>
        public void SetVector3(string id, Vector3 value)
        {
            effect.Parameters[id].SetValue(value);
        }

        /// <summary>
        /// Set the value of a vector4.
        /// </summary>
        /// <param name="id">Name of the parameter.</param>
        /// <param name="value">Value.</param>
        public void SetVector4(string id, Vector4 value)
        {
            effect.Parameters[id].SetValue(value);
        }

        /// <summary>
        /// Set the value of a color.
        /// </summary>
        /// <param name="id">Name of the parameter.</param>
        /// <param name="value">Value.</param>
        public void SetColor(string id, Color value)
        {
            SetVector4(id, value.ToVector4());
        }

        /// <summary>
        /// Set the value of a texture.
        /// </summary>
        /// <param name="id">Name of the parameter.</param>
        /// <param name="value">Value.</param>
        public void SetTexture(string id, Texture2D value)
        {
            effect.Parameters[id].SetValue(value);
        }

        public int Layer
        {
            get { return layer; }
        }

        public Effect Effect
        {
            get { return effect; }
        }
    }
}

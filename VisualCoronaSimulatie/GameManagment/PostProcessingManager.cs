using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game1.GameManagment.Assets;
using System.Collections.Generic;
using System;

namespace Game1.GameManagment
{
    public class PostProcessingManager
    {

        Dictionary<string, Shader> shaders;
        List<Shader> shaderOrder;
        GraphicsDevice graphicsDevice;
        RenderTarget2D finalRender;

        RenderTarget2D[] renders;

        public PostProcessingManager()
        {
            shaders = new Dictionary<string, Shader>();
            shaderOrder = new List<Shader>();
            renders = new RenderTarget2D[2];
        }

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            renders[1] = new RenderTarget2D(graphicsDevice, GameEnvironment.Screen.ScreenSize.X, GameEnvironment.Screen.ScreenSize.Y);
        }

        public void ResizeScreen()
        {
            renders[1] = new RenderTarget2D(graphicsDevice, GameEnvironment.Screen.ScreenSize.X, GameEnvironment.Screen.ScreenSize.Y);
        }

        /// <summary>
        /// Add a shader to the list.
        /// </summary>
        /// <param name="id">The name of the shader.</param>
        /// <param name="shader">The shader to be added.</param>
        /// <param name="layer">The layer when the shader needs to be executed.</param>
        public void AddPostProcess(string id, Shader shader)
        {
            if (shaders.ContainsKey(id))
                return;

            shaders.Add(id, shader);

            for (int i = shaderOrder.Count; i > 0; i--)
            {
                if (shaderOrder[i-1].Layer <= shader.Layer)
                {
                    shaderOrder.Insert(i, shader);
                    return;
                }
            }
            shaderOrder.Insert(0, shader);
        }

        /// <summary>
        /// Removes a shader based on the id.
        /// </summary>
        /// <param name="id"></param>
        public void RemoveShader(string id)
        {
            if (!shaders.ContainsKey(id))
                return;
            shaderOrder.Remove(shaders[id]);
            shaders.Remove(id);
        }

        /// <summary>
        /// Returns the shader that belongs to the id.
        /// </summary>
        /// <param name="id">The id of the shader.</param>
        /// <returns></returns>
        public Shader GetShader(string id)
        {
            if (shaders.ContainsKey(id))
                return shaders[id];
            return null;
        }

        public void Draw(RenderTarget2D render, SpriteBatch spriteBatch)
        {
            renders[0] = render;

            int index = 1;
            for (int i = 0; i < shaderOrder.Count; i++)
            {
                graphicsDevice.SetRenderTarget(renders[(i + 1) % 2]);
                graphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, shaderOrder[i].Effect, Matrix.Identity);
                spriteBatch.Draw(renders[i % 2], Vector2.Zero, Color.White);
                spriteBatch.End();

                index = i;
            }
            graphicsDevice.SetRenderTarget(null);
            finalRender = renders[(index+1) % 2];
        }

        /// <summary>
        /// The final render of the scene.
        /// </summary>
        public RenderTarget2D FinalRender
        {
            get { return finalRender; }
        }
    }
}
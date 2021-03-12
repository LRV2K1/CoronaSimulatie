using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;

namespace Engine.GameManagment.Assets
{
    public struct RenderTexture2D
    {
        public SpriteSheet sprite;
        public Matrix transform;

        public RenderTexture2D(SpriteSheet sprite, Matrix transform)
        {
            this.sprite = sprite;
            this.transform = transform;
        }
    }

    public class AssetManager
    {
        protected ContentManager contentManager;
        protected Dictionary<string, Texture2D> textures;
        protected Dictionary<string, SoundEffect> soundEffects;
        protected SpriteBatch spriteBatch;

        public AssetManager(ContentManager content)
        {
            contentManager = content;

            //dictionaries to save textures and effects to avoid having to reload them.
            textures = new Dictionary<string, Texture2D>();
            soundEffects = new Dictionary<string, SoundEffect>();

            spriteBatch = null;
        }

        public void Initialize(SpriteBatch graphics)
        {
            this.spriteBatch = graphics;
        }

        /// <summary>
        /// Combine sprites into a single sprite. Not to be used during actual rendering!
        /// </summary>
        /// <param name="sprites">Array of sprites to be combined.</param>
        /// <returns></returns>
        public SpriteSheet CombineSprites(RenderTexture2D[] sprites, int width, int height)
        {
            if (spriteBatch == null)
                return null;

            RenderTarget2D render = new RenderTarget2D(spriteBatch.GraphicsDevice, width, height);

            spriteBatch.GraphicsDevice.SetRenderTarget(render);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].sprite.Draw(spriteBatch, sprites[i].transform, null);
            }
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(null);

            SpriteSheet comsprite = new SpriteSheet(render);
            return comsprite;
        }

        /// <summary>
        /// Combine sprites into a single sprite. Not to be used during actual rendering!
        /// </summary>
        /// <param name="sprites">List of sprites to be combined.</param>
        /// <returns></returns>
        public SpriteSheet CombineSprites(List<RenderTexture2D> sprites, int width, int height)
        {
            return CombineSprites(sprites.ToArray(), width, height);
        }

        /// <summary>
        /// Gives a texture based on the name.
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        /// <returns> .Xna.Texture2D, null when no texture is found.</returns>
        public Texture2D GetSprite(string assetName)
        {
            if (assetName == "")
            {
                return null;
            }
            else if (textures.ContainsKey(assetName))
            {
                return textures[assetName];
            }

            try
            {
                Texture2D tex = contentManager.Load<Texture2D>(assetName);
                textures.Add(assetName, tex);
                return tex;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Loads a texture based on it's name to be used later.
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        public void LoadTexture(string assetName)
        {
            if (assetName == "" || textures.ContainsKey(assetName))
            {
                return;
            }

            try
            {
                Texture2D tex = contentManager.Load<Texture2D>(assetName);
                textures.Add(assetName, tex);
            }
            catch { }
        }

        /// <summary>
        /// Unload a texture based on it's name.
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        public void UnloadTexture(string assetName)
        {
            if (assetName != "" && textures.ContainsKey(assetName))
            {
                textures.Remove(assetName);
            }
        }

        /// <summary>
        /// Plays a soundeffect based on the name.
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        public void PlaySound(string assetName)
        {
            if (assetName == "")
            {
                return;
            }
            else if (soundEffects.ContainsKey(assetName))
            {
                soundEffects[assetName].Play();
                return;
            }

            try
            {
                SoundEffect snd = contentManager.Load<SoundEffect>(assetName);
                soundEffects.Add(assetName, snd);
                snd.Play();
            }
            catch { }
        }

        /// <summary>
        /// Load a soundeffect based on the name.
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        public void LoadSound(string assetName)
        {
            if (assetName == "" || soundEffects.ContainsKey(assetName))
            {
                return;
            }

            try
            {
                SoundEffect snd = contentManager.Load<SoundEffect>(assetName);
                soundEffects.Add(assetName, snd);
            }
            catch { }
        }

        /// <summary>
        /// Unload soundeffect based on it's name.
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        public void UnloadSound(string assetName)
        {
            if (assetName != "" && soundEffects.ContainsKey(assetName))
            {
                soundEffects.Remove(assetName);
            }
        }

        /// <summary>
        /// Plays music.
        /// </summary>
        /// <param name="assetname">Name of the asset.</param>
        /// <param name="repeat">True if the song needs to repeat.</param>
        public void PlayMusic(string assetname, bool repeat = true)
        {
            MediaPlayer.IsRepeating = repeat;
            try
            {
                MediaPlayer.Play(contentManager.Load<Song>(assetname));
            }
            catch { }
        }

        /// <summary>
        /// Unload all assets.
        /// </summary>
        public void Unload()
        {
            contentManager.Unload();
            textures.Clear();
            soundEffects.Clear();
        }

        /// <summary>
        /// Clears all asset lists.
        /// </summary>
        public void Clear()
        {
            textures.Clear();
            soundEffects.Clear();
        }

        public ContentManager Content
        {
            get { return contentManager; }
        }
    }
}
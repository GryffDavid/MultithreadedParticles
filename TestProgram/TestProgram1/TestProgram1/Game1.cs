using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TestProgram1
{    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Emitter> EmitterList = new List<Emitter>();
        Texture2D ParticleTexture;

        DoubleBuffer DoubleBuffer;
        RenderManager RenderManager;
        UpdateManager UpdateManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
        }
        
        protected override void Initialize()
        {
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ParticleTexture = Content.Load<Texture2D>("diamond");

            DoubleBuffer = new DoubleBuffer();
            RenderManager = new RenderManager(DoubleBuffer, this);
            RenderManager.LoadContent();

            UpdateManager = new UpdateManager(DoubleBuffer, this);

            UpdateManager.StartOnNewThread();



            //EmitterList.Add(new Emitter(ParticleTexture, new Vector2(400, 400), new Vector2(0, 360)));
        }
        
        protected override void UnloadContent()
        {

        }
        
        protected override void Update(GameTime gameTime)
        {
            foreach (Emitter emitter in EmitterList)
            {
                emitter.Update(gameTime);
            }
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            foreach (Emitter emitter in EmitterList)
            {
                emitter.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

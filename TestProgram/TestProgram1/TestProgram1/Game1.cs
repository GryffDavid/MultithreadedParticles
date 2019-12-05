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
using System.Threading;
using System.Diagnostics;

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

        Stopwatch watch = new Stopwatch();

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

            RenderData renderData;
            GameData gameData;

            UpdateManager.CreateEmitter(new Vector2(400, 400), new Vector2(0, 360), out gameData, out renderData);
            RenderManager.RenderDataObjects.Add(renderData);
            UpdateManager.GameDataObjects.Add(gameData);
            
            UpdateManager.StartOnNewThread();
        }
        
        protected override void UnloadContent()
        {

        }
        
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            //spriteBatch.Begin();
            //foreach (Emitter emitter in EmitterList)
            //{
            //    emitter.Draw(spriteBatch);
            //}
            //spriteBatch.End();

            watch.Reset();
            watch.Start();

            DoubleBuffer.GlobalStartFrame(gameTime);

            RenderManager.FrameWatch.Reset();
            RenderManager.FrameWatch.Start();

            graphics.GraphicsDevice.Clear(Color.Black);

            RenderManager.DoFrame();

            base.Draw(gameTime);
        }
    }
}

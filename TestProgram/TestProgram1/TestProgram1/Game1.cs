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
    public enum ChangeMessageType
    {
        UpdateParticlePosition,
        CreateNewRenderData,
        DeleteRenderData,
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D ParticleTexture;
        SpriteFont Font;

        DoubleBuffer DoubleBuffer;
        RenderManager RenderManager;
        UpdateManager UpdateManager;

        static Random Random = new Random();

        Stopwatch watch = new Stopwatch();

        float CurrentTime, MaxTime;

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
            Font = Content.Load<SpriteFont>("SpriteFont");

            DoubleBuffer = new DoubleBuffer();

            RenderManager = new RenderManager(DoubleBuffer, this);
            RenderManager.LoadContent();

            UpdateManager = new UpdateManager(DoubleBuffer, this);            
            UpdateManager.StartOnNewThread();

            RenderData renderData;
            GameData gameData;

            UpdateManager.AddParticle(new Vector2(400, 400), new Vector2(0, -1), new Color(Random.Next(0, 255)/255f, Random.Next(0, 255)/255f, Random.Next(0, 255)/255f), out gameData, out renderData);
            RenderManager.RenderDataObjects.Add(renderData);
            UpdateManager.GameDataObjects.Add(gameData);

            //UpdateManager.AddParticle(new Vector2(400, 405), new Vector2(0, -1), out gameData, out renderData);
            //RenderManager.RenderDataObjects.Add(renderData);
            //UpdateManager.GameDataObjects.Add(gameData);
        }
        
        protected override void UnloadContent()
        {


        }
        
        protected override void Update(GameTime gameTime)
        {
            CurrentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (CurrentTime > 150)
            {
                RenderData renderData;
                GameData gameData;

                UpdateManager.AddParticle(new Vector2(400, 400), new Vector2(0, Random.Next(-5, 5)), new Color(Random.Next(0, 255) / 255f, Random.Next(0, 255) / 255f, Random.Next(0, 255) / 255f), out gameData, out renderData);
                RenderManager.RenderDataObjects.Add(renderData);
                UpdateManager.GameDataObjects.Add(gameData);

                CurrentTime = 0;
            }

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
            spriteBatch.Begin();
            spriteBatch.DrawString(Font, "ChangeMessageCount: " + DoubleBuffer.ChangeMessageCount.ToString(), new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(Font, "RenderDataObjects: " + RenderManager.RenderDataObjects.Count.ToString(), new Vector2(0, 24), Color.White);
            spriteBatch.DrawString(Font, "ParticleDataObjects: " + UpdateManager.GameDataObjects.Count.ToString(), new Vector2(0, 48), Color.White);
            spriteBatch.End();

            RenderManager.DoFrame();


            base.Draw(gameTime);
        }

        protected override void EndDraw()
        {
            base.EndDraw();
            RenderManager.FrameWatch.Stop();
            DoubleBuffer.GlobalSynchronize();
            watch.Stop();

        }


        protected override void OnExiting(object sender, EventArgs args)
        {
            if (UpdateManager.RunningThread != null)
                UpdateManager.RunningThread.Abort();
        }
    }
}

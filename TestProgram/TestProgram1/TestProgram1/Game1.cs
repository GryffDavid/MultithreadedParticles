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

        DoubleBuffer DoubleBuffer;
        RenderManager RenderManager;
        UpdateManager UpdateManager;
        static Random Random = new Random();

        SpriteFont Font;

        float CurrentTime;

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
            Font = Content.Load<SpriteFont>("Font");

            DoubleBuffer = new DoubleBuffer();

            RenderManager = new RenderManager(DoubleBuffer, this);
            RenderManager.LoadContent();

            UpdateManager = new UpdateManager(DoubleBuffer, this);            
            UpdateManager.StartOnNewThread();
            Debug.WriteLine(UpdateManager.RunningThread.ManagedThreadId.ToString());

            //RenderData renderData;
            //ParticleData gameData;

            //UpdateManager.AddParticle(new Vector2(400, 400), new Vector2(0, -1), out gameData, out renderData);
            //RenderManager.RenderDataObjects.Add(renderData);
            //UpdateManager.ParticleDataObjects.Add(gameData);

            //for (int i = 0; i < 1600; i++)
            //{
            //    RenderData renderData;
            //    ParticleData gameData;

            //    UpdateManager.AddParticle(new Vector2(400, 400), new Vector2(Random.Next(-4, 4), Random.Next(-4, 4)), out gameData, out renderData);
            //    RenderManager.RenderDataObjects.Add(renderData);
            //    UpdateManager.ParticleDataObjects.Add(gameData);
            //}            
        }
        
        protected override void UnloadContent()
        {

        }

        
        protected override void Update(GameTime gameTime)
        {
            CurrentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (CurrentTime > 100)
            {
                RenderData renderData;
                ParticleData gameData;

                UpdateManager.AddParticle(new Vector2(1280/2, 720/2), new Vector2(Random.Next(-2, 2), Random.Next(-2, 2)), RandomColor(), out gameData, out renderData);
                RenderManager.RenderDataObjects.Add(renderData);
                UpdateManager.ParticleDataObjects.Add(gameData);
            }

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            DoubleBuffer.GlobalStartFrame(gameTime);
            graphics.GraphicsDevice.Clear(Color.Black);
            RenderManager.DoFrame();

            spriteBatch.Begin();
            spriteBatch.DrawString(Font, "ChangeMessageCount: " + DoubleBuffer.ChangeMessageCount.ToString(), new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(Font, "RenderDataObjects: " + RenderManager.RenderDataObjects.Count.ToString(), new Vector2(0, 24), Color.White);
            spriteBatch.DrawString(Font, "ParticleDataObjects: " + UpdateManager.ParticleDataObjects.Count.ToString(), new Vector2(0, 48), Color.White);

            //spriteBatch.DrawString(Font, "RenderDataObjects: " + RenderDataObjects.Count.ToString(), new Vector2(0, 0), Color.White);
            //spriteBatch.DrawString(Font, "ChangeMessageCount: " + DoubleBuffer.ChangeMessageCount.ToString(), new Vector2(0, 24), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }


        protected override void EndDraw()
        {
            base.EndDraw();
            DoubleBuffer.GlobalSynchronize();
        }


        protected override void OnExiting(object sender, EventArgs args)
        {
            if (UpdateManager.RunningThread != null)
                UpdateManager.RunningThread.Abort();
        }

        public Color RandomColor()
        {
            return new Color(Random.Next(0, 255) / 255f, Random.Next(0, 255) / 255f, Random.Next(0, 255) / 255f);
        }
    }
}

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
        UpdateParticle,
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

        RenderData renderData;
        ParticleData gameData;

        static Random Random = new Random();

        SpriteFont Font;

        float CurrentTime;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
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
            //UpdateManager.RunningThread.IsBackground = false;
            //UpdateManager.RunningThread.Priority = ThreadPriority.AboveNormal;
            UpdateManager.RunningThread.Name = "UPDATE_MANAGER";
            Debug.WriteLine(UpdateManager.RunningThread.ManagedThreadId.ToString());          
        }
        
        protected override void UnloadContent()
        {

        }

        
        protected override void Update(GameTime gameTime)
        {
            CurrentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (CurrentTime > 1)
            {
                for (int i = 0; i < 5; i++)
                {
                    UpdateManager.AddParticle(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), 
                        new Vector2(0, 360), new Vector2(2, 3),
                        new Vector2(2, 2), Color.OrangeRed, Color.Yellow, 0.02f,
                        true, false, new Vector2(0, 360), new Vector2(-5, 5), 1f, new Vector2(2500, 4000),
                        out gameData, out renderData);

                    RenderManager.RenderDataObjects.Add(renderData);
                    UpdateManager.ParticleDataObjects.Add(gameData);
                }

                CurrentTime = 0;
            }

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            DoubleBuffer.GlobalStartFrame(gameTime);
            graphics.GraphicsDevice.Clear(Color.Black);
            

            spriteBatch.Begin();
            RenderManager.DoFrame(spriteBatch);

            spriteBatch.DrawString(Font, "ChangeMessageCount: " + DoubleBuffer.ChangeMessageCount.ToString(), new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(Font, "RenderDataObjects: " + RenderManager.RenderDataObjects.Count.ToString(), new Vector2(0, 24), Color.White);
            spriteBatch.DrawString(Font, "ParticleDataObjects: " + UpdateManager.ParticleDataObjects.Count.ToString(), new Vector2(0, 48), Color.White);

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

        public static float RandomFloat(float a, float b)
        {
            return a + (float)Random.NextDouble() * (b - a);
        }
    }
}

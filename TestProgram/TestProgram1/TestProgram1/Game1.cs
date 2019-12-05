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
        
        static Random Random = new Random();

        SpriteFont Font;

        List<Emitter> EmitterList = new List<Emitter>();
        

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
            UpdateManager = new UpdateManager(DoubleBuffer, this);    
            
            UpdateManager.StartOnNewThread();

            Emitter.UpdateManager = UpdateManager;
            Emitter.RenderManager = RenderManager;


            Emitter newEmitter = new Emitter(ParticleTexture, new Vector2(500, 500));
            Emitter newEmitter2 = new Emitter(ParticleTexture, new Vector2(1050, 500));

            EmitterList.Add(newEmitter);
            EmitterList.Add(newEmitter2);
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

        //public Texture2D RandomTexture()
        //{
        //    float num = (float)Random.NextDouble();

        //    if (num > 0.5f)
        //    {
        //        return ParticleTexture;
        //    }
        //    else
        //    {
        //        return ParticleTexture2;
        //    }
        //}
    }
}

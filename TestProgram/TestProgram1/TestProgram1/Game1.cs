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

            DoubleBuffer = new DoubleBuffer();
            RenderManager = new RenderManager(DoubleBuffer);
            UpdateManager = new UpdateManager(DoubleBuffer);    
            
            UpdateManager.StartOnNewThread();

            Emitter.UpdateManager = UpdateManager;
            Emitter.RenderManager = RenderManager;
            
            Emitter newEmitter = new Emitter(ParticleTexture, new Vector2(1280/2, 720/2), new Vector2(0, 0), new Vector2(3, 3), 
                new Vector2(6000, 6000), 1f, false, new Vector2(0, 0), new Vector2(0, 0), new Vector2(1, 1),
                Color.Green, Color.Fuchsia, 0f, -1f, 350, 1, false, new Vector2(1080, 1080), false);

            Emitter newEmitter2 = new Emitter(ParticleTexture, new Vector2(1050, 500), new Vector2(90, 180), new Vector2(3, 5), 
            new Vector2(500, 1500), 0.5f, false, new Vector2(0, 360), new Vector2(-3, 3), new Vector2(1, 3),
                Color.Red, Color.Orange, 0.2f, -1f, 15, 200, false, new Vector2(1080, 1080), true);

            //Emitter newEmitter3 = new Emitter(ParticleTexture, new Vector2(800, 80), new Vector2(0, 180), new Vector2(3, 5),
            //new Vector2(500, 1500), 0.5f, false, new Vector2(0, 360), new Vector2(-3, 3), new Vector2(1, 3),
            //    Color.Green, Color.Gold, 0.2f, -1f, 30, 200, false, new Vector2(1080, 1080), true);

            //Emitter newEmitter4 = new Emitter(ParticleTexture, new Vector2(800, 900), new Vector2(0, 180), new Vector2(3, 5),
            //new Vector2(500, 1500), 0.5f, false, new Vector2(0, 360), new Vector2(-3, 3), new Vector2(1, 3),
            //    Color.Blue, Color.LightSkyBlue, 0.2f, -1f, 30, 200, false, new Vector2(1080, 1080), true);

            EmitterList.Add(newEmitter);
            EmitterList.Add(newEmitter2);
            //EmitterList.Add(newEmitter3);
            //EmitterList.Add(newEmitter4);
        }

        protected override void UnloadContent()
        {

        }

        
        protected override void Update(GameTime gameTime)
        {
            //EmitterList[1].Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

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

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
        Texture2D ParticleTexture, ParticleTexture2;

        DoubleBuffer DoubleBuffer;
        RenderManager RenderManager;
        UpdateManager UpdateManager;
        
        static Random Random = new Random();
        
        List<Emitter> EmitterList = new List<Emitter>();

        KeyboardState CurrentKeyboardState, PreviousKeyboardState;

        SpriteFont Font;
        

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

            Font = Content.Load<SpriteFont>("Font");

            ParticleTexture = Content.Load<Texture2D>("diamond");
            ParticleTexture2 = Content.Load<Texture2D>("BigShell");

            DoubleBuffer = new DoubleBuffer();
            RenderManager = new RenderManager(DoubleBuffer);
            RenderManager.LoadContent(Content);

            UpdateManager = new UpdateManager(DoubleBuffer);    
            
            UpdateManager.StartOnNewThread();

            Emitter.UpdateManager = UpdateManager;
            Emitter.RenderManager = RenderManager;
            
            //Emitter newEmitter = new Emitter(ParticleTexture, new Vector2(1280/2, 720/2), new Vector2(0, 0), new Vector2(3, 3), 
            //    new Vector2(6000, 6000), 1f, false, new Vector2(0, 0), new Vector2(0, 0), new Vector2(1, 1),
            //    Color.Green, Color.Fuchsia, 0f, -1f, 350, 1, false, new Vector2(1080, 1080), false);

            //Emitter newEmitter2 = new Emitter(ParticleTexture, new Vector2(1050, 500), new Vector2(90, 180), new Vector2(3, 5), 
            //new Vector2(500, 1500), 0.5f, false, new Vector2(0, 360), new Vector2(-3, 3), new Vector2(1, 3),
            //    Color.Red, Color.Orange, 0.2f, 5f, 15, 30, false, new Vector2(1080, 1080), true);

            //Emitter newEmitter3 = new Emitter(ParticleTexture, new Vector2(800, 80), new Vector2(0, 180), new Vector2(3, 5),
            //new Vector2(500, 1500), 0.5f, false, new Vector2(0, 360), new Vector2(-3, 3), new Vector2(1, 3),
            //    Color.Green, Color.Gold, 0.2f, -1f, 30, 30, false, new Vector2(1080, 1080), true);

            

            //EmitterList.Add(newEmitter);
            //EmitterList.Add(newEmitter2);
            //EmitterList.Add(newEmitter3);
            
        }

        protected override void UnloadContent()
        {

        }

        
        protected override void Update(GameTime gameTime)
        {
            //EmitterList[1].Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            CurrentKeyboardState = Keyboard.GetState();

            if (CurrentKeyboardState.IsKeyUp(Keys.Space) &&
                PreviousKeyboardState.IsKeyDown(Keys.Space))
            {
                for (int i = 0; i < 5; i++)
                {
                    Emitter newEmitter4 = new Emitter(ParticleTexture, new Vector2(800, 200), new Vector2(0, 360), new Vector2(6, 10),
                    new Vector2(500, 1500), 0.99f, true, new Vector2(0, 360), new Vector2(-3, 3), new Vector2(0.25f, 0.5f),
                    new Color(Color.Orange.R, Color.Orange.G, Color.Orange.B, 100),
                    new Color(Color.OrangeRed.R, Color.OrangeRed.G, Color.OrangeRed.B, 20), 
                    0.05f, 2f, 16, 10, true, new Vector2(750, 800), false,
                    null, true, true, new Vector2(12, 15), new Vector2(0, 180), 0.6f, true, new Vector2(0, 0), true, true, 2000, null, null, false);

                    EmitterList.Add(newEmitter4);
                }
            }

            foreach (Emitter emitter in EmitterList)
            {
                emitter.Update(gameTime);
            }

            PreviousKeyboardState = CurrentKeyboardState;

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

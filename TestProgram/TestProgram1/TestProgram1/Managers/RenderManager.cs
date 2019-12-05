using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Threading;
using System.Diagnostics;

namespace TestProgram1
{
    class RenderManager
    {
        public List<RenderData> RenderDataObjects { get; set; }
        private DoubleBuffer DoubleBuffer;
        private GameTime GameTime;

        protected ChangeBuffer MessageBuffer;
        protected Game Game;

        SpriteBatch spriteBatch;
        Texture2D ParticleTexture;

        public RenderManager(DoubleBuffer doubleBuffer, Game game)
        {
            DoubleBuffer = doubleBuffer;
            Game = game;
            RenderDataObjects = new List<RenderData>();
        }

        public virtual void LoadContent()
        {
            ParticleTexture = Game.Content.Load<Texture2D>("diamond");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }
        
        public void Draw(GameTime gameTime)
        {
            

            foreach (ChangeMessage msg in MessageBuffer.Messages)
            {
                switch (msg.MessageType)
                {
                    case ChangeMessageType.UpdateParticlePosition:
                        {
                            RenderDataObjects[msg.ID].Position = msg.Position;
                        }
                        break;

                    case ChangeMessageType.CreateNewRenderData:
                        {
                            if (RenderDataObjects.Count == msg.ID)
                            {
                                RenderData newRenderData = new RenderData();
                                newRenderData.Position = msg.Position;
                                RenderDataObjects.Add(newRenderData);
                            }
                            else if (msg.ID < RenderDataObjects.Count)
                            {
                                RenderDataObjects[msg.ID].Position = msg.Position;                                
                            }
                        }
                        break;

                    case ChangeMessageType.DeleteRenderData:
                        {
                            RenderDataObjects.RemoveAt(msg.ID);
                        }
                        break;
                }
            }

            spriteBatch.Begin();
            foreach (RenderData renderData in RenderDataObjects)
            {
                spriteBatch.Draw(ParticleTexture, renderData.Position, renderData.Color);
            }
            spriteBatch.End();
        }

        public void DoFrame()
        {
            DoubleBuffer.StartRenderProcessing(out MessageBuffer, out GameTime);
            Draw(GameTime);
            DoubleBuffer.SubmitRender();
        }
    }
}

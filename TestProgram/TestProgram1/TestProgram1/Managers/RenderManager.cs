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
        
        public void DoFrame()
        {
            DoubleBuffer.StartRenderProcessing(out MessageBuffer, out GameTime);
            Draw(GameTime);
            DoubleBuffer.SubmitRender();
        }

        public void Draw(GameTime gameTime)
        {
            foreach (ChangeMessage msg in MessageBuffer.Messages)
            {
                switch (msg.MessageType)
                {
                    #region UpdateParticlePosition
                    case ChangeMessageType.UpdateParticle:
                        {
                            RenderDataObjects[msg.ID].Position = msg.Position;
                            RenderDataObjects[msg.ID].Rotation = msg.Rotation;
                            RenderDataObjects[msg.ID].Color = msg.Color;
                        }
                        break;
                    #endregion

                    #region DeleteRenderData
                    case ChangeMessageType.DeleteRenderData:
                        {
                            RenderDataObjects.RemoveAt(msg.ID);
                        }
                        break;
                        #endregion
                }
            }

            

            #region Draw particles
            spriteBatch.Begin();
            foreach (RenderData renderData in RenderDataObjects)
            {
                spriteBatch.Draw(ParticleTexture, new 
                    Rectangle((int)renderData.Position.X, (int)renderData.Position.Y, 
                                   ParticleTexture.Width, ParticleTexture.Height), 
                    null, renderData.Color, renderData.Rotation, 
                    new Vector2(ParticleTexture.Width/2, ParticleTexture.Height/2), SpriteEffects.None, 0);
            }
            spriteBatch.End();
            #endregion
        }
    }
}

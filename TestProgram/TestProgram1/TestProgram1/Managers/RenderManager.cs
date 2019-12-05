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
                
        public RenderManager(DoubleBuffer doubleBuffer)
        {
            DoubleBuffer = doubleBuffer;
            RenderDataObjects = new List<RenderData>();
        }
        
        public void DoFrame(SpriteBatch spriteBatch)
        {
            DoubleBuffer.StartRenderProcessing(out MessageBuffer, out GameTime);

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
                            RenderDataObjects[msg.ID].Scale = msg.Scale;
                            RenderDataObjects[msg.ID].Transparency = msg.Transparency;
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
            foreach (RenderData renderData in RenderDataObjects)
            {
                spriteBatch.Draw(renderData.Texture, new
                    Rectangle((int)renderData.Position.X, (int)renderData.Position.Y,
                              (int)(renderData.Texture.Width * renderData.Scale), (int)(renderData.Texture.Height * renderData.Scale)),
                    null, renderData.Color * renderData.Transparency, renderData.Rotation,
                    new Vector2(renderData.Texture.Width / 2, renderData.Texture.Height / 2), SpriteEffects.None, 0);
            }
            #endregion

            DoubleBuffer.SubmitRender();
        }
    }
}

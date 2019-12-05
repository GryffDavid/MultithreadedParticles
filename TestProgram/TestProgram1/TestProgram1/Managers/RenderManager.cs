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


        //Texture2D texture,
        //Vector2 position, 
        //float angle,
        //float speed,
        //float maxTime,
        //float startingTransparency,
        //bool fade,
        //float startingRotation,
        //float rotationChange,
        //float scale,
        //Color startColor, 
        //Color endColor,
        //float gravity,
        //bool canBounce,
        //float maxY,
        //bool shrink,
        //float drawDepth
        //bool stopBounce
        //bool hardBounce
        //bool shadow
        //bool rotateVelocity
        //Vector2 friction
        //SpriteEffects orientation = SpriteEffects.None,
        //float fadeDelay
        //bool sortDepth
        //bool grow

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
                spriteBatch.Draw(ParticleTexture, new
                    Rectangle((int)renderData.Position.X, (int)renderData.Position.Y,
                                   ParticleTexture.Width, ParticleTexture.Height),
                    null, renderData.Color, renderData.Rotation,
                    new Vector2(ParticleTexture.Width / 2, ParticleTexture.Height / 2), SpriteEffects.None, 0);
            }
            #endregion

            DoubleBuffer.SubmitRender();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestProgram1
{
    class Emitter
    {
        float CurrentTime, MaxTime;
        

        Texture2D Texture;

        Vector2 Position;

        RenderData renderData;        
        ParticleData gameData;

        public static UpdateManager UpdateManager;
        public static RenderManager RenderManager;

        int Burst;
        
       //Texture2D texture,
       //Vector2 position, 
       //Vector2 angleRange,
       //Vector2 speedRange, 
       //Vector2 timeRange,
       //float startingTransparency,
       //bool fade,
       //Vector2 startingRotationRange, 
       //Vector2 rotationIncrement,
       //Vector2 scaleRange,
       //Color startColor,
       //Color endColor, 
       //float gravity,
       //float activeSeconds,
       //float interval,
       //int burst,
       //bool canBounce,
       //Vector2 yrange, 
       //bool? shrink = null,
       //float? drawDepth = null,
       //bool? stopBounce = null,
       //bool? hardBounce = null,
       //Vector2? emitterSpeed = null, 
       //Vector2? emitterAngle = null,
       //float? emitterGravity = null,
       //bool? rotateVelocity = null,
       //Vector2? friction = null, 
       //bool? flipHor = null,
       //bool? flipVer = null,
       //float? fadeDelay = null,
       //bool? reduceDensity = null,
       //bool? sortParticles = null,
       //bool? grow = false,
       //Vector4? thirdColor = null




        public Emitter(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;

            MaxTime = 300f;
            CurrentTime = 0f;

            Burst = 300;
        }

        public void Update(GameTime gameTime)
        {
            CurrentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (CurrentTime > MaxTime)
            {
                for (int i = 0; i < Burst; i++)
                {
                    UpdateManager.AddParticle(
                            Texture, new Vector2(Position.X, Position.Y),
                            new Vector2(0, 360), new Vector2(2, 3),
                            new Vector2(2, 2), Color.OrangeRed, Color.Yellow, 0.02f,
                            true, false, new Vector2(0, 360), new Vector2(-5, 5), 1f, new Vector2(2500, 4000),
                            out gameData, out renderData);

                    RenderManager.RenderDataObjects.Add(renderData);
                    UpdateManager.ParticleDataObjects.Add(gameData);
                }

                CurrentTime = 0;
            }
        }
    }
}

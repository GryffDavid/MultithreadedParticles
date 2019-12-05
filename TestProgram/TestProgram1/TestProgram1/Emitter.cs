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

        UpdateManager UpdateManager;
        RenderManager RenderManager;

        int Burst;

        public Emitter(RenderManager rManager, UpdateManager uManager, Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;

            MaxTime = 300f;
            CurrentTime = 0f;

            Burst = 30;

            UpdateManager = uManager;
            RenderManager = rManager;
        }

        public void Update(GameTime gameTime)
        {
            CurrentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (CurrentTime > MaxTime)
            {
                for (int i = 0; i < Burst; i++)
                {
                    UpdateManager.AddParticle(Texture, new Vector2(Position.X, Position.Y),
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

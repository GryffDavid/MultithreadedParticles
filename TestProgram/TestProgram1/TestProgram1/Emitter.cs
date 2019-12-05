using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestProgram1
{
    public class Emitter
    {
        static Random Random = new Random();
        Texture2D Texture;
        public Vector2 Position, AngleRange;
        float CurrentTime, MaxTime, Angle;
        List<Particle> ParticleList = new List<Particle>();        

        public Emitter(Texture2D texture, Vector2 position, Vector2 angle)
        {
            Texture = texture;
            AngleRange = angle;
            Position = position;
            MaxTime = 100f;
        }

        public void Update(GameTime gameTime)
        {
            CurrentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (CurrentTime > MaxTime)
            {
                Angle = MathHelper.ToRadians(Random.Next((int)AngleRange.X, (int)AngleRange.Y));
                Vector2 vel = new Vector2((float)Math.Cos(Angle) * 4, (float)Math.Sin(Angle) * 4);
                Particle newParticle = new Particle(Texture, Position, vel);
                ParticleList.Add(newParticle);
            }

            foreach (Particle particle in ParticleList)
            {
                particle.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle particle in ParticleList)
            {
                particle.Draw(spriteBatch);
            }
        }
    }
}

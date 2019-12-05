using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestProgram1
{
    class Particle
    {
        Texture2D Texture;
        Vector2 Position, Velocity;

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
        }

        public void Update(GameTime gameTime)
        {
            Position += Velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}

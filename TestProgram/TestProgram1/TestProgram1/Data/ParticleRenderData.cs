using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestProgram1
{
    class ParticleRenderData
    {
        public Texture2D Texture;
        public Vector2 Position, Origin;
        public Color Color;
        public float Rotation, DrawDepth;
        public SpriteEffects Orientation;
    }
}

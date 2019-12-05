using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestProgram1
{
    class RenderData
    {
        //What is needed to draw the objects
        public Texture2D Texture;
        public Vector2 Position;
        public float Rotation, Scale, Transparency;
        public Color Color;
    }
}

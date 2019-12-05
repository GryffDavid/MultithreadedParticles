using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TestProgram1
{
    class ParticleData
    {
        //What is needed to perform updates
        public Vector2 Position, Angle, Velocity;
        public Color StartColor, EndColor;
        public float Rotation, CurrentTime, MaxTime, Gravity, Scale, StartingTransparency, RotationIncrement;
        public bool Shrink, Fade;
    }
}

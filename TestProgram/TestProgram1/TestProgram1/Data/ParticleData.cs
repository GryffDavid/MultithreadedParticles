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
        public Vector2 Position, Angle, Velocity, Friction;
        public Color StartColor, EndColor;
        public float
            Rotation, RotationIncrement,
            CurrentTime, MaxTime,
            FadeDelay, CurrentFadeDelay,
            CurrentScale, MaxScale,
            Gravity, StartingTransparency;
        public bool Active, Shrink, Grow, Fade, RotateVelocity;
    }
}

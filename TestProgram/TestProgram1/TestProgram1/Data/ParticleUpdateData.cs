using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestProgram1
{
    class ParticleData
    {
        public Texture2D Texture;
        public Vector2 CurrentPosition, Direction, Velocity,
                       YRange, Origin, StartingPosition, Friction;
        public Rectangle DestinationRectangle;
        public float Angle, Speed,
                     MaxY,
                     RotationIncrement, CurrentRotation,
                     Gravity,
                     CurrentTime, MaxTime,
                     CurrentScale, MaxScale,
                     CurrentTransparency, MaxTransparency,
                     CurrentFadeDelay, FadeDelay,
                     RadRotation, PercentageTime;        
        public bool Fade, BouncedOnGround, CanBounce, Shrink, StopBounce, 
                    HardBounce, Shadow, RotateVelocity, SortDepth, Grow;
        static Random Random = new Random();
        public SpriteEffects Orientation;
        public Color CurrentColor, EndColor, StartColor;        
    }
}

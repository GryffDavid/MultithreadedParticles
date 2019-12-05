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
        //float CurrentTime, MaxTime;
        //Texture2D Texture;
        //Vector2 Position, AngleRange, SpeedRange, TimeRange, StartingRotationRange, RotationIncrement, ScaleRange, YRange, EmitterSpeed, EmitterAngle, Friction;
        //float StartingTransparency, Gravity, ActiveSeconds, Interval, EmitterGravity, FadeDelay, DrawDepth;
        //int Burst;
        //bool Fade, CanBounce, Shrink, StopBounce, HardBounce, RotateVelocity, FlipHor, FlipVer, ReduceDensity, SortParticles, Grow;
        //Color StartColor, EndColor, ThirdColor;
        static Random Random = new Random();

        Texture2D Texture;
        public Vector2 PreviousPosition, AngleRange;
        public Vector2 Position, ScaleRange, TimeRange, RotationIncrementRange, SpeedRange, StartingRotationRange, EmitterDirection, EmitterVelocity, YRange, Friction;
        public float Transparency, Gravity, ActiveSeconds, Interval, EmitterSpeed,
                     EmitterAngle, EmitterGravity, FadeDelay, StartingInterval;
        public Color StartColor, EndColor, ThirdColor;
        public bool Fade, CanBounce, AddMore, Shrink, StopBounce, HardBounce, BouncedOnGround,
                    RotateVelocity, FlipHor, FlipVer, ReduceDensity, SortParticles;
        public bool Grow, Active;
        public string TextureName;
        public int Burst;
        public double IntervalTime, CurrentTime, MaxTime;
        public SpriteEffects Orientation = SpriteEffects.None;
        public object Tether;
        public float BounceY;
        public float DrawDepth;


        RenderData renderData;        
        ParticleData gameData;

        public static UpdateManager UpdateManager;
        public static RenderManager RenderManager;
        
        public Emitter(Texture2D texture, Vector2 position, Vector2 angleRange, Vector2 speedRange, Vector2 timeRange,
                       float startingTransparency, bool fade, Vector2 startingRotationRange, Vector2 rotationIncrement, Vector2 scaleRange,
                       Color startColor, Color endColor, float gravity, float activeSeconds, float interval, int burst, bool canBounce,
                       Vector2 yrange, bool? shrink = null, float? drawDepth = null, bool? stopBounce = null, bool? hardBounce = null,
                       Vector2? emitterSpeed = null, Vector2? emitterAngle = null, float? emitterGravity = null, bool? rotateVelocity = null,
                       Vector2? friction = null, bool? flipHor = null, bool? flipVer = null, float? fadeDelay = null, bool? reduceDensity = null,
                       bool? sortParticles = null, bool? grow = false, Vector4? thirdColor = null)
        {
            Active = true;
            Texture = texture;
            SpeedRange = speedRange;
            TimeRange = timeRange;
            Transparency = startingTransparency;
            Fade = fade;
            StartingRotationRange = startingRotationRange;
            RotationIncrementRange = rotationIncrement;
            ScaleRange = scaleRange;
            StartColor = startColor;
            EndColor = endColor;
            Position = position;
            AngleRange = angleRange;
            Gravity = gravity;
            ActiveSeconds = activeSeconds;
            Interval = interval;
            StartingInterval = interval;
            IntervalTime = Interval;
            Burst = burst;
            CanBounce = canBounce;

            if (grow != null)
                Grow = grow.Value;
            else
                Grow = false;

            if (thirdColor != null)
                ThirdColor = new Color(thirdColor.Value.X, thirdColor.Value.Y, thirdColor.Value.Z, thirdColor.Value.W);
            else
                ThirdColor = Color.Transparent;

            if (shrink == null)
                Shrink = false;
            else
                Shrink = shrink.Value;

            if (drawDepth == null)
                DrawDepth = 0;
            else
                DrawDepth = drawDepth.Value;

            if (stopBounce == null)
                StopBounce = false;
            else
                StopBounce = stopBounce.Value;

            if (hardBounce == null)
                HardBounce = false;
            else
                HardBounce = hardBounce.Value;

            if (friction != null)
                Friction = friction.Value;
            else
                Friction = new Vector2(0, 0);

            if (fadeDelay != null)
                FadeDelay = fadeDelay.Value;
            else
                FadeDelay = 0;

            if (emitterSpeed != null)
                EmitterSpeed = (float)DoubleRange(emitterSpeed.Value.X, emitterSpeed.Value.Y);
            else
                EmitterSpeed = 0;

            if (emitterAngle != null)
            {
                EmitterAngle = -MathHelper.ToRadians((float)DoubleRange(emitterAngle.Value.X, emitterAngle.Value.Y));
            }
            else
            {
                EmitterAngle = 0;
            }

            if (emitterGravity != null)
                EmitterGravity = emitterGravity.Value;
            else
                EmitterGravity = 0;

            if (EmitterSpeed != 0)
            {
                EmitterDirection.X = (float)Math.Cos(EmitterAngle);
                EmitterDirection.Y = (float)Math.Sin(EmitterAngle);
                EmitterVelocity = EmitterDirection * EmitterSpeed;
                AngleRange = new Vector2(
                                -(MathHelper.ToDegrees((float)Math.Atan2(-EmitterVelocity.Y, -EmitterVelocity.X))) - 20,
                                -(MathHelper.ToDegrees((float)Math.Atan2(-EmitterVelocity.Y, -EmitterVelocity.X))) + 20);
            }

            if (rotateVelocity != null)
                RotateVelocity = rotateVelocity.Value;
            else
                RotateVelocity = false;

            if (reduceDensity != null)
                ReduceDensity = reduceDensity.Value;
            else
                ReduceDensity = false;

            if (flipHor == null)
                FlipHor = false;
            else
                FlipHor = flipHor.Value;

            if (flipVer == null)
                FlipVer = false;
            else
                FlipVer = flipVer.Value;

            if (sortParticles == null)
                SortParticles = false;
            else
                SortParticles = sortParticles.Value;

            YRange = yrange;
            BounceY = Random.Next((int)yrange.X, (int)yrange.Y);
            AddMore = true;
        }

        public void Update(GameTime gameTime)
        {
            CurrentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (CurrentTime > MaxTime)
            {
                for (int i = 0; i < Burst; i++)
                {
                    UpdateManager.AddParticle(
                            Texture, Position, AngleRange, SpeedRange, ScaleRange, StartColor, EndColor, Gravity, Shrink, Fade, StartingRotationRange, RotationIncrementRange, Transparency, TimeRange, out gameData, out renderData);

                    RenderManager.RenderDataObjects.Add(renderData);
                    UpdateManager.ParticleDataObjects.Add(gameData);
                }

                CurrentTime = 0;
            }
        }

        private SpriteEffects RandomOrientation(params SpriteEffects[] Orientations)
        {
            return Orientations[Random.Next(0, Orientations.Length)];
        }

        public double DoubleRange(double one, double two)
        {
            return one + Random.NextDouble() * (two - one);
        }
    }
}

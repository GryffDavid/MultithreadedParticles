﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestProgram1
{
    public class Emitter : Drawable
    {
        public Texture2D Texture;
        public Vector2 Position, PreviousPosition, AngleRange;        
        public Vector2 ScaleRange, TimeRange, RotationIncrementRange, SpeedRange, StartingRotationRange, 
                       EmitterDirection, EmitterVelocity, YRange, Friction;
        public float Transparency, Gravity, ActiveSeconds, Interval, MaxY, EmitterSpeed,
                     EmitterAngle, EmitterGravity, FadeDelay, StartingInterval;
        public Color StartColor, EndColor;
        public bool Fade, CanBounce, AddMore, Shrink, StopBounce, HardBounce, BouncedOnGround,
                    RotateVelocity, FlipHor, FlipVer, ReduceDensity, SortParticles, Grow;
        public int Burst;
        static Random Random = new Random();
        public double IntervalTime, CurrentTime;
        public SpriteEffects Orientation = SpriteEffects.None;
        public object Tether;

        /// <summary>
        /// Create a particle emitter with the specified parameters
        /// </summary>
        /// <param name="texture">The texture each particle will use</param>
        /// <param name="position">The position from which all particles will be emitted</param>
        /// <param name="angleRange">The range of the angles the particles will be emitted at</param>
        /// <param name="speedRange">The range of the speed the particles will start with</param>
        /// <param name="timeRange">The range of the HP the particles will have - how long they're active for</param>
        /// <param name="startingTransparency">How transparent the particles will start out 0.0 to 1.0</param>
        /// <param name="fade">Whether the particles will fade out over their lifetime or not</param>
        /// <param name="startingRotationRange">The range of rotation that the particles start with</param>
        /// <param name="rotationIncrement">How fast the particles rotate after being created</param>
        /// <param name="scaleRange">The range of scale each particle starts with</param>
        /// <param name="startColor">The color the particles start as</param>
        /// <param name="endColor">The color the particles end as</param>
        /// <param name="gravity">How fast the particles accelerate in the Y axis</param>
        /// <param name="activeSeconds">How long the emitter is active for</param>
        /// <param name="interval">The time interval in ms between particles being emitted</param>
        /// <param name="burst">How many particles are emitted after each interval</param>
        /// <param name="canBounce">Whether the particles bounce after exceeding their MaxY or not</param>
        /// <param name="yrange">The range of Y positions the particles will bounce/stop at</param>
        /// <param name="shrink">Whether the particles shrinks over their lifetime</param>
        /// <param name="drawDepth">The depth at which every particle from this emitter is drawn</param>
        /// <param name="stopBounce">The particles bounce, but don't fall through the floor</param>
        /// <param name="hardBounce">The particles bounce retaining a lot of their energy</param>
        /// <param name="emitterSpeed">How fast the emitter travels when created</param>
        /// <param name="emitterAngle">The angle the emitter travels at when created</param>
        /// <param name="emitterGravity">The Y axis acceleration of the emitter</param>
        /// <param name="rotateVelocity">Whether the emitter emits particles in the opposite direction to its velocity</param>
        /// <param name="friction">How much friction the particles have when created</param>
        /// <param name="flipHor">Whether the particles should be randomly flipped horizontally when created</param>
        /// <param name="flipVer">Whether the particles should be randomly flipped vertically when created</param>
        /// <param name="fadeDelay">The delay in ms before the particles start to fade out</param>
        /// <param name="reduceDensity">Whether the number of particles burst every interval reduces over time or not</param>
        /// <param name="sortParticles">Whether every particle needs to be individually depth sorted based on the Y position</param>
        public Emitter(Texture2D texture, Vector2 position, Vector2 angleRange, Vector2 speedRange, Vector2 timeRange,
           float startingTransparency, bool fade, Vector2 startingRotationRange, Vector2 rotationIncrement, Vector2 scaleRange,
           Color startColor, Color endColor, float gravity, float activeSeconds, float interval, int burst, bool canBounce,
           Vector2 yrange, bool? shrink = null, float? drawDepth = null, bool? stopBounce = null, bool? hardBounce = null,
           Vector2? emitterSpeed = null, Vector2? emitterAngle = null, float? emitterGravity = null, bool? rotateVelocity = null,
            Vector2? friction = null, bool? flipHor = null, bool? flipVer = null, float? fadeDelay = null, bool? reduceDensity = null,
            bool? sortParticles = null, bool? grow = false)
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
            Grow = grow.Value;

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
            MaxY = Random.Next((int)yrange.X, (int)yrange.Y);
            AddMore = true;
        }
        
        public void Update(GameTime gameTime)
        {
            if (Active == true)
            {
                //If the emitter is given a value smaller than or equal to 0, it will carry on emitting infinitely//
                //If the value is bigger than zero, it will only emit particles for the length of time given//
                if (ActiveSeconds > 0)
                {
                    CurrentTime += gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (CurrentTime > ActiveSeconds * 1000)
                    {
                        AddMore = false;
                    }
                }

                if (ReduceDensity == true)
                {
                    //After halftime, begin reducing the density from 100% down to 0% as the time continues to expire                    
                    //Interval = MathHelper.Lerp((float)Interval, (float)(Interval * 5), 0.0001f);
                    float PercentageThrough = ((float)CurrentTime / (ActiveSeconds * 1000)) * 100;

                    if (PercentageThrough >= 50)
                        Interval = StartingInterval + (Interval / 100 * PercentageThrough);

                }

                if (EmitterSpeed != 0)
                {
                    EmitterVelocity.Y += EmitterGravity * ((float)gameTime.ElapsedGameTime.TotalSeconds * 60.0f);
                    Position += EmitterVelocity * ((float)gameTime.ElapsedGameTime.TotalSeconds * 60.0f);

                    if (CanBounce == true)
                        if (Position.Y >= MaxY && BouncedOnGround == false)
                        {
                            if (HardBounce == true)
                                Position.Y -= EmitterVelocity.Y * ((float)gameTime.ElapsedGameTime.TotalSeconds * 60.0f);

                            EmitterVelocity.Y = (-EmitterVelocity.Y / 3) * ((float)gameTime.ElapsedGameTime.TotalSeconds * 60.0f);
                            EmitterVelocity.X = (EmitterVelocity.X / 3) * ((float)gameTime.ElapsedGameTime.TotalSeconds * 60.0f);
                            BouncedOnGround = true;
                        }

                    if (StopBounce == true &&
                        BouncedOnGround == true &&
                        Position.Y > MaxY)
                    {
                        EmitterVelocity.Y = (-EmitterVelocity.Y / 2) * ((float)gameTime.ElapsedGameTime.TotalSeconds * 60.0f);

                        EmitterVelocity.X *= 0.9f * ((float)gameTime.ElapsedGameTime.TotalSeconds * 60.0f);

                        if (EmitterVelocity.Y < 0.2f && EmitterVelocity.Y > 0)
                        {
                            EmitterVelocity.Y = 0;
                        }

                        if (EmitterVelocity.Y > -0.2f && EmitterVelocity.Y < 0)
                        {
                            EmitterVelocity.Y = 0;
                        }

                        if (EmitterVelocity.X < 0.2f && EmitterVelocity.X > 0)
                        {
                            EmitterVelocity.X = 0;
                        }

                        if (EmitterVelocity.X > -0.2f && EmitterVelocity.X < 0)
                        {
                            EmitterVelocity.X = 0;
                        }
                    }
                }

                if (FlipHor == true && FlipVer == false)
                {
                    Orientation = RandomOrientation(SpriteEffects.None, SpriteEffects.FlipHorizontally);
                    //Get back None or FlipHor
                    //1
                }

                if (FlipHor == false && FlipVer == true)
                {
                    Orientation = RandomOrientation(SpriteEffects.None, SpriteEffects.FlipVertically);
                    //Get back None or FlipVer
                    //2
                }

                if (FlipHor == true && FlipVer == true)
                {
                    Orientation = RandomOrientation(SpriteEffects.None, SpriteEffects.FlipVertically, SpriteEffects.FlipHorizontally);
                    //Get back None, FlipHor, FlipVer
                    //3
                }

                IntervalTime += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (IntervalTime > Interval && AddMore == true)
                {
                    int newBurst = (int)(Burst * (IntervalTime / Interval));

                    for (int i = 0; i < newBurst; i++)
                    {
                        float angle, hp, scale, rotation, speed, startingRotation;

                        angle = -MathHelper.ToRadians((float)DoubleRange(AngleRange.X, AngleRange.Y));
                        hp = (float)DoubleRange(TimeRange.X, TimeRange.Y);
                        scale = (float)DoubleRange(ScaleRange.X, ScaleRange.Y);
                        rotation = (float)DoubleRange(RotationIncrementRange.X, RotationIncrementRange.Y);
                        speed = (float)DoubleRange(SpeedRange.X, SpeedRange.Y);
                        startingRotation = (float)DoubleRange(StartingRotationRange.X, StartingRotationRange.Y);
                        MaxY = Random.Next((int)YRange.X, (int)YRange.Y);

                        Particle NewParticle = new Particle(Texture, Position, angle, speed, hp, Transparency, Fade, startingRotation,
                                                            rotation, scale, StartColor, EndColor, Gravity, CanBounce, MaxY, Shrink,
                                                            DrawDepth, StopBounce, HardBounce, false, RotateVelocity, Friction, Orientation,
                                                            FadeDelay, SortParticles, Grow);


                        //TRIGGER EVENT HERE THAT ADDS PARTICLE DATA TO THE UPDATER/RENDERER INSTEAD OF ADDING TO THE PARTICLE LIST
                        //ParticleList.Add(NewParticle);
                    }

                    IntervalTime = 0;
                }
            }
            
            PreviousPosition = Position;
        }


        public double DoubleRange(double one, double two)
        {
            return one + Random.NextDouble() * (two - one);
        }

        private SpriteEffects RandomOrientation(params SpriteEffects[] Orientations)
        {
            //List<SpriteEffects> OrientationList = new List<SpriteEffects>();

            //foreach (SpriteEffects orientation in Orientations)
            //{
            //    OrientationList.Add(orientation);
            //}

            return Orientations[Random.Next(0, Orientations.Length)];
        }
    }
}

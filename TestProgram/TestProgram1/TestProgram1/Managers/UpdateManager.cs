using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;
using System.Diagnostics;

namespace TestProgram1
{
    class UpdateManager
    {
        protected ChangeBuffer MessageBuffer;
        private GameTime GameTime; 
        public List<ParticleData> ParticleDataObjects { get; set; }
        public DoubleBuffer DoubleBuffer;
        public Thread RunningThread;

        static Random Random = new Random();

        public UpdateManager(DoubleBuffer doubleBuffer)
        {
            DoubleBuffer = doubleBuffer;
            ParticleDataObjects = new List<ParticleData>();
        }

        public void Update(GameTime gameTime)
        {
            MessageBuffer.Clear();

            for (int i = 0; i < ParticleDataObjects.Count; i++)
            {
                ParticleData gameData = ParticleDataObjects[i];

                gameData.CurrentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                gameData.Velocity.Y += gameData.Gravity;

                Vector2 newPos = gameData.Position + gameData.Velocity;
                float Rot = gameData.Rotation + MathHelper.ToRadians(gameData.RotationIncrement);
                float newScale = gameData.Scale;
                float transparency = gameData.StartingTransparency;

                float percTime = gameData.CurrentTime / gameData.MaxTime;
                Color newCol = Color.Lerp(gameData.StartColor, gameData.EndColor, percTime);

                if (gameData.Shrink == true)
                    newScale = MathHelper.Lerp(gameData.Scale, 0, percTime);

                if (gameData.Fade == true)
                    transparency = MathHelper.Lerp(gameData.StartingTransparency, 0, percTime);
                
                ChangeMessage msg = new ChangeMessage()
                {
                    ID = i
                };

                if (gameData.CurrentTime > gameData.MaxTime)
                {
                    msg.MessageType = ChangeMessageType.DeleteRenderData;
                    ParticleDataObjects.Remove(gameData);
                    MessageBuffer.Add(msg);
                    i--;
                }

                if (gameData.CurrentTime <= gameData.MaxTime)
                {
                    msg.MessageType = ChangeMessageType.UpdateParticle;
                    msg.Position = newPos;
                    msg.Rotation = Rot;
                    msg.Color = newCol;
                    msg.Scale = newScale;
                    msg.Transparency = transparency;
                    MessageBuffer.Add(msg);

                    gameData.Position = newPos;
                    gameData.Rotation = Rot;
                }
            }
        }

        public void AddParticle(Texture2D texture, Vector2 pos, Vector2 angleRange, Vector2 speedRange, Vector2 scaleRange, 
            Color startColor, Color endColor, float gravity, bool shrink, bool fade, Vector2 startingRotation, 
            Vector2 rotationIncrement, float startingTransparency, Vector2 timeRange,
            out ParticleData gameData, out RenderData renderData)
        {
            float myAngle, mySpeed, myScale, myRotation, myIncrement, myTime;
            Vector2 Direction, myVelocity;

            mySpeed = (float)DoubleRange(speedRange.X, speedRange.Y);
            myAngle = -MathHelper.ToRadians((float)DoubleRange(angleRange.X, angleRange.Y));
            myScale = (float)DoubleRange(scaleRange.X, scaleRange.Y);
            myRotation = (float)DoubleRange(startingRotation.X, startingRotation.Y);
            myIncrement = (float)DoubleRange(rotationIncrement.X, rotationIncrement.Y);
            myTime = (float)DoubleRange(timeRange.X, timeRange.Y);

            Direction.X = (float)Math.Cos(myAngle);
            Direction.Y = (float)Math.Sin(myAngle);

            myVelocity = Direction * mySpeed;

            gameData = new ParticleData()
            {
                Position = pos,
                Angle = angleRange,
                CurrentTime = 0,
                MaxTime = myTime,
                Velocity = myVelocity,
                StartColor = startColor,
                EndColor = endColor,
                Gravity = gravity,
                Scale = myScale,
                Shrink = shrink,
                Fade = fade,
                StartingTransparency = startingTransparency,
                Rotation = myRotation,
                RotationIncrement = myIncrement
            };

            renderData = new RenderData()
            {
                Texture = texture,
                Position = gameData.Position,
                Color = startColor,
                Rotation = Random.Next(0, 360),
                Scale = myScale,
                Transparency = 0.5f
            };
        }


        public void StartOnNewThread()
        {
            ThreadStart threadStart = new ThreadStart(Run);
            RunningThread = new Thread(threadStart);
            RunningThread.Start();
        }

        private void Run()
        {
            while (true)
            {
                DoFrame();
            }
        }

        public void DoFrame()
        {
            DoubleBuffer.StartUpdateProcessing(out MessageBuffer, out GameTime);
            Update(GameTime);
            DoubleBuffer.SubmitUpdate();
        }

        public double DoubleRange(double one, double two)
        {
            return one + Random.NextDouble() * (two - one);
        }
    }
}

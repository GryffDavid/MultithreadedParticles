﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System.Diagnostics;

namespace TestProgram1
{
    class UpdateManager
    {
        protected ChangeBuffer MessageBuffer;
        protected Game Game;

        private GameTime GameTime;      
 
        public List<ParticleData> ParticleDataObjects { get; set; }
        public Stopwatch FrameWatch { get; set; }
        public DoubleBuffer DoubleBuffer;
        public Thread RunningThread;
        Rectangle ScreenRect = new Rectangle(0, 0, 1920, 1080);

        static Random Random = new Random();

        public UpdateManager(DoubleBuffer doubleBuffer, Game game)
        {
            DoubleBuffer = doubleBuffer;
            Game = game;
            ParticleDataObjects = new List<ParticleData>();

            FrameWatch = new Stopwatch();
            FrameWatch.Reset();
        }

        public void Update(GameTime gameTime)
        {
            MessageBuffer.Clear();

            for (int i = 0; i < ParticleDataObjects.Count; i++)
            {
                ParticleData gameData = ParticleDataObjects[i];
                gameData.CurrentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                Vector2 newPos = gameData.Position + gameData.Velocity;
                float Rot = gameData.Rotation + 3f;               
                
                float percTime = gameData.CurrentTime / gameData.MaxTime;
                Color newCol = Color.Lerp(gameData.StartColor, gameData.EndColor, percTime);

                ChangeMessage msg = new ChangeMessage()
                {
                    ID = i
                };

                #region Remove particle if it leaves the screen
                if (!ScreenRect.Contains(new Point((int)newPos.X, (int)newPos.Y)) ||
                    gameData.CurrentTime > gameData.MaxTime)
                {
                    msg.MessageType = ChangeMessageType.DeleteRenderData;
                    ParticleDataObjects.Remove(gameData);
                    MessageBuffer.Add(msg);
                }
                #endregion
                else
                {
                    msg.MessageType = ChangeMessageType.UpdateParticle;
                    msg.Position = newPos;                    
                    msg.Rotation = Rot;
                    msg.Color = newCol;
                    MessageBuffer.Add(msg);

                    gameData.Position = newPos;
                    gameData.Rotation = Rot;
                    
                }
            }
        }

        public void AddParticle(Vector2 pos, Vector2 angleRange, Vector2 speedRange, Color startColor, Color endColor, out ParticleData gameData, out RenderData renderData)
        {
            float myAngle, mySpeed;
            Vector2 Direction, myVelocity;

            mySpeed = (float)DoubleRange(speedRange.X, speedRange.Y);
            myAngle = -MathHelper.ToRadians((float)DoubleRange(angleRange.X, angleRange.Y));

            
            Direction.X = (float)Math.Cos(myAngle);
            Direction.Y = (float)Math.Sin(myAngle);

            myVelocity = Direction * mySpeed;

            gameData = new ParticleData()
            {
                Position = pos,
                Angle = angleRange,
                CurrentTime = 0,
                MaxTime = Random.Next(500, 2000),
                Velocity = myVelocity,
                StartColor = startColor,
                EndColor = endColor,
            };

            renderData = new RenderData()
            {
                Position = gameData.Position,
                Color = startColor,
                Rotation = Random.Next(0, 360)
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

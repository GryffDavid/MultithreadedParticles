using System;
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
        public MouseState CurrentMouseState, PreviousMouseState;
        Rectangle ScreenRect = new Rectangle(0, 0, 1280, 720);

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

                Vector2 newPos = gameData.Position + gameData.Velocity;
                float Rot = gameData.Rotation + 3f;

                gameData.CurrentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

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
                    MessageBuffer.Add(msg);
                    gameData.Position = newPos;
                    gameData.Rotation = Rot;                    
                    
                }
            }
        }


        private void Run()
        {
            while(true)
            {
                DoFrame();
            }
        }

        public void StartOnNewThread()
        {
            ThreadStart threadStart = new ThreadStart(Run);
            RunningThread = new Thread(threadStart);
            RunningThread.Start();
        }


        public void DoFrame()
        {
            DoubleBuffer.StartUpdateProcessing(out MessageBuffer, out GameTime);
            Update(GameTime);
            DoubleBuffer.SubmitUpdate();
        }
          

        public void AddParticle(Vector2 pos, Vector2 vel, Color color, out ParticleData gameData, out RenderData renderData)
        {
            gameData = new ParticleData();
            gameData.Position = pos;
            gameData.Velocity = vel;
            gameData.CurrentTime = 0;
            gameData.MaxTime = 1000f;

            renderData = new RenderData();
            renderData.Position = gameData.Position;
            renderData.Color = color;
            renderData.Rotation = Random.Next(0, 360);
            
        }
    }
}

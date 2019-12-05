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
        public List<GameData> GameDataObjects { get; set; }

        public DoubleBuffer DoubleBuffer;
        private GameTime GameTime;

        protected ChangeBuffer MessageBuffer;
        protected Game Game;

        public Thread RunningThread;

        public Stopwatch FrameWatch { get; set; }

        public MouseState CurrentMouseState, PreviousMouseState;

        public UpdateManager(DoubleBuffer doubleBuffer, Game game)
        {
            DoubleBuffer = doubleBuffer;
            Game = game;
            GameDataObjects = new List<GameData>();

            FrameWatch = new Stopwatch();
            FrameWatch.Reset();
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

        public void Update(GameTime gameTime)
        {
            MessageBuffer.Clear();
            CurrentMouseState = Mouse.GetState();

            for (int i = 0; i < GameDataObjects.Count; i++)
            {
                GameData gameData = GameDataObjects[i];
                Vector2 newPos = gameData.Position + (gameData.Velocity * (float)(gameTime.ElapsedGameTime.TotalSeconds * 60f));

                

                ChangeMessage msg = new ChangeMessage();
                msg.ID = i;
                msg.MessageType = ChangeMessageType.UpdateParticlePosition;

                if (!new Rectangle(0, 0, 1280, 720).Contains(new Point((int)newPos.X, (int)newPos.Y)))
                {
                    msg.MessageType = ChangeMessageType.DeleteRenderData;
                }
                else
                {
                    msg.MessageType = ChangeMessageType.UpdateParticlePosition;
                }

                msg.Position = newPos;
                MessageBuffer.Add(msg);

                gameData.Position = newPos;
            }

            PreviousMouseState = CurrentMouseState;
        }


        public void AddParticle(Vector2 pos, Vector2 vel, Color color, out GameData gameData, out RenderData renderData)
        {
            gameData = new GameData();
            gameData.Position = pos;
            gameData.Velocity = vel;
            gameData.Color = color;

            renderData = new RenderData();
            renderData.Position = gameData.Position;
            renderData.Color = color;
        }
    }
}

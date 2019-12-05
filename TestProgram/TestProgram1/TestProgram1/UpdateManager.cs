using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading;
using System.Diagnostics;

namespace TestProgram1
{
    class UpdateManager
    {
        public List<GameData> GameDataObjects { get; set; }
        List<Emitter> EmitterList = new List<Emitter>();

        public DoubleBuffer DoubleBuffer;
        private GameTime GameTime;

        protected ChangeBuffer MessageBuffer;
        protected Game Game;

        public Thread RunningThread;

        public Stopwatch FrameWatch { get; set; }

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
            for (int i = 0; i < GameDataObjects.Count; i++)
            {
                GameData gameData = GameDataObjects[i];

            }
        }

        public void CreateEmitter(Vector2 pos, Vector2 angR, out GameData gameData, out RenderData renderData)
        {
            gameData = new GameData();
            gameData.Position = pos;
            gameData.AngleRange = angR;

            renderData = new RenderData();
            renderData.Position = gameData.Position;
        }
    }
}

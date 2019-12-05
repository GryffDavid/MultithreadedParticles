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
        public DoubleBuffer DoubleBuffer;
        private GameTime GameTime;

        protected ChangeBuffer MessageBuffer;
        protected Game Game;

        public Thread RunningThread;

        public UpdateManager(DoubleBuffer doubleBuffer, Game game)
        {
            DoubleBuffer = doubleBuffer;
            Game = game;
            GameDataObjects = new List<GameData>();
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

        public virtual void Update(GameTime gameTime)
        {

        }
    }
}

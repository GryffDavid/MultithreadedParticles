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
        public List<Emitter> EmitterList = new List<Emitter>();
        
        public Stopwatch FrameWatch { get; set; }
        public DoubleBuffer DoubleBuffer;
        public Thread RunningThread;

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
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading;
using System.Diagnostics;

namespace TestProgram1
{
    class RenderManager
    {
        public List<RenderData> RenderDataObjects { get; set; }
        private DoubleBuffer DoubleBuffer;
        private GameTime GameTime;

        protected ChangeBuffer MessageBuffer;
        protected Game Game;

        public RenderManager(DoubleBuffer doubleBuffer, Game game)
        {
            DoubleBuffer = doubleBuffer;
            Game = game;
            RenderDataObjects = new List<RenderData>();
        }

        public virtual void LoadContent()
        {

        }

        public void DoFrame()
        {
            DoubleBuffer.StartRenderProcessing(out MessageBuffer, out GameTime);
            Draw(GameTime);
            DoubleBuffer.SubmitRender();
        }

        public virtual void Draw(GameTime gameTime)
        {

        }
    }
}

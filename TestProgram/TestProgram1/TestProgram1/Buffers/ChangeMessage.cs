using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;

namespace TestProgram1
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ChangeMessage
    {
        [FieldOffset(0)]
        public ChangeMessageType MessageType;

        [FieldOffset(4)]
        public Vector2 Position;

        [FieldOffset(20)]
        public int ID;
    }
}

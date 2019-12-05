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
        //this appears in all messages
        //identifies how this message should be interpreted
        [FieldOffset(0)]
        public ChangeMessageType MessageType;

        //this is the field required when this message is of type UpdateCameraView
        [FieldOffset(4)]
        public Vector2 Position;

        //this field is used for all messages dealing with entities
        [FieldOffset(4)]
        public int ID;
        
        //this is the field required when this message is of type CreateNewRenderData
        [FieldOffset(8)]
        public Vector2 Position;

        [FieldOffset(20)]
        public Vector2 AngleRange;

        //nothing is required when this message is of type DeleteRenderData
    }
}

using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Diagnostics;

namespace ModelPreProcessing
{
    public class ClRenderObjectLine : ClBaseRenderObject
    {
        private List<CustomVertex.PositionColored> m_lRenderLineVertex = new List<CustomVertex.PositionColored>();

        public ClRenderObjectLine(float p_fBeginnigX, float p_fBeginnigY, float p_fBeginnigZ, float p_fEndX, float p_fEndY, float p_fEndZ, Color p_cColor)
            : base("RenderObjectLine")
        {
            m_lRenderLineVertex.Add(new CustomVertex.PositionColored(p_fBeginnigX, p_fBeginnigY, p_fBeginnigZ, p_cColor.ToArgb()));
            m_lRenderLineVertex.Add(new CustomVertex.PositionColored(p_fEndX, p_fEndY, p_fEndZ, p_cColor.ToArgb()));
        }

        public ClRenderObjectLine()
            : base("RenderObjectLine")
        {
            m_lRenderLineVertex.Add(new CustomVertex.PositionColored(0, 0, 0, Color.Red.ToArgb()));
            m_lRenderLineVertex.Add(new CustomVertex.PositionColored(300, 0, 0, Color.Red.ToArgb()));

        }

        public override void Render(Device p_dDevice, Control p_cRenderWindow)
        {
            p_dDevice.VertexFormat = CustomVertex.PositionColored.Format;
            p_dDevice.DrawUserPrimitives(PrimitiveType.LineList, 1, m_lRenderLineVertex.ToArray());
        }
    }
}

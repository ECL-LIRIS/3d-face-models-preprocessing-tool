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
    public class ClCoordinateSystem : ClBaseRenderObject
    {
        private List<CustomVertex.PositionColored> m_lRenderLineVertex = new List<CustomVertex.PositionColored>();
        private int m_iLineLong = 300;


        public ClCoordinateSystem(float p_fMoveX, float p_fMoveY, float p_fMoveZ)
            : base("CoordinateSystem")
        {
            m_lRenderLineVertex.Add(new CustomVertex.PositionColored(p_fMoveX, p_fMoveY, p_fMoveZ, Color.Red.ToArgb()));
            m_lRenderLineVertex.Add(new CustomVertex.PositionColored(-m_iLineLong + p_fMoveX, p_fMoveY, p_fMoveZ, Color.Red.ToArgb()));

            m_lRenderLineVertex.Add(new CustomVertex.PositionColored(p_fMoveX, p_fMoveY, p_fMoveZ, Color.Green.ToArgb()));
            m_lRenderLineVertex.Add(new CustomVertex.PositionColored(p_fMoveX, p_fMoveY + m_iLineLong, p_fMoveZ, Color.Green.ToArgb()));

            m_lRenderLineVertex.Add(new CustomVertex.PositionColored(p_fMoveX, p_fMoveY, p_fMoveZ, Color.Blue.ToArgb()));
            m_lRenderLineVertex.Add(new CustomVertex.PositionColored(p_fMoveX, p_fMoveY, p_fMoveZ + m_iLineLong, Color.Blue.ToArgb()));
        }

        public ClCoordinateSystem()
            : base("CoordinateSystem")
        {
            m_lRenderLineVertex.Add(new CustomVertex.PositionColored(0, 0, 0, Color.Red.ToArgb()));
            m_lRenderLineVertex.Add(new CustomVertex.PositionColored(-m_iLineLong, 0, 0, Color.Red.ToArgb()));

            m_lRenderLineVertex.Add(new CustomVertex.PositionColored(0, 0, 0, Color.Green.ToArgb()));
            m_lRenderLineVertex.Add(new CustomVertex.PositionColored(0, m_iLineLong, 0, Color.Green.ToArgb()));

            m_lRenderLineVertex.Add(new CustomVertex.PositionColored(0, 0, 0, Color.Blue.ToArgb()));
            m_lRenderLineVertex.Add(new CustomVertex.PositionColored(0, 0, m_iLineLong, Color.Blue.ToArgb()));
        }

        public override void Render(Device p_dDevice, Control p_cRenderWindow)
        {

            p_dDevice.VertexFormat = CustomVertex.PositionColored.Format;
            p_dDevice.DrawUserPrimitives(PrimitiveType.LineList, m_lRenderLineVertex.Count/2, m_lRenderLineVertex.ToArray());
        }
    }
}

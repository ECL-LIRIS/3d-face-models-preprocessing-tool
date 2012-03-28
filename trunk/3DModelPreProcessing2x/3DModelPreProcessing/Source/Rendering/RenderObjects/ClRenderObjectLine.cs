/*  Copyright (C) 2011 Przemyslaw Szeptycki <pszeptycki@gmail.com>, Ecole Centrale de Lyon,

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
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

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

using PreprocessingFramework;
/**************************************************************************
*
*                          ModelPreProcessing
*
* Copyright (C)         Przemyslaw Szeptycki 2007     All Rights reserved
*
***************************************************************************/

/**
*   @file       Cl3DRenderModel.cs
*   @brief      Object designed to render face models
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       05-06-2008
*   
*   @history
*   @item		05-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace ModelPreProcessing
{
    public class Cl3DRenderModel : ClBaseRenderObject
    {
        private List<CustomVertex.PositionColored> m_lRenderModelVertex = new List<CustomVertex.PositionColored>();
        private List<CustomVertex.PositionColored> m_lRenderLines = new List<CustomVertex.PositionColored>();
        private List<CustomVertex.PositionColored> m_lSpecificPoints = new List<CustomVertex.PositionColored>();
        Cl3DModel m_Base3DModel = null;

        public Cl3DRenderModel(Cl3DModel p_p3DModel)
            : base("3DModel")
        {
            m_Base3DModel = p_p3DModel;
        }

        public override void Render(Device p_dDevice, Control p_cRenderWindow)
        {
            if (m_Base3DModel.IsModelChanged)
            {
                //ClInformationSender.SendInformation("Creating render object ("+m_Base3DModel.ModelPointsCount+" points)...", ClInformationSender.eInformationType.eTextExternal);
                m_lRenderModelVertex.Clear();
                m_lRenderLines.Clear();
                m_Base3DModel.ResetVisitedPoints();
                Cl3DModel.Cl3DModelPointIterator iterator = m_Base3DModel.GetIterator();
                List<KeyValuePair<string, Cl3DModel.Cl3DModelPointIterator>> specificPoints = m_Base3DModel.GetAllSpecificPoints();
                float meanX = 0;
                float meanY = 0;
                float meanZ = 0;
                if (iterator.IsValid())
                {
                    do
                    {
                        meanX += -iterator.X;
                        meanY += iterator.Y;
                        meanZ += iterator.Z;
                        Color pointColor = new Color();
                        bool isSpecificPoint = false;
                        foreach (KeyValuePair<string, Cl3DModel.Cl3DModelPointIterator> specificPoint in specificPoints)
                        {
                            if (iterator.PointID == specificPoint.Value.PointID)
                            {
                                isSpecificPoint = true;
                                break;
                            }
                        }
                        if (!isSpecificPoint)
                            pointColor = iterator.Color;
                        else
                            pointColor = Color.Red;

                        m_lRenderModelVertex.Add(new CustomVertex.PositionColored(-iterator.X, iterator.Y, iterator.Z, pointColor.ToArgb()));

                        List<Cl3DModel.Cl3DModelPointIterator> neighbors = iterator.GetListOfNeighbors();
                        foreach (Cl3DModel.Cl3DModelPointIterator neighbor in neighbors)
                        {
                            if (!neighbor.AlreadyVisited)
                            {
                                m_lRenderLines.Add(new CustomVertex.PositionColored(-iterator.X, iterator.Y, iterator.Z, pointColor.ToArgb()));
                                m_lRenderLines.Add(new CustomVertex.PositionColored(-neighbor.X, neighbor.Y, neighbor.Z, neighbor.Color.ToArgb()));
                            }
                        }
                        iterator.AlreadyVisited = true;
                    }
                    while (iterator.MoveToNext());
                }

                m_Base3DModel.IsModelChanged = false;
                m_Base3DModel.ResetVisitedPoints();
                #if RENDER_1
                ClCamera camera = ClRender.getInstance().getCamera();
                if (camera != null)
                {
                    meanX /= m_lRenderModelVertex.Count;
                    meanY /= m_lRenderModelVertex.Count;
                    meanZ /= m_lRenderModelVertex.Count;
                    camera.MoveCameraLookAt(meanX, meanY, meanZ);
                //    ClRender.getInstance().AddRenderObj(new ClCoordinateSystem(meanX,meanY,meanZ));
                }
                #endif
            }

            if (m_lRenderModelVertex.Count != 0)
            {
                p_dDevice.VertexFormat = CustomVertex.PositionColored.Format;
                if (m_lRenderLines.Count != 0)
                    p_dDevice.DrawUserPrimitives(PrimitiveType.LineList, m_lRenderLines.Count / 2, m_lRenderLines.ToArray());
                
                p_dDevice.DrawUserPrimitives(PrimitiveType.PointList, m_lRenderModelVertex.Count, m_lRenderModelVertex.ToArray());
            }
        }

        public void ChangeFaceColor(Color p_Color)
        {
            m_Base3DModel.ResetColor(p_Color);
        }

        public List<String> WhatSpecificValuesAreCalculated()
        {
            return m_Base3DModel.GetIterator().GetListOfSpecificValues();
        }

    }
}

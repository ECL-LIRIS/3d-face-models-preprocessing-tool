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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;

using PreprocessingFramework;
/**************************************************************************
*
*                          ModelPreProcessing
*
* Copyright (C)         Przemyslaw Szeptycki 2007     All Rights reserved
*
***************************************************************************/

/**
*   @file       ClRotatieModel.cs
*   @brief      Algorithm to crop face
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-06-2008
*
*   @history
*   @item		10-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClRemovePathBetweenPoints : ClBaseFaceAlgorithm
    {
        public class Edge
        {
            public MyColor Color1;
            public MyColor Color2;
            public int X1;
            public int Y1;
            public int X2;
            public int Y2;

            public Edge(int x1, int y1, int x2, int y2)
            {
                if (y1 < y2)
                {
                    X1 = x1;
                    Y1 = y1;
                    X2 = x2;
                    Y2 = y2;
                }
                else
                {
                    X1 = x2;
                    Y1 = y2;
                    X2 = x1;
                    Y2 = y1;
                }
            }
        }

        public class Span
        {

            public int X1;
            public int X2;

            public Span(int x1, int x2)
            {
                if (x1 < x2)
                {
                    X1 = x1;
                    X2 = x2;
                }
                else
                {
                    X1 = x2;
                    X2 = x1;
                }
            }
        }

        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClRemovePathBetweenPoints();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Tools\Remove Path Between Points";

        public ClRemovePathBetweenPoints() : base(ALGORITHM_NAME) { }

        Cl3DModel.Cl3DModelPointIterator[,] Map = null;

        private void RemoveSpan(Span span, int y)
        {
            int xdiff = span.X2 - span.X1;
            if (xdiff == 0)
                return;
            float factor = 0.0f;
            float factorStep = 1.0f / (float)xdiff;

            // draw each pixel in the span
            for (int x = span.X1; x < span.X2; x++)
            {
                Cl3DModel model = Map[x, y].GetManagedModel();

                model.RemovePointFromModel(Map[x, y]);
                Map[x, y] = null;

                factor += factorStep;
            }
        }

        private void RemoveSpansBetweenEdges(Edge e1, Edge e2)
        {
            float e1ydiff = (float)(e1.Y2 - e1.Y1);
            if (e1ydiff == 0.0f)
                return;

            float e2ydiff = (float)(e2.Y2 - e2.Y1);
            if (e2ydiff == 0.0f)
                return;

            float e1xdiff = (float)(e1.X2 - e1.X1);
            float e2xdiff = (float)(e2.X2 - e2.X1);

            float factor1 = (float)(e2.Y1 - e1.Y1) / e1ydiff;
            float factorStep1 = 1.0f / e1ydiff;
            float factor2 = 0.0f;
            float factorStep2 = 1.0f / e2ydiff;

            // loop through the lines between the edges and draw spans
            for (int y = e2.Y1; y < e2.Y2; y++)
            {
                // create and draw span
                Span span = new Span(e1.X1 + (int)(e1xdiff * factor1),
                                      e2.X1 + (int)(e2xdiff * factor2)
                                      );

                RemoveSpan(span, y);

                // increase factors
                factor1 += factorStep1;
                factor2 += factorStep2;
            }
        }

        private void RemoveTriangle(Edge[] edges)
        {
            int maxLength = 0;
            int longEdge = 0;

            // find edge with the greatest length in the y axis
            for (int i = 0; i < 3; i++)
            {
                int length = edges[i].Y2 - edges[i].Y1;
                if (length > maxLength)
                {
                    maxLength = length;
                    longEdge = i;
                }
            }

            int shortEdge1 = (longEdge + 1) % 3;
            int shortEdge2 = (longEdge + 2) % 3;

            RemoveSpansBetweenEdges(edges[longEdge], edges[shortEdge1]);
            RemoveSpansBetweenEdges(edges[longEdge], edges[shortEdge2]);
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator leftLipsCorner = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftCornerOfLips);
            Cl3DModel.Cl3DModelPointIterator rightLipsCorner = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightCornerOfLips);

            Cl3DModel.Cl3DModelPointIterator UpperLip = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.UpperLip);
            Cl3DModel.Cl3DModelPointIterator BottomLip = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.BottomLip);

            Edge[] edges = new Edge[3];

            int Width = 0;
            int Height = 0;
            int OffsetX = 0;
            int OffsetY = 0;

            ClTools.CreateGridBasedOnRangeValues(p_Model, out Map, out Width, out Height, out OffsetX, out OffsetY);

            edges[0] = new Edge((int)leftLipsCorner.RangeImageX - OffsetX, (int)leftLipsCorner.RangeImageY - OffsetY, (int)UpperLip.RangeImageX - OffsetX, (int)UpperLip.RangeImageY - OffsetY);
            edges[1] = new Edge((int)UpperLip.RangeImageX - OffsetX, (int)UpperLip.RangeImageY - OffsetY, (int)BottomLip.RangeImageX - OffsetX, (int)BottomLip.RangeImageY - OffsetY);
            edges[2] = new Edge((int)BottomLip.RangeImageX - OffsetX, (int)BottomLip.RangeImageY - OffsetY, (int)leftLipsCorner.RangeImageX - OffsetX, (int)leftLipsCorner.RangeImageY - OffsetY);
            RemoveTriangle(edges);

            edges[0] = new Edge((int)UpperLip.RangeImageX - OffsetX, (int)UpperLip.RangeImageY - OffsetY, (int)rightLipsCorner.RangeImageX - OffsetX, (int)rightLipsCorner.RangeImageY - OffsetY);
            edges[1] = new Edge((int)rightLipsCorner.RangeImageX - OffsetX, (int)rightLipsCorner.RangeImageY - OffsetY, (int)BottomLip.RangeImageX - OffsetX, (int)BottomLip.RangeImageY - OffsetY);
            edges[2] = new Edge((int)BottomLip.RangeImageX - OffsetX, (int)BottomLip.RangeImageY - OffsetY, (int)UpperLip.RangeImageX - OffsetX, (int)UpperLip.RangeImageY - OffsetY);
            RemoveTriangle(edges);

        }
    }
}

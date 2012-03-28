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
*   @file       ClRemoveSpikesMedianFilter.cs
*   @brief      Algorithm to remove spikes
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       16-11-2007
*
*   @history
*   @item		16-11-2007 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClCalculateConformalFactor : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClCalculateConformalFactor();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Conformal Map Tools\Calculate Conformal Factor";

        public ClCalculateConformalFactor() : base(ALGORITHM_NAME) { }

        string ConformalMapFolder = "ConformalMaps";

        private float CalculateWholeModelArea(Cl3DModel p_Model)
        {
            float Area = 0;
            List<ClTools.ClTriangle> Triangles = null;

            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            if (iter.IsValid())
                do
                {
                    ClTools.GetListOfTriangles(out Triangles, iter);
                    foreach (ClTools.ClTriangle triangle in Triangles)
                    {
                        if (triangle.AlreadyVisited)
                            continue;

                        Area += ClTools.CalculateTriangleArea(triangle);
                        triangle.AlreadyVisited = true;
                    }

                } while (iter.MoveToNext());

            return Area;
        }


        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel ConformalModel = new Cl3DModel();
            ConformalModel.LoadModel(p_Model.ModelFileFolder+ConformalMapFolder+"\\"+p_Model.ModelFileName+".m_Out.pos.m");

            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            List<ClTools.ClTriangle> Triangles = null;

            float ModelArea = CalculateWholeModelArea(p_Model);

            float ConformalMapArea = CalculateWholeModelArea(ConformalModel);

            if(iter.IsValid())
                do
                {
                    float area3D = 0;
                    ClTools.GetListOfTriangles(out Triangles, iter);

                //    if (Triangles.Count != 6)
                 //       continue;

                    foreach (ClTools.ClTriangle triangle in Triangles)
                        area3D += ClTools.CalculateTriangleArea(triangle);

                    area3D /= Triangles.Count;

                    iter.AddSpecificValue("ConnectedTrianglesArea", area3D);
                    
                    Cl3DModel.Cl3DModelPointIterator ConformalIter = ConformalModel.GetIterator();
                    if (!ConformalIter.MoveToPoint(iter.PointID))
                        continue;//throw new Exception("Cannot find on conformal model point with no: " + iter.PointID.ToString());

                    float area2D = 0;
                    ClTools.GetListOfTriangles(out Triangles, ConformalIter);
                    foreach (ClTools.ClTriangle triangle in Triangles)
                        area2D += ClTools.CalculateTriangleArea(triangle);

                    area2D /= Triangles.Count;

                    float ConformalFactor = (area3D / ModelArea) / (area2D / ConformalMapArea);

                    ConformalIter.AddSpecificValue("ConformalFactor", ConformalFactor);

                } while (iter.MoveToNext());
           
            p_Model = ConformalModel;
        }
    }
}

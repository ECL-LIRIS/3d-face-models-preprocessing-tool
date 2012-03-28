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
using Iridium.Numerics.LinearAlgebra;

using PreprocessingFramework;
/**************************************************************************
*
*                          ModelPreProcessing
*
* Copyright (C)         Przemyslaw Szeptycki 2007     All Rights reserved
*
***************************************************************************/

/**
*   @file       ClCalculateNormalVectors.cs
*   @brief      Algorithm to ClCalculateNormalVectors
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-12-2008
*
*   @history
*   @item		10-12-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClCalculateNormalVectors : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClCalculateNormalVectors();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Calculations\Normal Vectors";

        public ClCalculateNormalVectors() : base(ALGORITHM_NAME) { }

        // ------------------------- Properities
        int NeighborhoodSize = 25;
        bool UsePCA = false;
        bool ShowNormals = false;
        List<Cl3DModel.Cl3DModelPointIterator> NormalsAdded = new List<Cl3DModel.Cl3DModelPointIterator>();
        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("PCA Neighborhood Size"))
            {
                NeighborhoodSize = Int32.Parse(p_sValue);
            }
            else if (p_sProperity.Equals("Use PCA"))
            {
                UsePCA = Boolean.Parse(p_sValue);
            }
            else if (p_sProperity.Equals("Show Normals"))
            {
                ShowNormals = Boolean.Parse(p_sValue);
            }
                
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Use PCA", UsePCA.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("PCA Neighborhood Size", NeighborhoodSize.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Show Normals", ShowNormals.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            if (NormalsAdded.Count != 0)
            {
                foreach (Cl3DModel.Cl3DModelPointIterator pt in NormalsAdded)
                    p_Model.RemovePointFromModel(pt);

                NormalsAdded = new List<Cl3DModel.Cl3DModelPointIterator>();
            }

            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            do
            {
                Vector NormalVector = null;
                if (UsePCA)
                {
                    ClTools.CalculateNormalVectorInPointUsingPCA(iter.X, iter.Y, iter.Z, iter.GetListOfNeighbors(), out NormalVector);
                }
                else
                {
                    ClTools.CalculateNormalVectorInPoint(iter.X, iter.Y, iter.Z, iter.GetListOfNeighbors(), out NormalVector);
                }

                double norm = Math.Sqrt(Math.Pow(NormalVector[0], 2) + Math.Pow(NormalVector[1], 2) + Math.Pow(NormalVector[2], 2));
                if (norm != 0)
                {
                    NormalVector[0] /= (float)norm;
                    NormalVector[1] /= (float)norm;
                    NormalVector[2] /= (float)norm;
                }
                iter.NormalVector = NormalVector;

                if (ShowNormals)
                {
                    Cl3DModel.Cl3DModelPointIterator point = p_Model.AddPointToModel(iter.X - (float)NormalVector[0]*3, iter.Y - (float)NormalVector[1]*3, iter.Z - (float)NormalVector[2]*3);
                    iter.AddNeighbor(point);
                    NormalsAdded.Add(point);
                }


            } while (iter.MoveToNext());
        }
    }
}
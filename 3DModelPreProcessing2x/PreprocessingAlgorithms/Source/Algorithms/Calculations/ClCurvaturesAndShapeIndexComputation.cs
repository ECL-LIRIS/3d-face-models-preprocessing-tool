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
*   @file       ClCurvaturesAndShapeIndexComputation.cs
*   @brief      Algorithm to Curvatures And Shape Index Computation
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-06-2008
*
*   @history
*   @item		10-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClCurvaturesAndShapeIndexComputation : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClCurvaturesAndShapeIndexComputation();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Calculations\Curvature H, K, k1, k2 and ShapeIndex";

        public ClCurvaturesAndShapeIndexComputation() : base(ALGORITHM_NAME) { }

        public ClCurvaturesAndShapeIndexComputation(float p_fNeighborhood)
            : base(@"Calculations\Curvature H, K, k1, k2 and ShapeIndex") 
        {
            m_fNeighborhoodSize = p_fNeighborhood;
        } 

        // ------------------------- Properities
        private float m_fNeighborhoodSize = 25f;
        private bool m_bRecalculateCurvature = false;
        private bool m_bFirstNeighbor = false;
        private bool m_bNeighborhoodRotation = false;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Recalculate Curvature"))
            {
                m_bRecalculateCurvature = bool.Parse(p_sValue);
            }
            else if (p_sProperity.Equals("Distance value"))
            {
                m_fNeighborhoodSize = Single.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (p_sProperity.Equals("First Neighbor"))
            {
                m_bFirstNeighbor = Boolean.Parse(p_sValue);
            }
            else if (p_sProperity.Equals("Neighborhood Rotation"))
            {
                m_bNeighborhoodRotation = Boolean.Parse(p_sValue);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Distance value", m_fNeighborhoodSize.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Recalculate Curvature", m_bRecalculateCurvature.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("First Neighbor", m_bFirstNeighbor.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Neighborhood Rotation", m_bNeighborhoodRotation.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();

            uint current = 0;
            string NeighborhoodSize = m_fNeighborhoodSize.ToString();
            if (m_bFirstNeighbor)
                NeighborhoodSize = "1Neighb";
            string ShapeIndexString = "ShapeIndex_" + NeighborhoodSize;
            if (iter.IsValid())
            {
                do
                {
                    double A = 0;
                    double B = 0;
                    double C = 0;
                    double D = 0;
                    double E = 0;
                    double F = 0;

                    double H = 0;
                    double K = 0;
                    double k1 = 0;
                    double k2 = 0;
                    double ShapeIndex = 0;
                    double CurvednessIndex = 0;
                    List<Cl3DModel.Cl3DModelPointIterator> ListOfneighborhood;
                    double dx;
                    double dy;
                    double dxy;
                    double dxx;
                    double dyy;

                    //------------------------- Neighborhood Custom --------------------------------------------------
                    if (!iter.IsSpecificValueCalculated(ShapeIndexString) || m_bRecalculateCurvature)
                    {
                        if (!m_bFirstNeighbor)
                            ClTools.GetNeighborhoodWithGeodesicDistance(out ListOfneighborhood, iter, m_fNeighborhoodSize);
                        else
                            ListOfneighborhood = iter.GetListOfNeighbors();

                        if (m_bNeighborhoodRotation)
                        {
                            Matrix Rotation = ClTools.CalculateRotationMatrix(iter.NormalVector, new Vector(new double[] { iter.NormalVector[0], iter.NormalVector[1], 1 }));
                            List<ClTools.MainPoint3D> Neighbors = new List<ClTools.MainPoint3D>();
                            foreach (Cl3DModel.Cl3DModelPointIterator Neighb in ListOfneighborhood)
                            {
                                Matrix after = Rotation * Neighb;
                                Neighbors.Add(new ClTools.MainPoint3D((float)after[0, 0], (float)after[1, 0], (float)after[2, 0], ""));
                            }

                            if (!ClTools.CountSurfaceCoefficients(Neighbors, ref A, ref B, ref C, ref D, ref E, ref F))
                            {
                                iter.RemoveSpecificValue("Gaussian_" + NeighborhoodSize);
                                iter.RemoveSpecificValue("Mean_" + NeighborhoodSize);
                                iter.RemoveSpecificValue("K1_" + NeighborhoodSize);
                                iter.RemoveSpecificValue("K2_" + NeighborhoodSize);
                                iter.RemoveSpecificValue("ShapeIndex_" + NeighborhoodSize);
                                iter.RemoveSpecificValue("CurvednessIndex_" + NeighborhoodSize);
                                continue;
                            }
                        }
                        else
                        {
                            if (!ClTools.CountSurfaceCoefficients(ListOfneighborhood, ref A, ref B, ref C, ref D, ref E, ref F))
                            {
                                iter.RemoveSpecificValue("Gaussian_" + NeighborhoodSize);
                                iter.RemoveSpecificValue("Mean_" + NeighborhoodSize);
                                iter.RemoveSpecificValue("K1_" + NeighborhoodSize);
                                iter.RemoveSpecificValue("K2_" + NeighborhoodSize);
                                iter.RemoveSpecificValue("ShapeIndex_" + NeighborhoodSize);
                                iter.RemoveSpecificValue("CurvednessIndex_" + NeighborhoodSize);
                                continue;
                            }
                        }
                        

                        dx = B + 2 * D * iter.X + E * iter.Y;
                        dy = C + E * iter.X + 2 * F * iter.Y;
                        dxy = E;
                        dxx = 2 * D;
                        dyy = 2 * F;
                        //Mean
                        H = ((((1 + Math.Pow(dy, 2)) * dxx) - (2 * dx * dy * dxy) + ((1 + Math.Pow(dx, 2)) * dyy)) / (2 * Math.Pow((1 + Math.Pow(dx, 2) + Math.Pow(dy, 2)), 3.0d / 2.0d)));
                        //Gaussian  
                        K = (((dxx * dyy) - Math.Pow(dxy, 2)) / Math.Pow((1 + Math.Pow(dx, 2) + Math.Pow(dy, 2)), 2));
                        k1 = H + Math.Sqrt(Math.Pow(H, 2) - K);
                        k2 = H - Math.Sqrt(Math.Pow(H, 2) - K);
                        ShapeIndex = 0.5d - (1 / Math.PI) * Math.Atan((k1 + k2) / (k1 - k2));
                        CurvednessIndex = Math.Sqrt(Math.Pow(k1, 2) + Math.Pow(k2, 2)) / 2;

                        iter.AddSpecificValue("Gaussian_" + NeighborhoodSize, K);
                        iter.AddSpecificValue("Mean_" + NeighborhoodSize, H);
                        iter.AddSpecificValue("K1_" + NeighborhoodSize, k1);
                        iter.AddSpecificValue("K2_" + NeighborhoodSize, k2);
                        iter.AddSpecificValue("ShapeIndex_" + NeighborhoodSize, ShapeIndex);
                        iter.AddSpecificValue("CurvednessIndex_" + NeighborhoodSize, CurvednessIndex);
                    }
                    
                    ClInformationSender.SendInformation(( current * 100 / p_Model.ModelPointsCount).ToString(System.Globalization.CultureInfo.InvariantCulture), ClInformationSender.eInformationType.eProgress);
                    current++;
                } while (iter.MoveToNext());
           }
        }
    }
}

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
*   @file       ClAdjustAntropometryPoints.cs
*   @brief      Algorithm to ClAdjustAntropometryPoints
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       23-09-2008
*
*   @history
*   @item		23-09-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClAdjustAntropometryPoints : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClAdjustAntropometryPoints();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Anthropometry Points\Adjust points from GenericModel";

        public ClAdjustAntropometryPoints() : base(ALGORITHM_NAME) { }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            //------------------- LeftCornerOfLips -------------------------
            Cl3DModel.Cl3DModelPointIterator basicPoint = null;
            List<Cl3DModel.Cl3DModelPointIterator> Neighborhood;
            double max = 0;
            Cl3DModel.Cl3DModelPointIterator NewCorner = null;

            if (p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftCornerOfLips.ToString(), ref basicPoint))
            {
                ClTools.GetNeighborhoodWithEuclideanDistanceCheckNeighborhood(out Neighborhood, basicPoint, 15);

                Neighborhood.Add(basicPoint);

                max = 0;
                NewCorner = null;
                foreach (Cl3DModel.Cl3DModelPointIterator point in Neighborhood)
                {
                    List<Cl3DModel.Cl3DModelPointIterator> PointNeighborhood;
                    ClTools.GetNeighborhoodWithEuclideanDistanceCheckNeighborhood(out PointNeighborhood, point, 10);
                    point.Color = Color.Magenta;

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

                    if (!ClTools.CountSurfaceCoefficients(PointNeighborhood, ref A, ref B, ref C, ref D, ref E, ref F))
                        continue;

                    double dx = B + 2 * D * point.X + E * point.Y;
                    double dy = C + E * point.X + 2 * F * point.Y;
                    double dxy = E;
                    double dxx = 2 * D;
                    double dyy = 2 * F;

                    //Mean
                    H = ((((1 + Math.Pow(dy, 2)) * dxx) - (2 * dx * dy * dxy) + ((1 + Math.Pow(dx, 2)) * dyy)) / (2 * Math.Pow((1 + Math.Pow(dx, 2) + Math.Pow(dy, 2)), 3.0d / 2.0d)));
                    //Gaussian  
                    K = (((dxx * dyy) - Math.Pow(dxy, 2)) / Math.Pow((1 + Math.Pow(dx, 2) + Math.Pow(dy, 2)), 2));
                    k1 = H + Math.Sqrt(Math.Pow(H, 2) - K);
                    k2 = H - Math.Sqrt(Math.Pow(H, 2) - K);
                    ShapeIndex = 0.5d - (1 / Math.PI) * Math.Atan((k1 + k2) / (k1 - k2));

                    if (H > 0 && K > max)
                    {
                        max = K;
                        NewCorner = point.CopyIterator();
                    }
                }
                if (NewCorner != null)
                {
                    p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.LeftCornerOfLips, NewCorner);
                }
            }

            //------------------- RightCornerOfLips -------------------------
            if (p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightCornerOfLips, ref basicPoint))
            {
                ClTools.GetNeighborhoodWithEuclideanDistanceCheckNeighborhood(out Neighborhood, basicPoint, 15);
                Neighborhood.Add(basicPoint);

                max = 0;
                NewCorner = null;
                foreach (Cl3DModel.Cl3DModelPointIterator point in Neighborhood)
                {
                    List<Cl3DModel.Cl3DModelPointIterator> PointNeighborhood;
                    ClTools.GetNeighborhoodWithEuclideanDistanceCheckNeighborhood(out PointNeighborhood, point, 10);
                    point.Color = Color.Magenta;

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

                    if (!ClTools.CountSurfaceCoefficients(PointNeighborhood, ref A, ref B, ref C, ref D, ref E, ref F))
                        continue;

                    double dx = B + 2 * D * point.X + E * point.Y;
                    double dy = C + E * point.X + 2 * F * point.Y;
                    double dxy = E;
                    double dxx = 2 * D;
                    double dyy = 2 * F;

                    //Mean
                    H = ((((1 + Math.Pow(dy, 2)) * dxx) - (2 * dx * dy * dxy) + ((1 + Math.Pow(dx, 2)) * dyy)) / (2 * Math.Pow((1 + Math.Pow(dx, 2) + Math.Pow(dy, 2)), 3.0d / 2.0d)));
                    //Gaussian  
                    K = (((dxx * dyy) - Math.Pow(dxy, 2)) / Math.Pow((1 + Math.Pow(dx, 2) + Math.Pow(dy, 2)), 2));
                    k1 = H + Math.Sqrt(Math.Pow(H, 2) - K);
                    k2 = H - Math.Sqrt(Math.Pow(H, 2) - K);
                    ShapeIndex = 0.5d - (1 / Math.PI) * Math.Atan((k1 + k2) / (k1 - k2));

                    if (H > 0 && K > max)
                    {
                        max = K;
                        NewCorner = point.CopyIterator();
                    }
                }
                if (NewCorner != null)
                {
                    p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.RightCornerOfLips, NewCorner);
                }
            }
            //------------------- LeftCornerOfNose -------------------------
            if (p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftCornerOfNose, ref basicPoint))
            {
                ClTools.GetNeighborhoodWithEuclideanDistanceCheckNeighborhood(out Neighborhood, basicPoint, 10);
                Neighborhood.Add(basicPoint);

                max = 0;
                NewCorner = null;
                foreach (Cl3DModel.Cl3DModelPointIterator point in Neighborhood)
                {
                    List<Cl3DModel.Cl3DModelPointIterator> PointNeighborhood;
                    ClTools.GetNeighborhoodWithEuclideanDistanceCheckNeighborhood(out PointNeighborhood, point, 10);
                    point.Color = Color.Orchid;

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

                    if (!ClTools.CountSurfaceCoefficients(PointNeighborhood, ref A, ref B, ref C, ref D, ref E, ref F))
                        continue;

                    double dx = B + 2 * D * point.X + E * point.Y;
                    double dy = C + E * point.X + 2 * F * point.Y;
                    double dxy = E;
                    double dxx = 2 * D;
                    double dyy = 2 * F;

                    //Mean
                    H = ((((1 + Math.Pow(dy, 2)) * dxx) - (2 * dx * dy * dxy) + ((1 + Math.Pow(dx, 2)) * dyy)) / (2 * Math.Pow((1 + Math.Pow(dx, 2) + Math.Pow(dy, 2)), 3.0d / 2.0d)));
                    //Gaussian  
                    K = (((dxx * dyy) - Math.Pow(dxy, 2)) / Math.Pow((1 + Math.Pow(dx, 2) + Math.Pow(dy, 2)), 2));
                    k1 = H + Math.Sqrt(Math.Pow(H, 2) - K);
                    k2 = H - Math.Sqrt(Math.Pow(H, 2) - K);
                    ShapeIndex = 0.5d - (1 / Math.PI) * Math.Atan((k1 + k2) / (k1 - k2));

                    if (H > 0 && K > max)
                    {
                        max = K;
                        NewCorner = point.CopyIterator();
                    }
                }
                if (NewCorner != null)
                {
                    p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.LeftCornerOfNose, NewCorner);
                }
            }
            //------------------- RightCornerOfNose -------------------------
            if (p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightCornerOfNose, ref basicPoint))
            {
                ClTools.GetNeighborhoodWithEuclideanDistanceCheckNeighborhood(out Neighborhood, basicPoint, 10);
                Neighborhood.Add(basicPoint);

                max = 0;
                NewCorner = null;
                foreach (Cl3DModel.Cl3DModelPointIterator point in Neighborhood)
                {
                    List<Cl3DModel.Cl3DModelPointIterator> PointNeighborhood;
                    ClTools.GetNeighborhoodWithEuclideanDistanceCheckNeighborhood(out PointNeighborhood, point, 10);
                    point.Color = Color.Orchid;

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

                    if (!ClTools.CountSurfaceCoefficients(PointNeighborhood, ref A, ref B, ref C, ref D, ref E, ref F))
                        continue;

                    double dx = B + 2 * D * point.X + E * point.Y;
                    double dy = C + E * point.X + 2 * F * point.Y;
                    double dxy = E;
                    double dxx = 2 * D;
                    double dyy = 2 * F;

                    //Mean
                    H = ((((1 + Math.Pow(dy, 2)) * dxx) - (2 * dx * dy * dxy) + ((1 + Math.Pow(dx, 2)) * dyy)) / (2 * Math.Pow((1 + Math.Pow(dx, 2) + Math.Pow(dy, 2)), 3.0d / 2.0d)));
                    //Gaussian  
                    K = (((dxx * dyy) - Math.Pow(dxy, 2)) / Math.Pow((1 + Math.Pow(dx, 2) + Math.Pow(dy, 2)), 2));
                    k1 = H + Math.Sqrt(Math.Pow(H, 2) - K);
                    k2 = H - Math.Sqrt(Math.Pow(H, 2) - K);
                    ShapeIndex = 0.5d - (1 / Math.PI) * Math.Atan((k1 + k2) / (k1 - k2));

                    if (H > 0 && K > max)
                    {
                        max = K;
                        NewCorner = point.CopyIterator();
                    }
                }
                if (NewCorner != null)
                {
                    p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.RightCornerOfNose, NewCorner);
                }
            }
        }
    }
}

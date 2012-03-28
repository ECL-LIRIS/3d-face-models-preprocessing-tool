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
    public class ClRemoveSpikesGaussianFilter : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClRemoveSpikesGaussianFilter();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Remove spikes\Remove spikes (Gaussian filter)";

        public ClRemoveSpikesGaussianFilter() : base(ALGORITHM_NAME) { }
        int MaskSize = 11;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("MaskSize"))
            {
                int tmp = Int32.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
                if (tmp % 2 != 1)
                    throw new Exception("The number should be odd");

                MaskSize = tmp;
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("MaskSize", MaskSize.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        private double[,] GetGaussianMatrix(int p_GaussianMatrixSize, float p_Theta, out double p_SumOfKernelMatrix)
        {
            double[,] matrix = new double[p_GaussianMatrixSize, p_GaussianMatrixSize];
            p_SumOfKernelMatrix = 0;
            for (int i = 0; i < p_GaussianMatrixSize; i++)
            {
                for (int j = 0; j < p_GaussianMatrixSize; j++)
                {
                    int pointx = i - (p_GaussianMatrixSize / 2);
                    int pointy = j - (p_GaussianMatrixSize / 2);

                    matrix[i, j] = Math.Exp(-1 * ((pointx * pointx + pointy * pointy) / (2 * p_Theta * p_Theta))) / (2 * Math.PI * p_Theta * p_Theta);
                    p_SumOfKernelMatrix += matrix[i, j];
                }
            }

            return matrix;
        }
        

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator[,] Map;
            int width = 0;
            int height = 0;
            int offsetX = 0;
            int offsetY = 0;
            ClTools.CreateGridBasedOnRangeValues(p_Model, out Map, out width, out height, out offsetX, out offsetY);

            double SumOfKernelMatrix = 0;
           
            double[,] Mask = GetGaussianMatrix(MaskSize, 2.0f, out SumOfKernelMatrix);

            int pointsFromCenter = (MaskSize / 2);
            for (int centX = 0; centX < width; centX++)
            {
                for (int centY = 0; centY < height; centY++)
                {
                    if (Map[centX, centY] == null)
                        continue;

                    int XBeginning = centX - pointsFromCenter;
                    int YBeginning = centY - pointsFromCenter;

                    double value = 0;
                    bool didBreak = false;
                    for (int xi = 0; xi < MaskSize; xi++)
                    {
                        for (int yi = 0; yi < MaskSize; yi++)
                        {
                            if (XBeginning + xi >= 0 && XBeginning + xi < width && YBeginning + yi >= 0 && YBeginning + yi < height &&
                                Map[XBeginning + xi, YBeginning + yi] != null)
                                value += Mask[xi, yi] * Map[XBeginning + xi, YBeginning + yi].Z;
                            else
                            {
                                Map[centX, centY].AlreadyVisited = true;
                              //  Map[centX, centY].Color = Color.Red;
                                didBreak = true;
                                break;
                            }
                        }
                        if (didBreak)
                            break;
                    }
                    if(!didBreak)
                        Map[centX, centY].Z = (float)(value / SumOfKernelMatrix);
                }
            }
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            do
            {
                if (iter.AlreadyVisited)
                    iter = p_Model.RemovePointFromModel(iter);
                else
                    iter.MoveToNext();
            } while (iter.IsValid());

        }
    }
}

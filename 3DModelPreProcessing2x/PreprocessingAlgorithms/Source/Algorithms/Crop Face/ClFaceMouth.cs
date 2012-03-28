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
*   @file       ClCropFaceBySphere.cs
*   @brief      Algorithm to crop face
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-06-2008
*
*   @history
*   @item		10-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClFaceMouth : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClFaceMouth();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Crop Face\Face Mouth Position";

        public ClFaceMouth() : base(ALGORITHM_NAME) { }

        
        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            
            Cl3DModel.Cl3DModelPointIterator NoseTip = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip);
            Cl3DModel.Cl3DModelPointIterator LeftEye = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner);
            Cl3DModel.Cl3DModelPointIterator RightEye = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner);

            float eyesDistance = LeftEye - RightEye;

            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();

            do
            {
                float NoseDistance = NoseTip - iter;
                float LeftEyeDistance = LeftEye - iter;
                float RightEyeDistance = RightEye - iter;

                if (NoseDistance > 40 && NoseDistance < 50 && LeftEyeDistance > 50 && LeftEyeDistance < 70 && RightEyeDistance > 50 && RightEyeDistance < 70)
                {
                    iter.Color = Color.Red;
                }
            } while (iter.MoveToNext());




            /*List<Cl3DModel.Cl3DModelPointIterator> neighbors;
            ClTools.GetNeighborhoodWithEuclideanDistanceCheckNeighborhood(out neighbors, MinPoint, 20);

            foreach (Cl3DModel.Cl3DModelPointIterator pt in neighbors)
                pt.Color = Color.Red;
            */

            /*
            iter = p_Model.GetIterator();
            minDistance = float.MaxValue;
            MinPoint = iter;
            do
            {
                float currDistance = (float)Math.Sqrt(Math.Pow(NoseTip - iter - 30, 2) + Math.Pow(LeftEye - iter - 64, 2) + Math.Pow(RightEye - iter - 64, 2));
                if (currDistance < minDistance)
                {
                    minDistance = currDistance;
                    MinPoint = iter.CopyIterator();
                }
            } while (iter.MoveToNext());

            ClTools.GetNeighborhoodWithEuclideanDistanceCheckNeighborhood(out neighbors, MinPoint, 20);

            foreach (Cl3DModel.Cl3DModelPointIterator pt in neighbors)
                pt.Color = Color.Green;

            */
            
            /*
            Cl3DModel.Cl3DModelPointIterator NoseTip = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip);
            Cl3DModel.Cl3DModelPointIterator LeftEye = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner);
            Cl3DModel.Cl3DModelPointIterator RightEye = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner);

            Cl3DModel.Cl3DModelPointIterator RightLip = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightCornerOfLips);
            Cl3DModel.Cl3DModelPointIterator UpperLip = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.UpperLip);
            Cl3DModel.Cl3DModelPointIterator LeftLip = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftCornerOfLips);
            Cl3DModel.Cl3DModelPointIterator BottomLip = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.BottomLip);

            TextWriter tw = new StreamWriter("d:\\Model.txt", true);

            String toWrite = (LeftEye - RightEye).ToString(System.Globalization.CultureInfo.InvariantCulture) + " ";
            toWrite += (RightLip - NoseTip).ToString(System.Globalization.CultureInfo.InvariantCulture) + " ";
            toWrite += (RightLip - LeftEye).ToString(System.Globalization.CultureInfo.InvariantCulture) + " ";
            toWrite += (RightLip - RightEye).ToString(System.Globalization.CultureInfo.InvariantCulture) + " ";
            
            toWrite += (UpperLip - NoseTip).ToString(System.Globalization.CultureInfo.InvariantCulture) + " ";
            toWrite += (UpperLip - LeftEye).ToString(System.Globalization.CultureInfo.InvariantCulture) + " ";
            toWrite += (UpperLip - RightEye).ToString(System.Globalization.CultureInfo.InvariantCulture) + " ";
          
            toWrite += (LeftLip - NoseTip).ToString(System.Globalization.CultureInfo.InvariantCulture) + " ";
            toWrite += (LeftLip - LeftEye).ToString(System.Globalization.CultureInfo.InvariantCulture) + " ";
            toWrite += (LeftLip - RightEye).ToString(System.Globalization.CultureInfo.InvariantCulture) + " ";

            toWrite += (BottomLip - NoseTip).ToString(System.Globalization.CultureInfo.InvariantCulture) + " ";
            toWrite += (BottomLip - LeftEye).ToString(System.Globalization.CultureInfo.InvariantCulture) + " ";
            toWrite += (BottomLip - RightEye).ToString(System.Globalization.CultureInfo.InvariantCulture);
            tw.WriteLine(toWrite);

            tw.Close();
            */
        }
    }
}

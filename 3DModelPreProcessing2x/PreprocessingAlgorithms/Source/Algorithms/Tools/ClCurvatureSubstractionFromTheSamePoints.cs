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
*   @file       ClCurvatureSubstractionFromTheSamePoints.cs
*   @brief      Algorithm to ClCurvatureSubstractionFromTheSamePoints
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-12-2008
*
*   @history
*   @item		10-12-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClCurvatureSubstractionFromTheSamePoints : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClCurvatureSubstractionFromTheSamePoints();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Tools\Curvature Substraction From The Same Points";

        public ClCurvatureSubstractionFromTheSamePoints() : base(ALGORITHM_NAME) { }

        // ------------------------- Properities

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            return list;
        }

        Cl3DModel NeutralModel = null;
        Cl3DModel ExpressionModel = null;
        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            if(p_Model.ModelFileName.Equals("Neutral"))
                NeutralModel = p_Model;
            else
                ExpressionModel = p_Model;

            if(NeutralModel == null || ExpressionModel == null)
                return;

            Cl3DModel.Cl3DModelPointIterator NeutralSavedNoseTip = null;
            Cl3DModel.Cl3DModelPointIterator NeutralLeftEye = null;
            Cl3DModel.Cl3DModelPointIterator NeutralRightEye = null;

            if(!NeutralModel.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip, ref NeutralSavedNoseTip) || !NeutralModel.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner, ref NeutralLeftEye) || !NeutralModel.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner, ref NeutralRightEye))
                throw new Exception("Cannot get all specific points (Neutral model)");

            Cl3DModel.Cl3DModelPointIterator ExpressionSavedNoseTip = null;
            Cl3DModel.Cl3DModelPointIterator ExpressionLeftEye = null;
            Cl3DModel.Cl3DModelPointIterator ExpressionRightEye = null;

            if (!ExpressionModel.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip, ref ExpressionSavedNoseTip) || !ExpressionModel.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner, ref ExpressionLeftEye) || !ExpressionModel.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner, ref ExpressionRightEye))
                throw new Exception("Cannot get all specific points (Exprssion model)");

            ClTools.CalculateGeodesicDistanceFromSourcePointToAllPoints(NeutralSavedNoseTip, Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToNoseTip.ToString());
            ClTools.CalculateGeodesicDistanceFromSourcePointToAllPoints(NeutralLeftEye, Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToLeftEye.ToString());
            ClTools.CalculateGeodesicDistanceFromSourcePointToAllPoints(NeutralRightEye, Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToRightEye.ToString());

            ClTools.CalculateGeodesicDistanceFromSourcePointToAllPoints(ExpressionSavedNoseTip, Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToNoseTip.ToString());
            ClTools.CalculateGeodesicDistanceFromSourcePointToAllPoints(ExpressionLeftEye, Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToLeftEye.ToString());
            ClTools.CalculateGeodesicDistanceFromSourcePointToAllPoints(ExpressionRightEye, Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToRightEye.ToString());

            Cl3DModel.Cl3DModelPointIterator iterNeutral = NeutralModel.GetIterator();
            do
            {
                // search for the closest point in distance geodesic distance domain
                Cl3DModel.Cl3DModelPointIterator iterExpression = ExpressionModel.GetIterator();
                Cl3DModel.Cl3DModelPointIterator ClossestPoint = null;
                double ClosestPointDistance = 0;
                bool first = true;
                double NoseDistanceNeutral = 0;
                double LeftEyeDistanceNeutral = 0;
                double RightEyeDistanceNeutral = 0;
                
                if(!iterNeutral.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToNoseTip, out NoseDistanceNeutral) || !iterNeutral.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToLeftEye, out LeftEyeDistanceNeutral) || !iterNeutral.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToRightEye, out RightEyeDistanceNeutral))
                    throw new Exception("Cannot get specific value");

                double NoseDistanceExpression;
                double LeftEyeDistanceExpression;
                double RightEyeDistanceExpression;

                do
                {    
                    if(!iterExpression.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToNoseTip, out NoseDistanceExpression) || !iterExpression.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToLeftEye, out LeftEyeDistanceExpression) || !iterExpression.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.GeodesicDistanceToRightEye, out RightEyeDistanceExpression))
                        throw new Exception("Cannot get specific value");

                    double newClosestPointDistance = Math.Sqrt(Math.Pow(NoseDistanceNeutral - NoseDistanceExpression, 2) + Math.Pow(LeftEyeDistanceNeutral - LeftEyeDistanceExpression, 2) + Math.Pow(RightEyeDistanceNeutral - RightEyeDistanceExpression, 2));
                    if (first)
                    {
                        ClossestPoint = iterExpression.CopyIterator();
                        ClosestPointDistance = newClosestPointDistance;
                        first = false;
                        continue;
                    }

                    if (ClosestPointDistance > newClosestPointDistance)
                    {
                        ClossestPoint = iterExpression.CopyIterator();
                        ClosestPointDistance = newClosestPointDistance;
                    }
                } while (iterExpression.MoveToNext());

                
                double ValNatural;
                double ValExpression;

                iterNeutral.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.Mean_25, out ValNatural);
                ClossestPoint.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.Mean_25, out ValExpression);
                iterNeutral.AddSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.DifferenceBetween_HCurvatures_25, ValNatural - ValExpression);

                iterNeutral.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.Gaussian_25, out ValNatural);
                ClossestPoint.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.Gaussian_25, out ValExpression);
                iterNeutral.AddSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.DifferenceBetween_KCurvatures_25, ValNatural - ValExpression);

                iterNeutral.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.K1_25, out ValNatural);
                ClossestPoint.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.K1_25, out ValExpression);
                iterNeutral.AddSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.DifferenceBetween_K1Curvatures_25, ValNatural - ValExpression);

                iterNeutral.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.K2_25, out ValNatural);
                ClossestPoint.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.K2_25, out ValExpression);
                iterNeutral.AddSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.DifferenceBetween_K2Curvatures_25, ValNatural - ValExpression);

                iterNeutral.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.ShapeIndex_25, out ValNatural);
                ClossestPoint.GetSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.ShapeIndex_25, out ValExpression);
                iterNeutral.AddSpecificValue(Cl3DModel.Cl3DModelPointIterator.eSpecificValues.DifferenceBetween_ShapeIndexCurvatures_25, ValNatural - ValExpression);

            } while (iterNeutral.MoveToNext());
        }
    }
}

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
*   @file       ClLoadModelCurvaturesValues.cs
*   @brief      ClLoadModelCurvaturesValues
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       08-01-2009
*
*   @history
*   @item		08-01-2009 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClCalculateDistanceOnUVFrom3Points : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClCalculateDistanceOnUVFrom3Points();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Calculations\Calculate UV distances from main points";

        public ClCalculateDistanceOnUVFrom3Points() : base(ALGORITHM_NAME) { }

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            return list;
        }


        private void CalculateDistancesFromPoint(string DistanceName, float U, float V, Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            do
            {
                double distance = Math.Sqrt(Math.Pow(U - iter.U, 2) + Math.Pow(V - iter.V, 2));
                iter.AddSpecificValue(DistanceName, distance);
            } while (iter.MoveToNext());
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator LeftEyeRightCornerPoint = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner);
            Cl3DModel.Cl3DModelPointIterator RightEyeLeftCornerPoint = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner);

            CalculateDistancesFromPoint("UV_LeftEyeRightCornerDistance", LeftEyeRightCornerPoint.U, LeftEyeRightCornerPoint.V, p_Model);
            CalculateDistancesFromPoint("UV_RightEyeLeftCornerDistance", RightEyeLeftCornerPoint.U, RightEyeLeftCornerPoint.V, p_Model);
            CalculateDistancesFromPoint("UV_NoseTipDistance", 0.0f, 0.0f, p_Model);

            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            do
            {
                double UV_LeftEyeRightCornerDistance = 0;
                iter.GetSpecificValue("UV_LeftEyeRightCornerDistance", out UV_LeftEyeRightCornerDistance);

                double UV_RightEyeLeftCornerDistance = 0;
                iter.GetSpecificValue("UV_RightEyeLeftCornerDistance", out UV_RightEyeLeftCornerDistance);

                double UV_NoseTipDistance = 0;
                iter.GetSpecificValue("UV_NoseTipDistance", out UV_NoseTipDistance);

                double alldistances = Math.Sqrt(Math.Pow(UV_LeftEyeRightCornerDistance, 2) + Math.Pow(UV_RightEyeLeftCornerDistance, 2) + Math.Pow(UV_NoseTipDistance, 2));
                iter.AddSpecificValue("AllDistancesUV", alldistances);
            } while (iter.MoveToNext());
        }
    }
}

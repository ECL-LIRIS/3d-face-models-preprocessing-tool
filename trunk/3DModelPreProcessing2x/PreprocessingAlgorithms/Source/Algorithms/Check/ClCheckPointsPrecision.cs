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
*   @file       ClCheckPointsPrecision.cs
*   @brief      ClCheckPointsPrecision
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       07-12-2010
*
*   @history
*   @item		16-03-2009 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClCheckPointsPrecision : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClCheckPointsPrecision();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Check\Check the main points precision on Bosphorus";

        public ClCheckPointsPrecision() : base(ALGORITHM_NAME) { }

        Dictionary<string, Dictionary<int, int[]>> Precision = new Dictionary<string, Dictionary<int, int[]>>();
        int TotalModels = 0;

        protected override void Algorithm(ref Cl3DModel p_Model)
        {

            Cl3DModel.Cl3DModelPointIterator ManualNoseTip = p_Model.GetSpecificPoint("NoseTip");
            Cl3DModel.Cl3DModelPointIterator ManualLeftEyeCorner = p_Model.GetSpecificPoint("LeftEyeRightCorner");
            Cl3DModel.Cl3DModelPointIterator ManualRightEyeCorner = p_Model.GetSpecificPoint("RightEyeLeftCorner");

            Cl3DModel.Cl3DModelPointIterator NoseTip = p_Model.GetSpecificPoint("AutomaticNoseTip");
            Cl3DModel.Cl3DModelPointIterator LeftEyeRightCorner = p_Model.GetSpecificPoint("AutomaticLeftEyeRightCorner");
            Cl3DModel.Cl3DModelPointIterator RightEyeLeftCorner = p_Model.GetSpecificPoint("AutomaticRightEyeLeftCorner");

            float NoseDistance = ManualNoseTip - NoseTip;
            float RightEyeDistance = ManualRightEyeCorner - RightEyeLeftCorner;
            float LeftEyeDistance = ManualLeftEyeCorner - LeftEyeRightCorner;
            TotalModels++;

            if (NoseDistance > 20 || RightEyeDistance > 20 || LeftEyeDistance > 20)
            {
                TextWriter rr = new StreamWriter("d:\\Incorrect.txt", true);
                rr.WriteLine(p_Model.ModelFilePath + " " + NoseDistance.ToString() + " " + RightEyeDistance.ToString() + " " + LeftEyeDistance.ToString());
                rr.Close();
            }

            Dictionary<int, int[]> Dict = null;
            if(!Precision.TryGetValue("NoseTip", out Dict))
            {
                Dict = new Dictionary<int, int[]>();
                Precision.Add("NoseTip", Dict);
            }
            int[] outVal = null;
            if (!Dict.TryGetValue((int)NoseDistance, out outVal))
            {
                outVal = new int[1];
                Dict.Add((int)NoseDistance, outVal);
            }
            outVal[0]++;

            if (!Precision.TryGetValue("LeftEye", out Dict))
            {
                Dict = new Dictionary<int, int[]>();
                Precision.Add("LeftEye", Dict);
            }
            if (!Dict.TryGetValue((int)LeftEyeDistance, out outVal))
            {
                outVal = new int[1];
                Dict.Add((int)LeftEyeDistance, outVal);
            }
            outVal[0]++;

            if (!Precision.TryGetValue("RightEye", out Dict))
            {
                Dict = new Dictionary<int, int[]>();
                Precision.Add("RightEye", Dict);
            }
            if (!Dict.TryGetValue((int)RightEyeDistance, out outVal))
            {
                outVal = new int[1];
                Dict.Add((int)RightEyeDistance, outVal);
            }
            outVal[0]++;

            TextWriter tw = new StreamWriter("d:\\PointsPrecision.txt", false);
            foreach(KeyValuePair<string, Dictionary<int, int[]>> prc in Precision)
            {
                tw.WriteLine(prc.Key);
                string Line1 = "";
                string Line2 = "";
                foreach (KeyValuePair<int, int[]> dd in prc.Value)
                {
                    Line1 += " " + dd.Key.ToString();
                    Line2 += " " + dd.Value[0].ToString();
                }
                tw.WriteLine(Line1);
                tw.WriteLine(Line2);
            }
            tw.Close();
           
        }
    }
}

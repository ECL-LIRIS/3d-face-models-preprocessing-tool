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
*   @file       ClColorMainPointsNeighborhood.cs
*   @brief      Show ClColorMainPointsNeighborhood
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       27-03-2009
*
*   @history
*   @item		27-03-2009 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClColorModelNormalVectors : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClColorModelNormalVectors();
        }

        public static string ALGORITHM_NAME = @"Show\Color Normal Vectors";

        public ClColorModelNormalVectors() : base(ALGORITHM_NAME) { }

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            do
            {
                if (iter.NormalVector != null)
                {
                    iter.Color = Color.FromArgb((int)Math.Abs((iter.NormalVector[0]) * 255),
                                                (int)Math.Abs((iter.NormalVector[1]) * 255),
                                                (int)Math.Abs((iter.NormalVector[2]) * 255));
                }
            } while (iter.MoveToNext());
        }
    }
}

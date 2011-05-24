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
*   @file       ClFindNoseTipMaxVal.cs
*   @brief      Algorithm to find nose tip
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-06-2008
*
*   @history
*   @item		10-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClFindNoseTipMaxVal : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClFindNoseTipMaxVal();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Antropometry Points\Nose Tip (Max Z val.)";

        public ClFindNoseTipMaxVal() : base(ALGORITHM_NAME) { }


        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator maxIter = p_Model.GetIterator();
            Cl3DModel.Cl3DModelPointIterator iter = maxIter.CopyIterator();

            while (iter.MoveToNext())
            {
                if (maxIter.Z < iter.Z)
                    maxIter = iter.CopyIterator();
            }

            if (maxIter.IsValid())
            {
                p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip.ToString(), maxIter);
            }
        }
    }
}

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
*   @file       ClTestLandmarks.cs
*   @brief      Test Landmarks
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       18-10-2008
*
*   @history
*   @item		18-10-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClRemoveAllLandmarks : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClRemoveAllLandmarks();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Tools\Remove All Landmarks";

        public ClRemoveAllLandmarks() : base(ALGORITHM_NAME) { }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            p_Model.RemoveAllSpecificPoints();
        }
    }
}

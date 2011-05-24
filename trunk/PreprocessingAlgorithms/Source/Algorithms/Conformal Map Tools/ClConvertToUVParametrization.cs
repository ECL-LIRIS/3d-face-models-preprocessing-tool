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
    public class ClConvertToUVParametrization : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClConvertToUVParametrization();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Conformal Map Tools\Convert to UV parameters";

        public ClConvertToUVParametrization() : base(ALGORITHM_NAME) { }

   //     public override void SetProperitis(string p_sProperity, string p_sValue)
   //     {
   //     }

   //     public override List<KeyValuePair<string, string>> GetProperitis()
   //     {
   //     }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();

            do
            {
                iter.X = iter.U;
                iter.Y = iter.V;
                iter.Z = 0;
            } while (iter.MoveToNext());
        }
    }
}

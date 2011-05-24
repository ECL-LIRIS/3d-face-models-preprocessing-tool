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
*   @file       ClColorModelDifference.cs
*   @brief      ClColorModelDifference
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       16-12-2008
*
*   @history
*   @item		16-12-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClAddExpressionFRGC : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClAddExpressionFRGC();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Tools\Add Expression FRGC";

         

         public ClAddExpressionFRGC()
             : base(ALGORITHM_NAME) 
        {
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            string fileName = ClTools.ExtractOryginalFileNameFRGC(p_Model.ModelFileName);

            string expression = "";
            if (!ClTools.ClExpressionFRGC.GetExpression(fileName, out expression))
                throw new Exception("Cannot find Expression for file " + p_Model.ModelFilePath);

            p_Model.ModelExpression = expression;

            ClInformationSender.SendInformation("Model has expression: " + p_Model.ModelExpression, ClInformationSender.eInformationType.eDebugText);
        }
    }
}

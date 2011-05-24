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
*   @file       ClSaveSpecificPointsCoordinates.cs
*   @brief      ClSaveSpecificPointsCoordinates
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       18-10-2008
*
*   @history
*   @item		18-10-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClSaveSpecificPointsCoordinates : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClSaveSpecificPointsCoordinates();
        }

        public static string ALGORITHM_NAME = @"Save\Specific points\Save Specific Points Coordinates (.crr)";

        public ClSaveSpecificPointsCoordinates() : base(ALGORITHM_NAME) { }

        public ClSaveSpecificPointsCoordinates(string p_sFilePostFix)
            : base(ALGORITHM_NAME) 
        {
            m_sFilePostFix = p_sFilePostFix;
        }

        string m_sFilePostFix = "";


        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            string name = p_Model.ModelFileFolder + p_Model.ModelFileName + m_sFilePostFix+ ".crr";

            List<KeyValuePair<string,Cl3DModel.Cl3DModelPointIterator>> specPoints = p_Model.GetAllSpecificPoints();
            using (TextWriter tw = new StreamWriter(name, false))
            {
                tw.WriteLine("@----------------------------------------");
                tw.WriteLine("@      Przemyslaw Szeptycki LIRIS 2008");
                tw.WriteLine("@ Automatic Model Specific Points Coordinates");
                tw.WriteLine("@   Model name: " + p_Model.ModelFileName);
                tw.WriteLine("@----------------------------------------");
                foreach(KeyValuePair<string,Cl3DModel.Cl3DModelPointIterator> point in specPoints)
                {
                    string line = point.Key + " X: " + point.Value.X.ToString(System.Globalization.CultureInfo.InvariantCulture) + " Y: " + point.Value.Y.ToString(System.Globalization.CultureInfo.InvariantCulture) + " Z: " + point.Value.Z.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    tw.WriteLine(line);               
                }
                tw.Close();
            }
        }
    }
}


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
*   @file       ClSaveModelCurvaturesValues.cs
*   @brief      ClSaveModelCurvaturesValues
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       07-01-2009
*
*   @history
*   @item		07-01-2009 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClSaveModelCurvaturesValues : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClSaveModelCurvaturesValues();
        }

        public static string ALGORITHM_NAME = @"Save\Different\Save Curvatures Values (.curv)";

        private string m_sFilePostFix = "";

        public ClSaveModelCurvaturesValues() : base(ALGORITHM_NAME) { }
        public ClSaveModelCurvaturesValues(string p_sFilePostFix)
            : base(ALGORITHM_NAME) 
        {
            m_sFilePostFix = p_sFilePostFix;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            string name = p_Model.ModelFileFolder + p_Model.ModelFileName + ".curv";
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            if (!iter.IsValid())
                throw new Exception("Iterator in the model is not valid");

            using (TextWriter tw = new StreamWriter(name, false))
            {
                tw.WriteLine("@----------------------------------------");
                tw.WriteLine("@     Przemyslaw Szeptycki LIRIS 2008");
                tw.WriteLine("@          Face model curvatures");
                tw.WriteLine("@  Model name: " + p_Model.ModelFileName);
                tw.WriteLine("@----------------------------------------");
                tw.WriteLine("@ (PointID) X Y Z");
                tw.WriteLine("@ \tCurvatureName: value");
                do
                {
                    List<String> SpecValList = iter.GetListOfSpecificValues();
                    string line = "(" + iter.PointID.ToString(System.Globalization.CultureInfo.InvariantCulture) + ")" + iter.X.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " + iter.Y.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " + iter.Z.ToString(System.Globalization.CultureInfo.InvariantCulture) + "\n";
                    foreach (String SPV in SpecValList)
                    {
                        double val;
                        iter.GetSpecificValue(SPV, out val);
                        line += "\t" + SPV + ": " + val.ToString(System.Globalization.CultureInfo.InvariantCulture) + "\n";
                    }
                    tw.WriteLine(line);

                } while (iter.MoveToNext());

                tw.Close();
            }
        }
    }
}


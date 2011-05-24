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
*   @file       ClSaveModel.cs
*   @brief      Test Landmarks
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       03-12-2008
*
*   @history
*   @item		03-12-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClSaveModel : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClSaveModel();
        }

        public static string ALGORITHM_NAME = @"Save\Model\Save Model (.binaryModel || .model)";

        private string m_sFilePostFix = "";
        private bool m_bBinaryMode = true;

        public ClSaveModel() : base(ALGORITHM_NAME) { }
        public ClSaveModel(string p_sFilePostFix)
            : base(ALGORITHM_NAME) 
        {
            m_sFilePostFix = p_sFilePostFix;
        }

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Binary mode"))
            {
                m_bBinaryMode = bool.Parse(p_sValue);
            }
            else if (p_sProperity.Equals("File Post Fix"))
            {
                m_sFilePostFix = p_sValue;
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Binary mode", m_bBinaryMode.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("File Post Fix", m_sFilePostFix.ToString()));

            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            string name = p_Model.ModelFileFolder + p_Model.ModelFileName + m_sFilePostFix;


            if (m_bBinaryMode)
            {
                p_Model.SaveModel(name);
            }
            else
            {
                name += ".model";
                Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
                if (!iter.IsValid())
                    throw new Exception("Iterator in the model is not valid");

                using (TextWriter tw = new StreamWriter(name, false))
                {
                    tw.WriteLine("@----------------------------------------");
                    tw.WriteLine("@     Przemyslaw Szeptycki LIRIS 2008");
                    tw.WriteLine("@                Face model");
                    tw.WriteLine("@  Model name: " + p_Model.ModelFileName);
                    tw.WriteLine("@----------------------------------------");
                    tw.WriteLine("@ PointID X Y Z (TextureX TextureY) (Neighbors PointID)");
                    do
                    {
                        string line = iter.PointID.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " + iter.X.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " + iter.Y.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " + iter.Z.ToString(System.Globalization.CultureInfo.InvariantCulture) + " ( " + iter.RangeImageX.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " + iter.RangeImageY.ToString(System.Globalization.CultureInfo.InvariantCulture) + " ) ( ";
                        List<Cl3DModel.Cl3DModelPointIterator> neighbors = iter.GetListOfNeighbors();
                        foreach (Cl3DModel.Cl3DModelPointIterator neighbor in neighbors)
                            line += neighbor.PointID.ToString(System.Globalization.CultureInfo.InvariantCulture) + " ";
                        line += ")";

                        tw.WriteLine(line);

                    } while (iter.MoveToNext());
                    
                    int nop = p_Model.GetAllSpecificPoints().Count;
                    if(nop != 0)
                    {
                        tw.WriteLine("Landmark points (ptID): "+nop.ToString());
                        foreach (KeyValuePair<string, Cl3DModel.Cl3DModelPointIterator> specificPoint in p_Model.GetAllSpecificPoints())
                        {
                            tw.WriteLine(specificPoint.Key + " " + specificPoint.Value.PointID.ToString());
                        }
                    }

                    tw.Close();
                }
            }
        }
    }
}

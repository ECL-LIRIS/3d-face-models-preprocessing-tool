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
    public class ClLoadPtsWithId : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClLoadPtsWithId();
        }

        public static string ALGORITHM_NAME = @"Load\Load Pts based on pt ID (*.pts - conformal model)";

        public ClLoadPtsWithId() : base(ALGORITHM_NAME) { }

        private string m_sFilePostFix = "";
        private bool IDS = false;
        private int m_CharsToSubstract = 0;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("PostFix"))
            {
                m_sFilePostFix = p_sValue;
            }
            else if (p_sProperity.Equals("ID's only"))
            {
                IDS = bool.Parse(p_sValue);
            }
            else if (p_sProperity.Equals("Chars To Substract"))
            {
                m_CharsToSubstract = Int32.Parse(p_sValue);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("PostFix", m_sFilePostFix.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("ID's only", IDS.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Chars To Substract", m_CharsToSubstract.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            string name = p_Model.ModelFileName;
           
            if(m_CharsToSubstract != 0)
                name = name.Substring(0, name.Length - m_CharsToSubstract);

            if (!IDS)
            {
                string fileName = p_Model.ModelFileFolder + name + m_sFilePostFix + ".pts";
                Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
                using (StreamReader FileStream = File.OpenText(fileName))
                {
                    string line = "";
                    while ((line = FileStream.ReadLine()) != null)
                    {
                        if (line.Length == 0)
                            continue;

                        if (line[0].Equals('@'))
                            continue;

                        string[] splited = line.Split(' ');

                        Cl3DModel.eSpecificPoints type = Cl3DModel.eSpecificPoints.LeftEyeRightCorner;

                        if (splited[0].Equals("LeftEyeRightCorner"))
                            type = Cl3DModel.eSpecificPoints.LeftEyeRightCorner;
                        else if (splited[0].Equals("RightEyeLeftCorner"))
                            type = Cl3DModel.eSpecificPoints.RightEyeLeftCorner;
                        else if (splited[0].Equals("NoseTip"))
                            type = Cl3DModel.eSpecificPoints.NoseTip;
                        else
                            continue;

                        uint ID = UInt32.Parse(splited[1]);
                        if (!iter.MoveToPoint(ID))
                            throw new Exception("Cannot find point with ID: " + splited[1]);

                        p_Model.AddSpecificPoint(type, iter);
                    }
                }
            }
            else
            {
                string fileName = p_Model.ModelFileFolder + name + m_sFilePostFix + ".ptsID";
                Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
                using (StreamReader FileStream = File.OpenText(fileName))
                {
                    string line = "";
                    int count = 0;
                    uint NoOfPoints = 0;
                    bool ReadHeader = false;
                    while ((line = FileStream.ReadLine()) != null)
                    {
                        if (!ReadHeader)
                        {
                            NoOfPoints = UInt32.Parse(line);
                            if (NoOfPoints != 3)
                                throw new Exception("Method ready to read 3 points");
                            ReadHeader = true;
                        }
                        else
                        {
                            uint id = UInt32.Parse(line);
                            iter.MoveToPoint(id);
                            if (count == 0)
                                p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip, iter);
                            if (count == 1)
                                p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner, iter);
                            if (count == 2)
                                p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner, iter);

                            count++;
                        }
                    }
                }
            }
        }
    }
}

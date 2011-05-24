using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using Iridium.Numerics.LinearAlgebra;

using PreprocessingFramework;
/**************************************************************************
*
*                          ModelPreProcessing
*
* Copyright (C)         Przemyslaw Szeptycki 2007     All Rights reserved
*
***************************************************************************/

/**
*   @file       ClCalculateNormalVectors.cs
*   @brief      Algorithm to ClCalculateNormalVectors
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-12-2008
*
*   @history
*   @item		10-12-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClCalculateUVExternalApp : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClCalculateUVExternalApp();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Calculations\Calculate UV External App";

        public ClCalculateUVExternalApp() : base(ALGORITHM_NAME) { }

        // ------------------------- Properities
        private string m_sPath = "d:\\Mapping Apps\\Harmonic Mapper\\bin\\HarmonicMap.exe";
        private bool removeOutput = true;
        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("App path"))
            {
                m_sPath = p_sValue;
            }
            if (p_sProperity.Equals("removeOutput"))
            {
                removeOutput = bool.Parse(p_sValue);
            } 
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("App path", m_sPath.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("removeOutput", removeOutput.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            try
            {
                if (!File.Exists(m_sPath))
                    throw new Exception("External file does not exist, the path is incorrect: " + m_sPath);

                ClSaveToMFileNew algSave = new ClSaveToMFileNew(p_Model.ModelFileFolder + p_Model.ModelFileName + ".m2");
                algSave.MakeAlgorithm(p_Model);

                Process proc = new Process();
                proc.StartInfo.FileName = m_sPath;
                proc.StartInfo.Arguments = "-harmonic_map \"" + p_Model.ModelFileFolder + p_Model.ModelFileName + ".m2\" \"" + p_Model.ModelFileFolder + p_Model.ModelFileName + "OUT.m\"";
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc.Start();
                
                if (!proc.WaitForExit(10000))
                {
                    proc.Kill();
                    File.Delete(p_Model.ModelFileFolder + p_Model.ModelFileName + ".m2");
                }

                ClInformationSender.SendInformation("Parametrization calculated", ClInformationSender.eInformationType.eDebugText);

                Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
                using (StreamReader FileStream = File.OpenText(p_Model.ModelFileFolder + p_Model.ModelFileName + "OUT.m"))
                {
                    string line = "";
                    while ((line = FileStream.ReadLine()) != null)
                    {
                        if (!line.Contains("Vertex"))
                            continue;
                        int index = line.LastIndexOf("uv=") + 3;
                        string substring = line.Substring(index);
                        string[] subsubs = substring.Substring(substring.IndexOf('(') + 1, substring.LastIndexOf(')') - substring.IndexOf('(') - 1).Split(' ');
                        float U = float.Parse(subsubs[0], System.Globalization.CultureInfo.InvariantCulture);
                        float V = float.Parse(subsubs[1], System.Globalization.CultureInfo.InvariantCulture);

                        uint ID = uint.Parse(line.Split(' ')[1]);
                        if (!iter.MoveToPoint(ID - 1))
                            throw new Exception("Cannot localize point with number: " + ID.ToString());

                        iter.U = U;
                        iter.V = V;
                    }
                }
                File.Delete(p_Model.ModelFileFolder + p_Model.ModelFileName + ".m2");
                if(removeOutput)
                    File.Delete(p_Model.ModelFileFolder + p_Model.ModelFileName + "OUT.m");
            }
            catch (Exception)
            {
                File.Delete(p_Model.ModelFileFolder + p_Model.ModelFileName + ".m2");
                File.Delete(p_Model.ModelFileFolder + p_Model.ModelFileName + "OUT.m");
                throw;
            }
        }
    }
}

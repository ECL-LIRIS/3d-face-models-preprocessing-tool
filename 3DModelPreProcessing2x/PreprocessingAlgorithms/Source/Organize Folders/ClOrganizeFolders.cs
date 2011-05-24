using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using Iridium.Numerics;

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
    public class ClOrganizeFolders : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClOrganizeFolders();
        }

        public static string ALGORITHM_NAME = @"Organize folders\Organize files to folders";

        public ClOrganizeFolders() : base(ALGORITHM_NAME) { }

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
          /*  if (p_sProperity.Equals("Theta"))
            {
                m_Theta = Double.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else*/
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            //list.Add(new KeyValuePair<string, string>("Theta", m_Theta.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            string[] nameAndID = p_Model.ModelFileName.Split('d');
            if (!Directory.Exists(p_Model.ModelFileFolder + nameAndID[0]))
                Directory.CreateDirectory(p_Model.ModelFileFolder + nameAndID[0]);

            File.Copy(p_Model.ModelFilePath, p_Model.ModelFileFolder + nameAndID[0] + "\\" + p_Model.ModelFileName + "." + p_Model.ModelType);
        }
    }
}
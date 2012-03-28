/*  Copyright (C) 2011 Przemyslaw Szeptycki <pszeptycki@gmail.com>, Ecole Centrale de Lyon,

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
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
    public class ClLoadModelCurvaturesValues : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClLoadModelCurvaturesValues();
        }

        public static string ALGORITHM_NAME = @"Load\Load Model Curvatures Values (*.curv)";

        public ClLoadModelCurvaturesValues() : base(ALGORITHM_NAME) { }

        bool m_bMFile = false;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Is it M file"))
            {
                m_bMFile = bool.Parse(p_sValue);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Is it M file", m_bMFile.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            string name = p_Model.ModelFileName;
            if(name.IndexOf('.') != -1)
                name = name.Substring(0,name.IndexOf('.'));

            m_bMFile = p_Model.ModelType.Equals("m");

            string fileName = p_Model.ModelFileFolder + name + ".curv";
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            using (StreamReader FileStream = File.OpenText(fileName))
            {
                string line;
                uint pointNo;
                bool skipThisPoint = false;
                while ((line = FileStream.ReadLine()) != null)
                {
                    if(line.Length == 0)
                        continue;
                    
                    if (line[0].Equals('@'))
                        continue;

                    if (line[0].Equals('('))
                    {
                        int startNo = line.IndexOf('(');
                        int endNo = line.IndexOf(')');
                        string NO = line.Substring(startNo+1, endNo - startNo-1);
                        pointNo = UInt32.Parse(NO, System.Globalization.CultureInfo.InvariantCulture);
                        if (m_bMFile)
                            pointNo++;

                        if (iter.MoveToPoint(pointNo))
                        {
                            skipThisPoint = false;
                            continue;
                        }
                        else
                            skipThisPoint = true;
                    }
                    if (line[0].Equals('\t') && skipThisPoint == false)
                    {
                        string[] splited = line.Split(':');
                        string curvatureName = splited[0].Substring(1);
                        double curvatureVal = Double.Parse(splited[1], System.Globalization.CultureInfo.InvariantCulture);

                        iter.AddSpecificValue(curvatureName, curvatureVal);
                    }
                }
            }
        }
    }
}


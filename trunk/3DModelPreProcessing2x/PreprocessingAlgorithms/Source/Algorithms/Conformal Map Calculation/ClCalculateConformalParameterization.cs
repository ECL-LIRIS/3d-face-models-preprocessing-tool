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
*   @file       ClCalculateConformalParameterization.cs
*   @brief      Algorithm to correct face pose based on eyes
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       04-09-2008
*
*   @history
*   @item		04-09-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClCalculateConformalParameterization : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClCalculateConformalParameterization();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Conformal Map Calculation\Spectral Conformal Parameterization (External App.)";

        public ClCalculateConformalParameterization() : base(ALGORITHM_NAME) { }

        private string m_sRunApplication =  AppDomain.CurrentDomain.BaseDirectory+"SpectralConformalParam.exe";
        private const string paramFileName = "paramModel";
        private bool m_bAreaWeight = false;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Run Application"))
            {
                m_sRunApplication = p_sValue;
            }
            else if (p_sProperity.Equals("Area Weight"))
            {
                m_bAreaWeight = Boolean.Parse(p_sValue);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Run Application", m_sRunApplication.ToString()));
            list.Add(new KeyValuePair<string, string>("Area Weight", m_bAreaWeight.ToString()));
            return list;
        }

        void SaveModel(Cl3DModel p_Model)
        {
            string name = p_Model.ModelFileFolder + paramFileName+".tmp";
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();

            using (TextWriter tw = new StreamWriter(name, false))
            {
                uint vertexNO = 1;
                Dictionary<uint, uint> MapVertexNo = new Dictionary<uint, uint>();
                do
                {
                    string line = "v " + iter.X.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                                    " " + iter.Y.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                                    " " + iter.Z.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                                    " " + iter.PointID.ToString(System.Globalization.CultureInfo.InvariantCulture);

                    tw.WriteLine(line);

                    MapVertexNo.Add(iter.PointID, vertexNO++);

                } while (iter.MoveToNext());

                // --- create faces
                iter = p_Model.GetIterator();
                do
                {
                    iter.AlreadyVisited = true;

                    List<Cl3DModel.Cl3DModelPointIterator> neighbors = iter.GetListOfNeighbors();

                    uint MainPointID;
                    if (!MapVertexNo.TryGetValue(iter.PointID, out MainPointID))
                        throw new Exception("Cannot find point ID in the ID dictionary: " + iter.PointID);

                    uint point1 = 0;
                    uint point2 = 0;
                    uint point3 = 0;

                    foreach (Cl3DModel.Cl3DModelPointIterator point in neighbors)
                    {
                        //if (point.AlreadyVisited)
                        //    continue;

                        if (point.RangeImageY == iter.RangeImageY && point.RangeImageX > iter.RangeImageX) // first point
                        {
                            if (!MapVertexNo.TryGetValue(point.PointID, out point1))
                                throw new Exception("Cannot find point ID in the ID dictionary: " + iter.PointID);
                        }
                        if (point.RangeImageY > iter.RangeImageY && point.RangeImageX > iter.RangeImageX) // second point
                        {
                            if (!MapVertexNo.TryGetValue(point.PointID, out point2))
                                throw new Exception("Cannot find point ID in the ID dictionary: " + iter.PointID);
                        }
                        if (point.RangeImageY > iter.RangeImageY && point.RangeImageX == iter.RangeImageX) // third point
                        {
                            if (!MapVertexNo.TryGetValue(point.PointID, out point3))
                                throw new Exception("Cannot find point ID in the ID dictionary: " + iter.PointID);
                        }
                    }

                    string line = "";
                    if (point1 != 0 && point2 != 0)
                    {
                        line += "f " + MainPointID.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point2.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point1.ToString(System.Globalization.CultureInfo.InvariantCulture) + "\n";
                        if (point3 != 0)
                        {
                            line += "f " + MainPointID.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point3.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point2.ToString(System.Globalization.CultureInfo.InvariantCulture) + "\n";
                        }
                    }
                    else if (point1 != 0 && point3 != 0)
                    {
                        line += "f " + MainPointID.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point3.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point1.ToString(System.Globalization.CultureInfo.InvariantCulture) + "\n";
                    }
                    else if (point2 != 0 && point3 != 0)
                    {
                        line += "f " + MainPointID.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point3.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point2.ToString(System.Globalization.CultureInfo.InvariantCulture) + "\n";
                    }

                    tw.Write(line);

                } while (iter.MoveToNext());
                tw.Close();
            }
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            SaveModel(p_Model);

            string ParameterString = "\"" + p_Model.ModelFileFolder+paramFileName + ".tmp\" " + "0";
            if(m_bAreaWeight)
            {
                ParameterString = "\"" + p_Model.ModelFileFolder + paramFileName + ".tmp\" " + "1";
            }

            ProcessStartInfo proces = new ProcessStartInfo(m_sRunApplication, ParameterString);
            proces.WindowStyle = ProcessWindowStyle.Minimized;

            Process ConformalProcess = System.Diagnostics.Process.Start(proces);

            ConformalProcess.WaitForExit();

            string FileName = p_Model.ModelFileFolder + paramFileName + "_AreaWeight.UV";
            if(m_bAreaWeight)
            {
                FileName = p_Model.ModelFileFolder + paramFileName + "_NoAreaWeight.UV";
            }
                

            if (!File.Exists(FileName))
            {
                throw new Exception("Something goes wrong with calculation of UV parametrization for the file:" + p_Model.ModelFileName);
            }

            using (StreamReader FileStream = File.OpenText(FileName))
            {
                Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
                string line = "";
                while ((line = FileStream.ReadLine()) != null)
                {
                    if (line.Length == 0)
                        continue;

                    // UV verID=41496 U=0.0854425 V=0.224188
                    char[] delimiters = new char[] { ' ', '=' };
                    string[] parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    uint ID = UInt32.Parse(parts[2], System.Globalization.CultureInfo.InvariantCulture);
                    float U = Single.Parse(parts[4], System.Globalization.CultureInfo.InvariantCulture);
                    float V = Single.Parse(parts[6], System.Globalization.CultureInfo.InvariantCulture);
                    if (!iter.MoveToPoint(ID))
                        throw new Exception("Cannot move iterator to the position: " + ID.ToString());

                    iter.U = U;
                    iter.V = V;
                }
                FileStream.Close();
            }
            File.Delete(FileName);
            File.Delete(p_Model.ModelFileFolder + paramFileName + ".tmp");
        }
    }
}

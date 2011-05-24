﻿using System;
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
*   @file       ClSaveModelToObj.cs
*   @brief      ClSaveModelToObj
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       03-12-2008
*
*   @history
*   @item		03-12-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClSaveModelToMFile : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClSaveModelToMFile();
        }

        public static string ALGORITHM_NAME = @"Save\Model\Save Model (.m)";

        private string m_sFilePostFix = "";

        public ClSaveModelToMFile() : base(ALGORITHM_NAME) { }
        public ClSaveModelToMFile(string p_sFilePostFix)
            : base(ALGORITHM_NAME) 
        {
            m_sFilePostFix = p_sFilePostFix;
        }


        protected override void Algorithm(ref Cl3DModel p_Model)
        {
           // if (p_Model.ModelType != "abs")
             //   throw new Exception("Saveing to .obj files works only for ABS files");

            string name = p_Model.ModelFileFolder + p_Model.ModelFileName + ".m";
            string namePoints = p_Model.ModelFileFolder + p_Model.ModelFileName + ".m";
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            if (!iter.IsValid())
                throw new Exception("Iterator in the model is not valid");

            using (TextWriter tw = new StreamWriter(name, false))
            {
                do
                {
                    uint ID = iter.PointID + 1;

                    int R = 0;
                    R = (R << 8) + (int)iter.Color.R;
                    int G = 0;
                    G = (G << 8) + (int)iter.Color.G;
                    int B = 0;
                    B = (B << 8) + (int)iter.Color.B;
                    
                    string line = "Vertex " + ID.ToString(System.Globalization.CultureInfo.InvariantCulture) + "  "
                                            + iter.X.ToString(System.Globalization.CultureInfo.InvariantCulture) + " "
                                            + iter.Y.ToString(System.Globalization.CultureInfo.InvariantCulture) + " "
                                            + iter.Z.ToString(System.Globalization.CultureInfo.InvariantCulture) + " {rgb=("
                                            + ((float)R / 255.0f).ToString(System.Globalization.CultureInfo.InvariantCulture) + " "
                                            + ((float)G / 255.0f).ToString(System.Globalization.CultureInfo.InvariantCulture) + " "
                                            + ((float)B / 255.0f).ToString(System.Globalization.CultureInfo.InvariantCulture) + ")}";

                    tw.WriteLine(line);

                } while (iter.MoveToNext());

                // --- create faces
                iter = p_Model.GetIterator();
                uint FaceNO = 1;
                do
                {
                    iter.AlreadyVisited = true;
                    
                    List<Cl3DModel.Cl3DModelPointIterator> neighbors = iter.GetListOfNeighbors();

                    uint MainPointID = iter.PointID+1;

                    uint point1 = 0;
                    bool point1Founded = false;
                    uint point2 = 0;
                    bool point2Founded = false;
                    uint point3 = 0;
                    bool point3Founded = false;
                    uint point4Exc = 0;
                    bool point4ExcFounded = false;
                    uint point5Exc = 0;
                    bool point5ExcFounded = false;
                    uint point6Exc = 0;
                    bool point6ExcFounded = false;
                    
                    foreach (Cl3DModel.Cl3DModelPointIterator point in neighbors)
                    {
                        if (point.RangeImageY == iter.RangeImageY && point.RangeImageX > iter.RangeImageX) // first point
                        {
                            point1 = point.PointID+1;
                            point1Founded = true;
                        }
                        if (point.RangeImageY > iter.RangeImageY && point.RangeImageX > iter.RangeImageX) // second point
                        {
                            point2 = point.PointID+1;
                            point2Founded = true;
                        }
                        if (point.RangeImageY > iter.RangeImageY && point.RangeImageX == iter.RangeImageX) // third point
                        {
                            point3 = point.PointID+1;
                            point3Founded = true;
                        }
                        if (point.RangeImageY < iter.RangeImageY && point.RangeImageX > iter.RangeImageX) // second point
                        {
                            point4Exc = point.PointID + 1;
                            point4ExcFounded = true;
                        }
                        if (point.RangeImageY > iter.RangeImageY && point.RangeImageX < iter.RangeImageX) // second point
                        {
                            point5Exc = point.PointID + 1;
                            point5ExcFounded = true;
                        }
                        if (point.RangeImageY < iter.RangeImageY && point.RangeImageX == iter.RangeImageX) // second point
                        {
                            point6Exc = point.PointID + 1;
                            point6ExcFounded = true;
                        }
                    }

                    string line = "";
                    if (point1Founded && point2Founded)
                    {
                        line += "Face " + FaceNO.ToString() + "  " + MainPointID.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point2.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point1.ToString(System.Globalization.CultureInfo.InvariantCulture) + "\n";
                        FaceNO++;
                        if (point3Founded)
                        {
                            line += "Face " + FaceNO.ToString() + "  " + MainPointID.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point3.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point2.ToString(System.Globalization.CultureInfo.InvariantCulture)+"\n";
                            FaceNO++;
                        }
                        if (!point6ExcFounded && point4ExcFounded)
                        {
                            line += "Face " + FaceNO.ToString() + "  " + MainPointID.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point1.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point4Exc.ToString(System.Globalization.CultureInfo.InvariantCulture) + "\n";
                            FaceNO++;
                        }
                        
                    }
                    else if (point1Founded && point3Founded)
                    {
                        line += "Face " + FaceNO.ToString() + "  " + MainPointID.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point3.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point1.ToString(System.Globalization.CultureInfo.InvariantCulture) +"\n";
                        FaceNO++;
                    }
                    else if (point2Founded && point3Founded)
                    {
                        line += "Face " + FaceNO.ToString() + "  " + MainPointID.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point3.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point2.ToString(System.Globalization.CultureInfo.InvariantCulture) +"\n";
                        FaceNO++;
                        
                    }
                    /*else if (point1Founded && !point2Founded && !point3Founded && point4ExcFounded)
                    {
                        line += "Face " + FaceNO.ToString() + "  " + MainPointID.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point4Exc.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point1.ToString(System.Globalization.CultureInfo.InvariantCulture) + "\n";
                        FaceNO++;

                    }
                     */
                    /*else if (!point1Founded && !point2Founded && point3Founded && point5ExcFounded)
                    {
                        line += "Face " + FaceNO.ToString() + "  " + MainPointID.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point3.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                                        point5Exc.ToString(System.Globalization.CultureInfo.InvariantCulture) + "\n";
                        FaceNO++;

                    }
                */
                    tw.Write(line);

                } while (iter.MoveToNext());
                tw.Close();
            }
        }
    }
}

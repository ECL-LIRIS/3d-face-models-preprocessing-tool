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
*   @file       ClLoadAutomaticSpecificPoints.cs
*   @brief      ClLoadAutomaticSpecificPoints
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-04-2009
*
*   @history
*   @item		10-04-2009 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClLoadAutomaticSpecificPoints : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClLoadAutomaticSpecificPoints();
        }

        public static string ALGORITHM_NAME = @"Load\Load Automatic Specific Points (*.crr)";

        public ClLoadAutomaticSpecificPoints() : base(ALGORITHM_NAME) { }

        private class Point3D
        {
            public Point3D(double X, double Y, double Z)
            {
                this.X = X;
                this.Y = Y;
                this.Z = Z;
            }
            public double X;
            public double Y;
            public double Z;

            public static double operator -(Point3D point1, Point3D point2)
            {
                double diff = Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2) + Math.Pow(point1.Z - point2.Z, 2));
                return diff;
            }
        }
        private class CRRFile
        {
            public Point3D LeftCornerOfLeftEye;
            public Point3D LeftEyeRightCorner;
            public Point3D LeftCornerOfRightEye;
            public Point3D RightCornerOfRightEye;
            public Point3D LeftCornerOfNose;
            public Point3D NostTip;
            public Point3D RightCornerOfNose;
            public Point3D LeftCornerOfLips;
            public Point3D RightCornerOfLips;
            public string FilePath = "";

            public bool IsCorrectlyRead()
            {
                return LeftCornerOfLeftEye != null &&
                    LeftEyeRightCorner != null &&
                    LeftCornerOfRightEye != null &&
                    RightCornerOfRightEye != null &&
                    LeftCornerOfNose != null &&
                    NostTip != null &&
                    RightCornerOfNose != null &&
                    LeftCornerOfLips != null &&
                    RightCornerOfLips != null;
            }

            private string MyFileName = "";
            public string FileName
            {
                get
                {
                    if (MyFileName.Equals(""))
                    {
                        int index = FilePath.LastIndexOf('\\');
                        MyFileName = FilePath.Substring(index + 1);
                    }
                    return MyFileName;
                }
            }
        }

        private Point3D ReadLineFromCORorCRRFile(string p_sLine, string p_sTocken, char p_sCoordinatesSeparator)
        {
            string coordinates = p_sLine.Substring(p_sTocken.Length);
            string x;
            string y;
            string z;
            if (p_sCoordinatesSeparator == ' ')
            {
                string[] onlyCoor = coordinates.Split(' ');
                if (onlyCoor.Length != 7)
                    throw new Exception("File has wrong structure: line" + p_sLine);
                x = onlyCoor[2];
                y = onlyCoor[4];
                z = onlyCoor[6];
            }
            else if (p_sCoordinatesSeparator == '\t')
            {
                string[] onlyCoorWithLetter = coordinates.Split('\t');
                if (onlyCoorWithLetter.Length != 4)
                    throw new Exception("File has wrong structure: line" + p_sLine);

                int spaceIndex = onlyCoorWithLetter[1].IndexOf(' ');
                x = onlyCoorWithLetter[1].Substring(spaceIndex + 1);
                spaceIndex = onlyCoorWithLetter[2].IndexOf(' ');
                y = onlyCoorWithLetter[2].Substring(spaceIndex + 1);
                spaceIndex = onlyCoorWithLetter[3].IndexOf(' ');
                z = onlyCoorWithLetter[3].Substring(spaceIndex + 1);
            }
            else
                throw new Exception("Unsupported coordinates separator, supported separators <space, tab>");

            if (x.Contains(",") || y.Contains(",") || z.Contains(","))
                throw new Exception("Line has comas, it cause errors in parsing: " + p_sLine);

            double X = Double.Parse(x, System.Globalization.CultureInfo.InvariantCulture);
            double Y = Double.Parse(y, System.Globalization.CultureInfo.InvariantCulture);
            double Z = Double.Parse(z, System.Globalization.CultureInfo.InvariantCulture);

            Point3D newPoint = new Point3D(X, Y, Z);
            return newPoint;
        }
        private CRRFile ReadCRRFile(string p_sFileName)
        {
            CRRFile fileStructure = new CRRFile();
            fileStructure.FilePath = p_sFileName;
            using (StreamReader FileStream = File.OpenText(p_sFileName))
            {
                string line;
                while ((line = FileStream.ReadLine()) != null)
                {
                    if (line.Contains("@"))
                        continue;

                    string tocken = "NoseTip";
                    if (line.Contains(tocken))
                    {
                        fileStructure.NostTip = ReadLineFromCORorCRRFile(line, tocken, ' ');
                        continue; 
                    }
                    tocken = "LeftEyeRightCorner";
                    if (line.Contains(tocken))
                    {
                        fileStructure.LeftEyeRightCorner = ReadLineFromCORorCRRFile(line, tocken, ' ');
                        continue;
                    }
                    tocken = "RightEyeLeftCorner";
                    if (line.Contains(tocken))
                    {
                        fileStructure.LeftCornerOfRightEye = ReadLineFromCORorCRRFile(line, tocken, ' ');
                        continue;
                    }
                    tocken = "LeftEyeLeftCorner";
                    if (line.Contains(tocken))
                    {
                        fileStructure.LeftCornerOfLeftEye = ReadLineFromCORorCRRFile(line, tocken, ' ');
                        continue;
                    }
                    tocken = "RightEyeRightCorner";
                    if (line.Contains(tocken))
                    {
                        fileStructure.RightCornerOfRightEye = ReadLineFromCORorCRRFile(line, tocken, ' ');
                        continue;
                    }
                    tocken = "LeftCornerOfNose";
                    if (line.Contains(tocken))
                    {
                        fileStructure.LeftCornerOfNose = ReadLineFromCORorCRRFile(line, tocken, ' ');
                        continue;
                    }
                    tocken = "RightCornerOfNose";
                    if (line.Contains(tocken))
                    {
                        fileStructure.RightCornerOfNose = ReadLineFromCORorCRRFile(line, tocken, ' ');
                        continue;
                    }
                    tocken = "LeftCornerOfLips";
                    if (line.Contains(tocken))
                    {
                        fileStructure.LeftCornerOfLips = ReadLineFromCORorCRRFile(line, tocken, ' ');
                        continue;
                    }
                    tocken = "RightCornerOfLips";
                    if (line.Contains(tocken))
                    {
                        fileStructure.RightCornerOfLips = ReadLineFromCORorCRRFile(line, tocken, ' ');
                        continue;
                    }
                    throw new Exception("Unknown tocken in the line: " + line + " of file: " + p_sFileName);
                }
            }

            return fileStructure;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            CRRFile landmarkFile = ReadCRRFile(p_Model.ModelFileFolder + p_Model.ModelFileName + ".crr");
            //search for the closest point from the model to the read landmark
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            if (!iter.IsValid())
                throw new Exception("Iterator in the model is not valid!");

            Cl3DModel.Cl3DModelPointIterator SavedNoseTip = null;
            Cl3DModel.Cl3DModelPointIterator SavedLeftEyeRightCorner = null;
            Cl3DModel.Cl3DModelPointIterator SavedRightEyeLeftCorner = null;
            Cl3DModel.Cl3DModelPointIterator SavedRightEyeRightCorner = null;
            Cl3DModel.Cl3DModelPointIterator SavedLeftEyeLeftCorner = null;
            Cl3DModel.Cl3DModelPointIterator SavedLeftCornerOfNose = null;
            Cl3DModel.Cl3DModelPointIterator SavedRightCornerOfNose = null;
            Cl3DModel.Cl3DModelPointIterator SavedLeftCornerOfLips = null;
            Cl3DModel.Cl3DModelPointIterator SavedRightCornerOfLips = null;
            float RightCornerOfLipsDistance = 0;
            float NoseTipDistance = 0;
            float LeftEyeRightCornerDistance = 0;
            float RightEyeLeftCornerDistance = 0;
            float RightEyeRightCornerDistance = 0;
            float LeftEyeLeftCornerDistance = 0;
            float LeftCornerOfNoseDistance = 0;
            float RightCornerOfNoseDistance = 0;
            float LeftCornerOfLipsDistance = 0;

            if (landmarkFile.NostTip != null)
            {
                SavedNoseTip = iter.CopyIterator();
                NoseTipDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.NostTip.X - SavedNoseTip.X, 2) + Math.Pow(landmarkFile.NostTip.Y - SavedNoseTip.Y, 2) + Math.Pow(landmarkFile.NostTip.Z - SavedNoseTip.Z, 2));
            }

            if (landmarkFile.LeftEyeRightCorner != null)
            {
                SavedLeftEyeRightCorner = iter.CopyIterator();
                LeftEyeRightCornerDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftEyeRightCorner.X - SavedLeftEyeRightCorner.X, 2) + Math.Pow(landmarkFile.LeftEyeRightCorner.Y - SavedLeftEyeRightCorner.Y, 2) + Math.Pow(landmarkFile.LeftEyeRightCorner.Z - SavedLeftEyeRightCorner.Z, 2));
            }

            if (landmarkFile.LeftCornerOfRightEye != null)
            {
                SavedRightEyeLeftCorner = iter.CopyIterator();
                RightEyeLeftCornerDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftCornerOfRightEye.X - SavedRightEyeLeftCorner.X, 2) + Math.Pow(landmarkFile.LeftCornerOfRightEye.Y - SavedRightEyeLeftCorner.Y, 2) + Math.Pow(landmarkFile.LeftCornerOfRightEye.Z - SavedRightEyeLeftCorner.Z, 2));
            }

            if (landmarkFile.RightCornerOfRightEye != null)
            {
                SavedRightEyeRightCorner = iter.CopyIterator();
                RightEyeRightCornerDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.RightCornerOfRightEye.X - SavedRightEyeRightCorner.X, 2) + Math.Pow(landmarkFile.RightCornerOfRightEye.Y - SavedRightEyeRightCorner.Y, 2) + Math.Pow(landmarkFile.RightCornerOfRightEye.Z - SavedRightEyeRightCorner.Z, 2));
            }

            if (landmarkFile.LeftCornerOfLeftEye != null)
            {
                SavedLeftEyeLeftCorner = iter.CopyIterator();
                LeftEyeLeftCornerDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftCornerOfLeftEye.X - SavedLeftEyeLeftCorner.X, 2) + Math.Pow(landmarkFile.LeftCornerOfLeftEye.Y - SavedLeftEyeLeftCorner.Y, 2) + Math.Pow(landmarkFile.LeftCornerOfLeftEye.Z - SavedLeftEyeLeftCorner.Z, 2));
            }

            if (landmarkFile.LeftCornerOfNose != null)
            {
                SavedLeftCornerOfNose = iter.CopyIterator();
                LeftCornerOfNoseDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftCornerOfNose.X - SavedLeftCornerOfNose.X, 2) + Math.Pow(landmarkFile.LeftCornerOfNose.Y - SavedLeftCornerOfNose.Y, 2) + Math.Pow(landmarkFile.LeftCornerOfNose.Z - SavedLeftCornerOfNose.Z, 2));
            }

            if (landmarkFile.RightCornerOfNose != null)
            {
                SavedRightCornerOfNose = iter.CopyIterator();
                RightCornerOfNoseDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.RightCornerOfNose.X - SavedRightCornerOfNose.X, 2) + Math.Pow(landmarkFile.RightCornerOfNose.Y - SavedRightCornerOfNose.Y, 2) + Math.Pow(landmarkFile.RightCornerOfNose.Z - SavedRightCornerOfNose.Z, 2));
            }

            if (landmarkFile.LeftCornerOfLips != null)
            {
                SavedLeftCornerOfLips = iter.CopyIterator();
                LeftCornerOfLipsDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftCornerOfLips.X - SavedLeftCornerOfLips.X, 2) + Math.Pow(landmarkFile.LeftCornerOfLips.Y - SavedLeftCornerOfLips.Y, 2) + Math.Pow(landmarkFile.LeftCornerOfLips.Z - SavedLeftCornerOfLips.Z, 2));
            }

            if (landmarkFile.RightCornerOfLips != null)
            {
                SavedRightCornerOfLips = iter.CopyIterator();
                RightCornerOfLipsDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.RightCornerOfLips.X - SavedRightCornerOfLips.X, 2) + Math.Pow(landmarkFile.RightCornerOfLips.Y - SavedRightCornerOfLips.Y, 2) + Math.Pow(landmarkFile.RightCornerOfLips.Z - SavedRightCornerOfLips.Z, 2));
            }

            do
            {
                if (SavedNoseTip != null)
                {
                    float NewNoseTipDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.NostTip.X - iter.X, 2) + Math.Pow(landmarkFile.NostTip.Y - iter.Y, 2) + Math.Pow(landmarkFile.NostTip.Z - iter.Z, 2));
                    if (NewNoseTipDistance < NoseTipDistance)
                    {
                        NoseTipDistance = NewNoseTipDistance;
                        SavedNoseTip = iter.CopyIterator();
                    }
                }

                if (SavedLeftEyeRightCorner != null)
                {
                    float NewLeftEyeRightCornerDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftEyeRightCorner.X - iter.X, 2) + Math.Pow(landmarkFile.LeftEyeRightCorner.Y - iter.Y, 2) + Math.Pow(landmarkFile.LeftEyeRightCorner.Z - iter.Z, 2));
                    if (NewLeftEyeRightCornerDistance < LeftEyeRightCornerDistance)
                    {
                        LeftEyeRightCornerDistance = NewLeftEyeRightCornerDistance;
                        SavedLeftEyeRightCorner = iter.CopyIterator();
                    }
                }

                if (SavedRightEyeLeftCorner != null)
                {
                    float NewRightEyeLeftCornerDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftCornerOfRightEye.X - iter.X, 2) + Math.Pow(landmarkFile.LeftCornerOfRightEye.Y - iter.Y, 2) + Math.Pow(landmarkFile.LeftCornerOfRightEye.Z - iter.Z, 2));
                    if (NewRightEyeLeftCornerDistance < RightEyeLeftCornerDistance)
                    {
                        RightEyeLeftCornerDistance = NewRightEyeLeftCornerDistance;
                        SavedRightEyeLeftCorner = iter.CopyIterator();
                    }
                }

                if (SavedRightEyeRightCorner != null)
                {
                    float NewRightEyeRightCornerDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.RightCornerOfRightEye.X - iter.X, 2) + Math.Pow(landmarkFile.RightCornerOfRightEye.Y - iter.Y, 2) + Math.Pow(landmarkFile.RightCornerOfRightEye.Z - iter.Z, 2));
                    if (NewRightEyeRightCornerDistance < RightEyeRightCornerDistance)
                    {
                        RightEyeRightCornerDistance = NewRightEyeRightCornerDistance;
                        SavedRightEyeRightCorner = iter.CopyIterator();
                    }
                }

                if (SavedLeftEyeLeftCorner != null)
                {
                    float NewLeftEyeLeftCornerDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftCornerOfLeftEye.X - iter.X, 2) + Math.Pow(landmarkFile.LeftCornerOfLeftEye.Y - iter.Y, 2) + Math.Pow(landmarkFile.LeftCornerOfLeftEye.Z - iter.Z, 2));
                    if (NewLeftEyeLeftCornerDistance < LeftEyeLeftCornerDistance)
                    {
                        LeftEyeLeftCornerDistance = NewLeftEyeLeftCornerDistance;
                        SavedLeftEyeLeftCorner = iter.CopyIterator();
                    }
                }

                if (SavedLeftCornerOfNose != null)
                {
                    float NewLeftCornerOfNoseDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftCornerOfNose.X - iter.X, 2) + Math.Pow(landmarkFile.LeftCornerOfNose.Y - iter.Y, 2) + Math.Pow(landmarkFile.LeftCornerOfNose.Z - iter.Z, 2));
                    if (NewLeftCornerOfNoseDistance < LeftCornerOfNoseDistance)
                    {
                        LeftCornerOfNoseDistance = NewLeftCornerOfNoseDistance;
                        SavedLeftCornerOfNose = iter.CopyIterator();
                    }
                }

                if (SavedRightCornerOfNose != null)
                {
                    float NewRightCornerOfNoseDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.RightCornerOfNose.X - iter.X, 2) + Math.Pow(landmarkFile.RightCornerOfNose.Y - iter.Y, 2) + Math.Pow(landmarkFile.RightCornerOfNose.Z - iter.Z, 2));
                    if (NewRightCornerOfNoseDistance < RightCornerOfNoseDistance)
                    {
                        RightCornerOfNoseDistance = NewRightCornerOfNoseDistance;
                        SavedRightCornerOfNose = iter.CopyIterator();
                    }
                }

                if (SavedLeftCornerOfLips != null)
                {
                    float NewLeftCornerOfLipsDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftCornerOfLips.X - iter.X, 2) + Math.Pow(landmarkFile.LeftCornerOfLips.Y - iter.Y, 2) + Math.Pow(landmarkFile.LeftCornerOfLips.Z - iter.Z, 2));
                    if (NewLeftCornerOfLipsDistance < LeftCornerOfLipsDistance)
                    {
                        LeftCornerOfLipsDistance = NewLeftCornerOfLipsDistance;
                        SavedLeftCornerOfLips = iter.CopyIterator();
                    }
                }

                if (SavedRightCornerOfLips != null)
                {
                    float NewRightCornerOfLipsDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.RightCornerOfLips.X - iter.X, 2) + Math.Pow(landmarkFile.RightCornerOfLips.Y - iter.Y, 2) + Math.Pow(landmarkFile.RightCornerOfLips.Z - iter.Z, 2));
                    if (NewRightCornerOfLipsDistance < RightCornerOfLipsDistance)
                    {
                        RightCornerOfLipsDistance = NewRightCornerOfLipsDistance;
                        SavedRightCornerOfLips = iter.CopyIterator();
                    }
                }


            } while (iter.MoveToNext());

            if(SavedNoseTip != null)
                p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip, SavedNoseTip);
            if (SavedRightEyeLeftCorner != null)
                p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner, SavedRightEyeLeftCorner);
            if (SavedLeftEyeRightCorner != null)
                p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner, SavedLeftEyeRightCorner);
            if (SavedLeftEyeLeftCorner != null)
                p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeLeftCorner, SavedLeftEyeLeftCorner);
            if (SavedRightEyeRightCorner != null)
                p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeRightCorner, SavedRightEyeRightCorner);
            if (SavedLeftCornerOfNose != null)
                p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.LeftCornerOfNose, SavedLeftCornerOfNose);
            if (SavedRightCornerOfNose != null)
                p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.RightCornerOfNose, SavedRightCornerOfNose);
            if (SavedLeftCornerOfLips != null)
                p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.LeftCornerOfLips, SavedLeftCornerOfLips);
            if (SavedRightCornerOfLips != null)
                p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.RightCornerOfLips, SavedRightCornerOfLips);
        }
    }
}

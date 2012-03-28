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
*   @file       ClLoadManualSpecificPoints.cs
*   @brief      ClLoadManualSpecificPoints
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       18-10-2008
*
*   @history
*   @item		18-10-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClLoadManualSpecificPoints : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClLoadManualSpecificPoints();
        }

        public static string ALGORITHM_NAME = @"Load\Load Manual Specific Points (*.cor)";

        public ClLoadManualSpecificPoints() : base(ALGORITHM_NAME) { }

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
        private class CORFile
        {
            public Point3D LeftEyeLeftCorner;
            public Point3D LeftEyeRightCorner;
            public Point3D LeftEyeUpperEyelid;
            public Point3D LeftEyeBottomEyelid;
            public Point3D RightEyeLeftCorner;
            public Point3D RightEyeRightCorner;
            public Point3D RightEyeUpperEyelid;
            public Point3D RightEyeBottomEyelid;
            public Point3D LeftCornerOfNose;
            public Point3D NoseTip;
            public Point3D RightCornerOfNose;
            public Point3D LeftCornerOfLips;
            public Point3D UpperLip;
            public Point3D BottomLip;
            public Point3D RightCornerOfLips;
            public string User = "";
            public string FilePath = "";

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
        private CORFile ReadCORFile(string p_sFileName)
        {
            CORFile fileStructure = new CORFile();
            fileStructure.FilePath = p_sFileName;
            using (StreamReader FileStream = File.OpenText(p_sFileName))
            {
                string line;
                bool bFileVersionOK = false;
                char cFileSubVersionCoordinateSeparator = ' ';
                while ((line = FileStream.ReadLine()) != null)
                {
                    if (line.Contains("@ File Version 1.1") || line.Contains("@ File Version 1.2"))
                    {
                        if (line.Contains("@ File Version 1.1"))
                            cFileSubVersionCoordinateSeparator = ' ';
                        else if (line.Contains("@ File Version 1.2"))
                            cFileSubVersionCoordinateSeparator = '\t';

                        bFileVersionOK = true;
                        continue;
                    }
                    string tocken = "@ Model landmarked by: ";
                    if (line.Contains(tocken))
                    {
                        fileStructure.User = line.Substring(tocken.Length);
                        continue;
                    }
                    if (line.StartsWith("@"))
                        continue;

                    if (!bFileVersionOK)
                        throw new Exception("COR file version is not 1.1 or 1.2, file: " + p_sFileName);

                    tocken = "Left Eye Left Corner";
                    if (line.Contains(tocken))
                    {
                        fileStructure.LeftEyeLeftCorner = ReadLineFromCORorCRRFile(line, tocken, cFileSubVersionCoordinateSeparator);
                        continue;
                    }
                    tocken = "Left Eye Right Corner";
                    if (line.Contains(tocken))
                    {
                        fileStructure.LeftEyeRightCorner = ReadLineFromCORorCRRFile(line, tocken, cFileSubVersionCoordinateSeparator);
                        continue;
                    }
                    tocken = "Left Eye Upper Eyelid";
                    if (line.Contains(tocken))
                    {
                        fileStructure.LeftEyeUpperEyelid = ReadLineFromCORorCRRFile(line, tocken, cFileSubVersionCoordinateSeparator);
                        continue;
                    }
                    tocken = "Left Eye Bottom Eyelid";
                    if (line.Contains(tocken))
                    {
                        fileStructure.LeftEyeBottomEyelid = ReadLineFromCORorCRRFile(line, tocken, cFileSubVersionCoordinateSeparator);
                        continue;
                    }
                    tocken = "Right Eye Left Corner";
                    if (line.Contains(tocken))
                    {
                        fileStructure.RightEyeLeftCorner = ReadLineFromCORorCRRFile(line, tocken, cFileSubVersionCoordinateSeparator);
                        continue;
                    }
                    tocken = "Right Eye Right Corner";
                    if (line.Contains(tocken))
                    {
                        fileStructure.RightEyeRightCorner = ReadLineFromCORorCRRFile(line, tocken, cFileSubVersionCoordinateSeparator);
                        continue;
                    }
                    tocken = "Right Eye Upper Eyelid";
                    if (line.Contains(tocken))
                    {
                        fileStructure.RightEyeUpperEyelid = ReadLineFromCORorCRRFile(line, tocken, cFileSubVersionCoordinateSeparator);
                        continue;
                    }
                    tocken = "Right Eye Bottom Eyelid";
                    if (line.Contains(tocken))
                    {
                        fileStructure.RightEyeBottomEyelid = ReadLineFromCORorCRRFile(line, tocken, cFileSubVersionCoordinateSeparator);
                        continue;
                    }
                    tocken = "Left Corner Of Nose";
                    if (line.Contains(tocken))
                    {
                        fileStructure.LeftCornerOfNose = ReadLineFromCORorCRRFile(line, tocken, cFileSubVersionCoordinateSeparator);
                        continue;
                    }
                    tocken = "Nose Tip";
                    if (line.Contains(tocken))
                    {
                        fileStructure.NoseTip = ReadLineFromCORorCRRFile(line, tocken, cFileSubVersionCoordinateSeparator);
                        continue;
                    }
                    tocken = "Right Corner Of Nose";
                    if (line.Contains(tocken))
                    {
                        fileStructure.RightCornerOfNose = ReadLineFromCORorCRRFile(line, tocken, cFileSubVersionCoordinateSeparator);
                        continue;
                    }
                    tocken = "Left Corner Of Lips";
                    if (line.Contains(tocken))
                    {
                        fileStructure.LeftCornerOfLips = ReadLineFromCORorCRRFile(line, tocken, cFileSubVersionCoordinateSeparator);
                        continue;
                    }
                    tocken = "Upper Lip";
                    if (line.Contains(tocken))
                    {
                        fileStructure.UpperLip = ReadLineFromCORorCRRFile(line, tocken, cFileSubVersionCoordinateSeparator);
                        continue;
                    }
                    tocken = "Bottom Lip";
                    if (line.Contains(tocken))
                    {
                        fileStructure.BottomLip = ReadLineFromCORorCRRFile(line, tocken, cFileSubVersionCoordinateSeparator);
                        continue;
                    }
                    tocken = "Right Corner Of Lips";
                    if (line.Contains(tocken))
                    {
                        fileStructure.RightCornerOfLips = ReadLineFromCORorCRRFile(line, tocken, cFileSubVersionCoordinateSeparator);
                        continue;
                    }
                    throw new Exception("Unknown tocken in the line: " + line + " of file: " + p_sFileName);
                }

            }
            return fileStructure;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            CORFile landmarkFile = ReadCORFile(p_Model.ModelFileFolder + p_Model.ModelFileName + ".cor");
            //search for the closest point from the model to the read landmark
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            if (!iter.IsValid())
                throw new Exception("Iterator in the model is not valid!");

            Cl3DModel.Cl3DModelPointIterator SavedNoseTip = iter.CopyIterator();
            float NoseTipDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.NoseTip.X - SavedNoseTip.X,2)+Math.Pow(landmarkFile.NoseTip.Y - SavedNoseTip.Y,2)+Math.Pow(landmarkFile.NoseTip.Z - SavedNoseTip.Z,2));

            Cl3DModel.Cl3DModelPointIterator SavedLeftEyeRightCorner = iter.CopyIterator();
            float LeftEyeRightCornerDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftEyeRightCorner.X - SavedLeftEyeRightCorner.X, 2) + Math.Pow(landmarkFile.LeftEyeRightCorner.Y - SavedLeftEyeRightCorner.Y, 2) + Math.Pow(landmarkFile.LeftEyeRightCorner.Z - SavedLeftEyeRightCorner.Z, 2));
            
            Cl3DModel.Cl3DModelPointIterator SavedRightEyeLeftCorner = iter.CopyIterator();
            float RightEyeLeftCornerDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.RightEyeLeftCorner.X - SavedRightEyeLeftCorner.X, 2) + Math.Pow(landmarkFile.RightEyeLeftCorner.Y - SavedRightEyeLeftCorner.Y, 2) + Math.Pow(landmarkFile.RightEyeLeftCorner.Z - SavedRightEyeLeftCorner.Z, 2));

            Cl3DModel.Cl3DModelPointIterator SavedRightEyeRightCorner = iter.CopyIterator();
            float RightEyeRightCornerDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.RightEyeRightCorner.X - SavedRightEyeRightCorner.X, 2) + Math.Pow(landmarkFile.RightEyeRightCorner.Y - SavedRightEyeRightCorner.Y, 2) + Math.Pow(landmarkFile.RightEyeRightCorner.Z - SavedRightEyeRightCorner.Z, 2));

            Cl3DModel.Cl3DModelPointIterator SavedLeftEyeLeftCorner = iter.CopyIterator();
            float LeftEyeLeftCornerDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftEyeLeftCorner.X - SavedLeftEyeLeftCorner.X, 2) + Math.Pow(landmarkFile.LeftEyeLeftCorner.Y - SavedLeftEyeLeftCorner.Y, 2) + Math.Pow(landmarkFile.LeftEyeLeftCorner.Z - SavedLeftEyeLeftCorner.Z, 2));

            Cl3DModel.Cl3DModelPointIterator SavedLeftCornerOfNose = iter.CopyIterator();
            float LeftCornerOfNoseDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftCornerOfNose.X - SavedLeftCornerOfNose.X, 2) + Math.Pow(landmarkFile.LeftCornerOfNose.Y - SavedLeftCornerOfNose.Y, 2) + Math.Pow(landmarkFile.LeftCornerOfNose.Z - SavedLeftCornerOfNose.Z, 2));

            Cl3DModel.Cl3DModelPointIterator SavedRightCornerOfNose = iter.CopyIterator();
            float RightCornerOfNoseDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.RightCornerOfNose.X - SavedRightCornerOfNose.X, 2) + Math.Pow(landmarkFile.RightCornerOfNose.Y - SavedRightCornerOfNose.Y, 2) + Math.Pow(landmarkFile.RightCornerOfNose.Z - SavedRightCornerOfNose.Z, 2));

            Cl3DModel.Cl3DModelPointIterator SavedLeftCornerOfLips = iter.CopyIterator();
            float LeftCornerOfLipsDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftCornerOfLips.X - SavedLeftCornerOfLips.X, 2) + Math.Pow(landmarkFile.LeftCornerOfLips.Y - SavedLeftCornerOfLips.Y, 2) + Math.Pow(landmarkFile.LeftCornerOfLips.Z - SavedLeftCornerOfLips.Z, 2));

            Cl3DModel.Cl3DModelPointIterator SavedRightCornerOfLips = iter.CopyIterator();
            float RightCornerOfLipsDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.RightCornerOfLips.X - SavedRightCornerOfLips.X, 2) + Math.Pow(landmarkFile.RightCornerOfLips.Y - SavedRightCornerOfLips.Y, 2) + Math.Pow(landmarkFile.RightCornerOfLips.Z - SavedRightCornerOfLips.Z, 2));



            Cl3DModel.Cl3DModelPointIterator SavedUpperLip = iter.CopyIterator();
            float UpperLipDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.UpperLip.X - SavedUpperLip.X, 2) + Math.Pow(landmarkFile.UpperLip.Y - SavedUpperLip.Y, 2) + Math.Pow(landmarkFile.UpperLip.Z - SavedUpperLip.Z, 2));

            Cl3DModel.Cl3DModelPointIterator SavedBottomLip = iter.CopyIterator();
            float BottomLipDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.BottomLip.X - SavedBottomLip.X, 2) + Math.Pow(landmarkFile.BottomLip.Y - SavedBottomLip.Y, 2) + Math.Pow(landmarkFile.BottomLip.Z - SavedBottomLip.Z, 2));

            Cl3DModel.Cl3DModelPointIterator SavedLeftEyeUpperEyelid = iter.CopyIterator();
            float LeftEyeUpperEyelidDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftEyeUpperEyelid.X - SavedLeftEyeUpperEyelid.X, 2) + Math.Pow(landmarkFile.LeftEyeUpperEyelid.Y - SavedLeftEyeUpperEyelid.Y, 2) + Math.Pow(landmarkFile.LeftEyeUpperEyelid.Z - SavedLeftEyeUpperEyelid.Z, 2));

            Cl3DModel.Cl3DModelPointIterator SavedLeftEyeBottomEyelid = iter.CopyIterator();
            float LeftEyeBottomEyelidDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftEyeBottomEyelid.X - SavedLeftEyeBottomEyelid.X, 2) + Math.Pow(landmarkFile.LeftEyeBottomEyelid.Y - SavedLeftEyeBottomEyelid.Y, 2) + Math.Pow(landmarkFile.LeftEyeBottomEyelid.Z - SavedLeftEyeBottomEyelid.Z, 2));

            Cl3DModel.Cl3DModelPointIterator SavedRightEyeUpperEyelid = iter.CopyIterator();
            float RightEyeUpperEyelidDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.RightEyeUpperEyelid.X - SavedRightEyeUpperEyelid.X, 2) + Math.Pow(landmarkFile.RightEyeUpperEyelid.Y - SavedRightEyeUpperEyelid.Y, 2) + Math.Pow(landmarkFile.RightEyeUpperEyelid.Z - SavedRightEyeUpperEyelid.Z, 2));

            Cl3DModel.Cl3DModelPointIterator SavedRightEyeBottomEyelid = iter.CopyIterator();
            float RightEyeBottomEyelidDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.RightEyeBottomEyelid.X - SavedRightEyeBottomEyelid.X, 2) + Math.Pow(landmarkFile.RightEyeBottomEyelid.Y - SavedRightEyeBottomEyelid.Y, 2) + Math.Pow(landmarkFile.RightEyeBottomEyelid.Z - SavedRightEyeBottomEyelid.Z, 2));
            
            
            do
            {
                float NewNoseTipDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.NoseTip.X - iter.X, 2) + Math.Pow(landmarkFile.NoseTip.Y - iter.Y, 2) + Math.Pow(landmarkFile.NoseTip.Z - iter.Z, 2));
                float NewLeftEyeRightCornerDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftEyeRightCorner.X - iter.X, 2) + Math.Pow(landmarkFile.LeftEyeRightCorner.Y - iter.Y, 2) + Math.Pow(landmarkFile.LeftEyeRightCorner.Z - iter.Z, 2));
                float NewRightEyeLeftCornerDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.RightEyeLeftCorner.X - iter.X, 2) + Math.Pow(landmarkFile.RightEyeLeftCorner.Y - iter.Y, 2) + Math.Pow(landmarkFile.RightEyeLeftCorner.Z - iter.Z, 2));
                float NewLeftEyeLeftCornerDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftEyeLeftCorner.X - iter.X, 2) + Math.Pow(landmarkFile.LeftEyeLeftCorner.Y - iter.Y, 2) + Math.Pow(landmarkFile.LeftEyeLeftCorner.Z - iter.Z, 2));
                float NewRightEyeRightCornerDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.RightEyeRightCorner.X - iter.X, 2) + Math.Pow(landmarkFile.RightEyeRightCorner.Y - iter.Y, 2) + Math.Pow(landmarkFile.RightEyeRightCorner.Z - iter.Z, 2));
                float NewLeftCornerOfNoseDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftCornerOfNose.X - iter.X, 2) + Math.Pow(landmarkFile.LeftCornerOfNose.Y - iter.Y, 2) + Math.Pow(landmarkFile.LeftCornerOfNose.Z - iter.Z, 2));
                float NewRightCornerOfNoseDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.RightCornerOfNose.X - iter.X, 2) + Math.Pow(landmarkFile.RightCornerOfNose.Y - iter.Y, 2) + Math.Pow(landmarkFile.RightCornerOfNose.Z - iter.Z, 2));
                float NewLeftCornerOfLipsDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftCornerOfLips.X - iter.X, 2) + Math.Pow(landmarkFile.LeftCornerOfLips.Y - iter.Y, 2) + Math.Pow(landmarkFile.LeftCornerOfLips.Z - iter.Z, 2));
                float NewRightCornerOfLipsDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.RightCornerOfLips.X - iter.X, 2) + Math.Pow(landmarkFile.RightCornerOfLips.Y - iter.Y, 2) + Math.Pow(landmarkFile.RightCornerOfLips.Z - iter.Z, 2));

                float NewUpperLipDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.UpperLip.X - iter.X, 2) + Math.Pow(landmarkFile.UpperLip.Y - iter.Y, 2) + Math.Pow(landmarkFile.UpperLip.Z - iter.Z, 2));
                float NewBottomLipDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.BottomLip.X - iter.X, 2) + Math.Pow(landmarkFile.BottomLip.Y - iter.Y, 2) + Math.Pow(landmarkFile.BottomLip.Z - iter.Z, 2));
                float NewLeftEyeUpperEyelidDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftEyeUpperEyelid.X - iter.X, 2) + Math.Pow(landmarkFile.LeftEyeUpperEyelid.Y - iter.Y, 2) + Math.Pow(landmarkFile.LeftEyeUpperEyelid.Z - iter.Z, 2));
                float NewLeftEyeBottomEyelidDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.LeftEyeBottomEyelid.X - iter.X, 2) + Math.Pow(landmarkFile.LeftEyeBottomEyelid.Y - iter.Y, 2) + Math.Pow(landmarkFile.LeftEyeBottomEyelid.Z - iter.Z, 2));
                float NewRightEyeUpperEyelidDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.RightEyeUpperEyelid.X - iter.X, 2) + Math.Pow(landmarkFile.RightEyeUpperEyelid.Y - iter.Y, 2) + Math.Pow(landmarkFile.RightEyeUpperEyelid.Z - iter.Z, 2));
                float NewRightEyeBottomEyelidDistance = (float)Math.Sqrt(Math.Pow(landmarkFile.RightEyeBottomEyelid.X - iter.X, 2) + Math.Pow(landmarkFile.RightEyeBottomEyelid.Y - iter.Y, 2) + Math.Pow(landmarkFile.RightEyeBottomEyelid.Z - iter.Z, 2));

                if (NewNoseTipDistance < NoseTipDistance)
                {
                    NoseTipDistance = NewNoseTipDistance;
                    SavedNoseTip = iter.CopyIterator();
                }
                if (NewLeftEyeRightCornerDistance < LeftEyeRightCornerDistance)
                {
                    LeftEyeRightCornerDistance = NewLeftEyeRightCornerDistance;
                    SavedLeftEyeRightCorner = iter.CopyIterator();
                }
                if (NewRightEyeLeftCornerDistance < RightEyeLeftCornerDistance)
                {
                    RightEyeLeftCornerDistance = NewRightEyeLeftCornerDistance;
                    SavedRightEyeLeftCorner = iter.CopyIterator();
                }
                if (NewRightEyeRightCornerDistance < RightEyeRightCornerDistance)
                {
                    RightEyeRightCornerDistance = NewRightEyeRightCornerDistance;
                    SavedRightEyeRightCorner = iter.CopyIterator();
                }
                if (NewLeftEyeLeftCornerDistance < LeftEyeLeftCornerDistance)
                {
                    LeftEyeLeftCornerDistance = NewLeftEyeLeftCornerDistance;
                    SavedLeftEyeLeftCorner = iter.CopyIterator();
                }
                if (NewLeftCornerOfNoseDistance < LeftCornerOfNoseDistance)
                {
                    LeftCornerOfNoseDistance = NewLeftCornerOfNoseDistance;
                    SavedLeftCornerOfNose = iter.CopyIterator();
                }
                if (NewRightCornerOfNoseDistance < RightCornerOfNoseDistance)
                {
                    RightCornerOfNoseDistance = NewRightCornerOfNoseDistance;
                    SavedRightCornerOfNose = iter.CopyIterator();
                }
                if (NewLeftCornerOfLipsDistance < LeftCornerOfLipsDistance)
                {
                    LeftCornerOfLipsDistance = NewLeftCornerOfLipsDistance;
                    SavedLeftCornerOfLips = iter.CopyIterator();
                }
                if (NewRightCornerOfLipsDistance < RightCornerOfLipsDistance)
                {
                    RightCornerOfLipsDistance = NewRightCornerOfLipsDistance;
                    SavedRightCornerOfLips = iter.CopyIterator();
                }

                if (NewUpperLipDistance < UpperLipDistance)
                {
                    UpperLipDistance = NewUpperLipDistance;
                    SavedUpperLip = iter.CopyIterator();
                }
                if (NewBottomLipDistance < BottomLipDistance)
                {
                    BottomLipDistance = NewBottomLipDistance;
                    SavedBottomLip = iter.CopyIterator();
                }
                if (NewLeftEyeUpperEyelidDistance < LeftEyeUpperEyelidDistance)
                {
                    LeftEyeUpperEyelidDistance = NewLeftEyeUpperEyelidDistance;
                    SavedLeftEyeUpperEyelid = iter.CopyIterator();
                }
                if (NewLeftEyeBottomEyelidDistance < LeftEyeBottomEyelidDistance)
                {
                    LeftEyeBottomEyelidDistance = NewLeftEyeBottomEyelidDistance;
                    SavedLeftEyeBottomEyelid = iter.CopyIterator();
                }
                if (NewRightEyeUpperEyelidDistance < RightEyeUpperEyelidDistance)
                {
                    RightEyeUpperEyelidDistance = NewRightEyeUpperEyelidDistance;
                    SavedRightEyeUpperEyelid = iter.CopyIterator();
                }
                if (NewRightEyeBottomEyelidDistance < RightEyeBottomEyelidDistance)
                {
                    RightEyeBottomEyelidDistance = NewRightEyeBottomEyelidDistance;
                    SavedRightEyeBottomEyelid = iter.CopyIterator();
                }

            } while (iter.MoveToNext());

            p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip, SavedNoseTip);
            p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeLeftCorner, SavedRightEyeLeftCorner);
            p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeRightCorner, SavedLeftEyeRightCorner);
            p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeLeftCorner, SavedLeftEyeLeftCorner);
            p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeRightCorner, SavedRightEyeRightCorner);
            p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.LeftCornerOfNose, SavedLeftCornerOfNose);
            p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.RightCornerOfNose, SavedRightCornerOfNose);
            p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.LeftCornerOfLips, SavedLeftCornerOfLips);
            p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.RightCornerOfLips, SavedRightCornerOfLips);

            p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.UpperLip, SavedUpperLip);
            p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.BottomLip, SavedBottomLip);
            p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeUpperEyelid, SavedLeftEyeUpperEyelid);
            p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.LeftEyeBottomEyelid, SavedLeftEyeBottomEyelid);
            p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeUpperEyelid, SavedRightEyeUpperEyelid);
            p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.RightEyeBottomEyelid, SavedRightEyeBottomEyelid);
        }
    }
}

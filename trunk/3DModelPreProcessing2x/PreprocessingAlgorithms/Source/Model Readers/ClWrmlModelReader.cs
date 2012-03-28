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
*   @file       ClWrmlModelReader.cs
*   @brief      Class responsible for reading models from VRML files 
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       05-06-2008
*
*   @history
*   @item		05-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClWrmlModelReader : ClBaseModelReader
    {
        public ClWrmlModelReader()
            : base("wrl")
        {
        }

        private bool FindTockenInFile(string p_sTocken, StreamReader p_fFile)
        {
            String line;
            bool foundTocken = false;
            while (!foundTocken && (line = p_fFile.ReadLine()) != null)
            {
                line = line.Replace('\t',' ');
                string[] parts = line.Split(' ');
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i].Equals(p_sTocken))
                    {
                        foundTocken = true;
                        break;
                    }
                }
            }
            return foundTocken;
        }

        private struct pair
        {
            public pair(float no1, float no2)
            {
                this.no1 = no1;
                this.no2 = no2;
            }
            public float no1;
            public float no2;
        }

        Bitmap texture = null;
        List<pair> TextureCoord = null;

        private void ReadTexture(String p_sFilePath)
        {

            using (StreamReader FileStream = File.OpenText(p_sFilePath))
            {
                // read texture 
                bool foundTocken = FindTockenInFile("PixelTexture", FileStream);
                if (!foundTocken)
                    return;
                string line = "";

                if ((line = FileStream.ReadLine()) == null)
                    return;
                string[] parsed = line.Split(' '); // image 800 400 3

                int width = Int32.Parse(parsed[12]);
                int height = Int32.Parse(parsed[13]);

                texture = new Bitmap(width, height);

                int i = 0;
                int j = 0;

                while ((line = FileStream.ReadLine()) != null)
                {
                    if (line.Contains("}"))
                        break;

                    string[] parsedRGB = line.Split(' ');
                    foreach (string col in parsedRGB)
                    {
                        if (col.Equals(""))
                            continue;

                       int colorR = Int32.Parse(col[2].ToString()+col[3].ToString(), System.Globalization.NumberStyles.AllowHexSpecifier);
                       int colorG = Int32.Parse(col[4].ToString() + col[5].ToString(), System.Globalization.NumberStyles.AllowHexSpecifier);
                       int colorB = Int32.Parse(col[6].ToString() + col[7].ToString(), System.Globalization.NumberStyles.AllowHexSpecifier);

                       texture.SetPixel(i, j, Color.FromArgb(colorR, colorG, colorB));

                        i++;
                        if (i >= width)
                        {
                            i = 0;
                            j++;
                        }
                    }                    
                }

                TextureCoord = new List<pair>();

                foundTocken = FindTockenInFile("TextureCoordinate", FileStream);
                if (!foundTocken)
                    return;

                while ((line = FileStream.ReadLine()) != null)
                {
                    if (line.Contains("point["))
                        continue;

                    if (line.Length == 0)
                        continue;

                    if(line.Contains("]"))
                        break;

                    string[] Coord = line.Replace(',',' ').Split(' ');

                    float no1 = Single.Parse(Coord[0], System.Globalization.CultureInfo.InvariantCulture);
                    float no2 = Single.Parse(Coord[1], System.Globalization.CultureInfo.InvariantCulture);

                    TextureCoord.Add(new pair(no1, no2));
                }
                FileStream.Close();
            }
        }

        public override void Read(Cl3DModel p_mModel3D, string p_sFilePath)
        {
            try
            {
                ReadTexture(p_sFilePath);
                // using ( cannot be used cause we open file stream once again
                StreamReader FileStream = File.OpenText(p_sFilePath);
                {
                    bool foundTocken = FindTockenInFile("Coordinate", FileStream);
                    if (!foundTocken)
                    {
                        FileStream = File.OpenText(p_sFilePath);
                        foundTocken = FindTockenInFile("Coordinate3", FileStream);
                        if (!foundTocken)
                            throw new Exception("Wrong file format, cannot find 'Coordinate' tocken");
                    }

                    bool finishReading = false;
                    String line;
                    int pointNO = 0;
                    while ((line = FileStream.ReadLine()) != null && !finishReading)
                    {
                        if (line.Contains("point"))
                            continue;

                        string[] parts = line.Split(' ');
                        for (int i = 0; i < parts.Length; i++)
                        {
                            if (parts[i].Contains("]"))
                            {
                                finishReading = true;
                                break;
                            }
                        }
                        if (finishReading)
                            break;

                        List<string> numbers = new List<string>();

                        foreach (string s in parts)
                        {
                            if (s.Equals(""))
                                continue;
                            else
                                numbers.Add(s);
                                                                
                        }

                        if (numbers.Count != 3)
                            throw new Exception("Wrong file format, less coordinates than expected");

                        float X = Single.Parse(numbers[0].Replace(',', ' '), System.Globalization.CultureInfo.InvariantCulture);
                        float Y = Single.Parse(numbers[1].Replace(',', ' '), System.Globalization.CultureInfo.InvariantCulture);
                        float Z = Single.Parse(numbers[2].Replace(',', ' '), System.Globalization.CultureInfo.InvariantCulture);

                        Cl3DModel.Cl3DModelPointIterator newPoint = p_mModel3D.AddPointToModel(X, Y, Z);
                        if (texture != null && TextureCoord.Count != 0)
                        {
                            newPoint.Color = texture.GetPixel((int)(TextureCoord[pointNO].no1 * texture.Width),
                                                (int)(TextureCoord[pointNO].no2 * texture.Height));
                            newPoint.AddSpecificValue("Texture", newPoint.Color.ToArgb());
                        }
                        pointNO++;
                    }

                    foundTocken = FindTockenInFile("coordIndex", FileStream);
                    if (!foundTocken)
                        throw new Exception("Wrong file format, cannot find 'coordIndex' tocken");

                    while ((line = FileStream.ReadLine()) != null)
                    {
                        if (line.Contains("]"))
                            break;

                        
                        List<Cl3DModel.Cl3DModelPointIterator> points = new List<Cl3DModel.Cl3DModelPointIterator>();
                        
                        Cl3DModel.Cl3DModelPointIterator iter = p_mModel3D.GetIterator();
                        string[] parts = line.Split(new Char [] {' ', ','});
                        bool EndOfLine = false;
                        foreach (string part in parts)
                        {
                            if (part.Length == 0)
                                continue;

                            int no = Int32.Parse(part);

                            if(no == -1)
                            {
                                EndOfLine = true;
                            }

                            if (EndOfLine)
                            {
                                for (int i = 0; i < points.Count; i++)
                                {
                                    if (i == points.Count - 2)
                                    {
                                        points[i].AddNeighbor(points[i + 1]);
                                        points[i + 1].AddNeighbor(points[0]);
                                        break;
                                    }
                                    if (i == points.Count - 3)
                                    {
                                        points[i].AddNeighbor(points[i + 1]);
                                        points[i+1].AddNeighbor(points[i+2]);
                                        points[i + 2].AddNeighbor(points[i]);
                                    }
                                    else
                                    {
                                        points[i].AddNeighbor(points[i + 1]);
                                        points[i + 1].AddNeighbor(points[i + 2]);
                                        points[i + 2].AddNeighbor(points[i]);
                                    }
                                }
                                points.Clear();
                                EndOfLine = false;
                            }
                            else
                            {
                                if (!iter.MoveToPoint((uint)no))
                                    throw new Exception("Cannot find point no: " + no);
                                points.Add(iter.CopyIterator());
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                p_mModel3D.ResetModel();
                throw;
            }
        }
    }
}

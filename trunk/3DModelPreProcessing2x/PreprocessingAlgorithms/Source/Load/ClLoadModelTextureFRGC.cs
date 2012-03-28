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
*   @file       ClLoadModelTexture.cs
*   @brief      Algorithm to ClLoadModelTexture
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       04-09-2008
*
*   @history
*   @item		04-09-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClLoadModelTextureFRGC : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClLoadModelTextureFRGC();
        }

        public static string ALGORITHM_NAME = @"Load\Load Model Texture FRGC (*.ppm)";

        public ClLoadModelTextureFRGC() : base(ALGORITHM_NAME) { }


        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            string name = "";
            if (p_Model.ModelType.Equals("abs") || p_Model.ModelType.Equals("binaryModel"))
            {
                string[] splitted = p_Model.ModelFileName.Split('d');
                int no = Int32.Parse(splitted[1]);
                no++;

                if(no<10)
                    name = p_Model.ModelFileFolder + splitted[0]+"d0"+no.ToString() + ".ppm";
                else
                    name = p_Model.ModelFileFolder + splitted[0] + "d" + no.ToString() + ".ppm";
            }
            else
                throw new Exception("Method does not supprt this kind of files");
            
            using (FileStream fs = File.OpenRead(name))
            {
                try
                {
                    Bitmap Texture = null;
                    int TextWidth = 0;
                    int TextHeight = 0;

                    bool readHeader = true;

                    bool doWeHaveFileType = false;
                    bool doWeHaveWidth = false;
                    bool doWeHaveHeight = false;
                    bool doWeHaveMaxPixelVal = false;

                    BinaryReader ReaderBinary = new BinaryReader(fs);

                    #region readHeader

                    while (readHeader)
                    {
                        string line = "";
                        char cSign;
                        while (true)
                        {
                            cSign = ReaderBinary.ReadChar();

                            if (cSign == '\n')
                                break;

                            line += cSign;
                        }

                        line = line.Split('#')[0];

                        if (line.Length == 0 || line.Equals(" "))
                            continue;

                        string[] val = line.Split(' ');
                        for (int i = 0; i < val.Length; i++)
                        {
                            if (val[i].Equals("P6") && i == 0 && doWeHaveFileType == false)
                            {
                                doWeHaveFileType = true;
                                continue;
                            }
                            else if (!doWeHaveFileType)
                                throw new Exception("Wrong file format, cannot find on beginning tag 'P6'");

                            if (doWeHaveWidth == false)
                            {
                                TextWidth = Int32.Parse(val[i]);
                                doWeHaveWidth = true;
                            }
                            else if (doWeHaveHeight == false)
                            {
                                TextHeight = Int32.Parse(val[i]);
                                doWeHaveHeight = true;
                            }
                            else if (doWeHaveMaxPixelVal == false)
                            {
                                if (!val[i].Equals("255"))
                                    throw new Exception("Method supports only max pixel value 255, and value is equal: " + val[i]);
                                doWeHaveMaxPixelVal = true;
                            }
                            else
                                throw new Exception("Unrecognized tocken");

                            if (doWeHaveWidth &&
                                doWeHaveMaxPixelVal &&
                                doWeHaveHeight &&
                                doWeHaveFileType)
                            {
                                readHeader = false;
                                break;
                            }
                        }
                    }
                    #endregion

                    Texture = new Bitmap(TextWidth, TextHeight);

                    int bitmapX = 0;
                    int bitmapY = 0;
                    byte info;
                    int count = 0;
                    int r = 0;
                    int g = 0;
                    int b = 0;
                    try
                    {

                        while (true)
                        {
                            info = ReaderBinary.ReadByte();

                            if (count == 0)
                                r = (int)info;
                            else if (count == 1)
                                g = (int)info;
                            else
                                b = (int)info;

                            if (count == 2)
                            {
                                if (bitmapX >= TextWidth || bitmapY >= TextHeight)
                                    throw new Exception("File has wrong format, too many pixels are saved in file");

                                Texture.SetPixel(bitmapX, bitmapY, Color.FromArgb(r, g, b));
                                bitmapX++;
                                count = 0;
                            }
                            else
                                count++;

                            if (bitmapX == TextWidth)
                            {
                                bitmapX = 0;
                                bitmapY++;
                                //    ClInformationSender.SendInformation(bitmapY.ToString() + "/" + m_TextHeight.ToString(), ClInformationSender.eInformationType.eProgress);
                            }
                        }
                    }
                    catch (System.IO.EndOfStreamException)
                    {
                        if (bitmapX != 0 || bitmapY != TextHeight)
                        {
                            fs.Close();
                            throw new Exception("File has wrong structure, not every pixel has value");
                        }
                    }
                    fs.Close();

                    Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
                    do
                    {
                        iter.Color = Texture.GetPixel(iter.RangeImageX, iter.RangeImageY);
                        iter.AddSpecificValue("Texture", iter.Color.ToArgb());
                    } while (iter.MoveToNext());
                }
                catch (Exception e)
                {
                    fs.Close();
                    throw;
                }
            }
        }
    }
}

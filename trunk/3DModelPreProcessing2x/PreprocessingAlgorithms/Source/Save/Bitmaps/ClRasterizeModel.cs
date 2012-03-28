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
*   @file       RoundAllxyValuesSimplifyModel.cs
*   @brief      RoundAllxyValuesSimplifyModel
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-06-2008
*
*   @history
*   @item		10-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClRasterizeModel : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClRasterizeModel();
        }

        public static string ALGORITHM_NAME = @"Save\Save RASTERIZED Model (real coordinates)(.bmp)";

        public ClRasterizeModel() : base(ALGORITHM_NAME) { }

        public ClRasterizeModel(int p_width, int p_height) : base(ALGORITHM_NAME) 
        {
            NewWidth = p_width;
            NewHeight = p_height;
            m_FixedSize = true;
        }

        private string m_sFilePostFix = "";
        private int NewWidth = 50;
        private int NewHeight = 50;
        private bool m_FixedSize = false;
        private bool rotate180 = false;
        private bool CenterFaceBasedOnNoseTip = false;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("PostFix"))
            {
                m_sFilePostFix = p_sValue;
            }
            else if (p_sProperity.Equals("FixedSize"))
            {
                m_FixedSize = bool.Parse(p_sValue);
            }
            else if (p_sProperity.Equals("width"))
            {
                NewWidth = Int32.Parse(p_sValue);
            }
            else if (p_sProperity.Equals("height"))
            {
                NewHeight = Int32.Parse(p_sValue);
            }
            else if (p_sProperity.Equals("Rotate180"))
            {
                rotate180 = bool.Parse(p_sValue);
            }
            else if (p_sProperity.Equals("CenterFaceBasedOnNoseTip"))
            {
                CenterFaceBasedOnNoseTip = bool.Parse(p_sValue);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("PostFix", m_sFilePostFix.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("FixedSize", m_FixedSize.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("width", NewWidth.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("height", NewHeight.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Rotate180", rotate180.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("CenterFaceBasedOnNoseTip", CenterFaceBasedOnNoseTip.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            float maxX = iter.X;
            float minX = iter.X;
            float maxY = iter.Y;
            float minY = iter.Y;

            do
            {
                if (maxX < iter.X)
                    maxX = iter.X;
                if (maxY < iter.Y)
                    maxY = iter.Y;

                if (minX > iter.X)
                    minX = iter.X;
                if (minY > iter.Y)
                    minY = iter.Y;
            } while (iter.MoveToNext());


            int width = (int)(maxX - minX) + 1;
            int height = (int)(maxY - minY) + 1;


            Bitmap map = new Bitmap(width, height);

            ClRaserizer rasterizer = new ClRaserizer(map, width, height);

            List<ClTools.ClTriangle> triangles = null;

            iter = p_Model.GetIterator();
            do
            {
                ClTools.GetListOfTriangles(out triangles, iter);

                foreach (ClTools.ClTriangle triangle in triangles)
                {
                    int x1 = (int)(((triangle.m_point1.X - minX)/(maxX-minX)) * (width-1));
                    int y1 = (int)(((triangle.m_point1.Y - minY)/(maxY-minY)) * (height-1));

                    int x2 = (int)(((triangle.m_point2.X - minX)/(maxX-minX)) * (width-1));
                    int y2 = (int)(((triangle.m_point2.Y - minY)/(maxY-minY)) * (height-1));

                    int x3 = (int)(((triangle.m_point3.X - minX)/(maxX-minX)) * (width-1));
                    int y3 = (int)(((triangle.m_point3.Y - minY)/(maxY-minY)) * (height-1));


                    rasterizer.DrawTriangle(    triangle.m_point1.Color, x1, y1, 
                                                triangle.m_point2.Color, x2, y2, 
                                                triangle.m_point3.Color, x3, y3                                                 
                                                );
                }
            } while (iter.MoveToNext());


            if (CenterFaceBasedOnNoseTip)
            {
                Cl3DModel.Cl3DModelPointIterator NoseTip = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip);

                int NoseTipX = (int)(((NoseTip.X - minX) / (maxX - minX)) * (width - 1));
                int NoseTipY = (int)(((NoseTip.Y - minY) / (maxY - minY)) * (height - 1));

                int MaxDistanceFromTheNoseTip = (int)Math.Max(Math.Max(NoseTipX, NoseTipY), Math.Max(Math.Abs(width - NoseTipX), Math.Abs(height - NoseTipY)));

                int OffsetX = (int)((MaxDistanceFromTheNoseTip) - NoseTipX);
                int OffsetY = (int)((MaxDistanceFromTheNoseTip) - NoseTipY);

                Bitmap NewMap = new Bitmap((MaxDistanceFromTheNoseTip * 2), (MaxDistanceFromTheNoseTip * 2));

                for (int i = 0; i < width; i++)
                    for (int j = 0; j < height; j++)
                        NewMap.SetPixel(OffsetX + i, OffsetY + j, map.GetPixel(i, j));

                map = NewMap;
            }

            if (m_FixedSize)
            {
                Bitmap bmp = new Bitmap(NewWidth, NewHeight);
                Graphics graphic = Graphics.FromImage((Image)bmp);
                graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphic.DrawImage(map, 0, 0, NewWidth, NewHeight);
                graphic.Dispose();
                map = bmp;
            }

            if (rotate180)
                map.RotateFlip(RotateFlipType.Rotate180FlipNone);
            map.Save(p_Model.ModelFilePath + m_sFilePostFix + ".bmp");

        }
    }
}

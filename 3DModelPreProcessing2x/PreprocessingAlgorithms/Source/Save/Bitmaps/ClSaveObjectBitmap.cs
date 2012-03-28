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
*   @file       ClSaveObjectBitmap.cs
*   @brief      SaveObjectBitmap
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       16-04-2009
*
*   @history
*   @item		16-04-2009 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClSaveObjectBitmap : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClSaveObjectBitmap();
        }

        public static string ALGORITHM_NAME = @"Save\Save Model as Bitmap (range coordinates)(.bmp)";

        private string m_sFilePostFix = "";
        private float m_fPower = 2.0f;
        private bool rotate180 = false;
        private bool m_bBackground = false;

        public ClSaveObjectBitmap() : base(ALGORITHM_NAME) { }
        public ClSaveObjectBitmap(string p_sFilePostFix)
            : base(ALGORITHM_NAME)
        {
            m_sFilePostFix = p_sFilePostFix;
        }

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Power"))
            {
                m_fPower = Single.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (p_sProperity.Equals("PostFix"))
            {
                m_sFilePostFix = p_sValue;
            }
            else if (p_sProperity.Equals("Rotate180"))
            {
                rotate180 = Boolean.Parse(p_sValue);
            }
            else if (p_sProperity.Equals("Background"))
            {
                m_bBackground = Boolean.Parse(p_sValue);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Power", m_fPower.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("PostFix", m_sFilePostFix.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Rotate180", rotate180.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            list.Add(new KeyValuePair<string, string>("Background", m_bBackground.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            string FileName = p_Model.ModelFileFolder + p_Model.ModelFileName + m_sFilePostFix+".bmp";

            Bitmap ModelBitmap = null;
            p_Model.GetBMPImage(out ModelBitmap, m_fPower);
            if(rotate180)
                ModelBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
            if(m_bBackground)
            {
                for (int i = 0; i < ModelBitmap.Width; i++)
                {
                    for (int j = 0; j < ModelBitmap.Height; j++)
                    {
                        Color cl = ModelBitmap.GetPixel(i, j);
                        if (cl.Name.Equals("0") )
                            ModelBitmap.SetPixel(i, j, Color.FromArgb(20,255,0));
                    }
                }
            }
            ModelBitmap.Save(FileName);
        }
    }
}

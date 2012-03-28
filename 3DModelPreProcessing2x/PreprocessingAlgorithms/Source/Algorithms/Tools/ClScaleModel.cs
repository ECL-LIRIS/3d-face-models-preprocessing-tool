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
*   @file       ClScaleModel.cs
*   @brief      ClScaleModel
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-06-2008
*
*   @history
*   @item		10-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClScaleModel : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClScaleModel();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Tools\Scale Model";

        public ClScaleModel() : base(ALGORITHM_NAME) { }

        private float m_fScale = 2.0f;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Scale"))
            {
                m_fScale = Single.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Scale", m_fScale.ToString(System.Globalization.CultureInfo.InvariantCulture)));

            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {

            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();

            while (iter.IsValid())
            {
                iter.X = iter.X * m_fScale;
                iter.Y = iter.Y * m_fScale;
                iter.Z = iter.Z * m_fScale;

                if (!iter.MoveToNext())
                    break;
            }
        }
    }
}

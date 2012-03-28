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
*   @file       ClRotatieModel.cs
*   @brief      Algorithm to crop face
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-06-2008
*
*   @history
*   @item		10-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClRotatieModel : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClRotatieModel();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Tools\Rotate model";

        public ClRotatieModel() : base(ALGORITHM_NAME) { }


        private float m_fAngleX = 0;
        private bool m_bRandomX = false;
        private float m_fAngleY = 0;
        private bool m_bRandomY = false;
        private float m_fAngleZ = 0;
        private bool m_bRandomZ = false;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Angle X"))
            {
                if (p_sValue.Equals("Random"))
                {
                    m_bRandomX = true;
                }
                else
                {
                    m_fAngleX = Single.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            else if (p_sProperity.Equals("Angle Y"))
            {
                if (p_sValue.Equals("Random"))
                {
                    m_bRandomY = true;
                }
                else
                {
                    m_fAngleY = Single.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            else if (p_sProperity.Equals("Angle Z"))
            {
                if (p_sValue.Equals("Random"))
                {
                    m_bRandomZ = true;
                }
                else
                {
                    m_fAngleZ = Single.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            if(!m_bRandomX)
                list.Add(new KeyValuePair<string, string>("Angle X", m_fAngleX.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            else
                list.Add(new KeyValuePair<string, string>("Angle X", "Random"));

            if (!m_bRandomY)
                list.Add(new KeyValuePair<string, string>("Angle Y", m_fAngleY.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            else
                list.Add(new KeyValuePair<string, string>("Angle Y", "Random"));

            if (!m_bRandomZ)
                list.Add(new KeyValuePair<string, string>("Angle Z", m_fAngleZ.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            else
                list.Add(new KeyValuePair<string, string>("Angle Z", "Random"));
           
            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {

            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            
            if (m_bRandomX)
            {
                Random random = new Random();
                m_fAngleX = (int)random.Next(-45, 45);
            }
            if (m_bRandomY)
            {
                Random random = new Random();
                m_fAngleY = (int)random.Next(-90, 90);
            }
            if (m_bRandomZ)
            {
                Random random = new Random();
                m_fAngleZ = (int)random.Next(-30, 30); 
            }

            ClInformationSender.SendInformation("ROTATIONS: X: " + m_fAngleX + " Y: " + m_fAngleY + " Z: " + m_fAngleZ, ClInformationSender.eInformationType.eDebugText);

            while(iter.IsValid())
            {
                float X = iter.X;
                float Y = iter.Y;
                float Z = iter.Z;
                ClTools.RotateXDirection(X, Y, Z, m_fAngleX * (float)(Math.PI / 180), out X, out Y, out Z);
                ClTools.RotateYDirection(X, Y, Z, m_fAngleY * (float)(Math.PI / 180), out X, out Y, out Z);
                ClTools.RotateZDirection(X, Y, Z, m_fAngleZ * (float)(Math.PI / 180), out X, out Y, out Z);

                iter.X = X;
                iter.Y = Y;
                iter.Z = Z;
                
                if (!iter.MoveToNext())
                    break;
            }
        }
    }
}

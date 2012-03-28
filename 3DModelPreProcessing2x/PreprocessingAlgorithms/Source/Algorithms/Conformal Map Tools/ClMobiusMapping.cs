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
using Iridium.Numerics;

using PreprocessingFramework;
/**************************************************************************
*
*                          ModelPreProcessing
*
* Copyright (C)         Przemyslaw Szeptycki 2007     All Rights reserved
*
***************************************************************************/

/**
*   @file       ClCropFaceBySphere.cs
*   @brief      Algorithm to crop face
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-06-2008
*
*   @history
*   @item		10-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClMobiusMapping : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClMobiusMapping();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Conformal Map Tools\Mobius Mapping";

        public ClMobiusMapping() : base(ALGORITHM_NAME) { }
        
        double m_Theta = 0;
        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Theta"))
            {
                m_Theta = Double.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Theta", m_Theta.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return list;
        }

        Complex Mobius( double theta, Complex z0, Complex z )
        {
            //  assert(  (abs(z) <= 1);
            Complex r = new Complex( Math.Cos(theta), Math.Sin(theta) );
            return r * (z - z0)/(1.0 - z0.Conjugate * z );
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator CenterPoint = p_Model.GetSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip);
            

            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            Cl3DModel.Cl3DModelPointIterator max = iter.CopyIterator();
            float distanceMax = 0;

            do
            {
                float distance = (float)Math.Sqrt(Math.Pow(iter.U, 2) + Math.Pow(iter.V, 2));
                if (distanceMax < distance)
                    distanceMax = distance;

            } while (iter.MoveToNext());

            Complex z0 = new Complex(CenterPoint.U / distanceMax, CenterPoint.V / distanceMax);
            iter = p_Model.GetIterator();
            do
            {
                Complex newPoint = Mobius(m_Theta, z0, new Complex((iter.U / distanceMax), (iter.V / distanceMax)));
                iter.U = (float)newPoint.Real * distanceMax;
                iter.V = (float)newPoint.Imag * distanceMax;
            } while (iter.MoveToNext());
            
           
        }
    }
}


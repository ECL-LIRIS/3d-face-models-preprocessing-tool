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
*   @file       ClFindNoseTipMaxVal.cs
*   @brief      Algorithm to find nose tip
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       10-06-2008
*
*   @history
*   @item		10-06-2008 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClFindNoseTipMaxVal : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClFindNoseTipMaxVal();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Anthropometry Points\Nose Tip (Max Z val.)";

        public ClFindNoseTipMaxVal() : base(ALGORITHM_NAME) { }


        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator maxIter = p_Model.GetIterator();
            Cl3DModel.Cl3DModelPointIterator iter = maxIter.CopyIterator();

            while (iter.MoveToNext())
            {
                if (maxIter.Z < iter.Z)
                    maxIter = iter.CopyIterator();
            }

            if (maxIter.IsValid())
            {
                p_Model.AddSpecificPoint(Cl3DModel.eSpecificPoints.NoseTip.ToString(), maxIter);
            }
        }
    }
}

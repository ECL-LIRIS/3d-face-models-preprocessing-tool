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
using Iridium.Numerics.LinearAlgebra;
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
    public class ClRotateToPlaneCut : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClRotateToPlaneCut();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Tools\Rotate model (plain is the cut part)";

        public ClRotateToPlaneCut() : base(ALGORITHM_NAME) { }


        

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            Cl3DModel.Cl3DModelPointIterator Center = p_Model.GetSpecificPoint( Cl3DModel.eSpecificPoints.NoseTip);

            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            List<Cl3DModel.Cl3DModelPointIterator> BoundaryVertexes = new List<Cl3DModel.Cl3DModelPointIterator>();
            Cl3DModel.Cl3DModelPointIterator BeginningPoint = null;
            do
            {
                if (iter.GetListOfNeighbors().Count <= 7)
                {
                    iter.AlreadyVisited = true;
                   // iter.Color = Color.Red;

                    if (BeginningPoint == null)
                        BeginningPoint = iter.CopyIterator();

                    BoundaryVertexes.Add(iter.CopyIterator());
                }
            } while (iter.MoveToNext());

            Cl3DModel.Cl3DModelPointIterator CurrentPoint = BeginningPoint.CopyIterator();
            Vector NormalVector = new Vector(3);
            bool IsNextInTheQue = false;
            float CurrentNo = 0;
            do
            {
                CurrentPoint.AlreadyVisited = false;
                Cl3DModel.Cl3DModelPointIterator NextPoint = null;
                IsNextInTheQue = false;    
                foreach (Cl3DModel.Cl3DModelPointIterator neighbor in CurrentPoint.GetListOfNeighbors())
                {
                    if (neighbor.AlreadyVisited) // means next in the que
                    {
                        NextPoint = neighbor.CopyIterator();
                        IsNextInTheQue = true;
                        break;
                    }
                }
                if (NextPoint == null)
                    break;
                
                Vector tmpVectorCurr = new Vector(new double[] { (CurrentPoint.X - Center.X), (CurrentPoint.Y - Center.Y), (CurrentPoint.Z - Center.Z) });
                Vector tmpVectorNext = new Vector(new double[] { (NextPoint.X - Center.X), (NextPoint.Y - Center.Y), (NextPoint.Z - Center.Z) });

                NormalVector[0] += (tmpVectorNext[1] * tmpVectorCurr[2] - tmpVectorNext[2] * tmpVectorCurr[1]);
                NormalVector[1] += (tmpVectorNext[2] * tmpVectorCurr[0] - tmpVectorNext[0] * tmpVectorCurr[2]);
                NormalVector[2] += (tmpVectorNext[0] * tmpVectorCurr[1] - tmpVectorNext[1] * tmpVectorCurr[0]);

                NormalVector /= 2;
                CurrentPoint.Color = ClTools.GetColorRGB(CurrentNo / BoundaryVertexes.Count, 1f);
                CurrentNo++;

                CurrentPoint = NextPoint.CopyIterator();
            } while (IsNextInTheQue);

            //NormalVector = NormalVector.Normalize();
            Center.AddNeighbor(p_Model.AddPointToModel((float)NormalVector[0]+ Center.X, (float)NormalVector[1] + Center.Y, (float)NormalVector[2] + Center.Z));


         /*   Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            while(iter.IsValid())
            {
                float X = iter.X;
                float Y = iter.Y;
                float Z = iter.Z;
                ClTools.RotateXDirection(X, Y, Z, agnleX, out X, out Y, out Z);
                ClTools.RotateYDirection(X, Y, Z, angleY, out X, out Y, out Z);
                ClTools.RotateZDirection(X, Y, Z, agnleZ, out X, out Y, out Z);

                iter.X = X;
                iter.Y = Y;
                iter.Z = Z;
                
                if (!iter.MoveToNext())
                    break;
            }
          */ 
        }
    }
}


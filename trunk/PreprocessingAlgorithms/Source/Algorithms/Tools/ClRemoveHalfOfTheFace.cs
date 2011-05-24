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
*   @file       ClRemoveHalfOfTheFace.cs
*   @brief      ClRemoveHalfOfTheFace
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       03-04-2009
*
*   @history
*   @item		03-04-2009 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClRemoveHalfOfTheFace : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClRemoveHalfOfTheFace();
        }

        public static string ALGORITHM_NAME = @"Algorithms\Tools\Remove Half Of The Face";

        public ClRemoveHalfOfTheFace() : base(ALGORITHM_NAME) { }


        float m_procentToRemove = 0.5f;

        public override void SetProperitis(string p_sProperity, string p_sValue)
        {
            if (p_sProperity.Equals("Procent To Remove"))
            {
                m_procentToRemove = Single.Parse(p_sValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
                throw new Exception("Unknown properity: " + p_sProperity);
        }

        public override List<KeyValuePair<string, string>> GetProperitis()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("Procent To Remove", m_procentToRemove.ToString(System.Globalization.CultureInfo.InvariantCulture)));

            return list;
        }

        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            if (p_Model.ModelType != "abs")
                throw new Exception("Remove Half Of The Face works only for ABS files");

            Cl3DModel.Cl3DModelPointIterator iter = p_Model.GetIterator();
            if (!iter.IsValid())
                throw new Exception("Model iterator not Valid");

            int MinX;
            int MaxX;
            int MinY;
            int MaxY;

            MinX = iter.RangeImageX;
            MaxX = iter.RangeImageX;
            MinY = iter.RangeImageY;
            MaxY = iter.RangeImageY;

            do
            {
                if (MinX > iter.RangeImageX)
                    MinX = iter.RangeImageX;
                if (MaxX < iter.RangeImageX)
                    MaxX = iter.RangeImageX;

                if (MinY > iter.RangeImageY)
                    MinY = iter.RangeImageY;
                if (MaxY < iter.RangeImageY)
                    MaxY = iter.RangeImageY;

            } while (iter.MoveToNext());

            int Width = (MaxX - MinX) + 1;
            int Height = (MaxY - MinY) + 1;
            Cl3DModel.Cl3DModelPointIterator[,] map = new Cl3DModel.Cl3DModelPointIterator[Width, Height];

            iter = p_Model.GetIterator();
            do
            {
                map[iter.RangeImageX - MinX, iter.RangeImageY - MinY] = iter.CopyIterator();
            } while (iter.MoveToNext());

            int procent = (int)(Width * m_procentToRemove);

            for (int i = procent; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    p_Model.RemovePointFromModel(map[i, j]);
                }
            }
        }
    }
}

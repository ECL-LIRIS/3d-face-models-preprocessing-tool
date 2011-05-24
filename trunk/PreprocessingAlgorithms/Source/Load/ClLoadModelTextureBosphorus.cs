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
    public class ClLoadModelTextureBosphorus : ClBaseFaceAlgorithm
    {
        public static IFaceAlgorithm CrateAlgorithm()
        {
            return new ClLoadModelTextureBosphorus();
        }

        public static string ALGORITHM_NAME = @"Load\Load Model Texture Bosphorus (*.png)";

        public ClLoadModelTextureBosphorus() : base(ALGORITHM_NAME) { }


        protected override void Algorithm(ref Cl3DModel p_Model)
        {
            string name = "";
            if (p_Model.ModelType.Equals("bnt"))
            {
                name = p_Model.ModelFileFolder + p_Model.ModelFileName + ".png";
            }
            else
                throw new Exception("Method does not supprt this kind of files");

            Bitmap Texture = new Bitmap(name);


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

            iter = p_Model.GetIterator();
            do
            {
                float x = ((float)iter.RangeImageX-MinX)/(MaxX-MinX+1);
                float y = ((float)iter.RangeImageY-MinY)/(MaxY-MinY+1);
                iter.Color = Texture.GetPixel( (int)(x *Texture.Width) ,(int)(y*Texture.Height));
                iter.AddSpecificValue("Texture", iter.Color.ToArgb());
            } while (iter.MoveToNext());
             
               
        }
    }
}

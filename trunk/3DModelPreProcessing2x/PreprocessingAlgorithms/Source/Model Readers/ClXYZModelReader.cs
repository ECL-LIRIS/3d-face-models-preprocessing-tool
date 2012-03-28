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

using PreprocessingFramework;
/**************************************************************************
*
*                          ModelPreProcessing
*
* Copyright (C)         Przemyslaw Szeptycki 2007     All Rights reserved
*
***************************************************************************/

/**
*   @file       ClXYZModelReader.cs
*   @brief      Class responsible for reading models from XYZ files 
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       30-03-2009
*
*   @history
*   @item		30-03-2009 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClXYZModelReader : ClBaseModelReader
    {
        public ClXYZModelReader()
            : base("xyz")
        {
        }

        public override void Read(Cl3DModel p_mModel3D, string p_sFilePath)
        {
            try
            {
                using (FileStream fs = File.OpenRead(p_sFilePath))
                {
                    BinaryReader ReaderBinary = new BinaryReader(fs);
                    while (true)
                    {
                        try
                        {
                            short number = ReaderBinary.ReadInt16();
                            for (short i = 0; i < number; i++)
                            {
                                short X = ReaderBinary.ReadInt16();
                                short Y = ReaderBinary.ReadInt16();
                                short Z = ReaderBinary.ReadInt16();

                                p_mModel3D.AddPointToModel((float)X/10.0f, (float)Y/10.0f, (float)-Z/10.0f);
                            }
                        }
                        catch (System.IO.EndOfStreamException)
                        {
                            break;
                        }
                    }
                    fs.Close();
                }
            }
            catch (Exception e)
            {
                p_mModel3D.ResetModel();
                throw e;
            }
        }
    }
}

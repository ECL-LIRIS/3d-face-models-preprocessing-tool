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

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
*   @file       ClRifModelReader.cs
*   @brief      Class responsible for reading models from RIF files 
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       25-04-2009
*
*   @history
*   @item		25-04-2009 Przemyslaw Szeptycki     created at ECL (普查迈克) (بشاماك)
*/
namespace Preprocessing
{
    public class ClPlyModelReader : ClBaseModelReader
    {
        public ClPlyModelReader()
            : base("ply")
        {
        }

        public override void Read(Cl3DModel p_mModel3D, string p_sFilePath)
        {

                using (StreamReader FileStream = File.OpenText(p_sFilePath))
                {

                    string line = "";
                    int vertexCount = -1;
                    int faceCount = -1;
                    line = FileStream.ReadLine();
                    if (line== null || !line.Equals("ply"))
                        throw new Exception("Incorrect file format");

                    line = FileStream.ReadLine();
                    if (line == null || !line.Equals("format binary_little_endian 1.0"))
                        throw new Exception("Incorrect file format (format binary_little_endian 1.0)");

                    if((line = FileStream.ReadLine())==null)
                        throw new Exception("Incorrect file format");

                    if (line.Contains("element vertex"))
                    {
                        vertexCount = Int32.Parse(line.Substring(line.LastIndexOf(' ')));
                    }

                    line = FileStream.ReadLine();
                    if (line == null || !line.Equals("property float x"))
                        throw new Exception("Incorrect file format (property float x)");

                    line = FileStream.ReadLine();
                    if (line == null || !line.Equals("property float y"))
                        throw new Exception("Incorrect file format (property float y)");

                    line = FileStream.ReadLine();
                    if (line == null || !line.Equals("property float z"))
                        throw new Exception("Incorrect file format (property float z)");

                    if ((line = FileStream.ReadLine()) == null)
                        throw new Exception("Incorrect file format");

                    if (line.Contains("element face"))
                    {
                        faceCount = Int32.Parse(line.Substring(line.LastIndexOf(' ')));
                    }

                    line = FileStream.ReadLine();
                    if (line == null || !line.Equals("property list uchar int vertex_indices"))
                        throw new Exception("Incorrect file format (property list uchar int vertex_indices)");
                    
                    line = FileStream.ReadLine();
                    if (line == null || !line.Equals("end_header"))
                        throw new Exception("Incorrect file format (end_header)");


                    BinaryReader ReaderBinary = new BinaryReader(FileStream.BaseStream);

                    int i = 0;
                    for (i = 0; i < vertexCount; i++)
                    {
                        float X = ReaderBinary.ReadSingle();
                        float Y = ReaderBinary.ReadSingle();
                        float Z = ReaderBinary.ReadSingle();
                        p_mModel3D.AddPointToModel(X, Y, Z);
                    }

                }
        }
    }
}

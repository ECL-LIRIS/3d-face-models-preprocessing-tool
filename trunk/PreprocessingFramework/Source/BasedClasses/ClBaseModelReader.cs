using System;
using System.Collections.Generic;
using System.Text;

using PreprocessingFramework;
/**************************************************************************
*
*                          ModelPreProcessing
*
* Copyright (C)         Przemyslaw Szeptycki 2007     All Rights reserved
*
***************************************************************************/

/**
*   @file       ClModelReader.cs
*   @brief      This is an based class for classes witch read models from files
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       05-06-2008
*
*   @history
*   @item		05-06-2008 Przemyslaw Szeptycki     created at ECL
*/

namespace PreprocessingFramework
{
    public abstract class ClBaseModelReader : IModelReader
    {
        private string m_sReaderFileExtension;

        public ClBaseModelReader(string p_sReaderFileExtension)
        {
            m_sReaderFileExtension = p_sReaderFileExtension;
        }

        public virtual void ReadModel(Cl3DModel p_mModel3D, string p_sFilePath)
        {
            if (m_sReaderFileExtension.Equals(p_mModel3D.ModelType))
            {
                try
                {
                    Read(p_mModel3D, p_sFilePath);
                }
                catch (Exception)
                {
                    p_mModel3D.ResetModel();
                    throw;
                }
            }
            else
                throw new ArgumentException("Extension of file doesn't fit to reader object");
        }

        public virtual string GetFileExtension()
        {
            return m_sReaderFileExtension;
        }

        public abstract void Read(Cl3DModel p_mModel3D, string p_sFilePath);
    }
}

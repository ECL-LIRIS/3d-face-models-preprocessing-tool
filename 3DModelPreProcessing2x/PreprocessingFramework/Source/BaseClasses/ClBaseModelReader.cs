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

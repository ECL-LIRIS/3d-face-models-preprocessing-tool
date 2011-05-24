using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
/**************************************************************************
*
*                          ModelPreProcessing
*
* Copyright (C)         Przemyslaw Szeptycki 2007     All Rights reserved
*
***************************************************************************/

/**
*   @file       IRenderObject.cs
*   @brief      This is an interface for classes witch would like to reander something on scene
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       25-10-2007
*
*   @history
*   @item		25-10-2007 Przemyslaw Szeptycki     created at ECL
*/

namespace ModelPreProcessing
{
    public interface IRenderObject
    {
        /**
         * 
         * @brief   -   Render method
         * @params  -   Device - we paint by this, Control - it is window, our paint surface
         * 
         **/
        void Render(Device p_dDevice, Control p_cRenderWindow);
        
    }
}

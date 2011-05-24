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
*   @file       IEventHandler.cs
*   @brief      This is an interface for classes witch would like to hadle events
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @date       29-10-2007
*
*   @history
*   @item		29-10-2007 Przemyslaw Szeptycki     created at ECL
*/
namespace ModelPreProcessing
{
    public interface IEventHandler
    {
        void MouseButtonDown(object sender, MouseEventArgs e);

        void MouseButtonUp(object sender, MouseEventArgs e);

        void MouseMove(object sender, MouseEventArgs e);

        void MouseWheel(object sender, MouseEventArgs e);

        void KeyDown(object sender, KeyEventArgs e);

        void KeyUp(object sender, KeyEventArgs e);
    }
}

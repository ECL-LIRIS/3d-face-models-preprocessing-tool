using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
/**************************************************************************
*
*                          ModelPreProcessing
*
* Copyright (C)         Przemyslaw Szeptycki 2007     All Rights reserved
*
***************************************************************************/

/**
*   @file       CScrollPanelMessageFilter.cs
*   @brief      Class to get MOUSEWHEEL mesages and applay them to panel
*   @author     Przemyslaw Szeptycki <pszeptycki@gmail.com>
*   @howToUse   //Application.AddMessageFilter(new CScrollPanelMessageFilter(Form.GetPanel()));
*   @date       18-11-2007
*
*   @history
*   @item		18-11-2007 Przemyslaw Szeptycki     created at ECL
*/
namespace ModelPreProcessing
{
    internal class CScrollPanelMessageFilter : IMessageFilter
    {
        int WM_MOUSEWHEEL = 0x20A;
        Panel panel;

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);
        [DllImport("User32.dll")]
        static extern Int32 SendMessage(int hWnd, int Msg, int wParam, int lParam);

        public CScrollPanelMessageFilter(Panel panel)
        {
            this.panel = panel;
        }

        public bool PreFilterMessage(ref Message m)
        {
            //filter out all other messages except than mousewheel
            //also only proceed with processing if the panel is focusable, no controls on the panel have focus 
            //and the vertical scroll bar is visible
            if (m.Msg == WM_MOUSEWHEEL )
            {
                //is mouse cordinates over the panel display rectangle?
                Rectangle rect = panel.RectangleToScreen(panel.ClientRectangle);
                Point cursorPoint = new Point();
                GetCursorPos(ref cursorPoint);
                if ((cursorPoint.X > rect.X && cursorPoint.X < rect.X + rect.Width) &&
                    (cursorPoint.Y > rect.Y && cursorPoint.Y < rect.Y + rect.Height))
                {
                    //send the mouse wheel message to the panel.
                    SendMessage((int)panel.Handle, m.Msg, (Int32)m.WParam, (Int32)m.LParam);
                    return true;
                }
            }
            return false;
        }

    }
}


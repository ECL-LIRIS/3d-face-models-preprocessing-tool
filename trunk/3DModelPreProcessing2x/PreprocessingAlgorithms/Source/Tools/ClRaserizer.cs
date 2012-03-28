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
using System.Drawing;

namespace Preprocessing
{
    public class Edge
    {
        public MyColor Color1;
        public MyColor Color2;
        public int X1;
        public int Y1;
        public int X2;
        public int Y2;

            public Edge(MyColor color1, int x1, int y1, MyColor color2, int x2, int y2)
            {
                if (y1 < y2)
                {
                    Color1 = color1;
                    X1 = x1;
                    Y1 = y1;
                    Color2 = color2;
                    X2 = x2;
                    Y2 = y2;
                }
                else
                {
                    Color1 = color2;
                    X1 = x2;
                    Y1 = y2;
                    Color2 = color1;
                    X2 = x1;
                    Y2 = y1;
                }
            }
    }

    public class Span
    {
        public MyColor Color1;
        public MyColor Color2;
		public int X1;
        public int X2;

        public Span(MyColor color1, int x1, MyColor color2, int x2)
        {
            if (x1 < x2)
            {
                Color1 = color1;
                X1 = x1;
                Color2 = color2;
                X2 = x2;
            }
            else
            {
                Color1 = color2;
                X1 = x2;
                Color2 = color1;
                X2 = x1;
            }
        }
    }

    public class MyColor
    {
        public int R = 0;
        public int G = 0;
        public int B = 0;

        public MyColor(Color p_color)
        {
            R = 0;
            R = (R << 8) + (int)p_color.R;
            G = 0;
            G = (G << 8) + (int)p_color.G;
            B = 0;
            B = (B << 8) + (int)p_color.B;
        }

        public MyColor(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }

        public static MyColor operator -(MyColor left, MyColor right)
        {
            return new MyColor(left.R - right.R, left.G - right.G, left.B - right.B);
        }
        public static MyColor operator +(MyColor left, MyColor right)
        {
            return new MyColor(left.R + right.R, left.G + right.G, left.B + right.B);
        }
        public static MyColor operator *(MyColor left, float div)
        {
            if (div <= 1 && div >= 0)
                return new MyColor((int)(left.R * div), (int)(left.G * div), (int)(left.B * div));
            else
                throw new Exception("Color multiplication can be between 0 and 1");
        }
    }

    public class ClRaserizer
    {
		private Bitmap m_FrameBuffer;
		private int m_Width, m_Height;

        public ClRaserizer(Bitmap frameBuffer, int width, int height)
        {
            m_FrameBuffer = frameBuffer;
            m_Width = width;
            m_Height = height;
        }

        private void DrawSpan(Span span, int y)
        {
            int xdiff = span.X2 - span.X1;
            if (xdiff == 0)
                return;

            MyColor colordiff = span.Color2 - span.Color1;

            float factor = 0.0f;
            float factorStep = 1.0f / (float)xdiff;

            // draw each pixel in the span
            for (int x = span.X1; x < span.X2; x++)
            {
                SetPixel(x, y, span.Color1 + (colordiff * factor));
                factor += factorStep;
            }
        }

        private void DrawSpansBetweenEdges(Edge e1, Edge e2)
        {
            // calculate difference between the y coordinates
	        // of the first edge and return if 0
	        float e1ydiff = (float)(e1.Y2 - e1.Y1);
	        if(e1ydiff == 0.0f)
		        return;

	        // calculate difference between the y coordinates
	        // of the second edge and return if 0
	        float e2ydiff = (float)(e2.Y2 - e2.Y1);
	        if(e2ydiff == 0.0f)
		        return;

	        // calculate differences between the x coordinates
	        // and colors of the points of the edges
	        float e1xdiff = (float)(e1.X2 - e1.X1);
	        float e2xdiff = (float)(e2.X2 - e2.X1);
            MyColor e1colordiff = e1.Color2 - e1.Color1;
            MyColor e2colordiff = e2.Color2 - e2.Color1;

	        // calculate factors to use for interpolation
	        // with the edges and the step values to increase
	        // them by after drawing each span
	        float factor1 = (float)(e2.Y1 - e1.Y1) / e1ydiff;
	        float factorStep1 = 1.0f / e1ydiff;
	        float factor2 = 0.0f;
	        float factorStep2 = 1.0f / e2ydiff;

	        // loop through the lines between the edges and draw spans
	        for(int y = e2.Y1; y < e2.Y2; y++) 
            {
		        // create and draw span
		        Span span = new Span(e1.Color1 + (e1colordiff * factor1), 
                                      e1.X1 + (int)(e1xdiff * factor1),
		                              e2.Color1 + (e2colordiff * factor2),
		                              e2.X1 + (int)(e2xdiff * factor2)
                                      );
		        
                DrawSpan(span, y);

		        // increase factors
		        factor1 += factorStep1;
		        factor2 += factorStep2;
	        }
        }

        public void SetPixel(int x, int y, MyColor color)
        {
            if (x >= m_Width || y >= m_Height)
                return;

            Color setColor = Color.FromArgb(color.R, color.G, color.B);
            m_FrameBuffer.SetPixel(x, y, setColor);
        }

        public void DrawTriangle(Color color1, float x1, float y1, Color color2, float x2, float y2, Color color3, float x3, float y3)
        {
            // create edges for the triangle
	        Edge[] edges = new Edge[3];

            MyColor Mycolor1 = new MyColor(color1);
            MyColor Mycolor2 = new MyColor(color2);
            MyColor Mycolor3 = new MyColor(color3);

            edges[0] = new Edge(Mycolor1, (int)x1, (int)y1, Mycolor2, (int)x2, (int)y2);
            edges[1] = new Edge(Mycolor2, (int)x2, (int)y2, Mycolor3, (int)x3, (int)y3);
            edges[2] = new Edge(Mycolor3, (int)x3, (int)y3, Mycolor1, (int)x1, (int)y1);

	        int maxLength = 0;
	        int longEdge = 0;

	        // find edge with the greatest length in the y axis
	        for(int i = 0; i < 3; i++) 
            {
		        int length = edges[i].Y2 - edges[i].Y1;
		        if(length > maxLength) 
                {
			        maxLength = length;
			        longEdge = i;
		        }
	        }

	        int shortEdge1 = (longEdge + 1) % 3;
	        int shortEdge2 = (longEdge + 2) % 3;

	        // draw spans between edges; the long edge can be drawn
	        // with the shorter edges to draw the full triangle
	        DrawSpansBetweenEdges(edges[longEdge], edges[shortEdge1]);
	        DrawSpansBetweenEdges(edges[longEdge], edges[shortEdge2]);
        }

        public void DrawLine(MyColor color1, float x1, float y1, MyColor color2, float x2, float y2)
        {
            float xdiff = (x2 - x1);
            float ydiff = (y2 - y1);

            if (xdiff == 0.0f && ydiff == 0.0f)
            {
                SetPixel((int)x1, (int)y1, color1);
                return;
            }

            if (Math.Abs(xdiff) > Math.Abs(ydiff))
            {
                float xmin, xmax;

                // set xmin to the lower x value given
                // and xmax to the higher value
                if (x1 < x2)
                {
                    xmin = x1;
                    xmax = x2;
                }
                else
                {
                    xmin = x2;
                    xmax = x1;
                }

                // draw line in terms of y slope
                float slope = ydiff / xdiff;
                for (float x = xmin; x <= xmax; x += 1.0f)
                {
                    float y = y1 + ((x - x1) * slope);
                    MyColor color = color1 + ((color2 - color1) * ((x - x1) / xdiff));
                    SetPixel((int)x, (int)y, color);
                }
            }
            else
            {
                float ymin, ymax;

                // set ymin to the lower y value given
                // and ymax to the higher value
                if (y1 < y2)
                {
                    ymin = y1;
                    ymax = y2;
                }
                else
                {
                    ymin = y2;
                    ymax = y1;
                }

                // draw line in terms of x slope
                float slope = xdiff / ydiff;
                for (float y = ymin; y <= ymax; y += 1.0f)
                {
                    float x = x1 + ((y - y1) * slope);
                    MyColor color = color1 + ((color2 - color1) * ((y - y1) / ydiff));
                    SetPixel((int)x, (int)y, color);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {

        // Die Common Language Runtime steuert das physische Layout der Datenfelder einer Klasse oder Struktur im verwalteten
        // Speicher.Wenn Sie jedoch den Typ an nicht verwalteten Code übergeben möchten, verwenden Sie können StructLayoutAttribute-Attribut, um den nicht verwalteten Layouts des Typs zu steuern. ...
        // [StructLayout(LayoutKind.Sequential)] // siehe http://msdn.microsoft.com/de-de/library/system.runtime.interopservices.structlayoutattribute.aspx
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            //public static implicit operator System.Drawing.Point(POINT p)
            //{
            //    return new System.Drawing.Point(p.X, p.Y);
            //}

            //public static implicit operator POINT(System.Drawing.Point p)
            //{
            //    return new POINT(p.X, p.Y);
            //}
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr hWnd, String text, String caption, uint type); // extern um mit Hilfe von InteropServices auf nicht verwalteten Code zuzugreifen

        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);
        //        hWnd [in] 
        //                  A handle to the window whose DC is to be retrieved. If this value is NULL, GetDC retrieves the DC for the entire screen.

        //        Return value
        //                 If the function succeeds, the return value is a handle to the DC for the specified window's client area.
        //                 If the function fails, the return value is NULL.

        [DllImport("user32.dll")]
        static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern bool DeleteDC(IntPtr hdc); // man sollte immer den geholten Device Context wieder freigeben wenn dieser nicht mehr gebraucht wird

        static void Main(string[] args)
        {
            SetCursorPos(900, 500);

            POINT p;
            uint pixel;
            IntPtr hdc;
            IntPtr z = new IntPtr(0);
            hdc = GetDC(z);
            int r = 0;
            //hdc = GetDC(z);

            while (true)
            {
                hdc = GetDC(z);
                GetCursorPos(out p);
                pixel = GetPixel(hdc, p.X, p.Y);
                r = ReleaseDC(z, hdc);
                //if (pixel == 4294967295) // wenn pixel außerhalb des Auswahlbereichs (normalerweise ist der ganze Bildschierm der auswahlbereich duch IntPtr(0) 
                //    r = ReleaseDC(z, hdc);
                //    hdc = GetDC(z);

                if (r == 1) Console.WriteLine("DC released");
                else Console.WriteLine("DC NOT released");

                Console.WriteLine("x: {0} - y: {1}", p.X, p.Y);
                Console.WriteLine("color: 0x{0}", pixel.ToString("X"));
                Thread.Sleep(30);
                Console.Clear();

            }
        }
    }
}

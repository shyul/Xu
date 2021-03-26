/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Xu.WindowsNativeMethods;

namespace Xu
{
    public static class GUI
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Bitmap ToBitmap(this Control c)
        {
            Bitmap newPic = new(c.Width, c.Height);
            c.DrawToBitmap(newPic, c.ClientRectangle);
            return newPic;
        }

        public static Bitmap CaptureDesktopRegion(Point location, Size size)
        {

            Bitmap myImage = new(size.Width, size.Height);

            using (Graphics g = Graphics.FromImage(myImage))
            {

                IntPtr destDeviceContext = g.GetHdc();
                IntPtr srcDeviceContext = User32.GetWindowDC(IntPtr.Zero); // capture desktop

                // TODO: throw exception
                bool result = Gdi32.BitBlt(destDeviceContext, 0, 0, size.Width, size.Height, srcDeviceContext, location.X, location.Y, TernaryRasterOperations.SRCCOPY);

                if (!result)
                {
                    // TODO: call GetLastError to dig down to the core of the problem.
                    throw new Exception("There was a problem with the BitBlt function call.");
                }

                User32.ReleaseDC(IntPtr.Zero, srcDeviceContext);
                g.ReleaseHdc(destDeviceContext);
            } // dispose the Graphics object

            return myImage;

        }

        public static void Invoke(this Control c, Action action)
        {
            if (!c.IsDisposed && c.InvokeRequired)
            {
                c?.Invoke((MethodInvoker)delegate { action?.Invoke(); });
            }
            else
            {
                action?.Invoke();
            }
        }

        /*
                 public static void Invoke(this Control c, Action action)
        {
            if (c.InvokeRequired)
            {
                c?.Invoke(() => action?.Invoke());
            }
            else
            {
                action?.Invoke();
            }
        }
         
         */

        public static void Add<T>(this ComboBox.ObjectCollection items) where T : Enum // struct, IConvertible
        {
            foreach (T item in ReflectionTool.ToArray<T>())
            {
                items.Add(item);
            }
        }

        [Obsolete]
        public static Image StretchImage(Image sourceImage, Point p, Rectangle zoomRegion, Size desiredSize)
        {
            Image myImage = new Bitmap(desiredSize.Width, desiredSize.Height);

            using (Graphics g = Graphics.FromImage(myImage))
            {

                // use the DrawImage method in the framework to achieve a 
                // similar, but not as pixelated effect.
                //				g.DrawImage( sourceImage, 
                //					new Rectangle( new Point( 0 ), desiredSize ),
                //					zoomRegion,
                //					GraphicsUnit.Pixel );

                // we're dealing with memory device contexts here. first, a 
                // device context for the screen is retrieved. this serves as the
                // handle to the device context. we then create a memory device 
                // context that is compatible with the device context of the
                // screen (desktop).

                IntPtr hdc = User32.GetDC(IntPtr.Zero);
                IntPtr memoryDC = Gdi32.CreateCompatibleDC(hdc);

                // we retrieve a handle to the source bitmap object. this is 
                // then used to select the bitmap into the memory device 
                // context.

                IntPtr hBitmap = ((Bitmap)sourceImage).GetHbitmap();
                IntPtr oldObject = Gdi32.SelectObject(memoryDC, hBitmap);

                // Delete the object that was previously selected into the 
                // memory device context (there is a default bitmap object
                // that is encapsulated by the returned object, so we need
                // to make sure we get rid of that, otherwise we'll have
                // gdi object leaks).

                Gdi32.DeleteObject(oldObject);

                // the handle to the destination device context is retrieved.
                IntPtr destDeviceContext = g.GetHdc();

                // the source bitmap is stretched and its bits are blitted to 
                // the canvas represented by the destination device context.

                bool result = User32.StretchBlt(destDeviceContext, 0, 0, desiredSize.Width, desiredSize.Height, memoryDC, p.X, p.Y, zoomRegion.Width, zoomRegion.Height, TernaryRasterOperations.SRCCOPY);

                // error checking - StretchBlt returns non-zero on success.

                if (!result)
                {

                    // TODO: call GetLastError to dig down to the core of the problem.
                    //throw new ImageManipulationException("There was a problem with the StretchBlt function call.");

                }

                // release everything
                g.ReleaseHdc(destDeviceContext);

                // clear the previously selected bitmap object from the memory
                // device context.
                Gdi32.DeleteObject(hBitmap);

                // get rid of the memory device context.
                Gdi32.DeleteDC(memoryDC);

                // return the device context to the device context pool.
                User32.ReleaseDC(IntPtr.Zero, hdc);

            } // dispose the Graphics object

            return myImage;

        }

        public static readonly IntPtr MSG_HANDLED = new(0);

        public static void Suspend(this Control c)
        {
            if (c != null && !c.IsDisposed)
            {
                SendMessage(c, WindowsMessages.SETREDRAW, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public static void Resume(this Control c)
        {
            if (c != null && !c.IsDisposed)
            {
                SendMessage(c, WindowsMessages.SETREDRAW, (IntPtr)(1), IntPtr.Zero);
                c.Invalidate(true);
            }
        }

        /// <summary>
        /// Send Message on behave of the control selected.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private static IntPtr SendMessage(Control control, int msg, IntPtr wParam, IntPtr lParam)
        {
            MethodInfo WndProc = control.GetType().GetMethod(
                "WndProc",
                BindingFlags.NonPublic |
                BindingFlags.InvokeMethod |
                BindingFlags.FlattenHierarchy |
                BindingFlags.IgnoreCase |
                BindingFlags.Instance);

            object[] args = new object[] { new Message() {
                HWnd = control.Handle,
                LParam = lParam,
                WParam = wParam,
                Msg = (int)msg
                } };

            WndProc.Invoke(control, args);
            return ((Message)args[0]).Result;
        }

        public static float ScalingFactor()
        {
            IntPtr desktop = Graphics.FromHwnd(IntPtr.Zero).GetHdc();
            IntPtr LogicalScreenHeight = Gdi32.GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            IntPtr PhysicalScreenHeight = Gdi32.GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);
            float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;
            return ScreenScalingFactor; // 1.25 = 125%
        }
    }
}

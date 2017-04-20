using System; // IntPtr, Random
using System.Diagnostics; // Stopwatch, Process
using System.Drawing; // Bitmap, Graphics
using System.Runtime.InteropServices; // DllImport
using System.Windows.Forms; // Screen

using NatiTools.xMath;

namespace NatiTools.xSystem
{
    public static class SystemTools
    {
        #region PInvoke
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        #endregion

        #region struct Rect
        private struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }
        #endregion

        #region GetProcess
        public static Process GetProcess(string processName)
        { return Process.GetProcessesByName(processName)[0]; }

        public static bool GetProcess(string processName, out Process process)
        {
            Process[] processes = Process.GetProcessesByName(processName);

            if (processes == null || processes.Length == 0)
            {
                process = new Process();
                return false;
            }
            else
            {
                process = processes[0];
                return true;
            }
        }
        #endregion
        #region GetWindow
        public static bool GetWindow(string processName, out Rectangle processRectangle)
        {
            Process proc;

            if (GetProcess(processName, out proc))
            {
                Rect procRect = new Rect();

                if (GetWindowRect(proc.MainWindowHandle, ref procRect))
                {
                    processRectangle = new Rectangle(procRect.Left,
                                                     procRect.Top,
                                                     procRect.Right - procRect.Left,
                                                     procRect.Bottom - procRect.Top);

                    return true;
                }
            }

            processRectangle = new Rectangle();
            return false;
        }

        public static Rectangle GetWindow(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);

            if (processes == null || processes.Length == 0)
                return new Rectangle(-1, -1, -1, -1);
            else
                return GetWindow(processes[0]);
        }

        public static Rectangle GetWindow(Process process)
        {
            return GetWindow(process.MainWindowHandle);
        }

        public static Rectangle GetWindow(IntPtr processMainWindowHandle)
        {
            Rect processRect = new Rect();
            GetWindowRect(processMainWindowHandle, ref processRect);
            return new Rectangle(processRect.Left, processRect.Top, processRect.Right - processRect.Left, processRect.Bottom - processRect.Top);
        }
        #endregion
        #region GetScreenCenter
        public static Point GetScreenCenter()
        {
            return Geometry.getCenter(Screen.PrimaryScreen.Bounds);
        }
        #endregion

        #region Sleep
        public static void Sleep(int milliseconds)
        {
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            while (sw.ElapsedMilliseconds < milliseconds) { }
            sw.Stop();
        }
        #endregion

        #region enum SetWindowPosFlags : uint
        [Flags]
        private enum SetWindowPosFlags : uint
        {
            /// <summary>If the calling thread and the thread that owns the window are attached to different input queues, 
            /// the system posts the request to the thread that owns the window. This prevents the calling thread from 
            /// blocking its execution while other threads process the request.</summary>
            /// <remarks>SWP_ASYNCWINDOWPOS</remarks>
            AsynchronousWindowPosition = 0x4000,

            /// <summary>Prevents generation of the WM_SYNCPAINT message.</summary>
            /// <remarks>SWP_DEFERERASE</remarks>
            DeferErase = 0x2000,

            /// <summary>Draws a frame (defined in the window's class description) around the window.</summary>
            /// <remarks>SWP_DRAWFRAME</remarks>
            DrawFrame = 0x0020,

            /// <summary>Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to 
            /// the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE 
            /// is sent only when the window's size is being changed.</summary>
            /// <remarks>SWP_FRAMECHANGED</remarks>
            FrameChanged = 0x0020,

            /// <summary>Hides the window.</summary>
            /// <remarks>SWP_HIDEWINDOW</remarks>
            HideWindow = 0x0080,

            /// <summary>Does not activate the window. If this flag is not set, the window is activated and moved to the 
            /// top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter 
            /// parameter).</summary>
            /// <remarks>SWP_NOACTIVATE</remarks>
            DoNotActivate = 0x0010,

            /// <summary>Discards the entire contents of the client area. If this flag is not specified, the valid 
            /// contents of the client area are saved and copied back into the client area after the window is sized or 
            /// repositioned.</summary>
            /// <remarks>SWP_NOCOPYBITS</remarks>
            DoNotCopyBits = 0x0100,

            /// <summary>Retains the current position (ignores X and Y parameters).</summary>
            /// <remarks>SWP_NOMOVE</remarks>
            IgnoreMove = 0x0002,

            /// <summary>Does not change the owner window's position in the Z order.</summary>
            /// <remarks>SWP_NOOWNERZORDER</remarks>
            DoNotChangeOwnerZOrder = 0x0200,

            /// <summary>Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to 
            /// the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent 
            /// window uncovered as a result of the window being moved. When this flag is set, the application must 
            /// explicitly invalidate or redraw any parts of the window and parent window that need redrawing.</summary>
            /// <remarks>SWP_NOREDRAW</remarks>
            DoNotRedraw = 0x0008,

            /// <summary>Same as the SWP_NOOWNERZORDER flag.</summary>
            /// <remarks>SWP_NOREPOSITION</remarks>
            DoNotReposition = 0x0200,

            /// <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</summary>
            /// <remarks>SWP_NOSENDCHANGING</remarks>
            DoNotSendChangingEvent = 0x0400,

            /// <summary>Retains the current size (ignores the cx and cy parameters).</summary>
            /// <remarks>SWP_NOSIZE</remarks>
            IgnoreResize = 0x0001,

            /// <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
            /// <remarks>SWP_NOZORDER</remarks>
            IgnoreZOrder = 0x0004,

            /// <summary>Displays the window.</summary>
            /// <remarks>SWP_SHOWWINDOW</remarks>
            ShowWindow = 0x0040,
        }
        #endregion

        #region SetWindowLocation
        public static bool SetWindowLocation(string processName, Point newLocation)
        {
            Process[] processes = Process.GetProcessesByName(processName);

            if (processes == null || processes.Length == 0)
                return false;
            else
                return SetWindowLocation(processes[0], newLocation);
        }

        public static bool SetWindowLocation(Process process, Point newLocation)
        {
            return SetWindowLocation(process.MainWindowHandle, newLocation);
        }

        public static bool SetWindowLocation(IntPtr processMainWindowHandle, Point newLocation)
        {
            Rect flashRect = new Rect();

            if (GetWindowRect(processMainWindowHandle, ref flashRect))
                return SetWindowPos(processMainWindowHandle, 0, newLocation.X, newLocation.Y, 0, 0, SetWindowPosFlags.IgnoreZOrder | SetWindowPosFlags.IgnoreResize | SetWindowPosFlags.ShowWindow);
            else
                return false;
        }
        #endregion
        #region SetWindowSize
        public static bool SetWindowSize(Process process, Size newSize)
        {
            Rect flashRect = new Rect();

            if (GetWindowRect(process.MainWindowHandle, ref flashRect))
                return SetWindowPos(process.MainWindowHandle, 0, 0, 0, newSize.Width, newSize.Height, SetWindowPosFlags.IgnoreZOrder | SetWindowPosFlags.IgnoreMove | SetWindowPosFlags.ShowWindow);
            else
                return false;
        }

        public static bool SetWindowSize(string processName, Size newSize)
        {
            Process[] processes = Process.GetProcessesByName(processName);

            if (processes == null || processes.Length == 0)
                return false;
            else
                return SetWindowSize(processes[0], newSize);
        }
        #endregion
        #region SetForegroundWindow
        public static bool SetForegroundWindow(Process process)
        { return SetForegroundWindow(process.MainWindowHandle); }

        public static bool SetForegroundWindow(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);

            if (processes == null || processes.Length == 0)
                return false;
            else
                return SetForegroundWindow(processes[0].MainWindowHandle);
        }
        #endregion
    }
}

using System.Runtime.InteropServices; // DllImport
using System.Windows.Forms; // Screen
using System.Windows.Input; // Key

namespace NatiTools.xIO
{
    public static class Keyboard
    {
        #region PInvoke
        // Used for simulation of keyboard events
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
        #endregion

        #region Constants
        //const uint KEYEVENTF_EXTENDEDKEY = 0x0001; // Used for sending numeric keys (not really necessary for anything)
        const uint KEYEVENTF_KEYUP = 0x0002;
        #endregion

        #region Press
        public static void Press(Key key)
        { Press((byte)KeyInterop.VirtualKeyFromKey(key)); }

        public static void Press(Keys key)
        { Press((byte)key); }

        public static void Press(byte key)
        {
            Down(key);
            Up(key);
        }
        #endregion
        #region Down
        public static void Down(Key key)
        { Down((byte)KeyInterop.VirtualKeyFromKey(key)); }

        public static void Down(Keys key)
        { Down((byte)key); }

        public static void Down(byte key)
        { keybd_event(key, 0, 0, 0); }
        #endregion
        #region Up
        public static void Up(Key key)
        { Up((byte)KeyInterop.VirtualKeyFromKey(key)); }

        public static void Up(Keys key)
        { Up((byte)key); }

        public static void Up(byte key)
        { keybd_event(key, 0, KEYEVENTF_KEYUP, 0); }
        #endregion
    }
}
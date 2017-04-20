using System; // IntPtr, Random
using System.Drawing; // Bitmap, Graphics
using System.Drawing.Imaging; // PixelFormat
using System.Runtime.InteropServices; // DllImport

namespace NatiTools.xGraphics
{
    public static class LockBitsTools
    {
        #region class IndexArray
        public class IndexArray
        {
            public int[] Indexes = new int[0];

            public IndexArray(int[] indexArr)
            { Indexes = indexArr; }
        }
        #endregion

        #region static LockedBitmap getBytes
        public static byte[] getBytes(Bitmap bmp, out int stride)
        {
            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Set the stride output
            stride = bmpData.Stride;

            // Declare an array to hold the bytes of the bitmap.
            int numBytes = bmpData.Stride * bmp.Height;
            byte[] colorValues = new byte[numBytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, colorValues, 0, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            return colorValues;
        }
        #endregion
    }
}

using System; // Math
using System.Drawing; // Bitmap, Graphics
using System.Windows.Forms; // Screen

namespace NatiTools.xGraphics
{
    public static class GraphicsTools
    {
        #region Bitmap takeScreenshot
        public static Bitmap takeScreenshot()
        { return takeScreenshot(new Rectangle(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)); }

        public static Bitmap takeScreenshot(Rectangle region)
        {
            Bitmap screenshot = new Bitmap(region.Width, region.Height);

            using (Graphics g = Graphics.FromImage(screenshot))
                g.CopyFromScreen(region.X, region.Y, 0, 0, region.Size, CopyPixelOperation.SourceCopy);

            return screenshot;
        }
        #endregion

        #region bool checkRGB
        public static bool checkRGB(Color col, int R, int G, int B)
        { return (col.R == R && col.G == G && col.B == B); }

        public static bool checkRGB(Color col, int R, int G, int B, int tolerance)
        {
            if (tolerance == 0)
                return checkRGB(col, R, G, B);
            else if (tolerance < 0)
                return false;
            else
            {
                int difference = 0;

                difference += Math.Abs(col.R - R);
                difference += Math.Abs(col.G - G);
                difference += Math.Abs(col.B - B);

                return (difference <= tolerance);
            }
        }
        #endregion
        #region bool checkColor
        public static bool checkColor(Color c1, Color c2)
        { return (c1.R == c2.R && c1.G == c2.G && c1.B == c2.B); }

        public static bool checkColor(Color c1, Color c2, int tolerance)
        {
            if (tolerance == 0)
                return checkColor(c1, c2);
            else if (tolerance < 0)
                return false;
            else
            {
                int difference = 0;

                difference += Math.Abs(c1.R - c2.R);
                difference += Math.Abs(c1.G - c2.G);
                difference += Math.Abs(c1.B - c2.B);

                return (difference <= tolerance);
            }
        }
        #endregion

        #region bool dominant[channel]
        public static bool dominantR(Color col, double scale = 1)
        {
            return col.R > (col.G * scale) &&
                   col.R > (col.B * scale);
        }
        public static bool dominantG(Color col, double scale = 1)
        {
            return col.G > (col.R * scale) &&
                   col.G > (col.B * scale);
        }
        public static bool dominantB(Color col, double scale = 1)
        {
            return col.B > (col.R * scale) &&
                   col.B > (col.G * scale);
        }
        #endregion

        #region Point[] findColor
        /// <summary>
        /// !!! OBSOLETE !!! (uses GetPixel instead of LockBits)
        /// </summary>
        public static Point[] findColor(Bitmap source, Color col, int stepX = 1, int stepY = 1, int limit = 1, int tolerance = 0)
        {
            Point[] matches = new Point[limit];
            int matchesFound = 0;

            for (int y = 0; y < source.Height; y += stepY)
            {
                for (int x = 0; x < source.Width; x += stepX)
                {
                    if (matchesFound >= limit)
                        return matches;

                    if (checkColor(source.GetPixel(x, y), col, tolerance))
                    {
                        matches[matchesFound] = new Point(x, y);
                        matchesFound++;
                    }
                }
            }

            if (matchesFound >= limit)
                return matches;

            Point[] points = new Point[matchesFound];
            for (int i = 0; i < matchesFound; i++)
            { points[i] = matches[i]; }

            return points;
        }
        #endregion
    }
}

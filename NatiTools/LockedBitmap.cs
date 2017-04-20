using System.Drawing; // Bitmap, Graphics
using System.Drawing.Imaging; // PixelFormat

using NatiTools.xMath; // Compare

namespace NatiTools.xGraphics
{
    public class LockedBitmap
    {
        #region Variables
        public Size Size;
        public byte[] Bytes;

        public int Stride;
        public int ByteWidth;
        #endregion
        #region Constants
        public int bytesPerPixel = 3; // PixelFormat.Format24bppRgb

        private const int defaultTolerance = 0;
        private const int defaultLimit = 1;
        #endregion

        #region LockedBitmap
        public LockedBitmap(string file)
        {
            processBitmap(new Bitmap(file));
        }

        public LockedBitmap(Bitmap source)
        {
            processBitmap(source);
        }
        #endregion

        #region void processBitmap
        private void processBitmap(Bitmap source)
        {
            if (source.PixelFormat != PixelFormat.Format24bppRgb)
            {
                using (Bitmap clone = new Bitmap(source.Size.Width, source.Size.Height, PixelFormat.Format24bppRgb))
                using (Graphics gr = Graphics.FromImage(clone))
                {
                    gr.DrawImage(source, new Rectangle(0, 0, clone.Width, clone.Height));
                    source = clone;
                }
            }

            Size = source.Size;
            Bytes = LockBitsTools.getBytes(source, out Stride);
            ByteWidth = Size.Width * bytesPerPixel;
        }
        #endregion

        #region int pointToIndex
        public int pointToIndex(Point p)
        {
            int index = (p.Y * Stride) + (p.X * bytesPerPixel);

            if (index < Bytes.Length)
                return index;
            else
                return 0;
        }
        #endregion
        #region Point indexToPoint
        public Point indexToPoint(int i)
        {
            if (i <= Bytes.Length)
            {
                int x = (i % Stride) / bytesPerPixel;
                int y = i / Stride;

                return new Point(x, y);
            }
            else
                return Point.Empty;
        }
        #endregion
        #region Point indexesToPoints
        public Point[] indexesToPoints(int[] i)
        {
            Point[] p = new Point[i.Length];

            for (int j = 0; j < i.Length; j++)
            {
                if (i[j] <= Bytes.Length)
                {
                    int x = (i[j] % Stride) / bytesPerPixel;
                    int y = i[j] / Stride;

                    p[j] = new Point(x, y);
                }
                else
                    p[j] = Point.Empty;
            }

            return p;
        }
        #endregion

        #region int[] findColor
        public int[] findColor(Color col, int tolerance = 0, int limit = 1, int step = 1)
        {
            // If the limit is less or equal to zero, there's nothing to do really
            if (limit <= 0)
                return new int[0];

            int byteStep = step * bytesPerPixel;

            // Create field for the matches of the maximum possible size
            int[] matches = new int[limit];
            int matchesFound = 0;

            // Go through all the pixels
            for (int i = 0; i < Bytes.Length; i += byteStep)
            {
                // If all the pixel colors match, save the current int
                // BEWARE! THE BYTES ARE IN REVERSE ORDER BECAUSE OF THE BYTE SIGNIFICANCE (BGR instead of RGB)
                if (MathTools.Compare(Bytes[i], col.B, tolerance) &&
                    MathTools.Compare(Bytes[i + 1], col.G, tolerance) &&
                    MathTools.Compare(Bytes[i + 2], col.R, tolerance))
                {
                    // Save the index
                    matches[matchesFound] = i;
                    matchesFound++;

                    // If there are enough matches found already, break the loop and return found matches
                    if (matchesFound >= limit)
                        return matches;
                }
            }

            // If there are less matches found than the limit, get rid of the empty ints and return the actual matches
            int[] foundMatches = new int[matchesFound];
            for (int i = 0; i < matchesFound; i++)
            { foundMatches[i] = matches[i]; }

            return foundMatches;
        }
        #endregion
        #region LockBitsTools.IndexArray[] findColors
        public LockBitsTools.IndexArray[] findColors(Color[] col, int[] tolerance = null, int[] limit = null)
        {
            // Create field for the matches of the maximum possible size
            LockBitsTools.IndexArray[] colorMatches = new LockBitsTools.IndexArray[col.Length];
            for (int i = 0; i < colorMatches.Length; i++)
                colorMatches[i] = new LockBitsTools.IndexArray(new int[limit[i]]);

            int[] matchesFound = new int[col.Length];

            for (int iColor = 0; iColor < col.Length; iColor++)
            {
                // Go through all the pixels
                for (int iPixel = 0; iPixel < Bytes.Length; iPixel += bytesPerPixel)
                {
                    // If all the pixel colors match, save the current int
                    // BEWARE! THE BYTES ARE IN REVERSE ORDER BECAUSE OF THE BYTE SIGNIFICANCE (BGR instead of RGB)
                    if (MathTools.Compare(Bytes[iPixel], col[iColor].B, tolerance[iColor]) &&
                        MathTools.Compare(Bytes[iPixel + 1], col[iColor].G, tolerance[iColor]) &&
                        MathTools.Compare(Bytes[iPixel + 2], col[iColor].R, tolerance[iColor]))
                    {
                        // Save the index
                        colorMatches[iColor].Indexes[matchesFound[iColor]] = iPixel;
                        matchesFound[iColor]++;

                        // If there are enough matches found already, break the loop
                        if (matchesFound[iColor] >= limit[iColor])
                            break;
                    }
                }
            }

            // Return the actual matches
            LockBitsTools.IndexArray[] foundMatches = new LockBitsTools.IndexArray[col.Length];
            for (int i = 0; i < foundMatches.Length; i++)
            {
                foundMatches[i] = new LockBitsTools.IndexArray(new int[matchesFound[i]]);

                for (int j = 0; j < matchesFound[i]; j++)
                { foundMatches[i].Indexes[j] = colorMatches[i].Indexes[j]; }
            }

            return foundMatches;
        }
        #endregion
        #region  int[] findColorWithin
        public int[] findColorWithin(Color col, Rectangle rect, int tolerance = 0, int limit = 1, int step = 1)
        {
            // If the limit is less or equal to zero, there's nothing to do really
            if (limit <= 0)
                return new int[0];

            int byteStep = step * bytesPerPixel;

            // Create field for the matches of the maximum possible size
            int[] matches = new int[limit];
            int matchesFound = 0;

            int[] colBGR = { col.B, col.G, col.R };

            // Get the pixel offsets
            int byteLeft = rect.Left * bytesPerPixel;
            int byteRight = rect.Right * bytesPerPixel;
            int byteTop = rect.Top * Stride;
            int byteBottom = rect.Bottom * Stride;

            // Both x and y are indexes in bytes
            for (int yLB = byteTop;
                yLB < byteBottom;
                yLB += Stride)
            {
                for (int xLB = byteLeft;
                xLB < byteRight;
                xLB += byteStep)
                {
                    bool isMatch = true;

                    // If all the pixel colors match, save the current point
                    for (int j = 0; j < bytesPerPixel && isMatch; j++)
                    {
                        if (xLB + yLB < Bytes.Length)
                            isMatch = MathTools.Compare(Bytes[xLB + yLB + j], colBGR[j], tolerance);
                        else
                            isMatch = false;
                    }

                    if (isMatch)
                    {
                        // Save the found match
                        matches[matchesFound] = xLB + yLB;
                        matchesFound++;

                        // If there are enough matches found already, break the loop and return found matches
                        if (matchesFound >= limit)
                            return matches;
                    }
                }
            }

            // If there are less matches found than the limit, get rid of the empty ints and return the actual matches
            int[] foundMatches = new int[matchesFound];
            for (int i = 0; i < matchesFound; i++)
            { foundMatches[i] = matches[i]; }

            return foundMatches;
        }
        #endregion

        #region  int[] findBitmap
        public int[] findBitmap(LockedBitmap lbTarget, int tolerance = 0, int limit = 1)
        {
            // If the limit is less or equal to zero, there's nothing to do really
            if (limit <= 0)
                return new int[0];

            // Get the parameters for findColor
            // BEWARE! THE BYTES ARE IN REVERSE ORDER BECAUSE OF THE BYTE SIGNIFICANCE (BGR instead of RGB)
            Color firstPixelColor = Color.FromArgb(lbTarget.Bytes[2], lbTarget.Bytes[1], lbTarget.Bytes[0]);
            int searchAreaSize = Size.Width * Size.Height;

            // Search bitmap for all occurances of the first pixel color
            int[] firstPixelMatches = findColor(firstPixelColor, tolerance, searchAreaSize);

            // If the first pixel color doesn't appear in the whole bitmap, there's no reason to continue
            if (firstPixelMatches.Length <= 0)
                return new int[0];

            // Create field for the matches of the maximum possible size
            int[] matches = new int[limit];
            int matchesFound = 0;

            // Go through all the first pixel color matches
            for (int i = 0; i < firstPixelMatches.Length; i++)
            {
                bool isMatch = true;

                // Get the pixel offsets
                int xOffset = firstPixelMatches[i] % Stride;
                int yOffset = firstPixelMatches[i] - xOffset;

                // Both x and y are indexes in bytes
                for (int yTarget = 0, ySource = yOffset;
                    yTarget < lbTarget.Bytes.Length && isMatch;
                    yTarget += lbTarget.Stride, ySource += Stride)
                {
                    for (int xTarget = 0, xSource = xOffset;
                    xTarget < lbTarget.ByteWidth && isMatch;
                    xTarget += bytesPerPixel, xSource += bytesPerPixel)
                    {
                        // If all the pixel colors match, save the current point
                        for (int j = 0; j < bytesPerPixel && isMatch; j++)
                        {
                            if (xSource + ySource < Bytes.Length)
                                isMatch = MathTools.Compare(Bytes[xSource + ySource + j], lbTarget.Bytes[xTarget + yTarget + j], tolerance);
                            else
                                isMatch = false;
                        }
                    }
                }

                if (isMatch)
                {
                    // Save the found match
                    matches[matchesFound] = firstPixelMatches[i];
                    matchesFound++;

                    // If there are enough matches found already, break the loop and return found matches
                    if (matchesFound >= limit)
                        return matches;
                }
            }

            // If there are less matches found than the limit, get rid of the empty Points and return the actual matches
            int[] foundMatches = new int[matchesFound];
            for (int i = 0; i < matchesFound; i++)
            { foundMatches[i] = matches[i]; }

            return foundMatches;
        }
        #endregion
        #region  LockBitsTools.IndexArray[] findBitmaps
        public LockBitsTools.IndexArray[] findBitmaps(LockedBitmap[] lbTarget, int[] tolerance = null, int[] limit = null)
        {
            // If the limit is not set, use the default value
            if (limit == null)
            {
                limit = new int[lbTarget.Length];
                for (int i = 0; i < limit.Length; i++)
                    limit[i] = defaultLimit;
            }

            // If the tolerance is not set, use the default value
            if (tolerance == null)
            {
                tolerance = new int[lbTarget.Length];
                for (int i = 0; i < tolerance.Length; i++)
                    tolerance[i] = defaultTolerance;
            }

            // Get the parameters for findColor
            // BEWARE! THE BYTES ARE IN REVERSE ORDER BECAUSE OF THE BYTE SIGNIFICANCE (BGR instead of RGB)
            Color[] firstPixelColors = new Color[lbTarget.Length];
            for (int i = 0; i < firstPixelColors.Length; i++)
                firstPixelColors[i] = Color.FromArgb(lbTarget[i].Bytes[2], lbTarget[i].Bytes[1], lbTarget[i].Bytes[0]);

            int searchAreaSize = Size.Width * Size.Height;
            int[] firstPixelColorsLimits = new int[limit.Length];

            for (int i = 0; i < firstPixelColorsLimits.Length; i++)
                firstPixelColorsLimits[i] = searchAreaSize;

            // Search bitmap for all occurances of the first pixel color
            LockBitsTools.IndexArray[] firstPixelMatches = new LockBitsTools.IndexArray[lbTarget.Length];
            firstPixelMatches = findColors(firstPixelColors, tolerance, firstPixelColorsLimits);

            // Create field for the matches of the maximum possible size
            LockBitsTools.IndexArray[] targetMatches = new LockBitsTools.IndexArray[lbTarget.Length];
            for (int i = 0; i < targetMatches.Length; i++)
                targetMatches[i] = new LockBitsTools.IndexArray(new int[limit[i]]);

            int[] matchesFound = new int[lbTarget.Length];

            // Go through all the targets
            for (int iTarget = 0; iTarget < targetMatches.Length; iTarget++)
            {
                // Go through all the first pixel color matches
                for (int iMatch = 0; iMatch < firstPixelMatches[iTarget].Indexes.Length; iMatch++)
                {
                    bool isMatch = true;

                    // Get the pixel offsets
                    int matchIndex = firstPixelMatches[iTarget].Indexes[iMatch];
                    int xOffset = matchIndex % Stride;
                    int yOffset = matchIndex - xOffset;

                    // Both x and y are indexes in bytes
                    for (int yTarget = 0, ySource = yOffset;
                        yTarget < lbTarget[iTarget].Bytes.Length && isMatch;
                        yTarget += lbTarget[iTarget].Stride, ySource += Stride)
                    {
                        for (int xTarget = 0, xSource = xOffset;
                        xTarget < lbTarget[iTarget].ByteWidth && isMatch;
                        xTarget += bytesPerPixel, xSource += bytesPerPixel)
                        {
                            // If all the pixel colors match, save the current point
                            for (int j = 0; j < bytesPerPixel && isMatch; j++)
                            {
                                if (xSource + ySource < Bytes.Length)
                                    isMatch = MathTools.Compare(Bytes[xSource + ySource + j], lbTarget[iTarget].Bytes[xTarget + yTarget + j], tolerance[iTarget]);
                                else
                                    isMatch = false;
                            }
                        }
                    }

                    if (isMatch)
                    {
                        // Save the found match
                        targetMatches[iTarget].Indexes[matchesFound[iTarget]] = firstPixelMatches[iTarget].Indexes[iMatch];
                        matchesFound[iTarget]++;

                        // If there are enough matches found already, break the loop
                        if (matchesFound[iTarget] >= limit[iTarget])
                            break;
                    }
                }
            }

            // Return the actual matches
            LockBitsTools.IndexArray[] foundMatches = new LockBitsTools.IndexArray[targetMatches.Length];
            for (int i = 0; i < foundMatches.Length; i++)
            {
                foundMatches[i] = new LockBitsTools.IndexArray(new int[matchesFound[i]]);

                for (int j = 0; j < matchesFound[i]; j++)
                { foundMatches[i].Indexes[j] = targetMatches[i].Indexes[j]; }
            }

            return foundMatches;
        }
        #endregion

        #region  Color getColor
        // Supposedly more efficient replacement for Bitmap.GetPixel(x, y)

        // Coordinates as Point object
        public Color getColor(Point pixel)
        { return getColor(pixel.X, pixel.Y); }

        // Coordinates separated into x and y
        public Color getColor(int x, int y)
        {
            // Calculate the index shift caused by rows
            int rowOffset = y * Size.Width * bytesPerPixel;

            // Calculate the index shift caused by columns
            int columnOffset = x * bytesPerPixel;

            // Index of the first byte of the current pixel
            int currentPixel = rowOffset + columnOffset;

            // Generate Color object from RGB
            // BEWARE! THE BYTES ARE IN REVERSE ORDER BECAUSE OF THE BYTE SIGNIFICANCE (BGR instead of RGB)
            return Color.FromArgb(Bytes[currentPixel + 2], Bytes[currentPixel + 1], Bytes[currentPixel]);
        }
        #endregion
    }
}

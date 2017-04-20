using System; // IntPtr, Random
using System.Drawing; // Bitmap, Graphics

namespace NatiTools.xMath
{
    public static class Geometry
    {
        #region static Point addOffset
        // Sum multiple Points
        public static Point addOffset(Point p, params Point[] offset)
        {
            int x = p.X;
            int y = p.Y;

            foreach (Point _offset in offset)
            {
                x += _offset.X;
                y += _offset.Y;
            }

            return new Point(x, y);
        }
        #endregion
        #region static Point getCenter
        // Get coordinates of the center of a Rectangle

        public static Point getCenter(Rectangle rect)
        {
            int x = rect.X + (rect.Width / 2);
            int y = rect.Y + (rect.Height / 2);
            return new Point(x, y);
        }
        #endregion
        #region static Point randomWithin
        // Generate a random point within certain area

        public static Point randomWithin(Rectangle rect)
        { return addOffset(rect.Location, randomWithin(rect.Size)); }

        public static Point randomWithin(Size size)
        {
            Random rand = new Random();

            int x = rand.Next(size.Width);
            int y = rand.Next(size.Height);

            return new Point(x, y);
        }
        #endregion
    }
}

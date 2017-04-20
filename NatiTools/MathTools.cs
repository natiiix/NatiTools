using System;

namespace NatiTools.xMath
{
    public static class MathTools
    {
        #region static bool Compare
        public static bool Compare(int i1, int i2, int tolerance = 0)
        { return (Math.Abs(i1 - i2) <= tolerance); }

        public static bool Compare(float f1, float f2, int tolerance = 0)
        { return (Math.Abs(f1 - f2) <= tolerance); }

        public static bool Compare(double d1, double d2, int tolerance = 0)
        { return (Math.Abs(d1 - d2) <= tolerance); }
        #endregion
    }
}

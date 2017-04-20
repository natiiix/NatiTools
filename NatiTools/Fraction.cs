using System;

namespace NatiTools.xMath
{
    class Fraction
    {
        #region Variables
        private long numerator;
        private long denominator;
        #endregion

        #region Constructor
        public Fraction(long _numerator = 0, long _denominator = 1)
        {
            numerator = _numerator;
            denominator = _denominator;

            Clean();
        }

        public Fraction(int _numerator = 0, int _denominator = 1)
        {
            numerator = (long)_numerator;
            denominator = (long)_denominator;

            Clean();
        }
        #endregion

        #region Inner Methods
        private void Clean()
        {
            for (long i = (Math.Abs(numerator) < Math.Abs(denominator) ? numerator : denominator); i > 1; i--)
            {
                if (numerator % i == 0 && denominator % i == 0)
                {
                    numerator /= i;
                    denominator /= i;
                }
            }

            MakeDenominatorPositive();
            CheckForDBZ();
        }

        private void MakeDenominatorPositive()
        {
            if (denominator < 0)
            {
                numerator = -numerator;
                denominator = -denominator;
            }
        }

        private void CheckForDBZ()
        {
            if (denominator == 0)
                throw new DivideByZeroException();
        }
        #endregion

        #region Negate
        public void Negate()
        {
            numerator = -numerator;
        }

        public static Fraction Negate(Fraction _fraction)
        {
            return new Fraction(-_fraction.numerator, _fraction.denominator);
        }
        #endregion
        #region Invert
        public void Invert()
        {
            numerator += denominator;
            denominator = numerator - denominator;
            numerator -= denominator;

            Clean();
        }

        public static Fraction Invert(Fraction _fraction)
        {
            return new Fraction(_fraction.denominator, _fraction.numerator);
        }
        #endregion
        #region Power
        public void Power(int _exponent)
        {
            if (_exponent < 0)
                throw new ArgumentOutOfRangeException();
            else
            {
                long _numerator = 1;
                long _denominator = 1;

                for (int i = 0; i < _exponent; i++)
                {
                    _numerator *= numerator;
                    _denominator *= denominator;
                }

                numerator = _numerator;
                denominator = _denominator;
            }
        }
        #endregion

        #region Add
        public void Add(long _numerator, long _denominator = 1)
        {
            if (denominator == _denominator)
                numerator += _numerator;
            else
            {
                numerator += _numerator * denominator;
                denominator *= _denominator;
            }

            Clean();
        }

        public void Add(int _numerator, int _denominator = 1)
        {
            Add((long)_numerator, (long)_denominator);
        }

        public void Add(Fraction _fraction)
        {
            Add(_fraction.numerator, _fraction.denominator);
        }
        #endregion
        #region Subtract
        public void Subtract(long _numerator, long _denominator = 1)
        {
            Add(-_numerator, _denominator);
        }

        public void Subtract(int _numerator, int _denominator = 1)
        {
            Add((long)-_numerator, (long)_denominator);
        }

        public void Subtract(Fraction _fraction)
        {
            Subtract(_fraction.numerator, _fraction.denominator);
        }
        #endregion
        #region Multiply
        public void Multiply(long _numerator, long _denominator = 1)
        {
            numerator *= _numerator;
            denominator *= _denominator;

            Clean();
        }

        public void Multiply(int _numerator, int _denominator = 1)
        {
            Multiply((long)_numerator, (long)_denominator);
        }

        public void Multiply(Fraction _fraction)
        {
            Multiply(_fraction.numerator, _fraction.denominator);
        }
        #endregion
        #region Divide
        public void Divide(long _numerator, long _denominator = 1)
        {
            Multiply(_denominator, _numerator);
        }

        public void Divide(int _numerator, int _denominator = 1)
        {
            Multiply((long)_denominator, (long)_numerator);
        }

        public void Divide(Fraction _fraction)
        {
            Divide(_fraction.numerator, _fraction.denominator);
        }
        #endregion

        #region Int
        public int ToInt()
        {
            return (int)ToLong();
        }
        #endregion
        #region Long
        public long ToLong()
        {
            return (long)numerator / (long)denominator;
        }
        #endregion
        #region Float
        public float ToFloat()
        {
            return (float)ToDouble();
        }
        #endregion
        #region Double
        public double ToDouble()
        {
            return (double)numerator / (double)denominator;
        }
        #endregion
    }
}

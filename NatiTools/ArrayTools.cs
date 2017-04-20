using System;

namespace NatiTools.xCSharp
{
    public static class ArrayTools
    {
        #region static void Append<T>(ref T[] array, params T[] obj)
        public static void Append<T>(ref T[] array, params T[] obj)
        {
            int formerLength = array.Length;
            Array.Resize(ref array, formerLength + obj.Length);

            for (int i = 0; i < obj.Length; i++)
            {
                array[formerLength + i] = obj[i];
            }
        }
        #endregion
        #region static void InsertAt<T>(ref T[] array, int index, params T[] obj)
        public static void InsertAt<T>(ref T[] array, int index, params T[] obj)
        {
            int formerLength = array.Length;
            Array.Resize(ref array, formerLength + obj.Length);

            for (int i = 0; i < formerLength - index; i++)
            {
                array[array.Length - i] = array[formerLength - i];
            }

            for (int i = 0; i < obj.Length; i++)
            {
                array[index + i] = obj[i];
            }
        }
        #endregion
        #region static void Remove<T>(ref T[] array, params int[] indexes)
        public static void Remove<T>(ref T[] array, params int[] indexes)
        {
            if (array.Length <= 0 || indexes.Length <= 0)
                return;

            Array.Sort(indexes);

            int currentIndex = 0;
            int moveOffset = 0;

            for (int i = indexes[currentIndex]; i < array.Length; i++)
            {
                if (i == indexes[currentIndex])
                {
                    moveOffset++;

                    if (currentIndex + 1 < indexes.Length)
                        currentIndex++;
                }
                else
                {
                    array[i - moveOffset] = array[i];
                }
            }

            Array.Resize(ref array, array.Length - moveOffset);
        }
        #endregion
        #region static void RemoveLast<T>(ref T[] array, int amount = 1)
        public static void RemoveLast<T>(ref T[] array, int amount = 1)
        {
            if (array.Length >= amount)
                Array.Resize(ref array, array.Length - 1);
        }
        #endregion
    }
}

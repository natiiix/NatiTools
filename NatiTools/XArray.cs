using System;

namespace NatiTools.xCSharp
{
    public class XArray<T>
    {
        public T[] Elements;

        public XArray(params T[] elements)
        {
            Elements = elements;
        }

        #region void Append(params T[] obj)
        public void Append(params T[] obj)
        {
            // Get the original length of Elements array
            int formerLength = Elements.Length;

            // Make space for elements to append
            Array.Resize(ref Elements, formerLength + obj.Length);

            // Copy elements from input array to Elements array
            Array.Copy(obj, 0, Elements, formerLength, obj.Length);
            /*for (int i = 0; i < obj.Length; i++)
                Elements[formerLength + i] = obj[i];*/
        }
        #endregion
        #region void InsertAt(int index, params T[] obj)
        public void InsertAt(int index, params T[] obj)
        {
            // Get the original Elements array length
            int formerLength = Elements.Length;

            // Make space for elements to append
            Array.Resize(ref Elements, formerLength + obj.Length);

            // Shift all the elements after index to their new indexes
            for (int i = 0; i < formerLength - index; i++)
                Elements[Elements.Length - i] = Elements[formerLength - i];

            // Copy input array into Elements array starting at specified index
            Array.Copy(obj, 0, Elements, index, obj.Length);
            /*for (int i = 0; i < obj.Length; i++)
                Elements[index + i] = obj[i];*/
        }
        #endregion
        #region void Remove(params int[] indexes)
        public void Remove(params int[] indexes)
        {
            if (Elements.Length <= 0 || indexes.Length <= 0)
                return;

            Array.Sort(indexes);

            int currentIndex = 0;
            int moveOffset = 0;

            for (int i = indexes[currentIndex]; i < Elements.Length; i++)
            {
                if (i == indexes[currentIndex])
                {
                    moveOffset++;

                    if (currentIndex + 1 < indexes.Length)
                        currentIndex++;
                }
                else
                {
                    Elements[i - moveOffset] = Elements[i];
                }
            }

            Array.Resize(ref Elements, Elements.Length - moveOffset);
        }
        #endregion
        #region void RemoveLast(int amount = 1)
        public void RemoveLast(int amount = 1)
        {
            if (Elements.Length >= amount)
                Array.Resize(ref Elements, Elements.Length - 1);
        }
        #endregion

        #region void Push(params T[] obj)
        public void Push(params T[] obj)
        {
            // Push is synonymous to Append
            Append(obj);
        }
        #endregion
        #region T[] Pop(int amount = 1)
        public T[] Pop(int amount = 1)
        {
            // Can't pop negative amount of elements
            if (amount < 0)
                throw new ArgumentException();

            // Can't pop more elements than how many are actually stored
            if (amount > Elements.Length)
                throw new IndexOutOfRangeException();

            // Create an array of specified size
            T[] obj = new T[amount];

            // Copy elements from the Elements array to an output array
            Array.Copy(Elements, Elements.Length - amount, obj, 0, amount);
            /*for(int i = 0; i < amount; i++)
                obj[i] = Elements[Elements.Length - amount + i];*/

            // Remove popped elements from the Elements array
            Array.Resize(ref Elements, Elements.Length - amount);

            // Return the output array
            return obj;
        }
        #endregion
    }
}

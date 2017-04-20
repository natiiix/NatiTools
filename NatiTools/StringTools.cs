namespace NatiTools.xCSharp
{
    public static class StringTools
    {
        #region static bool Equals(string str1, string str2, bool caseSensitive = true)
        public static bool Equals(string str1, string str2, bool caseSensitive = true)
        {
            string[] strArr = new string[2] {
                caseSensitive ? str1 : str1.ToLower(),
                caseSensitive ? str2 : str2.ToLower() };

            return (strArr[0] == strArr[1]);
        }
        #endregion
        #region static bool Contains(string str1, string str2, bool caseSensitive = true)
        public static bool Contains(string str1, string str2, bool caseSensitive = true)
        {
            string[] strArr = new string[2] {
                caseSensitive ? str1 : str1.ToLower(),
                caseSensitive ? str2 : str2.ToLower() };

            return (strArr[0].Contains(strArr[1]) || strArr[1].Contains(strArr[0]));
        }
        #endregion
        #region static bool ArrayContains(string[] strArr, string str, bool caseSensitive = true)
        public static bool ArrayContains(string[] strArr, string str, bool caseSensitive = true)
        {
            foreach (string strElement in strArr)
            {
                if (caseSensitive)
                {
                    if (strElement == str)
                        return true;
                }
                else if (strElement.ToLower() == str.ToLower())
                    return true;
            }

            return false;
        }
        #endregion
    }
}

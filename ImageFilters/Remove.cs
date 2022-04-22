using System;
using System.Collections.Generic;
using System.Text;

namespace ImageFilters
{
    public static class Remove
    {
        public static byte [] RemoveAt(byte [] oArray, int idx)
        {
            byte[] nArray = new byte[oArray.Length - 1];
            for (int i = 0; i < nArray.Length; ++i)
            {
                nArray[i] = (i < idx) ? oArray[i] : oArray[i + 1];
            }
            return nArray;
        }
    }
}

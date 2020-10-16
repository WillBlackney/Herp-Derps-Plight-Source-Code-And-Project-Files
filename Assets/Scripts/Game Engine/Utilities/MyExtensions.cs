using System;
using System.Collections.Generic;
using System.Security.Cryptography;

static class MyExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        // Function takes a list, then randomly shuffles the
        // order of its contents. Used mainly to shuffle card collections

        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

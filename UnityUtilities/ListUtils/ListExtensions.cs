using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace UnityUtilities.ListUtils {
   public static class ListExtensions {
        public static T GetAndRemoveLastListEl<T>(this List<T> collection) {

            int lastIndex = collection.Count - 1;
            T element = collection[lastIndex];

            collection.RemoveAt(lastIndex);

            return element;
        }

        /// <summary>
        /// Remove first element from collection of type List<T> then returns it.
        /// </summary>
        /// <returns>First element from given collection</returns>
        public static T GetAndRemoveFirstListEl<T>(this List<T> collection) {

            T element = collection[0];

            collection.RemoveAt(0);

            return element;
        }

        public static List<T> Shuffle<T>(this List<T> list) {

            var rg = RandomNumberGenerator.Create();

            byte[] rno = new byte[5];
            rg.GetBytes(rno);
            int seed = BitConverter.ToInt32(rno, 0);

            var sysRandomGen = new Random(seed);

            int n = list.Count;
            while(n > 1) {
                n--;
                int k = sysRandomGen.Next(0, n - 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }
}

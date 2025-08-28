using MatchMakerLib.MatchMakerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMakerLib
{
    public static class Utilities
    {
        static Random rng = new();
        public static void Shuffle<T>(this IList<T> list)
        {

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        public static List<TSource> MoveToFirst<TSource>(this List<TSource> source, TSource element)
        {
            if (!source.Contains(element))
                return source;

            source.Remove(element);
            source.Insert(0, element);
            return source;
        }

  
    }

}

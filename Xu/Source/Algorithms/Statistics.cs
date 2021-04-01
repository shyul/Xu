/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Math and numeric related basic functions.
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Xu
{
    public static partial class Algorithms
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double StdDev(this IEnumerable<double> values)
        {
            double value = 0;
            if (values.Count() > 0)
            {
                double avg = values.Average(); //Compute the Average 
                double sum = values.Sum(d => Math.Pow(d - avg, 2)); //Perform the Sum of (value-avg)_2_2     
                value = Math.Sqrt(sum / (values.Count() - 1));
            }
            return value;
        }

        public static double DeSpur(IEnumerable<double> values)
        {
            if (values.Count() > 3)
            {
                double max = values.Max();
                double min = values.Min();
                return (values.Sum() - max - min) / (values.Count() - 2);
            }
            else
            {
                throw new Exception("We need at least four data points to yield the result.");
            }
        }

        public static T Median<T>(this IList<T> list) where T : IComparable<T>
            => list.NthOrderStatistic((list.Count - 1) / 2);

        public static double Median<T>(this IEnumerable<T> source, Func<T, double> getValue)
            => source.Select(getValue).ToList().Median();

        /// <summary>
        /// Returns Nth smallest element from the list. Here n starts from 0 so that n=0 returns minimum, n=1 returns 2nd smallest element etc.
        /// Note: specified list would be mutated in the process.
        /// Reference: Introduction to Algorithms 3rd Edition, Corman et al, pp 216
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="n"></param>
        /// <param name="rand"></param>
        /// <returns></returns>
        public static T NthOrderStatistic<T>(this IList<T> list, int n, Random rand = null) where T : IComparable<T>
            => NthOrderStatistic(list, n, 0, list.Count - 1, rand);

        private static T NthOrderStatistic<T>(this IList<T> list, int n, int start, int end, Random rand) where T : IComparable<T>
        {
            while (true)
            {
                int pivotIndex = list.Partition(start, end, rand);
                if (pivotIndex == n)
                    return list[pivotIndex];

                if (n < pivotIndex)
                    end = pivotIndex - 1;
                else
                    start = pivotIndex + 1;
            }
        }

        /// <summary>
        /// Partitions the given list around a pivot element such that all elements on left of pivot are <= pivot and the ones at thr right are > pivot. This method can be used for sorting, N-order statistics such as median finding algorithms.
        /// Pivot is selected ranodmly if random number generator is supplied else its selected as last element in the list.
        /// Reference: Introduction to Algorithms 3rd Edition, Corman et al, pp 171
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="rand"></param>
        /// <returns></returns>
        public static int Partition<T>(this IList<T> list, int start, int end, Random rand = null) where T : IComparable<T>
        {
            if (rand != null)
                list.Swap(end, rand.Next(start, end + 1));

            T pivot = list[end];
            int lastLow = start - 1;
            for (int i = start; i < end; i++)
            {
                if (list[i].CompareTo(pivot) <= 0)
                    list.Swap(i, ++lastLow);
            }
            list.Swap(end, ++lastLow);

            return lastLow;
        }

        /// <summary>
        /// Swap two specified values of the list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            if (i != j)
            {
                T tmp = list[i];
                list[i] = list[j];
                list[j] = tmp;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LunaLib {
    public static partial class CollectionExtension {

        #region Basis
        #region First/Last
        #region Get
        public static T First<T>(this IEnumerable<T> enumerable) {
            foreach (T e in enumerable) {
                return e;
            }
            return default;
        }
        public static T Last<T>(this IEnumerable<T> enumerable) {
            T result = default;
            foreach (T e in enumerable) {
                result = e;
            }
            return result;
        }

        public static T LastBut<T>(this IList<T> list, int inverseIndex = 1, T valueIfEmpty = default) {
            return list.Count == 0 ? valueIfEmpty : list[list.Count - 1 - inverseIndex];
        }
        public static IEnumerable<T> LastRange<T>(this IEnumerable<T> enumerable, int count) {
            return enumerable.StartingAt(enumerable.Count() - count);
        }

        public static int LastIndex<T>(this IEnumerable<T> enumerable) {
            return enumerable.Count() - 1;
        }
        #endregion

        #region Set
        public static void SetFirst<T>(this IList<T> list, T value) {
            list[0] = value;
        }
        public static void SetLast<T>(this IList<T> list, T value) {
            list[list.Count - 1] = value;
        }
        #endregion

        #region Remove
        public static void RemoveFirst<T>(this IList<T> list) => list.RemoveAt(0);
        public static void RemoveLast<T>(this IList<T> list) => list.RemoveAt(list.Count - 1);

        public static List<T> WithoutFirst<T>(this List<T> list) => list.GetRange(1, list.Count - 1);
        public static List<T> WithoutLast<T>(this List<T> list) => list.GetRange(0, list.Count - 1);
        #endregion
        #endregion

        #region Valid Index
        public static bool IsValidPos<T>(this T[,] matrix, Vector2Int pos) => (pos.x >= 0) && (pos.x < matrix.GetLength(0)) && (pos.y >= 0) && (pos.y < matrix.GetLength(1));

        public static bool IsValidIndex<T>(this IEnumerable<T> list, int index) => (index >= 0) && (index < list.Count());
        public static bool IsValidIndex<T>(this ICollection<T> list, int index) => (index >= 0) && (index < list.Count);
        
        public static void EnsureIndex<T>(this IList<T> list, int index, T defaultValue = default) {
            for (int i = list.Count; i <= index; i++) {
                list.Add(defaultValue);
            }
        }
        #endregion

        #region Empty
        public static bool Empty<T>(this ICollection<T> list) => list.Count == 0;
        #endregion
        #endregion

        #region Random
        #region Single Element
        public static T RandomElement<T>(this IList<T> list) => list[list.RandomIndex()];
        public static T RandomElement<T>(this IList<T> list, IList<int> weights) => list[list.RandomIndex(weights)];
        public static T RandomElement<T>(this IList<T> list, IList<float> weights) => list[list.RandomIndex(weights)];
        public static T RandomElement<T>(this IList<T> list, Func<T, int> weightFunc) => list[list.RandomIndex(weightFunc)];
        public static T RandomElement<T>(this IList<T> list, Func<T, float> weightFunc) => list[list.RandomIndex(weightFunc)];
        #endregion

        #region Single Index
        public static int RandomIndex<T>(this ICollection<T> list) => UnityEngine.Random.Range(0, list.Count);
        public static int RandomIndex<T>(this IEnumerable<T> _, IList<int> weights) {
            int chosenIndex = UnityEngine.Random.Range(0, weights.Sum());
            int j = 0;
            for (int i = 0; i < weights.Count; i++) {
                j += weights[i];
                if (chosenIndex < j) {
                    return i;
                }
            }
            throw new Exception("Could not find chosen index.");
        }
        public static int RandomIndex<T>(this IEnumerable<T> _, IList<float> weights) {
            float chosenValue = UnityEngine.Random.Range(0f, weights.Sum());
            int i = 0;
            float count = 0;
            while (chosenValue >= count + weights[i]) {
                count += weights[i];
                i++;
            }
            return i;
        }
        public static int RandomIndex<T>(this IEnumerable<T> list, Func<T, int> weightFunc) => list.RandomIndex(new List<int>(list.Select(weightFunc)));
        public static int RandomIndex<T>(this IEnumerable<T> list, Func<T, float> weightFunc) => list.RandomIndex(new List<float>(list.Select(weightFunc)));
        #endregion

        #region Multiple Elements
        public static List<T> RandomElements<T>(this IList<T> list, int quantity, bool canRepeat = false) => list.SortedBy(list.RandomIndices(quantity, canRepeat));
        public static List<T> RandomElements<T>(this IList<T> list, int quantity, IList<int> weights, bool canRepeat = false) => list.SortedBy(list.RandomIndices(quantity, weights, canRepeat));
        public static List<T> RandomElements<T>(this IList<T> list, int quantity, IList<float> weights, bool canRepeat = false) => list.SortedBy(list.RandomIndices(quantity, weights, canRepeat));
        public static List<T> RandomElements<T>(this IList<T> list, int quantity, Func<T, int> weightFunc, bool canRepeat = false) => list.SortedBy(list.RandomIndices(quantity, weightFunc, canRepeat));
        public static List<T> RandomElements<T>(this IList<T> list, int quantity, Func<T, float> weightFunc, bool canRepeat = false) => list.SortedBy(list.RandomIndices(quantity, weightFunc, canRepeat));

        public static List<T> RandomElements<T>(this IList<T> list, int quantity, Func<T, List<T>, int> dynamicWeightFunc, bool canRepeat = false) {
            List<T> result = new List<T>();
            List<int> possibilities = CollectionUtils.NaturalsList(list.Count);
            while (result.Count < quantity) {
                int chosenIndex = possibilities.RandomIndex(i => dynamicWeightFunc.Invoke(list[i], result));
                int chosen = possibilities[chosenIndex];
                if (!canRepeat) possibilities.RemoveAt(chosenIndex);
                result.Add(list[chosen]);
            }
            return result;
        }
        public static List<T> RandomElements<T>(this IList<T> list, int quantity, Func<T, List<T>, float> dynamicWeightFunc, bool canRepeat = false) {
            List<T> result = new List<T>();
            List<int> possibilities = CollectionUtils.NaturalsList(list.Count);
            while (result.Count < quantity) {
                int chosenIndex = possibilities.RandomIndex(i => dynamicWeightFunc.Invoke(list[i], result));
                int chosen = possibilities[chosenIndex];
                if (!canRepeat) possibilities.RemoveAt(chosenIndex);
                result.Add(list[chosen]);
            }
            return result;
        }
        #endregion

        #region Multiple Indices
        public static List<int> RandomIndices<T>(this ICollection<T> list, int quantity, bool canRepeat = false) {
            List<int> result = new List<int>();
            List<int> possibilities = CollectionUtils.NaturalsList(list.Count);

            while (result.Count < quantity) {
                int chosenIndex = possibilities.RandomIndex();
                int chosen = possibilities[chosenIndex];
                if (!canRepeat) possibilities.RemoveAt(chosenIndex);
                result.Add(chosen);
            }

            return result;
        }
        public static List<int> RandomIndices<T>(this ICollection<T> list, int quantity, IList<int> weights, bool canRepeat = false) {
            List<int> result = new List<int>();
            List<int> possibilities = CollectionUtils.NaturalsList(list.Count);
            while (result.Count < quantity) {
                int chosenIndex = possibilities.RandomIndex(weights);
                int chosen = possibilities[chosenIndex];
                if (!canRepeat) possibilities.RemoveAt(chosenIndex);
                result.Add(chosen);
            }
            return result;
        }
        public static List<int> RandomIndices<T>(this ICollection<T> list, int quantity, IList<float> weights, bool canRepeat = false) {
            List<int> result = new List<int>();
            List<int> possibilities = CollectionUtils.NaturalsList(list.Count);

            while (result.Count < quantity) {
                int chosenIndex = possibilities.RandomIndex(weights);
                int chosen = possibilities[chosenIndex];
                if (!canRepeat) possibilities.RemoveAt(chosenIndex);
                result.Add(chosen);
            }

            return result;
        }
        public static List<int> RandomIndices<T>(this ICollection<T> list, int quantity, Func<T, int> weightFunc, bool canRepeat = false) {
            List<int> weights = new List<int>();
            foreach (T element in list) {
                weights.Add(weightFunc.Invoke(element));
            }
            return list.RandomIndices(quantity, weights, canRepeat);
        }
        public static List<int> RandomIndices<T>(this ICollection<T> list, int quantity, Func<T, float> weightFunc, bool canRepeat = false) {
            List<float> weights = new List<float>();
            foreach (T element in list) {
                weights.Add(weightFunc.Invoke(element));
            }
            return list.RandomIndices(quantity, weights, canRepeat);
        }
        #endregion

        #region Shuffle
        public static void Shuffle<T>(this IList<T> list) {
            List<T> teporaryList = list.Shuffled();
            list.Clear();
            list.AddRange(teporaryList);
        }

        public static List<T> Shuffled<T>(this IList<T> list) => list.SortedBy(list.RandomIndices(list.Count));
        #endregion

        #endregion

        #region Math
        #region Min/Max
        #region Value
        #region Specific
        /// <summary>
        /// Returns the smallest of all the floating point values in <paramref name="list"/>
        /// </summary>
        /// <param name="list">The IEnumerable containing all the of floating point values</param>
        /// <returns>The smallest of all the values in <paramref name="list"/></returns>
        public static float Min(this IEnumerable<float> list) {
            return Mathf.Min(list.ToArray());
        }

        /// <summary>
        /// Returns the largest of all the floating point values in <paramref name="list"/>
        /// </summary>
        /// <param name="list">The IEnumerable containing all the of floating point values</param>
        /// <returns>The largest of all the values in <paramref name="list"/></returns>
        public static float Max(this IEnumerable<float> list) {
            return Mathf.Max(list.ToArray());
        }

        /// <summary>
        /// Returns the smallest of all the values in <paramref name="list"/>
        /// </summary>
        /// <param name="list">The IEnumerable containing all the of integer values</param>
        /// <returns>The smallest of all the values in <paramref name="list"/></returns>
        public static int Min(this IEnumerable<int> list) {
            return list.Reduce<int, int>((a, b) => Mathf.Min(a, b), int.MaxValue);
        }

        /// <summary>
        /// Returns the largest of all the integer values in <paramref name="list"/>
        /// </summary>
        /// <param name="list">The IEnumerable containing all the of integer values</param>
        /// <returns>The largest of all the values in <paramref name="list"/></returns>
        public static int Max(this IEnumerable<int> list) {
            return list.Reduce<int, int>((a, b) => Mathf.Max(a, b), int.MinValue);
        }
        #endregion

        #region Generic
        /// <summary>
        /// Returns the smallest of all the floating point values returned by <paramref name="func"/> applied on the elements of <paramref name="list"/>
        /// </summary>
        /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
        /// <param name="list">The IEnumerable containing all the generic type elements</param>
        /// <param name="func">The delegate function that calculates the floating point value for an element of <paramref name="list"/></param>
        /// <returns>The smallest of all the values found</returns>
        public static float Min<T>(this IEnumerable<T> list, Func<T, float> func) {
            return list.Reduce((a, b) => Mathf.Min(func.Invoke(a), b), float.MaxValue);
        }

        /// <summary>
        /// Returns the largest of all the floating point values returned by <paramref name="func"/> applied on the elements of <paramref name="list"/>
        /// </summary>
        /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
        /// <param name="list">The IEnumerable containing all the generic type elements</param>
        /// <param name="func">The delegate function that calculates the floating point value for an element of <paramref name="list"/></param>
        /// <returns>The largest of all the values found</returns>
        public static float Max<T>(this IEnumerable<T> list, Func<T, float> func) {
            return list.Reduce((a, b) => Mathf.Max(func.Invoke(a), b), float.MinValue);
        }

        /// <summary>
        /// Returns the smallest of all the integer values returned by <paramref name="func"/> applied on the elements of <paramref name="list"/>
        /// </summary>
        /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
        /// <param name="list">The IEnumerable containing all the generic type elements</param>
        /// <param name="func">The delegate function that calculates the integer value for an element of <paramref name="list"/></param>
        /// <returns>The smallest of all the values found</returns>
        public static int Min<T>(this IEnumerable<T> list, Func<T, int> func) {
            return list.Reduce((a, b) => Mathf.Min(func.Invoke(a), b), int.MaxValue);
        }

        /// <summary>
        /// Returns the largest of all the integer values returned by <paramref name="func"/> applied on the elements of <paramref name="list"/>
        /// </summary>
        /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
        /// <param name="list">The IEnumerable containing all the generic type elements</param>
        /// <param name="func">The delegate function that calculates the integer value for an element of <paramref name="list"/></param>
        /// <returns>The largest of all the values found</returns>
        public static int Max<T>(this IEnumerable<T> list, Func<T, int> func) {
            return list.Reduce((a, b) => Mathf.Max(func.Invoke(a), b), int.MinValue);
        }
        #endregion
        #endregion

        #region Element
        /// <summary>
        /// Returns the <paramref name="list"/> element that results on the smallest floating point value returned by <paramref name="func"/>
        /// </summary>
        /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
        /// <param name="list">The IEnumerable containing all the elements to be evauated</param>
        /// <param name="func">The delegate function that calculates the floating point value for an element of <paramref name="list"/></param>
        /// <returns>The element that results on the smallest value returned by <paramref name="func"/></returns>
        public static T MinElement<T>(this IEnumerable<T> list, Func<T, float> func) {
            T result = default;
            float min = float.MaxValue;
            foreach (T item in list) {
                float cur = func.Invoke(item);
                if (min > cur) {
                    min = cur;
                    result = item;
                }
            }
            return result;
        }

        /// <summary>
        /// Returns the <paramref name="list"/> element that results on the largest floating point value returned by <paramref name="func"/>
        /// </summary>
        /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
        /// <param name="list">The IEnumerable containing all the elements to be evauated</param>
        /// <param name="func">The delegate function that calculates the floating point value for an element of <paramref name="list"/></param>
        /// <returns>The element that results on the largest value returned by <paramref name="func"/></returns>
        public static T MaxElement<T>(this IEnumerable<T> list, Func<T, float> func) {
            T result = default;
            float max = float.MinValue;
            foreach (T item in list) {
                float cur = func.Invoke(item);
                if (max < cur) {
                    max = cur;
                    result = item;
                }
            }
            return result;
        }

        /// <summary>
        /// Returns the <paramref name="list"/> element that results on the smallest integer value returned by <paramref name="func"/>
        /// </summary>
        /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
        /// <param name="list">The IEnumerable containing all the elements to be evauated</param>
        /// <param name="func">The delegate function that calculates the integer value for an element of <paramref name="list"/></param>
        /// <returns>The element that results on the smallest value returned by <paramref name="func"/></returns>
        public static T MinElement<T>(this IEnumerable<T> list, Func<T, int> func) {
            T result = default;
            int min = int.MaxValue;
            foreach (T item in list) {
                int cur = func.Invoke(item);
                if (min > cur) {
                    min = cur;
                    result = item;
                }
            }
            return result;
        }

        /// <summary>
        /// Returns the <paramref name="list"/> element that results on the largest integer value returned by <paramref name="func"/>
        /// </summary>
        /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
        /// <param name="list">The IEnumerable containing all the elements to be evauated</param>
        /// <param name="func">The delegate function that calculates the integer value for an element of <paramref name="list"/></param>
        /// <returns>The element that results on the largest value returned by <paramref name="func"/></returns>
        public static T MaxElement<T>(this IEnumerable<T> list, Func<T, int> func) {
            T result = default;
            int max = int.MinValue;
            foreach (T item in list) {
                int cur = func.Invoke(item);
                if (max < cur) {
                    max = cur;
                    result = item;
                }
            }
            return result;
        }
        #endregion

        #region Value, Element and Index
        /// <summary>
        /// Represents a comparable element of a list. Stores the element, it's index in the list and it's comparable value
        /// </summary>
        /// <typeparam name="E">The type of the elements of the list</typeparam>
        /// <typeparam name="V">The type of the values that the elements represent</typeparam>
        public struct ListElement<E, V> where V : IComparable {
            public E element;
            public int index;
            public V value;
        }

        /// <summary>
        /// Returns a <c>ListElement</c> based on the <paramref name="list"/> element that results on the smallest floating point value returned by <paramref name="func"/>
        /// </summary>
        /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
        /// <param name="list">The IList from which the element is taken</param>
        /// <param name="func">The delegate function that calculates the floating point value for an element of <paramref name="list"/></param>
        /// <returns>A <c>ListElement</c> based on the <paramref name="list"/> element that results on the smallest floating point value returned by <paramref name="func"/></returns>
        public static ListElement<T, float> FindMinimum<T>(this IList<T> list, Func<T, float> func) {
            ListElement<T, float> e = new ListElement<T, float>();
            e.value = float.MaxValue;
            e.index = -1;
            e.element = default;
            for (int i = 0; i < list.Count; i++) {
                float curValue = func(list[i]);
                if (e.value > curValue) {
                    e.value = curValue;
                    e.element = list[i];
                    e.index = i;
                }
            }
            return e;
        }

        /// <summary>
        /// Returns a <c>ListElement</c> based on the <paramref name="list"/> element that results on the largest floating point value returned by <paramref name="func"/>
        /// </summary>
        /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
        /// <param name="list">The IList from which the element is taken</param>
        /// <param name="func">The delegate function that calculates the floating point value for an element of <paramref name="list"/></param>
        /// <returns>A <c>ListElement</c> based on the <paramref name="list"/> element that results on the largest floating point value returned by <paramref name="func"/></returns>
        public static ListElement<T, float> FindMaximum<T>(this IList<T> list, Func<T, float> func) {
            ListElement<T, float> e = new ListElement<T, float>();
            e.value = float.MinValue;
            e.index = -1;
            e.element = default(T);
            for (int i = 0; i < list.Count; i++) {
                float curValue = func(list[i]);
                if (e.value < curValue) {
                    e.value = curValue;
                    e.element = list[i];
                    e.index = i;
                }
            }
            return e;
        }

        /// <summary>
        /// Returns a <c>ListElement</c> based on the <paramref name="list"/> element that results on the smallest integer value returned by <paramref name="func"/>
        /// </summary>
        /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
        /// <param name="list">The IList from which the element is taken</param>
        /// <param name="func">The delegate function that calculates the integer value for an element of <paramref name="list"/></param>
        /// <returns>A <c>ListElement</c> based on the <paramref name="list"/> element that results on the smallest value returned by <paramref name="func"/></returns>
        public static ListElement<T, int> FindMinimum<T>(this IList<T> list, Func<T, int> func) {
            ListElement<T, int> e = new ListElement<T, int>();
            e.value = int.MaxValue;
            e.index = -1;
            e.element = default(T);
            for (int i = 0; i < list.Count; i++) {
                int curValue = func(list[i]);
                if (e.value > curValue) {
                    e.value = curValue;
                    e.element = list[i];
                    e.index = i;
                }
            }
            return e;
        }

        /// <summary>
        /// Returns a <c>ListElement</c> based on the <paramref name="list"/> element that results on the largest integer value returned by <paramref name="func"/>
        /// </summary>
        /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
        /// <param name="list">The IList from which the element is taken</param>
        /// <param name="func">The delegate function that calculates the integer value for an element of <paramref name="list"/></param>
        /// <returns>A <c>ListElement</c> based on the <paramref name="list"/> element that results on the largest value returned by <paramref name="func"/></returns>
        public static ListElement<T, int> FindMaximum<T>(this IList<T> list, Func<T, int> func) {
            ListElement<T, int> e = new ListElement<T, int>();
            e.value = int.MinValue;
            e.index = -1;
            e.element = default(T);
            for (int i = 0; i < list.Count; i++) {
                int curValue = func(list[i]);
                if (e.value < curValue) {
                    e.value = curValue;
                    e.element = list[i];
                    e.index = i;
                }
            }
            return e;
        }
        #endregion
        #endregion

        #region Sum
        public static Vector2 Sum(this IEnumerable<Vector2> enumerable) {
            Vector2 result = Vector2.zero;
            foreach (Vector2 vector in enumerable) {
                result += vector;
            }
            return result;
        }
        public static Vector2Int Sum(this IEnumerable<Vector2Int> enumerable) {
            Vector2Int result = Vector2Int.zero;
            foreach (Vector2Int vector in enumerable) {
                result += vector;
            }
            return result;
        }
        public static Vector3 Sum(this IEnumerable<Vector3> enumerable) {
            Vector3 result = Vector3.zero;
            foreach (Vector3 vector in enumerable) {
                result += vector;
            }
            return result;
        }
        public static Vector3Int Sum(this IEnumerable<Vector3Int> enumerable) {
            Vector3Int result = Vector3Int.zero;
            foreach (Vector3Int vector in enumerable) {
                result += vector;
            }
            return result;
        }
        public static Vector4 Sum(this IEnumerable<Vector4> enumerable) {
            Vector4 result = Vector4.zero;
            foreach (Vector4 vector in enumerable) {
                result += vector;
            }
            return result;
        }
        public static T Sum<T>(this IEnumerable<T> enumerable, Func<T, T, T> addFunc, T initial = default) => enumerable.Aggregate(initial, addFunc);

        public static V SumPairs<V, E>(this IList<E> list, Func<V, E, E, V> addFunc, bool ciclic = false, bool jumpUsedElement = false) {
            V result = default(V);
            list.ForEachConsecutivePair((f, s) => result = addFunc(result, f, s), ciclic, jumpUsedElement);
            return result;
        }

        public static int SumPairs<T>(this IList<T> list, Func<T, T, int> converter, bool ciclic = false, bool jumpUsedElement = false) {
            int result = 0;
            list.ForEachConsecutivePair((e1, e2) => result += converter.Invoke(e1, e2), ciclic, jumpUsedElement);
            return result;
        }

        public static float SumPairs<T>(this IList<T> list, Func<T, T, float> converter, bool ciclic = false, bool jumpUsedElement = false) {
            float result = 0f;
            list.ForEachConsecutivePair((e1, e2) => result += converter.Invoke(e1, e2), ciclic, jumpUsedElement);
            return result;
        }

        public static float SumTrios<T>(this IList<T> list, Func<T, T, T, float> converter, bool ciclic = false, bool jumpUsedElement = false) {
            float result = 0f;
            list.ForEachConsecutiveTrio((e1, e2, e3) => result += converter.Invoke(e1, e2, e3), ciclic, jumpUsedElement);
            return result;
        }
        #endregion

        #region Average
        public static Vector2 Average(this IEnumerable<Vector2> enumerable) {
            Vector2 sum = Vector2.zero;
            int count = 0;
            foreach (Vector2 vector in enumerable) {
                sum += vector;
                count++;
            }
            return sum / count;
        }
        public static Vector2Int Average(this IEnumerable<Vector2Int> enumerable) {
            Vector2Int sum = Vector2Int.zero;
            int count = 0;
            foreach (Vector2Int vector in enumerable) {
                sum += vector;
                count++;
            }
            return sum / count;
        }
        public static Vector3 Average(this IEnumerable<Vector3> enumerable) {
            Vector3 sum = Vector3.zero;
            int count = 0;
            foreach (Vector3 vector in enumerable) {
                sum += vector;
                count++;
            }
            return sum / count;
        }
        public static Vector3Int Average(this IEnumerable<Vector3Int> enumerable) {
            Vector3Int sum = Vector3Int.zero;
            int count = 0;
            foreach (Vector3Int vector in enumerable) {
                sum += vector;
                count++;
            }
            return sum / count;
        }
        public static Vector4 Average(this IEnumerable<Vector4> enumerable) {
            Vector4 sum = Vector4.zero;
            int count = 0;
            foreach (Vector4 vector in enumerable) {
                sum += vector;
                count++;
            }
            return sum / count;
        }
        public static T Average<T>(this IEnumerable<T> enumerable, Func<T, T, T> addFunc, Func<T, int, T> divFunc) => divFunc(enumerable.Sum(addFunc), enumerable.Count());

        public static V Median<V, E>(this IEnumerable<E> enumerable, Comparison<E> comparison, Func<E, E, V> meanFunc) {
            List<E> temp = new(enumerable);
            temp.Sort(comparison);
            int n = temp.Count;
            if (n % 2 == 0) {
                return meanFunc.Invoke(temp[(n / 2) - 1], temp[n / 2]);
            }
            else {
                return meanFunc.Invoke(temp[n / 2], temp[n / 2]);
            }
        }
        public static V Median<V, E>(this IEnumerable<E> list, IComparer<E> comparer, Func<E, E, V> meanFunc) {
            List<E> temp = new(list);
            temp.Sort(comparer);
            int n = temp.Count;
            if (n % 2 == 0) {
                return meanFunc.Invoke(temp[(n / 2) - 1], temp[n / 2]);
            }
            else {
                return meanFunc.Invoke(temp[n / 2], temp[n / 2]);
            }
        }
        public static E Mode<E>(this IEnumerable<E> list) => list.Mode((e1, e2) => e1.Equals(e2));
        public static E Mode<E>(this IEnumerable<E> list, Func<E, E, bool> equalsFunc) => list.CountOccurrences().MaxElement(o => o.Value).Key;
        #endregion
        #endregion

        #region Iterations
        #region Action
        public static void ForEach<T>(this IEnumerable<T> list, Action action) {
            foreach (T _ in list) {
                action.Invoke();
            }
        }
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action) {
            foreach (T e in list) {
                action.Invoke(e);
            }
        }
        public static void ForEach<T>(this IList<T> list, Action<T, int> action) {
            for (int i = 0; i < list.Count; i++) {
                action.Invoke(list[i], i);
            }
        }

        public static void ForEachBreak<T>(this IEnumerable<T> list, Action action, Predicate<T> breakCondition, bool breakBeforeAction = false) {
            foreach (T e in list) {
                if (breakBeforeAction && breakCondition.Invoke(e)) {
                    break;
                }
                action.Invoke();
                if (!breakBeforeAction && breakCondition.Invoke(e)) {
                    break;
                }
            }
        }
        public static void ForEachBreak<T>(this IEnumerable<T> list, Action<T> action, Predicate<T> breakCondition, bool breakBeforeAction = false) {
            foreach (T e in list) {
                if (breakBeforeAction && breakCondition.Invoke(e)) {
                    break;
                }
                action.Invoke(e);
                if (!breakBeforeAction && breakCondition.Invoke(e)) {
                    break;
                }
            }
        }
        public static void ForEachBreak<T>(this IList<T> list, Action<T, int> action, Predicate<T> breakCondition, bool breakBeforeAction = false) {
            for (int i = 0; i < list.Count; i++) {
                if (breakBeforeAction && breakCondition.Invoke(list[i])) {
                    break;
                }
                action.Invoke(list[i], i);
                if (!breakBeforeAction && breakCondition.Invoke(list[i])) {
                    break;
                }
            }
        }

        public static void ForEachConditional<T>(this IEnumerable<T> list, Action action, Predicate<T> applyCondition) {
            foreach (T e in list) {
                if (applyCondition.Invoke(e))
                    action.Invoke();
            }
        }
        public static void ForEachConditional<T>(this IEnumerable<T> list, Action<T> action, Predicate<T> applyCondition) {
            foreach (T e in list) {
                if (applyCondition.Invoke(e))
                    action.Invoke(e);
            }
        }
        public static void ForEachConditional<T>(this IList<T> list, Action<T, int> action, Predicate<T> applyCondition) {
            for (int i = 0; i < list.Count; i++) {
                if (applyCondition.Invoke(list[i]))
                    action.Invoke(list[i], i);
            }
        }

        public static void ForEachConsecutivePair<T>(this IList<T> list, Action<T, T> action, bool ciclic = false, bool jumpUsedElement = false) {
            list.ForEachConsecutiveN(2, l => action.Invoke(l[0], l[1]), ciclic, jumpUsedElement);
        }
        public static void ForEachConsecutiveTrio<T>(this IList<T> list, Action<T, T, T> action, bool ciclic = false, bool jumpUsedElements = false) {
            list.ForEachConsecutiveN(3, l => action.Invoke(l[0], l[1], l[2]), ciclic, jumpUsedElements);
        }
        public static void ForEachConsecutiveN<T>(this IList<T> list, int N, Action<IList<T>> action, bool ciclic = false, bool jumpUsedElements = false) {
            int n = list.Count;
            if (n < N) {
                return;
            }
            int increment = (jumpUsedElements) ? N : 1;
            int size = (ciclic) ? n : n - 1;
            for (int i = 0; i < size; i += increment) {
                action.Invoke(new List<T>(list.GetRange(i, N, ciclic)));
            }
        }
        #endregion

        #region Func
        public static void SetEach<T>(this IList<T> list, Func<T> func) {
            for (int i = 0; i < list.Count; i++) {
                list[i] = func.Invoke();
            }
        }
        public static void SetEachElement<T>(this IList<T> list, Func<T, T> func) {
            for (int i = 0; i < list.Count; i++) {
                list[i] = func.Invoke(list[i]);
            }
        }
        public static void SetEachIndex<T>(this IList<T> list, Func<int, T> func) {
            for (int i = 0; i < list.Count; i++) {
                list[i] = func.Invoke(i);
            }
        }
        public static void SetEachElementIndex<T>(this IList<T> list, Func<T, int, T> func) {
            for (int i = 0; i < list.Count; i++) {
                list[i] = func.Invoke(list[i], i);
            }
        }

        public static List<T> ThruFunc<T>(this List<T> list, Func<T> func) {
            List<T> result = new List<T>(list);
            result.SetEach(func);
            return result;
        }
        public static List<T> ThruFuncElement<T>(this List<T> list, Func<T, T> eFunc) {
            List<T> result = new List<T>(list);
            result.SetEachElement(eFunc);
            return result;
        }
        public static List<T> ThruFuncIndex<T>(this List<T> list, Func<int, T> iFunc) {
            List<T> result = new List<T>(list);
            result.SetEachIndex(iFunc);
            return result;
        }
        public static List<T> ThruFuncElementIndex<T>(this List<T> list, Func<T, int, T> eiFunc) {
            List<T> result = new List<T>(list);
            result.SetEachElementIndex(eiFunc);
            return result;
        }

        #region Reduce
        /// <summary>
        /// Reduces an IEnumerable to a single value
        /// </summary>
        /// <typeparam name="E">The type of the elements in the <paramref name="list"/></typeparam>
        /// <typeparam name="V">The type of the value to which the <paramref name="list"/> is being reduced</typeparam>
        /// <param name="list">The IEnumerable that is being reduced</param>
        /// <param name="func">The reducing delegate function, it takes an element of the <paramref name="list"/> and the current reduced value and returns the newly reduced value</param>
        /// <param name="initial">The initial value to start the iteration. This value is sent as the parameter of <paramref name="func"/> for the first element on <paramref name="list"/></param>
        /// <returns>The resulting value after reducing each element from <paramref name="list"/></returns>
        public static V Reduce<E, V>(this IEnumerable<E> list, Func<E, V, V> func, V initial = default(V)) {
            V result = initial;
            list.ForEach(e => result = func(e, result));
            return result;
        }
        /// <summary>
        /// Reduces an IEnumerable to a single value
        /// </summary>
        /// <typeparam name="T">The type of the elements in the <paramref name="list"/> and the type of the return value</typeparam>
        /// <param name="list">The IEnumerable that is being reduced</param>
        /// <param name="func">The reducing delegate function, it takes an element of the <paramref name="list"/> and the current reduced value and returns the newly reduced value</param>
        /// <param name="initial">The initial value to start the iteration. This value is sent as the parameter of <paramref name="func"/> for the first element on <paramref name="list"/></param>
        /// <returns>The resulting value after reducing each element from <paramref name="list"/></returns>
        public static T Reduce<T>(this IEnumerable<T> list, Func<T, T, T> func, T initial = default(T)) {
            return list.Reduce<T, T>(func, initial);
        }
        /// <summary>
        /// Reduces an IEnumerable to a single value
        /// </summary>
        /// <typeparam name="T">The type of the elements in the <paramref name="list"/> and the type of the return value</typeparam>
        /// <param name="list">The IEnumerable that is being reduced</param>
        /// <param name="func">The reducing delegate function, it takes an element of the <paramref name="list"/> and the current reduced value and returns the newly reduced value</param>
        /// <param name="var">The variable from and to which the <paramref name="list"/> is reduced. In each iteration it is used as the second parameter of <paramref name="func"/> and it's result is applied to it</param>
        public static void Reduce<T>(this IEnumerable<T> list, Func<T, T, T> func, ref T var) {
            foreach (T e in list) {
                var = func.Invoke(e, var);
            }
        }

        public static V Reduce<E, V>(this IEnumerable<E> list, Func<E, V, V> func, Predicate<V> stopConditionBasedOnAccumulator, V initial = default(V), bool stopBeforeAction = false) {
            V result = initial;
            list.ForEachBreak(e => result = func(e, result), e => stopConditionBasedOnAccumulator.Invoke(result), stopBeforeAction);
            return result;
        }
        public static T Reduce<T>(this IEnumerable<T> list, Func<T, T, T> func, Predicate<T> stopConditionBasedOnAccumulator, T initial = default(T), bool stopBeforeAction = false) {
            T result = initial;
            list.ForEachBreak(e => result = func(e, result), e => stopConditionBasedOnAccumulator.Invoke(result), stopBeforeAction);
            return result;
        }

        public static List<T> Accumulated<T>(this IEnumerable<T> list, Func<T, T, T> func, T initial = default) {
            List<T> result = new();
            T accumulator = initial;
            list.ForEach(e => {
                accumulator = func(e, accumulator);
                result.Add(accumulator);
                }
            );
            return result;
        }
        #endregion
        #endregion
        #endregion

        #region List Method Variants
        #region Find
        public static T Find<T>(this IEnumerable<T> list, Predicate<T> predicate) {
            foreach (T e in list) {
                if (predicate.Invoke(e)) {
                    return e;
                }
            }
            return default(T);
        }
        public static int FindIndex<T>(this IList<T> list, Predicate<T> predicate) {
            int n = list.Count;
            for (int i = 0; i < n; i++) {
                if (predicate.Invoke(list[i])) {
                    return i;
                }
            }
            return -1;
        }
        public static List<T> FindAll<T>(this IEnumerable<T> list, Predicate<T> predicate) {
            List<T> result = new List<T>();
            foreach (T e in list) {
                if (predicate.Invoke(e)) {
                    result.Add(e);
                }
            }
            return result;
        }
        public static List<int> FindAllIndex<T>(this IList<T> list, Predicate<T> predicate) {
            List<int> result = new List<int>();
            int n = list.Count;
            for (int i = 0; i < n; i++) {
                if (predicate.Invoke(list[i])) {
                    result.Add(i);
                }
            }
            return result;
        }
        public static T FindThruIndex<T>(this IList<T> list, Predicate<int> indexPredicate) {
            for (int i = 0; i < list.Count; i++) {
                if (indexPredicate(i)) {
                    return list[i];
                }
            }
            return default(T);
        }
        public static int FindIndexThruIndex<T>(this ICollection<T> list, Predicate<int> indexPredicate) {
            for (int i = 0; i < list.Count; i++) {
                if (indexPredicate(i)) {
                    return i;
                }
            }
            return -1;
        }
        public static List<T> FindAllThruIndex<T>(this IList<T> list, Predicate<int> indexPredicate) {
            List<T> result = new List<T>();
            for (int i = 0; i < list.Count; i++) {
                if (indexPredicate(i)) {
                    result.Add(list[i]);
                }
            }
            return result;
        }
        public static List<int> FindAllIndexThruIndex<T>(this ICollection<T> list, Predicate<int> indexPredicate) {
            List<int> result = new List<int>();
            for (int i = 0; i < list.Count; i++) {
                if (indexPredicate(i)) {
                    result.Add(i);
                }
            }
            return result;
        }

        public static S FindOfType<T, S>(this IList<T> list) where S : T {
            foreach (T item in list) {
                if (item is S s) {
                    return s;
                }
            }
            return default;
        }
        public static List<S> FindAllOfType<T, S>(this IList<T> list) where S : T {
            List<S> result = new();
            foreach (T item in list) {
                if (item is S s) {
                    result.Add(s);
                }
            }
            return result;
        }

        public static T2 FindInside<T1,T2>(this IList<T1> list, Func<T1,List<T2>> innerListGetter, Predicate<T2> predicate) {
            foreach(T1 item in list) {
                foreach(T2 innerItem in innerListGetter.Invoke(item)) {
                    if (predicate.Invoke(innerItem)) {
                        return innerItem;
                    }
                }
            }
            return default;
        }

        public static int FindAccumulatedIndex(this IList<int> list, int targetValue, out int remainder) {
            int sum = 0;
            int i = 0;
            for (; i < list.Count; i++) {
                if (sum + list[i] >= targetValue) {
                    remainder = targetValue - sum;
                    return i;
                }
                sum += list[i];
            }
            remainder = targetValue - sum;
            return i;
        }
        public static int FindAccumulatedElement(this IList<int> list, int targetValue, out int remainder) {
            int sum = 0;
            foreach (int e in list) {
                if (sum + e >= targetValue) {
                    remainder = targetValue - sum;
                    return e;
                }
                sum += e;
            }
            remainder = targetValue - sum;
            return default;
        }
        public static int FindAccumulatedIndex(this IList<float> list, float targetValue, out float remainder) {
            float sum = 0;
            int i = 0;
            for (; i < list.Count; i++) {
                if (sum + list[i] >= targetValue) {
                    remainder = targetValue - sum;
                    return i;
                }
                sum += list[i];
            }
            remainder = targetValue - sum;
            return i;
        }
        public static float FindAccumulatedElement(this IList<float> list, float targetValue, out float remainder) {
            float sum = 0;
            foreach (float e in list) {
                if (sum + e >= targetValue) {
                    remainder = targetValue - sum;
                    return e;
                }
                sum += e;
            }
            remainder = targetValue - sum;
            return default;
        }
        public static int FindAccumulatedIndex<T>(this IList<T> list, int targetValue, Func<T, int> elementValue, out int remainder) {
            int sum = 0;
            int i = 0;
            for (; i < list.Count; i++) {
                int curValue = elementValue(list[i]);
                if (sum + curValue >= targetValue) {
                    remainder = targetValue - sum;
                    return i;
                }
                sum += curValue;
            }
            remainder = targetValue - sum;
            return i;
        }
        public static T FindAccumulatedElement<T>(this IEnumerable<T> list, int targetValue, Func<T, int> elementValue, out int remainder) {
            int sum = 0;
            foreach (T e in list) {
                int curValue = elementValue(e);
                if (sum + curValue >= targetValue) {
                    remainder = targetValue - sum;
                    return e;
                }
                sum += curValue;
            }
            remainder = targetValue - sum;
            return default;
        }
        public static int FindAccumulatedIndex<T>(this IList<T> list, float targetValue, Func<T, float> elementValue, out float remainder) {
            float sum = 0;
            int i = 0;
            for (; i<list.Count; i++) {
                float curValue = elementValue(list[i]);
                if (sum + curValue >= targetValue) {
                    remainder = targetValue - sum;
                    return i;
                }
                sum += curValue;
            }
            remainder = targetValue - sum;
            return i;
        }
        public static T FindAccumulatedElement<T>(this IEnumerable<T> list, float targetValue, Func<T, float> elementValue, out float remainder) {
            float sum = 0;
            foreach (T e in list) {
                float curValue = elementValue(e);
                if (sum + curValue >= targetValue) {
                    remainder = targetValue - sum;
                    return e;
                }
                sum += curValue;
            }
            remainder = targetValue - sum;
            return default;
        }

        public static bool FindAccumulatedElementPair<T>(this IList<T> list, float target, Func<T, T, float> pairValue, out float localTargetValue, out T element1, out T element2) {
            List<float> pairValues = list.MergedInPairs(pairValue); 
            float value = target * pairValues.Sum();
            float sum = 0;
            localTargetValue = 0;
            element1 = default;
            element2 = default;
            for (int i=0; i< pairValues.Count; i++) {
                float v = pairValues[i];
                if (sum + v >= value) {
                    localTargetValue = (value - sum) / v;
                    element1 = list[i];
                    element2 = list[i+1];
                    return true;
                }
                sum += v;
            }
            return false;
        }

        public static T FindLastConsecutive<T>(this IList<T> list, Predicate<T> predicate) {
            for (int i = 0; i < list.Count; i++) {
                if (!predicate(list[i])) {
                    if (i == 0) {
                        return default;
                    }
                    return list[i-1];
                }
            }
            return list.Last();
        }
        #endregion

        #region Add
        public static void AddRange<T>(this ICollection<T> list, IEnumerable<T> elements) {
            foreach (T element in elements) {
                list.Add(element);
            }
        }
        public static void AddIfDoesntContain<T>(this ICollection<T> list, T element) {
            if (!list.Contains(element)) {
                list.Add(element);
            }
        }
        public static void AddRangeIfDoesntContain<T>(this ICollection<T> list, IEnumerable<T> elements) {
            foreach (T element in elements) {
                if (!list.Contains(element)) {
                    list.Add(element);
                }
            }
        }

        public static void EnsureKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue) {
            if(!dictionary.ContainsKey(key)) {
                dictionary.Add(key, defaultValue);
            }
        }
        public static void Set<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value) {
            if (dictionary.ContainsKey(key)) {
                dictionary[key] = value;
            }
            else {
                dictionary.Add(key, value);
            }
        }
        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue) {
            if(dictionary.ContainsKey(key)) {
                return dictionary[key];
            } else {
                return defaultValue;
            }
        }
        public static void OperateValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue, TValue> operation, TValue defaultValue = default) {
            dictionary.Set(key, operation.Invoke(dictionary.Get(key, defaultValue)));
        }

        public static List<T> With<T>(this List<T> list, T element) => new List<T>(list) { element };
        public static List<T> With<T>(this List<T> list, int index, T element) {
            List<T> result = new List<T>(list);
            result.Insert(index ,element);
            return result;
        }
        public static List<T> WithFirst<T>(this List<T> list, T element) {
            List<T> result = new List<T>(list);
            result.Insert(0, element);
            return result;
        }
        #endregion

        #region Remove
        public static void RemoveAt<T>(this IList<T> list, IList<int> indices) {
            for (int i = 0; i < indices.Count; i++) {
                list.RemoveAt(indices[i] - i);
            }
        }
        public static void RemoveRange<T>(this IList<T> list, int start, int count) {
            for (int i = 0; i < count; i++) {
                list.RemoveAt(start);
            }
        }
        public static void RemoveRange<T>(this IList<T> list, List<T> elementsToRemove) {
            elementsToRemove.ForEach(e => list.Remove(e));
        }

        public static List<T> Without<T>(this ICollection<T> list, T elementToRemove) {
            List<T> result = new List<T>(list);
            result.Remove(elementToRemove);
            return result;
        }
        public static List<T> WithoutRange<T>(this ICollection<T> list, List<T> elementsToRemove) {
            List<T> result = new List<T>(list);
            result.RemoveRange(elementsToRemove);
            return result;
        }
        public static List<T> WithoutAll<T>(this ICollection<T> list, Predicate<T> predicate) {
            List<T> result = new List<T>(list);
            result.RemoveAll(predicate);
            return result;
        }
        #endregion

        #region GetRange
        public static List<T> StartingAt<T>(this List<T> list, int start) => list.GetRange(start, list.Count - start);
        public static IEnumerable<T> StartingAt<T>(this IEnumerable<T> list, int start) {
            int i = 0;
            foreach (T item in list) {
                if (i >= start) {
                    yield return item;
                }
                i++;
            }
        }
        public static List<T> GetRange<T>(this List<T> list, int start, int size, bool ciclic = false) {
            List<T> result = new List<T>();
            int listCount = list.Count;
            if (ciclic) {
                for (int i = 0; i < size; i++) {
                    int j = (start + i) % listCount;
                    result.Add(list[j]);
                }
            }
            else {
                if (start + size <= listCount) {
                    for (int i = 0; i < size; i++) {
                        result.Add(list[start + i]);
                    }
                }
                else {
                    return null;
                }
            }
            return result;
        }
        public static IEnumerable<T> GetRange<T>(this IEnumerable<T> enumerable, int start, int size, bool ciclic = false) {
            int i = 0;
            int count = 0;
            foreach (T e in enumerable) {
                if (count == size) {
                    yield break;
                }
                if (i >= start) {
                    yield return e;
                    count++;
                }
                i++;
            }
            if (ciclic) {
                while (count < size) {
                    foreach (T e in enumerable) {
                        if (count == size) {
                            yield break;
                        }
                        yield return e;
                        count++;
                    }
                }
            }
        }
        #endregion

        #region Count
        public static Dictionary<T, int> CountOccurrences<T>(this IEnumerable<T> list) {
            Dictionary<T, int> result = new();
            foreach (T e in list) {
                result.EnsureKey(e, 0);
                result[e]++;
            }
            return result;
        }
        #endregion

        #region Contains
        public static bool ContainsAny<T>(this IEnumerable<T> list, ICollection<T> possibilities) {
            foreach (T e in list) {
                if (possibilities.Contains(e)) {
                    return true;
                }
            }
            return false;
        }
        public static bool ContainsAll<T>(this IEnumerable<T> list, ICollection<T> other) {
            foreach (T e in other) {
                if (!list.Contains(e)) {
                    return false;
                }
            }
            return true;
        }

        public static bool ContainsInside<T, L>(this IList<L> list, T element) where L : IList<T> {
            foreach(L innerList in list) {
                if(innerList.Contains(element)) {
                    return true;
                }
            }
            return default;
        }
        public static bool ContainsInside<T1, T2>(this IList<T1> list, Func<T1, List<T2>> innerListGetter, T2 element) {
            foreach(T1 item in list) {
                if (innerListGetter.Invoke(item).Contains(element)) {
                    return true;
                }
            }
            return default;
        }
        #endregion

        #region Equals
        public static bool SameElementsAs<T>(this List<T> listA, List<T> listB) {
            if (listA == null || listB == null || listA.Count != listB.Count) {
                return false;
            }
            if (listA.Count == 0) {
                return true;
            }
            Dictionary<T, int> lookUp = listA.CountOccurrences();
            foreach (T element in listB) {
                if (lookUp.ContainsKey(element) && lookUp[element] > 0) {
                    lookUp[element]--;
                    if (lookUp[element] == 0) {
                        lookUp.Remove(element);
                    }
                }
                else {
                    return false;
                }
            }
            return lookUp.Count == 0;
        }
        #endregion

        #region Sort
        public static void SortAsInt<T>(this List<T> list, Func<T, int> converter) {
            list.Sort((a, b) => converter(a) > converter(b) ? 1 : converter(a) == converter(b) ? 0 : -1);
        }
        public static void SortAsFloat<T>(this List<T> list, Func<T, float> converter) {
            list.Sort((a, b) => converter(a) > converter(b) ? 1 : converter(a) == converter(b) ? 0 : -1);
        }

        public static List<int> Sorted(this IList<int> list) {
            List<int> result = new List<int>(list);
            result.Sort();
            return result;
        }
        public static List<float> Sorted(this IList<float> list) {
            List<float> result = new List<float>(list);
            result.Sort();
            return result;
        }
        public static List<T> SortedBy<T>(this IList<T> list, IList<int> indices) {
            List<T> result = new();
            for (int i = 0; i < indices.Count; i++) {
                result.Add(list[indices[i]]);
            }
            return result;
        }

        public static List<T> InvertedList<T>(this IEnumerable<T> list) {
            List<T> result = new List<T>();
            foreach (T e in list) {
                result.Insert(0, e);
            }
            return result;
        }
        public static void Invert<T>(this IList<T> list) {
            int n = list.Count;
            for (int i = 1; i < n; i++) {
                T e = list[i];
                list.RemoveAt(i);
                list.Insert(0, e);
            }
        }
        #endregion

        #region ConvertAll
        public static IEnumerable<TOut> ConvertAll<TIn, TOut>(this IEnumerable<TIn> list, Func<TIn, TOut> converter) {
            return list.Select(converter);
        }
        public static List<TOut> ConvertAllThruIndex<TIn, TOut>(this IList<TIn> list, Func<int, TIn, TOut> converter) {
            List<TOut> result = new List<TOut>();
            for (int i = 0; i < list.Count; i++) {
                result.Add(converter.Invoke(i, list[i]));
            }
            return result;
        }
        #endregion

        #region TrueForAll
        public static bool TrueForAll<T>(this IEnumerable<T> list, Predicate<T> match) {
            foreach (T e in list) {
                if (!match.Invoke(e)) {
                    return false;
                }
            }
            return true;
        }
        public static bool TrueForAllThruIndex<T>(this IList<T> list, Predicate<int> match) {
            for (int i=0; i<list.Count; i++) {
                if (!match.Invoke(i)) {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region Exists
        public static bool Exists<T>(this IEnumerable<T> list, Predicate<T> match) {
            foreach (T e in list) {
                if (match.Invoke(e)) {
                    return true;
                }
            }
            return false;
        }
        public static bool ExistsThruIndex<T>(this IList<T> list, Predicate<int> match) {
            for (int i = 0; i < list.Count; i++) {
                if (match.Invoke(i)) {
                    return true;
                }
            }
            return false;
        }
        public static bool ExistsConsecutivePair<T>(this IList<T> list, Func<T, T, bool> match, bool ciclic = false, bool jumpUsedElement = false) {
            bool exists = false;
            list.ForEachConsecutivePair((e0, e1) => {
                if (match.Invoke(e0, e1)) {
                    exists = true;
                }
            }, ciclic, jumpUsedElement);
            return exists;
        }
        public static bool ExistsConsecutiveN<T>(this IList<T> list, int N, Predicate<IList<T>> match, bool ciclic = false, bool jumpUsedElement = false) {
            bool exists = false;
            list.ForEachConsecutiveN(N, l => {
                if (match.Invoke(l)) {
                    exists = true;
                }
            }, ciclic, jumpUsedElement);
            return exists;
        }
        #endregion

        #region IndexOf
        internal static int IndexOf<T>(this IEnumerable<T> list, T element) {
            int i = 0;
            foreach(T item in list) {
                if ((item == null && element == null) || (item != null && item.Equals(element))) {
                    return i;
                }
                i++;
            }
            return -1;
        }
        #endregion

        #region To String
        public static string ToStringFull<T>(this IList<T> list, string begin = ": ", string separator = ", ", string end = ";") {
            return list.ToString() + begin + list.ToStringItems(separator) + end;
        }
        public static string ToStringItems<T>(this IList<T> list, string separator = ", ") {
            return list.ToStringItems(o => o.ToString(), separator);
        }
        public static string ToStringItems<T>(this IList<T> list, Func<T, string> toStringFunc, string separator = ", ") {
            string result = "";
            for (int i = 0; i < list.Count; i++) {
                result += (result.Length > 0? separator:"") + (list[i] == null ? "null" : toStringFunc.Invoke(list[i]));
            }
            return result;
        }
        #endregion
        #endregion

        #region Doubles
        public static bool HasDoubles<T>(this IEnumerable<T> list) {
            Dictionary<T, int> check = new Dictionary<T, int>();
            foreach (T e in list) {
                if (check.ContainsKey(e)) {
                    return true;
                }
                else {
                    check.Add(e, 1);
                }
            }
            return false;
        }
        public static bool HasDoubles<T>(this IList<T> list, Func<T, T, bool> equalsFunc) {
            int n = list.Count;
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    if (i != j && equalsFunc.Invoke(list[i], list[j])) {
                        return true;
                    }
                }
            }
            return false;
        }

        public static List<T> WithoutDoubles<T>(this IEnumerable<T> list) {
            List<T> result = new List<T>();
            list.ForEach(e => result.AddIfDoesntContain(e));
            return result;
        }
        #endregion

        #region Formating
        public enum SplitOptions {
            DiscardSplitPoint,
            PutSplitPointLeft,
            PutSplitPointRight
        }
        public static List<List<T>> Split<T>(this IList<T> list, IEnumerable<int> splitPoints, SplitOptions options) {
            List<List<T>> result = new List<List<T>>();
            List<T> curSection = new List<T>();
            for (int i = 0; i < list.Count; i++) {
                if (splitPoints.Contains(i)) {
                    switch (options) {
                        case SplitOptions.DiscardSplitPoint:
                            result.Add(curSection);
                            curSection = new List<T>();
                            break;
                        case SplitOptions.PutSplitPointLeft:
                            curSection.Add(list[i]);
                            result.Add(curSection);
                            curSection = new List<T>();
                            break;
                        case SplitOptions.PutSplitPointRight:
                            result.Add(curSection);
                            curSection = new List<T> { list[i] };
                            break;
                        default:
                            result.Add(curSection);
                            curSection = new List<T>();
                            break;
                    }
                }
                else {
                    curSection.Add(list[i]);
                }
            }
            return result;
        }

        public static List<List<T>> Group<T>(this IEnumerable<T> list, Func<T, T, bool> classifier) {
            List<List<T>> result = new List<List<T>>();
            foreach (T item in list) {
                List<T> group = result.Find(g => classifier.Invoke(g.First(), item));
                if (group != null) {
                    group.Add(item);
                }
                else {
                    result.Add(new List<T>() { item });
                }
            }
            return result;
        }

        public static List<T> AsSegments<T>(this List<T> list, Func<T, T, bool> sameSegment) {
            List<T> result = new List<T>(list);
            int k;
            for (int i = 0; i < result.Count; i++) {
                k = i;
                for (int j = i + 1; j < result.Count; j++) {
                    bool canRemoveBetween = sameSegment.Invoke(result[i], result[j]);
                    if (canRemoveBetween) {
                        k = j;
                    }
                    else if (k != i) {
                        int removeCount = k - i - 1;
                        result.RemoveRange(i + 1, removeCount);
                        k -= removeCount;
                        break;
                    }
                }
                if (k != i) {
                    int removeCount = k - i - 1;
                    result.RemoveRange(i + 1, removeCount);
                }
            }
            return result;
        }

        public static List<T> ToSingleList<C, T>(this IEnumerable<C> list, Func<C, List<T>> func) {
            List<T> result = new List<T>();
            foreach (C collection in list) {
                if (collection != null) {
                    result.AddRange(func.Invoke(collection));
                }
            }
            return result;
        }
        public static List<T> ToSingleList<T>(this IEnumerable<List<T>> list) {
            List<T> result = new List<T>();
            foreach (IEnumerable<T> group in list) {
                result.AddRange(group);
            }
            return result;
        }
        public static List<T> ToSingleList<T>(this IEnumerable<T[]> list) {
            List<T> result = new List<T>();
            foreach (IEnumerable<T> group in list) {
                result.AddRange(group);
            }
            return result;
        }
        public static List<T> ToSingleList<T>(this T[,] matrix) {
            List<T> result = new List<T>();
            foreach (T element in matrix) {
                result.Add(element);
            }
            return result;
        }

        public static List<T> IntercalatedWith<T>(this IList<T> list, IList<T> other, bool startWithOther = false) {
            List<T> result = new List<T>();

            int N = list.Count;
            int M = other.Count;
            int i;
            for (i = 0; i < N && i < M; i++) {
                if (startWithOther) {
                    result.Add(other[i]);
                    result.Add(list[i]);
                }
                else {
                    result.Add(list[i]);
                    result.Add(other[i]);
                }
            }
            if (i < N) {
                result.AddRange(list.GetRange(i, N - i));
            }
            else if (i < M) {
                result.AddRange(other.GetRange(i, M - i));
            }

            return result;
        }

        public static T[,] BreakIntoMatrix<T>(this T[] array, int width) {
            int length = array.Length;
            int height = Mathf.CeilToInt(length / (float)width);
            T[,] matrix = new T[height, width];
            int x = 0;
            int y = 0;
            for (int i = 0; i < length; i++) {
                matrix[y, x] = array[i];
                x++;
                if (x == width) {
                    x = 0;
                    y++;
                }
            }
            return matrix;
        }

        public static List<TOut> MergedInPairs<T, TOut>(this IList<T> list, Func<T, T, TOut> merger, bool ciclic = false, bool jumpUsedElement = false) {
            List<TOut> result = new List<TOut>();
            list.ForEachConsecutivePair((x1, x2) => result.Add(merger.Invoke(x1, x2)), ciclic, jumpUsedElement);
            return result;
        }
        public static List<TOut> MergedInsTrios<T, TOut>(this IList<T> list, Func<T, T, T, TOut> merger, bool ciclic = false, bool jumpUsedElement = false) {
            List<TOut> result = new List<TOut>();
            list.ForEachConsecutiveTrio((x1, x2, x3) => result.Add(merger.Invoke(x1, x2, x3)), ciclic, jumpUsedElement);
            return result;
        }
        public static List<TOut> MergedInN<T, TOut>(this IList<T> list, int N, Func<IList<T>, TOut> merger, bool ciclic = false, bool jumpUsedElement = false) {
            List<TOut> result = new List<TOut>();
            list.ForEachConsecutiveN(N, x => result.Add(merger.Invoke(x)), ciclic, jumpUsedElement);
            return result;
        }

        public static int JoinedAsDigits<T>(this IList<T> list, Func<T,int> toDigit) {
            return list.Reduce((e, acc) => toDigit.Invoke(e) + acc * 10, 0);
        }

        public static List<T> AlteredClone<T>(this IList<T> list, params Action<List<T>>[] actions) {
            List<T> result = new List<T>(list);
            actions.ForEach(a => a.Invoke(result));
            return result;
        }
        #endregion

    }

}

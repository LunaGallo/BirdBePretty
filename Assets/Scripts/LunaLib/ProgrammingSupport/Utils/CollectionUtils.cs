using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    
    public static partial class CollectionUtils {

        public static IEnumerable<int> Naturals(int quantity, int start = 0) => Enumerable.Range(start, quantity);
        public static List<int> NaturalsList(int quantity, int start = 0) => new (Naturals(quantity, start));
        public static int[] NaturalsArray(int quantity, int start = 0) => Naturals(quantity, start).ToArray();

        public static Vector2Int[,] Naturals2(int quantity, int start = 0) {
            Vector2Int[,] result = new Vector2Int[quantity, quantity];
            for (int i = 0; i < quantity; i++) {
                for (int j = 0; j < quantity; j++) {
                    result[i, j] = new Vector2Int(i + start, j + start);
                }
            }
            return result;
        }
        public static Vector2Int[,] Naturals2(Vector2Int quantity, int start = 0) {
            Vector2Int[,] result = new Vector2Int[quantity.x, quantity.y];
            for (int i = 0; i < quantity.x; i++) {
                for (int j = 0; j < quantity.y; j++) {
                    result[i, j] = new Vector2Int(i + start, j + start);
                }
            }
            return result;
        }
        public static Vector2Int[,] Naturals2(int quantity, Vector2Int start) {
            Vector2Int[,] result = new Vector2Int[quantity, quantity];
            for (int i = 0; i < quantity; i++) {
                for (int j = 0; j < quantity; j++) {
                    result[i, j] = new Vector2Int(i, j) + start;
                }
            }
            return result;
        }
        public static Vector2Int[,] Naturals2(Vector2Int quantity, Vector2Int start) {
            Vector2Int[,] result = new Vector2Int[quantity.x, quantity.y];
            for (int i = 0; i < quantity.x; i++) {
                for (int j = 0; j < quantity.y; j++) {
                    result[i, j] = new Vector2Int(i, j) + start;
                }
            }
            return result;
        }

        public static Vector3Int[,,] Naturals3(int quantity, int start = 0) {
            Vector3Int[,,] result = new Vector3Int[quantity, quantity, quantity];
            for (int i = 0; i < quantity; i++) {
                for (int j = 0; j < quantity; j++) {
                    for (int k = 0; k < quantity; k++) {
                        result[i, j, k] = new Vector3Int(i + start, j + start, k + start);
                    }
                }
            }
            return result;
        }
        public static Vector3Int[,,] Naturals3(Vector3Int quantity, int start = 0) {
            Vector3Int[,,] result = new Vector3Int[quantity.x, quantity.y, quantity.z];
            for (int i = 0; i < quantity.x; i++) {
                for (int j = 0; j < quantity.y; j++) {
                    for (int k = 0; k < quantity.z; k++) {
                        result[i, j, k] = new Vector3Int(i + start, j + start, k + start);
                    }
                }
            }
            return result;
        }
        public static Vector3Int[,,] Naturals3(int quantity, Vector3Int start) {
            Vector3Int[,,] result = new Vector3Int[quantity, quantity, quantity];
            for (int i = 0; i < quantity; i++) {
                for (int j = 0; j < quantity; j++) {
                    for (int k = 0; k < quantity; k++) {
                        result[i, j, k] = new Vector3Int(i, j, k) + start;
                    }
                }
            }
            return result;
        }
        public static Vector3Int[,,] Naturals3(Vector3Int quantity, Vector3Int start) {
            Vector3Int[,,] result = new Vector3Int[quantity.x, quantity.y, quantity.z];
            for (int i = 0; i < quantity.x; i++) {
                for (int j = 0; j < quantity.y; j++) {
                    for (int k = 0; k < quantity.z; k++) {
                        result[i, j, k] = new Vector3Int(i, j, k) + start;
                    }
                }
            }
            return result;
        }

        public static T[] CreateArray<T>(this T value, int length) {
            T[] result = new T[length];
            for (int i = 0; i < length; i++) {
                result[i] = value;
            }
            return result;
        }
        public static List<T> CreateList<T>(this T value, int length) {
            List<T> result = new();
            for (int i = 0; i < length; i++) {
                result.Add(value);
            }
            return result;
        }

        public static List<List<T>> NewNestedList<T>(int length) {
            List<List<T>> result = new();
            for (int i = 0; i < length; i++) {
                result.Add(new List<T>());
            }
            return result;
        }


        public static int FindAccumulatedIndex<T>(int count, float targetValue, Func<int, float> indexValue, out float remainder) {
            float sum = 0;
            int i = 0;
            for (; i < count; i++) {
                float curValue = indexValue(i);
                if (sum + curValue >= targetValue) {
                    remainder = targetValue - sum;
                    return i;
                }
                sum += curValue;
            }
            remainder = targetValue - sum;
            return i;
        }
        public static float SumPairs<T>(Func<T, T, float> converter, bool ciclic = false, bool jumpUsedElement = false, params T[] values) {
            return values.SumPairs(converter, ciclic, jumpUsedElement);
        }

    }

}
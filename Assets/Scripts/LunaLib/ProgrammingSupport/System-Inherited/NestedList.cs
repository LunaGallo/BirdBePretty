using System.Collections.Generic;

namespace LunaLib {
    public class NestedList<T> : List<List<T>> {
        public T this[(int outter, int inner) localIndex] => this[localIndex.outter][localIndex.inner];
        public T GetInside(int globalIndex) => this[GlobalToLocalIndex(globalIndex)];
        public int LocalToGlobalIndex((int outter, int inner) localIndex) {
            int sum = 0;
            for (int i = 0; i < localIndex.outter; i++) {
                sum += this[i].Count;
            }
            return sum + localIndex.inner;
        }
        public (int, int) GlobalToLocalIndex(int globalIndex) {
            int i = 0;
            int outter = 0;
            for (; outter < Count; outter++) {
                int length = this[i].Count;
                if (i + length > globalIndex) {
                    break;
                }
                i += length;
            }
            return (outter, globalIndex - i);
        }

    }
}

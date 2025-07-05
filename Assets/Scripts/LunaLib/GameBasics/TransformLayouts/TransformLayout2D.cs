using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LunaLib {

    [ExecuteAlways]
    public abstract class TransformLayout2D : TransformLayout {
        public abstract int ColumnCount(int row);
        protected IEnumerable<(Vector2Int pos, Transform transform)> Children2D {
            get {
                int row = 0;
                int column = -1;
                foreach ((int index, Transform transform) child in Children) {
                    int columnCount;
                    do {
                        column++;
                        columnCount = ColumnCount(row);
                        if (column == columnCount) {
                            column = 0;
                            row++;
                        }
                    } while (ColumnCount(row) <= 0);
                    yield return (new Vector2Int(column, row), child.transform);
                }
            }
        }
    }
}

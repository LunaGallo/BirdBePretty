using Sirenix.OdinInspector;
using UnityEngine;

namespace LunaLib {
    public class MatrixLayout : TransformLayout2D {
        public Vector2 pivot;
        public Vector3 columnStep;
        public Vector3 rowStep;
        public int columnCount = 1;

        protected override void Apply() {
            foreach ((Vector2Int pos, Transform transform) child in Children2D) {
                child.transform.localPosition = columnStep * (child.pos.x - pivot.x * (ColumnCount(0) - 1)) + rowStep * (child.pos.y - pivot.y * (RowCount - 1));
            }
        }
        public override int ColumnCount(int row) => columnCount;
        protected int RowCount => Mathf.CeilToInt((float)ChildCount / ColumnCount(0));
    }
}
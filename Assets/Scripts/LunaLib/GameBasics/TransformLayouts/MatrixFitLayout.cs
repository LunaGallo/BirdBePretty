using Sirenix.OdinInspector;
using UnityEngine;

namespace LunaLib {
    public class MatrixFitLayout : TransformLayout2D {
        public Vector2 pivot;
        public Vector3 columnStep;
        public Vector3 rowStep;
        public float desiredAspect = 1f;
        public bool prioritizeHorizontal = true;

        protected override void Apply() {
            foreach ((Vector2Int pos, Transform transform) child in Children2D) {
                child.transform.localPosition = columnStep * (child.pos.x - pivot.x * (ColumnCount(0) - 1)) + rowStep * (child.pos.y - pivot.y * (RowCount - 1));
            }
        }
        public override int ColumnCount(int row) {
            float rowValue = Mathf.Sqrt(ChildCount / desiredAspect);
            int rowCount;
            if (prioritizeHorizontal) {
                rowCount = Mathf.FloorToInt(rowValue);
            }
            else {
                rowCount = Mathf.CeilToInt(rowValue);
            }
            return Mathf.CeilToInt((float)ChildCount / rowCount);
        }

        public int RowCount => Mathf.CeilToInt((float)ChildCount / ColumnCount(0));
    }
}

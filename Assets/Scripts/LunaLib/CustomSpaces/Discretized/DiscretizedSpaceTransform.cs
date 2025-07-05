using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public class DiscretizedSpaceTransform<S, T, D> : CustomSpaceTransform<S, T> where S : DiscretizedSpace<T, D> {

        public bool autoSnapPosition = false;
        public D CurTile => Space.WorldToDiscretizedPoint(transform.position);
        public Vector3 CurTileCenter => Space.WorldToTileCenter(transform.position);

        public void SnapPosition() {
            transform.position = Space.DiscretizedToWorldPoint(Space.CustomToDiscretizedPoint(customPosition));
        }

        public bool SameTileAs(DiscretizedSpaceTransform<S, T, D> other) => CurTile.Equals(other.CurTile);

        public override void UpdatePosition_GetFromTransform(ref Vector3 newValue, ref bool transformMightChange) {
            base.UpdatePosition_GetFromTransform(ref newValue, ref transformMightChange);
            if (autoSnapPosition) {
                SnapPosition();
                newValue = Space.CustomToWorldPoint(customPosition);
                transformMightChange = true;
            }
        }
        public override void UpdatePosition_SetTransform(ref Vector3 newValue, ref bool transformMightChange) {
            customPosition = Space.ValidateCustomPoint(customPosition);
            if (autoSnapPosition) {
                SnapPosition();
            }
            base.UpdatePosition_SetTransform(ref newValue, ref transformMightChange);
        }

        public override void DrawCustomTransformGizmo() {
            Space.DrawDiscretizedGizmo(CurTile, 0.9f);
        }

    }
}
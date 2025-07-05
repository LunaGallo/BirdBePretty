using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LunaLib {
    [ExecuteAlways]
    public abstract class CustomSpaceTransform<S, T> : EditorBehaviour where S : CustomSpace<T> {

        public SerializableAutoComponent<S> spaceRef;
        public S Space => spaceRef.GetValue(this);

        public TransformControllingMode positionControllingMode;
        public T customPosition;
        protected override void ApplyOnEditorUpdate() {
            SafeModifyProperty<Vector3, Transform>(transform, UpdatePosition, (t, v) => t.position = v, (t, v) => t.position == v);
        }
        protected override bool CanApplyOnEditorUpdate => Space != null;

        private void UpdatePosition(ref Vector3 newValue, ref bool transformMightChange) {
            switch (positionControllingMode) {
                case TransformControllingMode.GetFromTransform:
                    UpdatePosition_GetFromTransform(ref newValue, ref transformMightChange);
                    break;
                case TransformControllingMode.SetTransform:
                    UpdatePosition_SetTransform(ref newValue, ref transformMightChange);
                    break;
            }
        }
        public virtual void UpdatePosition_GetFromTransform(ref Vector3 newValue, ref bool transformMightChange) {
            customPosition = Space.WorldToCustomPoint(transform.position);
        }
        public virtual void UpdatePosition_SetTransform(ref Vector3 newValue, ref bool transformMightChange) {
            customPosition = Space.ValidateCustomPoint(customPosition);
            newValue = Space.CustomToWorldPoint(customPosition);
            transformMightChange = true;
        }
        public virtual void SetPosition(T newCustomPosition) {
            if (Application.isPlaying) {
                customPosition = Space.ValidateCustomPoint(newCustomPosition);
                transform.position = Space.CustomToWorldPoint(customPosition);
            }
        }

        private void OnDrawGizmosSelected() {
            if (Space != null) {
                Gizmos.color = Color.yellow;
                DrawCustomTransformGizmo();
            }
        }
        public abstract void DrawCustomTransformGizmo();

    }

    public enum TransformControllingMode {
        GetFromTransform,
        SetTransform,
    }
}
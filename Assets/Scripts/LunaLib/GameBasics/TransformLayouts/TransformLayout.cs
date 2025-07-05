using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LunaLib {

    [ExecuteAlways]
    public abstract class TransformLayout : EditorBehaviour {

        protected override void ApplyOnEditorUpdate() {
            if (CanApply) Apply();
        }
        protected override void ApplyOnPlayUpdate() {
            if (CanApply) Apply();
        }

        protected virtual bool CanApply => true;
        protected abstract void Apply();
        protected IEnumerable<(int index, Transform transform)> Children {
            get {
                for(int i = 0; i < transform.childCount; i++) {
                    yield return (i, transform.GetChild(i));
                }
            }
        }
        protected int ChildCount => transform.childCount;

    }
}

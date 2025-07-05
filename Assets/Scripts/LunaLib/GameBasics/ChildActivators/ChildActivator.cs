using Sirenix.OdinInspector;
using UnityEngine;

namespace LunaLib {
    [ExecuteAlways]
    public abstract class ChildActivator : EditorBehaviour {
        protected override void ApplyOnEditorUpdate() => Apply();
        protected override void ApplyOnPlayUpdate() => Apply();
        public void Apply() {
            for (int i = 0; i < transform.childCount; i++) {
                Transform child = transform.GetChild(i);
                child.gameObject.SetActive(GetActive(i, child));
            }
        }

        protected abstract bool GetActive(int index, Transform child);

    }

}
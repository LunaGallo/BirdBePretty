using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    [ExecuteAlways]
    public abstract class RendererController<T> : EditorBehaviour where T : Renderer {
        
        public T rendererTarget;
        
        protected virtual bool CanApply => rendererTarget != null;
        protected abstract void Apply();

        protected override void ApplyOnEditorUpdate() { if (CanApply) Apply(); }
        protected override void ApplyOnPlayUpdate() { if (CanApply) Apply(); }

    }
}
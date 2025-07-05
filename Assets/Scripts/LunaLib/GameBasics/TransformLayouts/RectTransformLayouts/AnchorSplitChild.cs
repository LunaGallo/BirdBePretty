using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public class AnchorSplitChild : MonoBehaviour {

        public AnchorSplitLayout Layout {
            get {
                if (layout == null) {
                    layout = GetComponentInParent<AnchorSplitLayout>();
                }
                return layout;
            }
        }
        private AnchorSplitLayout layout;

        public float proportion = 1f;
        public void ApplyChange() {
            Layout.Apply();
        }

    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LunaLib {
    [ExecuteAlways]
    public class AnchorSplitLayout : MonoBehaviour {

        public enum Type {
            Horizontal,
            Vertical
        }
        public Type type = Type.Horizontal;
        public bool setZeroOffset = true;
        public bool invertOrder = false;

        private void Update() {
            Apply();
        }

        [Button]
        public void Apply() {
            List<float> proportionalSizes = new();
            float sum = 0f;
            for (int i = 0; i < transform.childCount; i++) {
                Transform child = transform.GetChild(i);
                if (child.gameObject.activeInHierarchy) {
                    AnchorSplitChild splitChild = child.GetComponent<AnchorSplitChild>();
                    float proportion = splitChild == null ? 1f : splitChild.proportion;
                    proportionalSizes.Add(proportion);
                    sum += proportion;
                }
            }
            float count = invertOrder? 1f : 0f;
            int j = 0;
            for (int i = 0; i < transform.childCount; i++) {
                Transform child = transform.GetChild(i);
                if (child.gameObject.activeInHierarchy) {
                    RectTransform rectChild = (RectTransform)child.transform;
                    switch (type) {
                        case Type.Horizontal:
                            if (invertOrder) {
                                rectChild.anchorMax = new Vector2(count, 1f);
                                count -= proportionalSizes[j] / sum;
                                rectChild.anchorMin = new Vector2(count, 0f);
                            }
                            else {
                                rectChild.anchorMin = new Vector2(count, 0f);
                                count += proportionalSizes[j] / sum;
                                rectChild.anchorMax = new Vector2(count, 1f);
                            }
                            break;
                        case Type.Vertical:
                            if (invertOrder) {
                                rectChild.anchorMax = new Vector2(1f, count);
                                count -= proportionalSizes[j] / sum;
                                rectChild.anchorMin = new Vector2(0f, count);
                            }
                            else {
                                rectChild.anchorMin = new Vector2(0f, count);
                                count += proportionalSizes[j] / sum;
                                rectChild.anchorMax = new Vector2(1f, count);
                            }
                            break;
                        default:
                            break;
                    }
                    if (setZeroOffset) {
                        rectChild.offsetMin = Vector2.zero;
                        rectChild.offsetMax = Vector2.zero;
                    }
                    j++;
                }
            }
        }

    }
}

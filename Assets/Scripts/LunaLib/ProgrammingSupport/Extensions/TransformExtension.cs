using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public static partial class TransformExtension {
    
        public static int GetChildIndex(this Transform t, Transform child) {
            int i = 0;
            foreach (Transform c in t) {
                if (c == child) {
                    return i;
                }
                i++;
            }
            return -1;
        }

        public static float DistanceTo(this Transform t1, Transform t2) {
            return t1.position.DistanceTo(t2.position);
        }
        public static void SetData(this Transform t, LocalTransformData data) {
            t.localPosition = data.localPosition;
            t.localRotation = data.localRotation;
            t.localScale = data.localScale;
        }

        public static void ShuffleChildrenOrder(this Transform t) {
            List<Transform> randomizedOrder = new List<Transform>();
            foreach (Transform child in t) {
                randomizedOrder.Add(child);
            }
            foreach (Transform child in t) {
                child.parent = null;
            }
            randomizedOrder.Shuffle();
            foreach (Transform child in randomizedOrder) {
                child.parent = t;
            }
        }

        public static void SetDoublePivot(this Transform t, Vector3 localP1, Vector3 localP2, Vector3 worldP1, Vector3 worldP2, bool setScale = true) {
            //Vector3 localMidPoint = localP1.MidpointTo(localP2);
            //Vector3 worldMidPoint = worldP1.MidpointTo(worldP2);
            Vector3 localDelta = localP2 - localP1;
            Vector3 worldDelta = worldP2 - worldP1;
            
        }

    }

    public static partial class RectTransformExtension {

        public static Rect GetAnchor(this RectTransform t) {
            return new Rect(t.anchorMin, t.anchorMax);
        }
        public static Rect GetOffset(this RectTransform t) {
            return new Rect(t.offsetMin, t.offsetMax);
        }

        public static void SetAnchor(this RectTransform t, Rect value) {
            t.anchorMin = value.min;
            t.anchorMax = value.max;
        }
        public static void SetOffset(this RectTransform t, Rect value) {
            t.offsetMin = value.min;
            t.offsetMax = value.max;
        }

    }

}

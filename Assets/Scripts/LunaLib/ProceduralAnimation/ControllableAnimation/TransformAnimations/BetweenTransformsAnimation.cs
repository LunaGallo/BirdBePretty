using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LunaLib {
    namespace InspectorCoding {
        public class BetweenTransformsAnimation : ControllableAnimation {

            public Transform startTransform;
            public Transform endTransform;
            public TransformProperty controledProperties = TransformProperty.Position;

            public override void UpdateProgress(float animationProgress) {
                if (controledProperties.HasFlag(TransformProperty.Position)) {
                    transform.position = Vector3.Lerp(startTransform.position, endTransform.position, animationProgress);
                }
                if (controledProperties.HasFlag(TransformProperty.Rotation)) {
                    transform.rotation = Quaternion.Slerp(startTransform.rotation, endTransform.rotation, animationProgress);
                }
                if (controledProperties.HasFlag(TransformProperty.Scale)) {
                    transform.localScale = Vector3.Lerp(startTransform.localScale, endTransform.localScale, animationProgress);
                }
            }

            public void SwitchEnds() {
                Transform oldStart = startTransform;
                startTransform = endTransform;
                endTransform = oldStart;
            }

            public void OnDrawGizmosSelected() {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(startTransform.position, endTransform.position);
                Gizmos.DrawIcon(startTransform.position, "blendKey", false, Color.yellow);
                Gizmos.DrawIcon(endTransform.position, "blendKey", false, Color.yellow);
                Gizmos.DrawIcon(transform.position, "blendSampler", false, Color.yellow);
            }

        }

    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public class SimpleRotationAnimation : ControllableAnimation {

        public Vector3 baseRotation;
        public Vector3 localAxis;
        public RemapedCurve animationCurve;

        public Quaternion BaseRotation => Quaternion.Euler(baseRotation);
        public override void UpdateProgress(float animationProgress) {
            localAxis.Normalize();
            transform.localRotation = Quaternion.AngleAxis(animationCurve.Evaluate(animationProgress), localAxis) * BaseRotation;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.yellow;
            Vector3 baseVector = BaseRotation * transform.localRotation.Inversed() * transform.forward;
            GizmosUtils.DrawArc(transform.position, 1f, BaseRotation, Vector3.forward, localAxis, animationCurve.Extension, 10f);
            Gizmos.DrawIcon(transform.position + baseVector, "blendKey", false, Color.yellow);
            Gizmos.DrawIcon(transform.position + (Quaternion.AngleAxis(animationCurve.Extension, localAxis) * baseVector), "blendKey", false, Color.yellow);
            Gizmos.DrawIcon(transform.position + transform.forward, "blendSampler", false, Color.yellow);
            Gizmos.color = Color.gray;
            Gizmos.DrawLine(transform.position, transform.position + baseVector);
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
            Gizmos.DrawLine(transform.position, transform.position + (Quaternion.AngleAxis(animationCurve.Extension, localAxis) * baseVector));
        }

    }
}
using UnityEngine;

namespace LunaLib {
    public class SimpleScaleAnimation : ControllableAnimation {

        public Vector3 baseScale;
        public Vector3 scaleDelta;
        public RemapedCurve animationCurve;

        public override void UpdateProgress(float animationProgress) {
            transform.localScale = baseScale + (scaleDelta * animationCurve.Evaluate(animationProgress)).ScaledBy(baseScale);
        }

        private void OnDrawGizmosSelected() {
            Vector3 startPoint = transform.position + transform.rotation * (baseScale / 2f);
            Vector3 curPoint = transform.position + transform.rotation * (transform.localScale / 2f);
            Vector3 finishPoint = transform.position + transform.rotation * ((baseScale + scaleDelta.ScaledBy(baseScale)) / 2f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(startPoint, finishPoint);
            Gizmos.DrawIcon(startPoint, "blendKey", false, Color.yellow);
            Gizmos.DrawIcon(finishPoint, "blendKey", false, Color.yellow);
            Gizmos.DrawIcon(curPoint, "blendSampler", false, Color.yellow);
        }

    }
}
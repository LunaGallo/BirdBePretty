using UnityEngine;

namespace LunaLib {
    public class SimplePositionAnimation : ControllableAnimation {

        public Vector3 basePosition;
        public Vector3 localMovementDir;
        public RemapedCurve animationCurve;

        public override void UpdateProgress(float animationProgress) {
            transform.localPosition = basePosition + localMovementDir.normalized * animationCurve.Evaluate(animationProgress);
        }
        
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.TransformPoint(basePosition - transform.localPosition), transform.TransformPoint(basePosition + localMovementDir - transform.localPosition));
            Gizmos.DrawIcon(transform.TransformPoint(basePosition - transform.localPosition), "blendKey", false, Color.yellow);
            Gizmos.DrawIcon(transform.TransformPoint(basePosition + localMovementDir - transform.localPosition), "blendKey", false, Color.yellow);
            Gizmos.DrawIcon(transform.position, "blendSampler", false, Color.yellow);
        }

    }
}
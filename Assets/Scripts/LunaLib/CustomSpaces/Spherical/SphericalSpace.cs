using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public class SphericalSpace : CustomSpace<Vector3> {
        public override Vector3 LocalToCustomPoint(Vector3 localPoint) {
            Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, localPoint);
            return new Vector3(rotation.eulerAngles.y, rotation.eulerAngles.x, localPoint.magnitude);
        }
        public override Vector3 CustomToLocalPoint(Vector3 customPoint) {
            return CustomPointToLongLatRotation(customPoint) * Vector3.forward * customPoint.z;
        }
        public override Vector3 ValidateCustomPoint(Vector3 customPoint) {
            return new Vector3(Mathf.Clamp(customPoint.x, -180f, 180f), Mathf.Clamp(customPoint.y, -90f, 90f), Mathf.Max(0f, customPoint.z));
        }

        public override void DrawSpaceGizmo() {
            Gizmos.DrawWireSphere(transform.position, 1f);
        }

        public static Quaternion CustomPointToLongLatRotation(Vector3 customPoint) => Quaternion.Euler(customPoint.y, customPoint.x, 0f);

    }
}
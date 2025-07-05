using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    [ExecuteAlways]
    public class SphericalTransform : CustomSpaceTransform<SphericalSpace, Vector3> {

        public void SetSphericalCoordinates(Vector3 newSphericalCoordinates) {
            customPosition = newSphericalCoordinates;
            transform.position = Space.CustomToWorldPoint(customPosition);
        }
        public void SetWorldPosition(Vector3 worldPosition) {
            transform.position = worldPosition;
            customPosition = Space.WorldToCustomPoint(worldPosition);
        }

        public Vector3 SphericalCoordinates {
            get => customPosition;
            set => customPosition = value;
        }
        public float Longitude {
            get => customPosition.x;
            set => customPosition.x = value;
        }
        public float Latitude {
            get => customPosition.y;
            set => customPosition.y = value;
        }
        public float Radius {
            get => customPosition.z;
            set => customPosition.z = value;
        }
        public Vector2 LongLat {
            get => new(Longitude, Latitude);
            set {
                Longitude = value.x;
                Latitude = value.y;
            }
        }
        public Quaternion LongLatRotation {
            get => Quaternion.Euler(Latitude, Longitude, 0f);
            set {
                Latitude = value.eulerAngles.x;
                Longitude = value.eulerAngles.y;
            }
        }
        public Vector3 North => LongLatRotation * Vector3.up;
        public Vector3 South => LongLatRotation * Vector3.down;
        public Vector3 East => LongLatRotation * Vector3.left;
        public Vector3 West => LongLatRotation * Vector3.right;
        public Vector3 Inward => LongLatRotation * Vector3.back;
        public Vector3 Outward => LongLatRotation * Vector3.forward;

        public override void DrawCustomTransformGizmo() {
            Gizmos.DrawLine(Space.transform.position, Space.transform.forward * Radius);
            GizmosUtils.DrawArc(Space.transform.position, Radius, Space.transform.rotation, Vector3.forward, Vector3.up, Longitude, 10f);
            GizmosUtils.DrawArc(Space.transform.position, Radius, Space.transform.rotation, Quaternion.AngleAxis(Longitude, Vector3.up) * Vector3.forward, Quaternion.AngleAxis(Longitude, Vector3.up) * Vector3.right, Latitude, 10f);
        }
    }
}
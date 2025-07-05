using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public abstract class CustomSpace<T> : MonoBehaviour {
        public Vector3 WorldToLocalPoint(Vector3 worldPoint) => transform.InverseTransformPoint(worldPoint);
        public Vector3 LocalToWorldPoint(Vector3 localPoint) => transform.TransformPoint(localPoint);

        public abstract T LocalToCustomPoint(Vector3 localPoint);
        public abstract Vector3 CustomToLocalPoint(T customPoint);
        public abstract T ValidateCustomPoint(T customPosition);

        public T WorldToCustomPoint(Vector3 worldPoint) => LocalToCustomPoint(WorldToLocalPoint(worldPoint));
        public Vector3 CustomToWorldPoint(T customPoint) => LocalToWorldPoint(CustomToLocalPoint(customPoint));

        public Vector3 Origin => CustomToWorldPoint(CustomOrigin);
        public virtual T CustomOrigin => default;

        //public Vector3 WorldToLocalDirection(Vector3 worldDirection) => transform.InverseTransformDirection(worldDirection);
        //public Vector3 LocalToWorldDirection(Vector3 localDirection) => transform.TransformDirection(localDirection);
        //
        //public Vector3 WorldToLocalVector(Vector3 worldVector) => transform.InverseTransformVector(worldVector);
        //public Vector3 LocalToWorldVector(Vector3 localVector) => transform.TransformVector(localVector);

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.cyan;
            DrawSpaceGizmo();
        }
        public abstract void DrawSpaceGizmo();
    }
}

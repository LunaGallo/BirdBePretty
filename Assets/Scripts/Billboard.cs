using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Billboard : MonoBehaviour {

    [Range(0f, 1f)] public float xAxisInfluence = 1f;
    public bool invertForward = false;

    private void Update() {
        Transform cameraTransform = Camera.main.transform;
        Vector3 up = Vector3.Lerp(Vector3.up, cameraTransform.up, xAxisInfluence).normalized;
        Vector3 forward = (invertForward ? -1f : 1f) * Vector3.Cross(cameraTransform.right, up);
        transform.rotation = Quaternion.LookRotation(forward, up);
    }

}

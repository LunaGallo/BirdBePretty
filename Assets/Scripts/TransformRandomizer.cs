using UnityEngine;
using LunaLib;

public class TransformRandomizer : MonoBehaviour {

    public Rect3 localPositionXRange = new(Vector3.zero, Vector3.zero);
    public Rect3 localRotationRange = new(Vector3.zero, Vector3.zero);
    public Rect3 localScaleRange = new(Vector3.one, Vector3.zero);

}

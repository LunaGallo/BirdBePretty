using UnityEngine;
using LunaLib;
using Sirenix.OdinInspector;

public class TransformRandomizer : MonoBehaviour {

    public Rect3 localPositionRange = new(Vector3.zero, Vector3.zero);
    public Rect3 localRotationRange = new(Vector3.zero, Vector3.zero);
    public Rect3 localScaleRange = new(Vector3.one, Vector3.zero);
    public bool randomizeOnStart = true;

    private void Start() {
        if (randomizeOnStart) {
            Randomize();
        }
    }
    [Button]
    public void Randomize() {
        transform.localPosition = localPositionRange.RandomPointInside();
        transform.localRotation = Quaternion.Euler(localRotationRange.RandomPointInside());
        transform.localScale = localScaleRange.RandomPointInside();
    }

}

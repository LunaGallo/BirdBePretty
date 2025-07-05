using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LunaLib {
    public abstract class TransformShufflerBase : EditorBehaviour {

        public TransformProperty property = TransformProperty.Position;
        public abstract List<Transform> Targets { get; }

        protected override void ApplyOnEditorUpdate() => Shuffle();
        protected override void ApplyOnPlayUpdate() => Shuffle();
        public void Shuffle() {
            List<Vector3> positions = new();
            List<Quaternion> rotations = new();
            List<Vector3> scales = new();
            foreach (Transform target in Targets) {
                if (property.HasFlag(TransformProperty.Position)) {
                    positions.Add(target.position);
                }
                if (property.HasFlag(TransformProperty.Rotation)) {
                    rotations.Add(target.rotation);
                }
                if (property.HasFlag(TransformProperty.Scale)) {
                    scales.Add(target.localScale);
                }
            }
            positions.Shuffle();
            rotations.Shuffle();
            scales.Shuffle();
            for (int i = 0; i < Targets.Count; i++) {
                if (property.HasFlag(TransformProperty.Position)) {
                    Targets[i].position = positions[i];
                }
                if (property.HasFlag(TransformProperty.Rotation)) {
                    Targets[i].rotation = rotations[i];
                }
                if (property.HasFlag(TransformProperty.Scale)) {
                    Targets[i].localScale = scales[i];
                }
            }
        }

    }
}

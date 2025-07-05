using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    [Serializable]
    public struct ArcParameter {
        public enum Mode {
            ByLength,
            ByAngle
        }
        public Mode mode;
        public float value;
        public float Length(float radius) => mode switch {
            Mode.ByLength => value,
            Mode.ByAngle => value * Mathf.PI * radius / 180f,
            _ => default,
        };
        public float Angle(float radius) => mode switch {
            Mode.ByLength => value * 180f / (Mathf.PI * radius),
            Mode.ByAngle => value,
            _ => default,
        };

        public ArcParameter(Mode mode, float value) {
            this.mode = mode;
            this.value = value;
        }

        public static implicit operator ArcParameter(float f) => new(default, f);
    }
}
using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LunaLib {
    [Serializable]
    public struct Gauge : IEquatable<Gauge> {

        public Range limits;
        public float currentValue;
        public float Left {
            get => limits.Max - currentValue;
            set => currentValue = ValidateValue(limits.Max - value);
        }
        public float CurrentNormalizedValue => limits.InverseLerp(currentValue);

        public Gauge(Range limits, float currentValue) {
            this.limits = limits;
            this.currentValue = limits.Clamp(currentValue);
        }
        public float ValidateValue(float value) => limits.Clamp(value);
        public void Validate() => currentValue = ValidateValue(currentValue);

        public bool Lose(float value, out float excess) {
            float totalValue = currentValue - value;
            currentValue = ValidateValue(totalValue);
            excess = currentValue - totalValue;
            return currentValue == limits.Min;
        }
        public bool Gain(float value, out float excess) {
            float totalValue = currentValue + value;
            currentValue = ValidateValue(totalValue);
            excess = totalValue - currentValue;
            return currentValue == limits.Max;
        }
        public bool Set(float value, out float excess) {
            currentValue = ValidateValue(value);
            excess = value - currentValue;
            return currentValue == limits.Min || currentValue == limits.Max;
        }

        public bool Equals(Gauge other) => limits == other.limits && currentValue == other.currentValue;
        public override bool Equals(object obj) => obj is Gauge gauge && Equals(gauge);
        public override int GetHashCode() => HashCode.Combine(limits, currentValue);

    }
}
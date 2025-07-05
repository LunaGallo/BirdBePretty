using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LunaLib {
    [Serializable]
    public struct GaugeInt : IEquatable<GaugeInt>{

        public RangeInt limits;
        public int currentValue;
        public int Left {
            get => limits.Max - currentValue;
            set => currentValue = ValidateValue(limits.Max - value);
        }
        public float CurrentNormalizedValue => limits.InverseLerp(currentValue);

        public GaugeInt(RangeInt limits, int currentValue) {
            this.limits = limits;
            this.currentValue = limits.Clamp(currentValue);
        }
        public int ValidateValue(int value) => limits.Clamp(value);
        public void Validate() => currentValue = ValidateValue(currentValue);

        public bool Lose(int value, out int excess) {
            int totalValue = currentValue - value;
            currentValue = ValidateValue(totalValue);
            excess = currentValue - totalValue;
            return currentValue == limits.Min;
        }
        public bool Gain(int value, out int excess) {
            int totalValue = currentValue + value;
            currentValue = ValidateValue(totalValue);
            excess = totalValue - currentValue;
            return currentValue == limits.Max;
        }
        public bool Set(int value, out int excess) {
            currentValue = ValidateValue(value);
            excess = value - currentValue;
            return currentValue == limits.Min || currentValue == limits.Max;
        }

        public bool Equals(GaugeInt other) => limits == other.limits && currentValue == other.currentValue;
        public override bool Equals(object obj) => obj is GaugeInt gauge && Equals(gauge);
        public override int GetHashCode() => HashCode.Combine(limits, currentValue);

    }

}
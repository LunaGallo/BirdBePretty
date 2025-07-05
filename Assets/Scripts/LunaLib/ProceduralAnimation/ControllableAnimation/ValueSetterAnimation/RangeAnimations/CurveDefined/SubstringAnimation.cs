using UnityEngine;

namespace LunaLib {
    [ExecuteAlways]
    public class SubstringAnimation : CurveDefinedRangeAnimation<string, StringEvent> {
        public string origString;
        protected override bool CanSetValue() => true;
        protected override int RangeLength() => origString.Length;
        protected override string ValueFromRange(RangeInt range) => origString.Substring(range.position, range.Size);
    }
}

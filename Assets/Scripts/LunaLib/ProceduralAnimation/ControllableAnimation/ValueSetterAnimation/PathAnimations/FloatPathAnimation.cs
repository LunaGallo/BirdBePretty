using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public class FloatPathAnimation : ValuePathAnimation<float, FloatEvent> {
        protected override Func<float, float, float, float> LerpFunc => Mathf.LerpUnclamped;
    }
}

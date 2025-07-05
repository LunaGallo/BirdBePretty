using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public class Vector4PathAnimation : ValuePathAnimation<Vector4, Vector4Event> {
        protected override Func<Vector4, Vector4, float, Vector4> LerpFunc => Vector4.LerpUnclamped;
    }
}

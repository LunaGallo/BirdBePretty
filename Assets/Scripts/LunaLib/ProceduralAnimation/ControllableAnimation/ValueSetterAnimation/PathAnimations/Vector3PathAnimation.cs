using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public class Vector3PathAnimation : ValuePathAnimation<Vector3, Vector3Event> {
        protected override Func<Vector3, Vector3, float, Vector3> LerpFunc => Vector3.LerpUnclamped;
    }
}

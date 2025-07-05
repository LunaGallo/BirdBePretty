using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public class Vector2PathAnimation : ValuePathAnimation<Vector2, Vector2Event> {
        protected override Func<Vector2, Vector2, float, Vector2> LerpFunc => Vector2.LerpUnclamped;
    }
}

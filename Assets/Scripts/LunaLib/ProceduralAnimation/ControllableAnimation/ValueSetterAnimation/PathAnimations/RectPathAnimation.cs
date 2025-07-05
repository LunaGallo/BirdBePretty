using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public class RectPathAnimation : ValuePathAnimation<Rect, RectEvent> {
        protected override Func<Rect, Rect, float, Rect> LerpFunc => RectUtils.Lerp;
    }
}

using System;
using UnityEngine;

namespace LunaLib {
    [ExecuteAlways]
    public class ColorPathAnimation : ValuePathAnimation<Color, ColorEvent> {
        protected override Func<Color, Color, float, Color> LerpFunc => Color.LerpUnclamped;
    }
}

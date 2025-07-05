using UnityEngine;
using System;

namespace LunaLib {
    public class BoolKeyframeAnimation : ValueKeyframeAnimation<BoolKeyframe, bool, BooleanEvent> { }
    [Serializable] public class BoolKeyframe : ValueKeyframe<bool> { }
}

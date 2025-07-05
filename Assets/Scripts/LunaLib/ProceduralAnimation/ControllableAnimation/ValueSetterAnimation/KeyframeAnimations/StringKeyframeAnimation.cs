using System;

namespace LunaLib {
    public class StringKeyframeAnimation : ValueKeyframeAnimation<StringKeyframe, string, StringEvent> { }
    [Serializable] public class StringKeyframe : ValueKeyframe<string> { }
}

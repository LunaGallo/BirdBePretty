using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    [Serializable] public class IntConditionalListEvent : ConditionalEvent<IntConditionList, IntegerEvent, int> { }
    [Serializable] public class FloatConditionalListEvent : ConditionalEvent<FloatConditionList, FloatEvent, float> { }
    [Serializable] public class CharConditionalListEvent : ConditionalEvent<CharConditionList, CharEvent, char> { }
    [Serializable] public class StringConditionalListEvent : ConditionalEvent<StringConditionList, StringEvent, string> { }
}
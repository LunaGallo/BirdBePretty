using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    [CreateAssetMenu(menuName = "LunaLib/Static Values/Basic Types/Bool", fileName = "StaticValueBool")] public class StaticValue_Bool : StaticValue<bool> { }
    [CreateAssetMenu(menuName = "LunaLib/Static Values/Basic Types/Integer", fileName = "StaticValueInt")] public class StaticValue_Int : StaticValue<int> { }
    [CreateAssetMenu(menuName = "LunaLib/Static Values/Basic Types/Float", fileName = "StaticValueFloat")] public class StaticValue_Float : StaticValue<float> { }
    [CreateAssetMenu(menuName = "LunaLib/Static Values/Basic Types/Char", fileName = "StaticValueChar")] public class StaticValue_Char : StaticValue<char> { }
    [CreateAssetMenu(menuName = "LunaLib/Static Values/Basic Types/String", fileName = "StaticValueString")] public class StaticValue_String : StaticValue<string> { }

    [CreateAssetMenu(menuName = "LunaLib/Static Values/Lists/Basic Types/Bool", fileName = "StaticValueBoolList")] public class StaticValue_BoolList : StaticValue<List<bool>> { }
    [CreateAssetMenu(menuName = "LunaLib/Static Values/Lists/Basic Types/Integer", fileName = "StaticValueIntList")] public class StaticValue_IntList : StaticValue<List<int>> { }
    [CreateAssetMenu(menuName = "LunaLib/Static Values/Lists/Basic Types/Float", fileName = "StaticValueFloatList")] public class StaticValue_FloatList : StaticValue<List<float>> { }
    [CreateAssetMenu(menuName = "LunaLib/Static Values/Lists/Basic Types/Char", fileName = "StaticValueCharList")] public class StaticValue_CharList : StaticValue<List<char>> { }
    [CreateAssetMenu(menuName = "LunaLib/Static Values/Lists/Basic Types/String", fileName = "StaticValueStringList")] public class StaticValue_StringList : StaticValue<List<string>> { }
}

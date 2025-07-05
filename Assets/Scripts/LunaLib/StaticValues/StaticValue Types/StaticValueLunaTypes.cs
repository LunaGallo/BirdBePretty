using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    [CreateAssetMenu(menuName = "LunaLib/Static Values/LunaLib/Range", fileName = "StaticValueRange")] public class StaticValue_Range : StaticValue<Range> { }
    [CreateAssetMenu(menuName = "LunaLib/Static Values/LunaLib/Range Int", fileName = "StaticValueRangeInt")] public class StaticValue_RangeInt : StaticValue<RangeInt> { }
    [CreateAssetMenu(menuName = "LunaLib/Static Values/LunaLib/Rect3", fileName = "StaticValueRect3")] public class StaticValue_Rect3 : StaticValue<Rect3> { }
    [CreateAssetMenu(menuName = "LunaLib/Static Values/LunaLib/Rect3 Int", fileName = "StaticValueRect3Int")] public class StaticValue_Rect3Int : StaticValue<Rect3Int> { }

    [CreateAssetMenu(menuName = "LunaLib/Static Values/Lists/LunaLib/Range", fileName = "StaticValueRangeList")] public class StaticValue_RangeList : StaticValue<List<Range>> { }
    [CreateAssetMenu(menuName = "LunaLib/Static Values/Lists/LunaLib/Range Int", fileName = "StaticValueRangeIntList")] public class StaticValue_RangeIntList : StaticValue<List<RangeInt>> { }
    [CreateAssetMenu(menuName = "LunaLib/Static Values/Lists/LunaLib/Rect3", fileName = "StaticValueRect3List")] public class StaticValue_Rect3List : StaticValue<List<Rect3>> { }
    [CreateAssetMenu(menuName = "LunaLib/Static Values/Lists/LunaLib/Rect3 Int", fileName = "StaticValueRect3IntList")] public class StaticValue_Rect3IntList : StaticValue<List<Rect3Int>> { }
}

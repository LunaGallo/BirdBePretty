using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public class IndexRangeChildActivator : ChildActivator {
        public RangeInt range;
        protected override bool GetActive(int index, Transform child) => range.Contains(index);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public class IndexListChildActivator : ChildActivator {
        public List<int> list;
        public List<int> List {
            set => list = value;
        }
        protected override bool GetActive(int index, Transform child) => list.Contains(index);
    }
}

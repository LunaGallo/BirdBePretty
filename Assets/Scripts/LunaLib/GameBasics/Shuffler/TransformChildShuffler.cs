using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public class TransformChildShuffler : TransformShufflerBase {
        public override List<Transform> Targets => transform.GetChildren();
    }
}

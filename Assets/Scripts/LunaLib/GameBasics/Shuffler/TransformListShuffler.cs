using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public class TransformListShuffler : TransformShufflerBase {
        public List<Transform> targets;
        public override List<Transform> Targets => targets;
    }
}

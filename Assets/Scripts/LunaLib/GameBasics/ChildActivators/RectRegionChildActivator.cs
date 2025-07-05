using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public class RectRegionChildActivator : RegionChildActivator {
        public Rect region;
        protected override bool IsPointInsideRegion(Vector3 point) => region.Contains(point);
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.cyan;
            GizmosUtils.DrawLocalRectXY(transform, region);
        }
    }
}

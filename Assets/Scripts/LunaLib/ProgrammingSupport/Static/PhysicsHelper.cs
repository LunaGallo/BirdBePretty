using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public static class PhysicsHelper {

        public static bool Raycast(RaycastConfig raycastConfig, out RaycastHit raycastHit) => Physics.Raycast(raycastConfig.ray, out raycastHit, raycastConfig.maxDistance, raycastConfig.layerMask, raycastConfig.queryTriggerInteraction);

    }

    [Serializable]
    public struct RaycastConfig {
        public Ray ray;
        public float maxDistance;
        public LayerMask layerMask;
        public QueryTriggerInteraction queryTriggerInteraction;
    }
}
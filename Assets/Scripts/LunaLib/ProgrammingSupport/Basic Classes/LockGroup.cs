using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    [Serializable]
    public class LockGroup {
        [SerializeField] private readonly List<string> lockedKeys = new();
        public bool IsLocked => lockedKeys.Count > 0;
        public bool IsUnlocked => lockedKeys.Count == 0;
        public void OpenLock(string key) => lockedKeys.Remove(key);
        public void CloseLock(string key) => lockedKeys.Add(key);
    }
}

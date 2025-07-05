using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public abstract class SingletonGroup<Self> : MonoBehaviour where Self : MonoBehaviour {

        private static readonly List<SingletonGroup<Self>> instanceList = new();
        public static List<Self> InstanceList => instanceList.ConvertAll(i => i as Self);
        protected virtual void Awake() => instanceList.Add(this);
        protected virtual void OnDestroy() => instanceList.Remove(this);
        protected static void EachInstance(Action<Self> action) => InstanceList.ForEach(action);


        private static readonly List<SingletonGroup<Self>> enabledList = new();
        public static List<Self> EnabledList => enabledList.ConvertAll(i => i as Self);
        protected virtual void OnEnable() => enabledList.Add(this);
        protected virtual void OnDisable() => enabledList.Remove(this);
        protected static void EachEnabled(Action<Self> action) => EnabledList.ForEach(action);

    }

}

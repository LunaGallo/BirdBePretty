using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LunaLib {
    public abstract class ConfigurableComponent<C, T> : IConfigurable<T> where C : Component {

        public C component;
        public virtual void Configure(T configuration) {
            if (component == null || configuration == null) {
                return;
            }
            ApplyConfiguration(component, configuration);
        }
        protected abstract void ApplyConfiguration(C component, T configuration);
    }
}
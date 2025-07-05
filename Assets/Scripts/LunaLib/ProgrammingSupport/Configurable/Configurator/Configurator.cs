using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LunaLib {
    public abstract class Configurator<T> : MonoBehaviour, IConfigurable<T> {
        protected bool reconfigureOnUpdate = false;
        protected T currentConfiguration;
        public virtual void Configure(T newConfiguration) {
            currentConfiguration = newConfiguration;
            if (newConfiguration != null) {
                ApplyConfiguration();
            }
        }
        protected abstract void ApplyConfiguration();
        public virtual void Update() => UpdateConfiguration();
        protected virtual void UpdateConfiguration() { if (reconfigureOnUpdate) ApplyConfiguration(); }
    }
    public abstract class ComponentConfigurator<Cm, Cn, T> : Configurator<T> where Cn : ConfigurableComponent<Cm, T> where Cm : Component {
        public abstract Cn ConfigurableComponent { get; }
        protected override void ApplyConfiguration() => ConfigurableComponent.Configure(currentConfiguration);
    }

    public abstract class ComponentPoolConfigurator<P, TOutter, TInner> : Configurator<TOutter> where P : Configurator<TInner> where TOutter : PoolConfiguration<TInner> {

        public SerializableComponentPool<P> elementPool;

        public List<P> ElementInstances => elementPool.ActiveElements;

        protected TInner[] PoolConfigurations => currentConfiguration.poolConfiguration;

        protected override void ApplyConfiguration() {
            if (PoolConfigurations == null) {
                return;
            }
            elementPool.SetActiveCount(PoolConfigurations.Length);
            for (int i = 0; i < ElementInstances.Count; i++) {
                int j = i;
                ApplyConfigurationsByIndex(j);
            }
        }
        protected virtual void ApplyConfigurationsByIndex(int index) {
            ElementInstances[index].Configure(PoolConfigurations[index]);
        }
    }

    public interface IChildConfiguration<TParent> {
        public TParent ParentConfiguration { get; set; }
    }
    public abstract class PoolConfiguration<T> {
        public T[] poolConfiguration;
    }
}
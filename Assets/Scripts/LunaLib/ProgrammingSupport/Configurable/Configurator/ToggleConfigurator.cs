using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace LunaLib {
    public abstract class CustomToggleConfigurator<T> : Configurator<T> where T : IChildConfiguration<UIToggleConfiguration> {
        public ConfigurableUIToggle toggle;
        protected override void ApplyConfiguration() => toggle.Configure(currentConfiguration.ParentConfiguration);
    }
    public abstract class CustomTogglePoolConfigurator<P, TOutter, TInner> : ComponentPoolConfigurator<P, TOutter, TInner>
        where P : CustomToggleConfigurator<TInner>
        where TOutter : CustomTogglePoolConfiguration<TInner>
        where TInner : IChildConfiguration<UIToggleConfiguration>, new() {

        protected Action<bool[]> onStateChanged = null;
        protected Action<int, bool> onValueChangedIndex = null;
        public bool[] State { get; protected set; } = null;

        protected override void ApplyConfiguration() {
            base.ApplyConfiguration();
            if (PoolConfigurations != null) {
                State = new bool[PoolConfigurations.Length];
            }
            if (currentConfiguration.onStateChanged != null) onStateChanged = currentConfiguration.onStateChanged;
            if (currentConfiguration.onValueChangedIndex != null) onValueChangedIndex = currentConfiguration.onValueChangedIndex;
        }
        protected override void ApplyConfigurationsByIndex(int index) {
            base.ApplyConfigurationsByIndex(index);
            ElementInstances[index].Configure(new() { ParentConfiguration = new() { onValueChanged = (v) => ElementValueChanged(index, v) } });
        }
        protected virtual void ElementValueChanged(int index, bool newValue) {
            onValueChangedIndex?.Invoke(index, newValue);
            State[index] = newValue;
            onStateChanged?.Invoke(State);
        }
    }
    public abstract class CustomTogglePoolConfiguration<T> : PoolConfiguration<T> {
        public Action<bool[]> onStateChanged = null;
        public Action<int, bool> onValueChangedIndex = null;
    }

    public class ToggleConfigurator : ComponentConfigurator<Toggle, ConfigurableUIToggle, UIToggleConfiguration> {
        public ConfigurableUIToggle toggle;
        public override ConfigurableUIToggle ConfigurableComponent => toggle;
    }
    public class TogglePoolConfigurator : ComponentPoolConfigurator<ToggleConfigurator, TogglePoolConfiguration, UIToggleConfiguration> {
        protected Action<bool[]> onStateChanged = null;
        protected Action<int, bool> onValueChangedIndex = null;
        public bool[] State { get; protected set; } = null;

        protected override void ApplyConfiguration() {
            base.ApplyConfiguration();
            if (PoolConfigurations != null) {
                State = new bool[PoolConfigurations.Length];
            }
            if (currentConfiguration.onStateChanged != null) onStateChanged = currentConfiguration.onStateChanged;
            if (currentConfiguration.onValueChangedIndex != null) onValueChangedIndex = currentConfiguration.onValueChangedIndex;
        }
        protected override void ApplyConfigurationsByIndex(int index) {
            base.ApplyConfigurationsByIndex(index);
            ElementInstances[index].Configure(new() { onValueChanged = (v) => ElementValueChanged(index, v) });
        }
        protected virtual void ElementValueChanged(int index, bool newValue) {
            onValueChangedIndex?.Invoke(index, newValue);
            State[index] = newValue;
            onStateChanged?.Invoke(State);
        }
    }
    public class TogglePoolConfiguration : PoolConfiguration<UIToggleConfiguration> {
        public Action<bool[]> onStateChanged = null;
        public Action<int, bool> onValueChangedIndex = null;
    }
}
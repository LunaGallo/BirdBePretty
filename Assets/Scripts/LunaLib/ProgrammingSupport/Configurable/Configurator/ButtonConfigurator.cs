using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace LunaLib {
    public abstract class CustomButtonConfigurator<T> : Configurator<T> where T : IChildConfiguration<UIButtonConfiguration> {
        public ConfigurableUIButton button;
        protected override void ApplyConfiguration() => button.Configure(currentConfiguration.ParentConfiguration);
    }
    public abstract class CustomButtonPoolConfigurator<P, TOutter, TInner> : ComponentPoolConfigurator<P, TOutter, TInner> 
        where P : CustomButtonConfigurator<TInner> 
        where TOutter : CustomButtonPoolConfiguration<TInner>
        where TInner : IChildConfiguration<UIButtonConfiguration>, new() {

        protected Action<int> onButtonClickedIndex = null;
        protected override void ApplyConfiguration() {
            base.ApplyConfiguration();
            if (currentConfiguration.onButtonClickedIndex != null) onButtonClickedIndex = currentConfiguration.onButtonClickedIndex;
        }
        protected override void ApplyConfigurationsByIndex(int index) {
            base.ApplyConfigurationsByIndex(index);
            ElementInstances[index].Configure(new() { ParentConfiguration = new() { onClick = () => ButtonWasPressed(index) } });
        }
        protected virtual void ButtonWasPressed(int index) {
            onButtonClickedIndex?.Invoke(index);
        }
    }
    public abstract class CustomButtonPoolConfiguration<T> : PoolConfiguration<T> {
        public Action<int> onButtonClickedIndex = null;
    }

    public class ButtonConfigurator : ComponentConfigurator<Button, ConfigurableUIButton, UIButtonConfiguration>{
        public ConfigurableUIButton button;
        public override ConfigurableUIButton ConfigurableComponent => button;
    }
    public class ButtonPoolConfigurator : ComponentPoolConfigurator<ButtonConfigurator, ButtonPoolConfiguration, UIButtonConfiguration> {
        protected Action<int> onButtonClickedIndex = null;
        protected override void ApplyConfiguration() {
            base.ApplyConfiguration();
            if (currentConfiguration.onButtonClickedIndex != null) onButtonClickedIndex = currentConfiguration.onButtonClickedIndex;
        }
        protected override void ApplyConfigurationsByIndex(int index) {
            base.ApplyConfigurationsByIndex(index);
            ElementInstances[index].Configure(new() { onClick = () => ButtonWasPressed(index) });
        }
        protected virtual void ButtonWasPressed(int index) {
            onButtonClickedIndex?.Invoke(index);
        }
    }
    public class ButtonPoolConfiguration : PoolConfiguration<UIButtonConfiguration> {
        public Action<int> onButtonClickedIndex = null;
    }
}
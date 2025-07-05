using System;

namespace LunaLib {
    public class MultipleChoiceMenu : CustomTogglePoolConfigurator<MenuChoiceToggle, MultipleChoiceMenuConfiguration, IconAndLabelToggleConfiguration> {
        public ConfigurableUIButton submitButton;
        protected Action<bool[]> onSubmitState;
        protected override void ApplyConfiguration() {
            base.ApplyConfiguration();
            if (submitButton != null 
                && currentConfiguration.submitButtonConfiguration != null) 
                { 
                submitButton.Configure(currentConfiguration.submitButtonConfiguration);
                submitButton.Configure(new() { onClick = SubmitButtonClicked });
            }
            if (currentConfiguration.onSubmitState != null) {
                onSubmitState = currentConfiguration.onSubmitState;
            }
        }
        protected void SubmitButtonClicked() {
            onSubmitState?.Invoke(State);
        }
    }
    public class MultipleChoiceMenuConfiguration : CustomTogglePoolConfiguration<IconAndLabelToggleConfiguration> {
        public UIButtonConfiguration submitButtonConfiguration = null;
        public Action<bool[]> onSubmitState = null;
    }
}
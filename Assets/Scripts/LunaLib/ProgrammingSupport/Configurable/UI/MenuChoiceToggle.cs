using System;

namespace LunaLib {
    public class MenuChoiceToggle : CustomToggleConfigurator<IconAndLabelToggleConfiguration> {
        public ConfigurableUIImage iconImage;
        public ConfigurableUIText labelText;

        protected override void ApplyConfiguration() {
            base.ApplyConfiguration();
            if (iconImage != null
                && currentConfiguration.iconAndLabel != null
                && currentConfiguration.iconAndLabel.icon != null) {
                iconImage.Configure(currentConfiguration.iconAndLabel.icon);
            }
            if (labelText != null
                && currentConfiguration.iconAndLabel != null
                && currentConfiguration.iconAndLabel.label != null) {
                labelText.Configure(currentConfiguration.iconAndLabel.label);
            }
        }
    }

    [Serializable]
    public class IconAndLabelToggleConfiguration : IChildConfiguration<UIToggleConfiguration> {
        public UIToggleConfiguration toggle = null;
        public IconAndLabelConfiguration iconAndLabel = null;
        public UIToggleConfiguration ParentConfiguration { get => toggle; set => toggle = value; }
    }
}
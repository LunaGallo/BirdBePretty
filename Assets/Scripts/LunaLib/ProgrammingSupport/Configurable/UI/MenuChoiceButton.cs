using System;
using UnityEngine;

namespace LunaLib {
    public class MenuChoiceButton : CustomButtonConfigurator<IconAndLabelButtonConfiguration> {
        public ConfigurableUIImage iconImage;
        public ConfigurableUIText labelText;

        protected override void ApplyConfiguration() {
            base.ApplyConfiguration();
            if (iconImage != null
                && currentConfiguration.iconAndLabel != null
                && currentConfiguration.iconAndLabel.icon != null)
                { 
                iconImage.Configure(currentConfiguration.iconAndLabel.icon); 
            }
            if (labelText != null
                && currentConfiguration.iconAndLabel != null
                && currentConfiguration.iconAndLabel.label != null) 
                { 
                labelText.Configure(currentConfiguration.iconAndLabel.label); 
            }
        }
    }

    [Serializable]
    public class IconAndLabelButtonConfiguration : IChildConfiguration<UIButtonConfiguration> {
        public UIButtonConfiguration button = null;
        public IconAndLabelConfiguration iconAndLabel = null;
        public UIButtonConfiguration ParentConfiguration { get => button; set => button = value; }
    }
}
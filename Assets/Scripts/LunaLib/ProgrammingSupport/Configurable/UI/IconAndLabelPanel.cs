using System;
using UnityEngine;

namespace LunaLib {
    public class IconAndLabelPanel : Configurator<IconAndLabelConfiguration> {
        public ConfigurableUIImage iconImage;
        public ConfigurableUIText labelText;

        protected override void ApplyConfiguration() {
            if (iconImage != null && currentConfiguration.icon != null) iconImage.Configure(currentConfiguration.icon);
            if (labelText != null && currentConfiguration.label != null) labelText.Configure(currentConfiguration.label);
        }
    }

    [Serializable]
    public class IconAndLabelConfiguration {
        public UIImageConfiguration icon = null;
        public UITextConfiguration label = null;
    }
}
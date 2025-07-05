using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace LunaLib {
    [Serializable]
    public class ConfigurableUIToggle : ConfigurableComponent<Toggle, UIToggleConfiguration> {
        protected override void ApplyConfiguration(Toggle component, UIToggleConfiguration configuration) {
            if (configuration.interactable.HasValue) component.interactable = configuration.interactable.Value;
            if (configuration.onValueChanged != null) component.onValueChanged.EnsureListener(configuration.onValueChanged);
        }
    }

    [Serializable]
    public class UIToggleConfiguration {
        public bool? interactable = null;
        public UnityAction<bool> onValueChanged = null;
    }
}

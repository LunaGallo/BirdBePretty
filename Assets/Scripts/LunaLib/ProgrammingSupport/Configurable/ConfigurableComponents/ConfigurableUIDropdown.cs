using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LunaLib {
    [Serializable]
    public class ConfigurableUIDropdown : ConfigurableComponent<Dropdown, UIDropdownConfiguration> {
        protected override void ApplyConfiguration(Dropdown component, UIDropdownConfiguration configuration) {
            if (configuration.interactable.HasValue) component.interactable = configuration.interactable.Value;
            if (configuration.onValueChanged != null) component.onValueChanged.EnsureListener(configuration.onValueChanged);
        }
    }
    [Serializable]
    public class UIDropdownConfiguration {
        public bool? interactable = null;
        public UnityAction<int> onValueChanged = null;
    }
}

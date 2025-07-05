using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace LunaLib {
    [Serializable]
    public class ConfigurableUIButton : ConfigurableComponent<Button, UIButtonConfiguration> {
        protected override void ApplyConfiguration(Button component, UIButtonConfiguration value) {
            if (value.interactable.HasValue)  component.interactable = value.interactable.Value;
            if (value.onClick != null) component.onClick.EnsureListener(value.onClick);
        }

    }
    [Serializable]
    public class UIButtonConfiguration {
        public bool? interactable = null;
        public UnityAction onClick = null;
    }
}

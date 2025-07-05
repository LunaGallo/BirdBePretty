using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace LunaLib {
    [Serializable]
    public class ConfigurableUIText : ConfigurableComponent<Text, UITextConfiguration> {
        protected override void ApplyConfiguration(Text component, UITextConfiguration value) {
            if (value.text != null) component.text = value.text;
            if (value.color.HasValue) component.color = value.color.Value;
        }
    }
    [Serializable]
    public class UITextConfiguration {
        public string text = null;
        public Color? color = null;
    }

}

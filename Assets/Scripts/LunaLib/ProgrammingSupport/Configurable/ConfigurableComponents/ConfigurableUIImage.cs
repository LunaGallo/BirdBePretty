using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace LunaLib {

    [Serializable]
    public class ConfigurableUIImage : ConfigurableComponent<Image, UIImageConfiguration> {
        protected override void ApplyConfiguration(Image component, UIImageConfiguration value) {
            if (value.sprite != null) component.sprite = value.sprite;
            if (value.color.HasValue) component.color = value.color.Value;
        }
    }

    [Serializable]
    public class UIImageConfiguration {
        public Sprite sprite = null;
        public Color? color = null;
    }
}
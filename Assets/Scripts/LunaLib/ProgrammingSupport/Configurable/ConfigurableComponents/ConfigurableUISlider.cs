using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace LunaLib {
    [Serializable]
    public class ConfigurableUISlider : ConfigurableComponent<Slider, UISliderConfiguration> {
        protected override void ApplyConfiguration(Slider component, UISliderConfiguration configuration) {
            if (configuration.minValue.HasValue) component.minValue = configuration.minValue.Value;
            if (configuration.maxValue.HasValue) component.maxValue = configuration.maxValue.Value;
            if (configuration.value.HasValue) component.value = configuration.value.Value;
            if (configuration.onValueChanged != null) component.onValueChanged.EnsureListener(configuration.onValueChanged);
        }
    }
    [Serializable]
    public class UISliderConfiguration {
        public float? minValue = null;
        public float? maxValue = null;
        public float? value = null;
        public UnityAction<float> onValueChanged = null;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LunaLib {
    public interface IConfigurable<T> {
        void Configure(T configuration);
    }
}
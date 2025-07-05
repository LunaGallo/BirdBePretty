using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LunaLib {
    public interface IPool<T> {
        List<T> ActiveElements { get; }
        T GetInstance();
        void ReturnInstance(T element);
        void ReturnAll();
        void Clear();
        void SetActiveCount(int newCount);

    }

}

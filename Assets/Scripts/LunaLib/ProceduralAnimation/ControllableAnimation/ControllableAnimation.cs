using System;
using UnityEngine;


namespace LunaLib {
    [Serializable]
    public abstract class ControllableAnimation : MonoBehaviour, IControllableAnimation {
        
        public float duration = 1f;
        public float Duration => duration;
        
        public abstract void UpdateProgress(float progress);

        public float TimerToProgress(float timer) => timer / Duration;
        public float ProgressToTimer(float progress) => progress * Duration;

    }


}

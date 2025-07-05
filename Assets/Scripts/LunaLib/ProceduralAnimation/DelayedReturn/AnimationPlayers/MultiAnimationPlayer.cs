using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    [Serializable]
    public class MultiAnimationPlayer : AnimationPlayer {
        public List<SubAnimation> subAnimations;
        public enum Mode {
            Sequential,
            Simultaneous
        }
        public Mode mode;
        public MultiAnimationPlayer(List<SubAnimation> subAnimations, Mode mode, bool isPlaying = false, float timer = 0f, float speed = 1f) 
            : base(isPlaying, timer, speed) {
            this.subAnimations = subAnimations;
            this.mode = mode;
        }

        public override float Duration => mode switch {
            Mode.Sequential => subAnimations.Sum(a => a.Duration),
            Mode.Simultaneous => subAnimations.Max(a => a.Duration),
            _ => -1f,
        };
        public override void Apply() {
            switch (mode) {
                case Mode.Sequential:
                    float durationSum = 0f;
                    foreach (IControllableAnimation animation in subAnimations) {
                        animation.UpdateTime(timer - durationSum);
                        durationSum += animation.Duration;
                    }
                    break;
                case Mode.Simultaneous:
                    foreach (IControllableAnimation animation in subAnimations) {
                        animation.UpdateTime(timer);
                    }
                    break;
            }
        }
    }

    [Serializable]
    public class SubAnimation : IControllableAnimation {
        public ControllableAnimation animation;
        public float delay = 0f;

        public float Duration => delay + animation.Duration;

        public void UpdateProgress(float animationProgress) => animation.UpdateProgress(animationProgress);
        public float TimerToProgress(float timer) => Mathf.Clamp01(Mathf.InverseLerp(delay, Duration, timer));
        public float ProgressToTimer(float progress) => progress * animation.Duration + delay;

    }

}

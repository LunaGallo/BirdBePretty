using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    [Serializable]
    public class SimpleAnimationPlayer : AnimationPlayer {
        public ControllableAnimation animation;
        public override float Duration => animation.duration;

        public SimpleAnimationPlayer(ControllableAnimation controllableAnimation, bool isPlaying = false, float timer = 0f, float speed = 1f) : base(isPlaying, timer, speed) => animation = controllableAnimation;

        public override void Apply() => animation.UpdateProgress(timer / Duration);
    }

}

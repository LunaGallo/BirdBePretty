using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LunaLib {
    public class ScriptedAnimator : ScriptedAction {
        public MultiAnimationPlayer player;
        public bool looping = true;

        public bool Inverted {
            get => !player.IsDirectionNormal;
            set => player.IsDirectionNormal = !value;
        }

        private void Start() => player.OnReturn = AnimationFinished;
        private void Update() => player.Update(Time.deltaTime);

        private void AnimationFinished() {
            if (looping) {
                player.Restart();
            }
            OnReturn?.Invoke();
        }

        public override void StartAction() => player.StartAction();
        public override void InterruptAction() => player.InterruptAction();
        public override void SkipAction() => player.SkipAction();
        public override void PauseAction() => player.PauseAction();
        public override void ResumeAction() => player.ResumeAction();
    }
}

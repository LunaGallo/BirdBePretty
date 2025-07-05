using System;
using System.Collections;
using UnityEngine;

namespace LunaLib {

    public class ScriptedDelay : ScriptedAction {
        public float duration = 1f;

        private float timer = 0f;
        private bool paused = false;

        public override void StartAction() => timer = duration;
        public override void InterruptAction() => timer = 0f;
        public override void SkipAction() {
            timer = 0f;
            OnReturn?.Invoke();
        }
        public override void PauseAction() => paused = true;
        public override void ResumeAction() => paused = false;
        private void Update() {
            if (!paused && timer > 0f) {
                timer -= Time.deltaTime;
                if (timer <= 0f) {
                    OnReturn.Invoke();
                }
            }
        }
    }

}

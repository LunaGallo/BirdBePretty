using System;
using UnityEngine;

namespace LunaLib {
    public class ScriptedAudio : ScriptedAction {

        public AudioSource audioSource;
        
        private bool active = false;
        private bool paused = false;

        public override void StartAction() {
            if (!active) {
                active = true;
                audioSource.Play();
            }
        }
        public override void InterruptAction() {
            if (active) {
                active = false;
                audioSource.Stop();
            }
        }
        public override void SkipAction() {
            if (active) {
                active = false;
                audioSource.Stop();
                OnReturn?.Invoke();
            }
        }

        public override void PauseAction() {
            if (active) {
                paused = true;
                audioSource.Pause();
            }
        }
        public override void ResumeAction() {
            if (active) {
                paused = false;
                audioSource.Play();
            }
        }
        private void Update() {
            if (active && !paused && !audioSource.isPlaying) {
                active = false;
                OnReturn.Invoke();
            }
        }

    }
}

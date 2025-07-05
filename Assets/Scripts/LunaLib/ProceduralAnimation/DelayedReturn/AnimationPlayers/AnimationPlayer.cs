using System;
using UnityEngine.Events;

namespace LunaLib {
    [Serializable]
    public abstract class AnimationPlayer : IDelayedReturn {
        public bool isPlaying = false;
        public float timer = 0f;
        public float speed = 1f;
        
        public abstract float Duration { get; }
        public Action OnReturn { get; set; }

        public virtual void OnFinished() => OnReturn?.Invoke();

        public void StartAction() {
            if (!isPlaying) {
                Restart();
            }
        }
        public void InterruptAction() {
            if (isPlaying) {
                Pause();
                Apply();
            }
        }
        public void SkipAction() {
            timer = EndTime;
            Pause();
            Apply();
            OnReturn?.Invoke();
        }
        public void PauseAction() {
            if (isPlaying) {
                isPlaying = false;
            }
        }
        public void ResumeAction() {
            isPlaying = true;
        }

        public AnimationPlayer(bool isPlaying = false, float timer = 0f, float speed = 1f) {
            this.isPlaying = isPlaying;
            this.timer = timer;
            this.speed = speed;
        }

        public void Update(float deltaTime) {
            if(isPlaying && timer <= Duration && timer >= 0f) {
                timer += deltaTime * speed;
                if(IsDirectionNormal && timer >= Duration) {
                    timer = Duration;
                    Pause();
                    OnFinished();
                } 
                else if(!IsDirectionNormal && timer <= 0f) {
                    timer = 0f;
                    Pause();
                    OnFinished();
                }
                Apply();
            }
        }
        public void Restart() {
            Play();
            Reset();
        }
        public void Stop() {
            Pause();
            Reset();
        }
        public void Play() => isPlaying = true;
        public void Pause() => isPlaying = false;
        public void Reset() {
            timer = StartTime;
            Apply();
        }
        public abstract void Apply();

        public bool IsDirectionNormal {
            get => speed.Sign() > 0f;
            set { if (IsDirectionNormal != value) speed *= -1f; }
        }
        public void InvertDirection() => speed *= -1f;
        public float StartTime => IsDirectionNormal ? 0f : Duration;
        public float EndTime => IsDirectionNormal ? Duration : 0f;

    }

}

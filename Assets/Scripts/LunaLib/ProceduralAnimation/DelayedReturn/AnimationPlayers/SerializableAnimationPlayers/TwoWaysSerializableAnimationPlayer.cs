using System;
using System.Collections.Generic;

namespace LunaLib {
    [Serializable] 
    public class TwoWaysSerializableAnimationPlayer : MultiAnimationPlayer {
        
        public Action OnFinishForward { get;  set; }
        public Action OnFinishBackward { get;  set; }

        public TwoWaysSerializableAnimationPlayer(Action onFinishForward, Action onFinishBackward, List<SubAnimation> subAnimations, Mode mode, bool isPlaying = false, float timer = 0f, float speed = 1f)
            : base(subAnimations, mode, isPlaying, timer, speed) {
            OnFinishForward = onFinishForward;
            OnFinishBackward = onFinishBackward;
        }

        public void GoForward() {
            IsDirectionNormal = true;
            Play();
        }
        public void RestartForward() {
            IsDirectionNormal = true;
            Restart();
        }
        public void GoBackwards() {
            IsDirectionNormal = false;
            Play();
        }
        public void RestartBackwards() {
            IsDirectionNormal = false;
            Restart();
        }

        public override void OnFinished() {
            if (IsDirectionNormal) {
                OnFinishForward?.Invoke();
            }
            else {
                OnFinishBackward?.Invoke();
            }
            base.OnFinished();
        }

    }

}

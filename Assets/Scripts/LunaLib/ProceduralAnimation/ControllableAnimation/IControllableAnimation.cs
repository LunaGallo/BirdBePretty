using System;

namespace LunaLib {
    public interface IControllableAnimation {
        void UpdateProgress(float progress);
        void UpdateTime(float timer) => UpdateProgress(TimerToProgress(timer));
        float Duration { get; }
        float TimerToProgress(float timer);
        float ProgressToTimer(float progress);
    }
}

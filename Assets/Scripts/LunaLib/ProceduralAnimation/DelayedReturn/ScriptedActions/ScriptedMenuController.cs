using System;
using UnityEngine;

namespace LunaLib {
    public class ScriptedMenuController : MenuController<ScriptedMenuController>, IDelayedReturn {

        public ScriptedAction scriptedAction;
        public Action OnReturn { get; set; }

        private enum State { Inactive, Oppening, Active, Closing }
        private State state = State.Inactive;

        protected override void Start() {
            base.Start();
            scriptedAction.OnReturn = ActionFinished;
        }

        public virtual void StartAction() {
            if (state == State.Inactive) {
                state = State.Oppening;
                Open();
            }
        }
        protected override void OpenFinished() {
            base.OpenFinished();
            state = State.Active;
            scriptedAction.StartAction();
        }
        protected virtual void ActionFinished() {
            state = State.Closing;
            Close();
        }
        protected override void CloseFinished() {
            base.CloseFinished();
            state = State.Inactive;
            OnReturn.Invoke();
        }

        public void InterruptAction() {
            switch (state) {
                case State.Oppening:
                    twoWaysAnimator.InterruptAction();
                    break;
                case State.Active:
                    scriptedAction.InterruptAction();
                    break;
                case State.Closing:
                    twoWaysAnimator.InterruptAction();
                    break;
            }
            state = State.Inactive;
        }
        public void SkipAction() {
            switch (state) {
                case State.Oppening:
                    twoWaysAnimator.SkipAction();
                    scriptedAction.SkipAction();
                    twoWaysAnimator.SkipAction();
                    state = State.Inactive;
                    break;
                case State.Active:
                    scriptedAction.SkipAction();
                    twoWaysAnimator.SkipAction();
                    state = State.Inactive;
                    break;
                case State.Closing:
                    twoWaysAnimator.SkipAction();
                    state = State.Inactive;
                    break;
            }
        }
        public void PauseAction() {
            switch (state) {
                case State.Oppening:
                    twoWaysAnimator.PauseAction();
                    break;
                case State.Active:
                    scriptedAction.PauseAction();
                    break;
                case State.Closing:
                    twoWaysAnimator.PauseAction();
                    break;
            }
        }
        public void ResumeAction() {
            switch (state) {
                case State.Oppening:
                    twoWaysAnimator.ResumeAction();
                    break;
                case State.Active:
                    scriptedAction.ResumeAction();
                    break;
                case State.Closing:
                    twoWaysAnimator.ResumeAction();
                    break;
            }
        }

    }
}
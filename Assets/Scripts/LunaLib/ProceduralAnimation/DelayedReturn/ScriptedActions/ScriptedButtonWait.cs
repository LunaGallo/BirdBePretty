using UnityEngine.UI;

namespace LunaLib {
    public class ScriptedButtonWait : ScriptedAction {

        public Button button;
        public bool endInteractionAfter = true;

        private bool active = false;
        private bool paused = false;

        public override void StartAction() {
            if (!active) {
                active = true;
                button.onClick.AddListener(ButtonPressed);
                button.interactable = true;
            }
        }
        public override void InterruptAction() {
            if (active) {
                active = false;
                button.onClick.RemoveListener(ButtonPressed);
            }
        }
        public override void SkipAction() {
            if (active) {
                active = false;
                button.onClick.RemoveListener(ButtonPressed);
                if (endInteractionAfter) {
                    button.interactable = false;
                }
                OnReturn?.Invoke();
            }
        }
        public override void PauseAction() {
            if (active) {
                paused = true;
            }
        }
        public override void ResumeAction() {
            if (active) {
                paused = false;
            }
        }
        private void ButtonPressed() {
            if (active && !paused) {
                active = false;
                button.onClick.RemoveListener(ButtonPressed);
                if (endInteractionAfter) {
                    button.interactable = false;
                }
                OnReturn.Invoke();
            }
        }

    }
}
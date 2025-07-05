using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public abstract class MenuController<T> : Singleton<T> where T : MonoBehaviour {

        public GameObject root;
        public TwoWaysSerializableAnimationPlayer twoWaysAnimator;
        public List<IdentifierEvent> onConfigSet;

        protected virtual void Start() {
            twoWaysAnimator.OnFinishForward = OpenFinished;
            twoWaysAnimator.OnFinishBackward = CloseFinished;
        }

        public virtual void SetConfig(string name) {
            onConfigSet.ForEach(e => e.TryInvoke(name));
            LastConfigSet = name;
        }
        public string LastConfigSet { get; private set; }

        public void Open() {
            if (root != null) {
                root.SetActive(true);
            }
            twoWaysAnimator.GoForward();
        }
        public void Close() {
            twoWaysAnimator.GoBackwards();
        }

        protected virtual void Update() {
            twoWaysAnimator.Update(Time.deltaTime);
        }

        protected virtual void OpenFinished() { }
        protected virtual void CloseFinished() {
            if (root != null) {
                root.SetActive(false);
            }
        }

    }
}
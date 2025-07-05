using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    [Serializable]
    public abstract class MovementAnimation<T> : IDelayedReturn {

        protected abstract Func<T, T, float, T> LerpFunc { get; }
        protected abstract Func<T, T, float> DistFunc { get; }
        public Action OnReturn { get; set; }

        public Action<T> onApply;

        public AnimationCurve animationCurve;
        public float speed = 1f;

        public bool ContinueLastSegment { get; set; } = false;

        private float progress = 0f;
        private T startingPoint;
        private T finishingPoint;
        private bool paused = false;

        public void Update(float deltaTime) {
            if (!paused) {
                float dist = DistFunc.Invoke(startingPoint, finishingPoint);
                if (progress < 1f && dist > 0f) {
                    progress += deltaTime * speed / dist;
                    if (progress >= 1f) {
                        EndSegment();
                    }
                }
                Apply();
            }
        }
        public void SetNewSegment(T start, T finish) {
            startingPoint = start;
            finishingPoint = finish;
        }
        public void SetNextSegment(T finish) {
            startingPoint = finishingPoint;
            finishingPoint = finish;
        }
        public void Apply() {
            onApply?.Invoke(LerpFunc.Invoke(startingPoint, finishingPoint, animationCurve.Evaluate(Mathf.Clamp01(progress))));
        }
        protected virtual void EndSegment() {
            OnReturn?.Invoke();
        }

        public void StartAction() {
            if (ContinueLastSegment) {
                progress -= 1f;
            }
            else {
                progress = 0f;
            }
        }
        public void InterruptAction() {
            progress = 1f;
        }
        public void SkipAction() {
            progress = 1f;
            Apply();
            OnReturn?.Invoke();
        }
        public void PauseAction() {
            paused = true;
        }
        public void ResumeAction() {
            paused = false;
        }

    }
    [Serializable]
    public class FloatMovementAnimation : MovementAnimation<float> {
        protected override Func<float, float, float, float> LerpFunc => Mathf.Lerp;
        protected override Func<float, float, float> DistFunc => (a, b) => Mathf.Abs(a - b);
    }
    [Serializable]
    public class Vector2MovementAnimation : MovementAnimation<Vector2> {
        protected override Func<Vector2, Vector2, float, Vector2> LerpFunc => Vector2.Lerp;
        protected override Func<Vector2, Vector2, float> DistFunc => Vector2.Distance;
    }
    [Serializable]
    public class Vector3MovementAnimation : MovementAnimation<Vector3> {
        protected override Func<Vector3, Vector3, float, Vector3> LerpFunc => Vector3.Lerp;
        protected override Func<Vector3, Vector3, float> DistFunc => Vector3.Distance;
    }
    [Serializable]
    public class Vector4MovementAnimation : MovementAnimation<Vector4> {
        protected override Func<Vector4, Vector4, float, Vector4> LerpFunc => Vector4.Lerp;
        protected override Func<Vector4, Vector4, float> DistFunc => Vector4.Distance;
    }


    [Serializable]
    public abstract class StateMovementAnimation<T> : MovementAnimation<T> {

        public int startingStateIndex = 0;

        [NonSerialized] public List<T> stateValues;

        public int CurStateIndex { get; private set; }

        public virtual void Start() {
            CurStateIndex = startingStateIndex;
            SetNewSegment(stateValues[CurStateIndex], stateValues[CurStateIndex]);
            ContinueLastSegment = false;
            StartAction();
        }
        public void StartTransitionToState(int stateIndex) {
            CurStateIndex = stateIndex;
            SetNextSegment(stateValues[CurStateIndex]);
            ContinueLastSegment = false;
            StartAction();
        }
        public void ContinueTransitionToState(int stateIndex) {
            CurStateIndex = stateIndex;
            SetNextSegment(stateValues[CurStateIndex]);
            ContinueLastSegment = true;
            StartAction();
        }

    }
    [Serializable]
    public class StateFloatMovementAnimation : StateMovementAnimation<float> {
        protected override Func<float, float, float, float> LerpFunc => Mathf.Lerp;
        protected override Func<float, float, float> DistFunc => (a, b) => Mathf.Abs(a - b);
    }
    [Serializable]
    public class StateVector2MovementAnimation : StateMovementAnimation<Vector2> {
        protected override Func<Vector2, Vector2, float, Vector2> LerpFunc => Vector2.Lerp;
        protected override Func<Vector2, Vector2, float> DistFunc => Vector2.Distance;
    }
    [Serializable]
    public class StateVector3MovementAnimation : StateMovementAnimation<Vector3> {
        protected override Func<Vector3, Vector3, float, Vector3> LerpFunc => Vector3.Lerp;
        protected override Func<Vector3, Vector3, float> DistFunc => Vector3.Distance;
    }
    [Serializable]
    public class StateVector4MovementAnimation : StateMovementAnimation<Vector4> {
        protected override Func<Vector4, Vector4, float, Vector4> LerpFunc => Vector4.Lerp;
        protected override Func<Vector4, Vector4, float> DistFunc => Vector4.Distance;
    }


    [Serializable]
    public abstract class GraphMovementAnimation<T> : StateMovementAnimation<T> {

        public Action onStop;

        [NonSerialized] public List<List<int>> connections;

        private List<int> statePath = new List<int>();

        public override void Start() {
            base.Start();
            statePath.Add(startingStateIndex);
        }
        public void GoToState(int finalIndex) {
            bool wasMoving = statePath.Count > 1;
            statePath.AddRange(GraphSolver.AStar_PathFinding(s => connections[s], (a, b) => DistFunc(stateValues[a], stateValues[b]), s => DistFunc(stateValues[s], stateValues[finalIndex]), statePath.Last(), finalIndex).WithoutFirst());
            if (!wasMoving && statePath.Count > 1) {
                StartTransitionToState(statePath[1]);
            }
        }
        protected override void EndSegment() {
            if (statePath.Count > 1) {
                statePath.RemoveFirst();
                if (statePath.Count > 1) {
                    base.EndSegment();
                    ContinueTransitionToState(statePath[1]);
                }
                else {
                    base.EndSegment();
                    StopMovement();
                }
            }
        }
        protected virtual void StopMovement() {
            onStop?.Invoke();
        }

    }
    [Serializable]
    public class GraphFloatMovementAnimation : GraphMovementAnimation<float> {
        protected override Func<float, float, float, float> LerpFunc => Mathf.Lerp;
        protected override Func<float, float, float> DistFunc => (a, b) => Mathf.Abs(a - b);
    }
    [Serializable]
    public class GraphVector2MovementAnimation : GraphMovementAnimation<Vector2> {
        protected override Func<Vector2, Vector2, float, Vector2> LerpFunc => Vector2.Lerp;
        protected override Func<Vector2, Vector2, float> DistFunc => Vector2.Distance;
    }
    [Serializable]
    public class GraphVector3MovementAnimation : GraphMovementAnimation<Vector3> {
        protected override Func<Vector3, Vector3, float, Vector3> LerpFunc => Vector3.Lerp;
        protected override Func<Vector3, Vector3, float> DistFunc => Vector3.Distance;
    }
    [Serializable]
    public class GraphVector4MovementAnimation : GraphMovementAnimation<Vector4> {
        protected override Func<Vector4, Vector4, float, Vector4> LerpFunc => Vector4.Lerp;
        protected override Func<Vector4, Vector4, float> DistFunc => Vector4.Distance;
    }
}
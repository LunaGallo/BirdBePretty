using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LunaLib {
    [ExecuteAlways]
    public abstract class EditorBehaviour : MonoBehaviour {

        public virtual bool AllowDestroyOnPlay => true;
        [ShowIf("AllowDestroyOnPlay")] public bool destroyOnPlay = false;

        private void Awake() {
            if (AllowDestroyOnPlay && destroyOnPlay && Application.isPlaying) {
                Destroy(this);
            }
        }

        protected virtual void Start() {
            if (!Application.isPlaying && CanApplyOnEditorStart) {
                ApplyOnEditorStart();
            }
            else if (Application.isPlaying && CanApplyOnPlayStart) {
                ApplyOnPlayStart();
            }
        }
        protected virtual void ApplyOnEditorStart() { }
        protected virtual bool CanApplyOnEditorStart => true;
        protected virtual void ApplyOnPlayStart() { }
        protected virtual bool CanApplyOnPlayStart => true;

        protected virtual void Update() {
            if (!Application.isPlaying && CanApplyOnEditorUpdate) {
                ApplyOnEditorUpdate();
            }
            else if (Application.isPlaying && CanApplyOnPlayUpdate) {
                ApplyOnPlayUpdate();
            }
        }
        protected virtual void ApplyOnEditorUpdate() { }
        protected virtual bool CanApplyOnEditorUpdate => true;
        protected virtual void ApplyOnPlayUpdate() { }
        protected virtual bool CanApplyOnPlayUpdate => true;

        protected virtual void OnValidate() {
            if (CanApplyOnValidate) {
                ApplyOnValidate();
            }
        }
        protected virtual void ApplyOnValidate() { }
        protected virtual bool CanApplyOnValidate => true;


        public delegate void PropertyCalculation<T>(ref T newValue, ref bool componentMightChange);
        public static void SafeModifyProperty<T, C>(C changeableComponent, PropertyCalculation<T> propertyCalculation, Action<C, T> propertyApplication, Func<C, T, bool> propertyEqualsPredicate) where C : Component {
            T newValue = default;
            bool componentMightChange = false;
            propertyCalculation(ref newValue, ref componentMightChange);
            if (Application.isPlaying && componentMightChange) {
                propertyApplication(changeableComponent, newValue);
            }
#if UNITY_EDITOR
            else if (componentMightChange && propertyEqualsPredicate(changeableComponent, newValue)) {
                Undo.RecordObject(changeableComponent, "Changed Position");
                propertyApplication(changeableComponent, newValue);
                ApplyChangeInObject(changeableComponent);
            }
#endif
        }
#if UNITY_EDITOR
        public static void ApplyChangeInObject(UnityEngine.Object obj) {
            EditorUtility.SetDirty(obj);
            if (PrefabUtility.IsPartOfPrefabInstance(obj)) {
                PrefabUtility.RecordPrefabInstancePropertyModifications(obj);
            }
        }
#endif

    }
}
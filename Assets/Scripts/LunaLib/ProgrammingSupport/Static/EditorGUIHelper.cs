using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace LunaLib {
    public static class EditorGUIHelper {

        public abstract class LabeledFieldConfig {
            public GUIContent label;
            public LabeledFieldConfig(GUIContent label) { 
                this.label = label; 
            }
            public void DrawLabel(Rect uiRect) => EditorGUI.LabelField(uiRect, label);
            public abstract void DrawValueField(Rect uiRect);
        }
        public class LabeledPropertyConfig : LabeledFieldConfig {
            public SerializedProperty property;
            public LabeledPropertyConfig(GUIContent label, SerializedProperty property) : base(label) {
                this.property = property;
            }
            public override void DrawValueField(Rect uiRect) => EditorGUI.PropertyField(uiRect, property, GUIContent.none);
        }
        public abstract class LabeledValueFieldConfig<T> : LabeledFieldConfig {
            public Func<T> getter;
            public Action<T> setter;
            public UnityEngine.Object undoObj;
            public string undoMsg;

            protected LabeledValueFieldConfig(GUIContent label, Func<T> getter, Action<T> setter, UnityEngine.Object undoObj, string undoMsg) : base(label) {
                this.getter = getter;
                this.setter = setter;
                this.undoObj = undoObj;
                this.undoMsg = undoMsg;
            }

            public override void DrawValueField(Rect uiRect) {
                EditorGUI.BeginChangeCheck();
                T value = getter();
                value = FieldDrawer(uiRect, value);
                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(undoObj, undoMsg);
                    setter(value);
                }
            }
            public abstract Func<Rect, T, T> FieldDrawer { get; }
        }
        public class LabeledFloatFieldConfig : LabeledValueFieldConfig<float> {
            public LabeledFloatFieldConfig(GUIContent label, Func<float> getter, Action<float> setter, UnityEngine.Object undoObj, string undoMsg) : base(label, getter, setter, undoObj, undoMsg) { }
            public override Func<Rect, float, float> FieldDrawer => EditorGUI.FloatField;
        }
        public class LabeledIntFieldConfig : LabeledValueFieldConfig<int> {
            public LabeledIntFieldConfig(GUIContent label, Func<int> getter, Action<int> setter, UnityEngine.Object undoObj, string undoMsg) : base(label, getter, setter, undoObj, undoMsg) { }
            public override Func<Rect, int, int> FieldDrawer => EditorGUI.IntField;
        }
        public static void LabeledFieldSingleLine(Rect uiRect, List<LabeledFieldConfig> labeledFields, float space = 5f) {
            float width = uiRect.width / labeledFields.Count;
            for (int i = 0; i < labeledFields.Count; i++) {
                float labelWidth = GUI.skin.label.CalcSize(labeledFields[i].label).x;
                float fieldWidth = width - labelWidth - space;
                labeledFields[i].DrawLabel(uiRect.WithPosX(x => x + (width + space) * i).WithWidth(labelWidth));
                labeledFields[i].DrawValueField(uiRect.WithPosX(x => x + labelWidth + (width + space) * i).WithWidth(fieldWidth));
            }
        }
        public static void LabeledFieldMultipleLine(Rect uiRect, List<List<LabeledFieldConfig>> labeledFields, float space = 5f) {
            float heigth = uiRect.height / labeledFields.Count;
            for (int i = 0; i < labeledFields.Count; i++) {
                float width = uiRect.width / labeledFields[i].Count;
                for (int j = 0; j < labeledFields[i].Count; j++) {
                    float labelWidth = GUI.skin.label.CalcSize(labeledFields[i][j].label).x;
                    Rect labelRect = new(uiRect.x + (width + space) * j, uiRect.y + heigth * (labeledFields.Count - 1 - i), labelWidth, heigth);
                    labeledFields[i][j].DrawLabel(labelRect);
                    float fieldWidth = width - labelWidth - space;
                    Rect fieldRect = new(labelRect.x + labelWidth, labelRect.y, fieldWidth, heigth);
                    labeledFields[i][j].DrawValueField(fieldRect);
                }
            }
        }

        public static float CalcEnumDropdownWidth(Type enumType) => Enum.GetNames(enumType).Max(n => GUI.skin.label.CalcSize(new(n)).x) + 20f;

    }
}
#endif
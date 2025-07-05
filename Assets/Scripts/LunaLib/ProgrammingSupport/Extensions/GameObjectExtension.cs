using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public static partial class GameObjectExtension {

        public static int IndexOfChild(this Transform self, Transform child) {
            for (int i = 0; i < self.childCount; i++) {
                if (child == self.GetChild(i)) {
                    return i;
                }
            }
            return -1;
        }

        public static void SetAllActive(this List<GameObject> list, bool value) {
            list.ForEach(go => go.SetActive(value));
        }

        public static void DestroyAllInactive(this List<GameObject> list) {
            foreach (GameObject obj in list) {
                if (!obj.activeSelf) {
                    UnityEngine.Object.Destroy(obj);
                }
            }
        }
        public static void DestroyAllInactive(this List<Transform> list) {
            foreach (Transform transform in list) {
                if (!transform.gameObject.activeSelf) {
                    UnityEngine.Object.Destroy(transform.gameObject);
                }
            }
        }
        public static void DestroyAllInactive(this List<Component> list) {
            foreach (Component component in list) {
                if (!component.gameObject.activeSelf) {
                    UnityEngine.Object.Destroy(component.gameObject);
                }
            }
        }
        public static void DestroyAllInactive<TComponent>(this List<TComponent> list) where TComponent : Component {
            foreach (TComponent component in list) {
                if (!component.gameObject.activeSelf) {
                    UnityEngine.Object.Destroy(component.gameObject);
                }
            }
        }

        public static TProperty GetComponentProperty<TComponent, TProperty>(this GameObject self, Func<TComponent, TProperty> propertyGetter, TProperty defaultValue = default) where TComponent : Component {
            TComponent component = self.GetComponent<TComponent>();
            return (component != null) ? propertyGetter.Invoke(component) : defaultValue;
        }
        public static TProperty GetComponentProperty<TComponent, TProperty>(this Transform self, Func<TComponent, TProperty> propertyGetter, TProperty defaultValue = default) where TComponent : Component {
            TComponent component = self.GetComponent<TComponent>();
            return (component != null) ? propertyGetter.Invoke(component) : defaultValue;
        }

        public static List<TComponent> GetAllComponents<TComponent>(this List<GameObject> list) where TComponent : Component {
            List<TComponent> result = new List<TComponent>();
            foreach (GameObject obj in list) {
                TComponent component = obj.GetComponent<TComponent>();
                if (component != null) {
                    result.Add(component);
                }
            }
            return result;
        }
        public static List<TComponent> GetAllComponents<TComponent>(this List<Transform> list) where TComponent : Component {
            List<TComponent> result = new List<TComponent>();
            foreach (Transform t in list) {
                TComponent component = t.GetComponent<TComponent>();
                if (component != null) {
                    result.Add(component);
                }
            }
            return result;
        }
        public static List<T2> GetAllComponents<T1, T2>(this List<T1> list) where T1 : Component where T2 : Component {
            List<T2> result = new List<T2>();
            foreach (T1 c in list) {
                T2 component = c.GetComponent<T2>();
                if (component != null) {
                    result.Add(component);
                }
            }
            return result;
        }

        public static List<GameObject> GetChildren(this GameObject self) {
            return self.transform.GetChildren().ConvertAll(t => t.gameObject);
        }
        public static List<Transform> GetChildren(this Transform self) {
            List<Transform> result = new List<Transform>();
            foreach (Transform child in self) {
                result.Add(child);
            }
            return result;
        }
        public static List<TComponent> GetChildren<TComponent>(this TComponent self) where TComponent : Component {
            List<TComponent> result = new List<TComponent>();
            foreach (Transform child in self.transform) {
                TComponent component = child.GetComponent<TComponent>();
                if (component != null) {
                    result.Add(component);
                }
            }
            return result;
        }

        public static List<GameObject> GetChildrenFamilies(this GameObject self) {
            return self.transform.GetChildrenFamilies().ConvertAll(t => t.gameObject);
        }
        public static List<Transform> GetChildrenFamilies(this Transform self) {
            List<Transform> result = new List<Transform>();
            foreach (Transform child in self) {
                result.Add(child);
                result.AddRange(child.GetChildrenFamilies());
            }
            return result;
        }
        public static List<TComponent> GetChildrenFamilies<TComponent>(this TComponent self) where TComponent : Component {
            List<TComponent> result = new List<TComponent>();
            List<GameObject> objList = self.gameObject.GetChildrenFamilies();
            foreach (GameObject obj in objList) {
                TComponent component = obj.GetComponent<TComponent>();
                if (component != null) {
                    result.Add(component);
                }
            }
            return result;
        }

        public static void SetChildRangeActive(this GameObject self, int start, int length) => self.transform.SetChildRangeActive(start, length);
        public static void SetChildRangeActive(this Transform self, int start, int length) {
            for (int i = 0; i < self.childCount; i++) {
                self.GetChild(i).gameObject.SetActive(i >= start && i < (start + length));
            }
        }

        public static void DestroyInactiveChildren(this GameObject self) => self.transform.DestroyInactiveChildren();
        public static void DestroyInactiveChildren(this Transform self) {
            foreach (Transform child in self) {
                if (!child.gameObject.activeSelf) {
                    UnityEngine.Object.Destroy(child.gameObject);
                }
            }
        }

        public static void DestroyAllChildren(this GameObject self) => self.transform.DestroyAllChildren();
        public static void DestroyAllChildren(this Transform self) {
            foreach (Transform child in self) {
                UnityEngine.Object.Destroy(child.gameObject);
            }
        }

        #region LineRenderer
        public static List<Vector3> GetPositionList(this LineRenderer line) {
            Vector3[] positions = new Vector3[line.positionCount];
            line.GetPositions(positions);
            return new List<Vector3>(positions);
        }
        public static void AddPosition(this LineRenderer line, Vector3 pos) {
            int count = line.positionCount;
            line.positionCount = count + 1;
            line.SetPosition(count, pos);
        }
        public static float GetLength(this LineRenderer line) {
            return line.GetPositionList().PathLength();
        }
        #endregion

        #region KeyFrame
        public static Keyframe WithValue(this Keyframe k, float newValue) {
            return new Keyframe(k.time, newValue, k.inTangent, k.outTangent, k.inWeight, k.outWeight);
        }
        public static Keyframe WithValue(this Keyframe k, Func<float, float> valueFunc) {
            return k.WithValue(valueFunc.Invoke(k.value));
        }

        public static Keyframe WithTime(this Keyframe k, float newTime) {
            return new Keyframe(newTime, k.value, k.inTangent, k.outTangent, k.inWeight, k.outWeight);
        }
        public static Keyframe WithTime(this Keyframe k, Func<float, float> timeFunc) {
            return k.WithTime(timeFunc.Invoke(k.time));
        }

        public static Keyframe ScaledBy(this Keyframe k, Vector2 vector) {
            return new Keyframe(
                k.time * vector.x,
                k.value * vector.y,
                k.inTangent * vector.y / vector.x,
                k.outTangent * vector.y / vector.x
            );
        }

        public static List<Keyframe> InvertedCurve(this List<Keyframe> list) {
            List<Keyframe> result = list.InvertedList();
            result.SetEachElement(k => k.ScaledBy(new Vector2(-1f, 1f)).WithTime(t => t + 1f));
            return result;
        }
        public static void InvertCurve(this List<Keyframe> list) {
            list.Invert();
            list.SetEachElement(k => k.ScaledBy(new Vector2(-1f, 1f)).WithTime(t => t + 1f));
        }
        #endregion

    }

}

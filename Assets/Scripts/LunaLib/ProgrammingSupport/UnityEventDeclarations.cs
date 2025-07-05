using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LunaLib {

    [Serializable] public class BooleanEvent : UnityEvent<bool> { }
    [Serializable] public class IntegerEvent : UnityEvent<int> { }
    [Serializable] public class FloatEvent : UnityEvent<float> { }
    [Serializable] public class CharEvent : UnityEvent<char> { }
    [Serializable] public class StringEvent : UnityEvent<string> { }
    [Serializable] public class Vector2Event : UnityEvent<Vector2> { }
    [Serializable] public class Vector2IntEvent : UnityEvent<Vector2Int> { }
    [Serializable] public class Vector3Event : UnityEvent<Vector3> { }
    [Serializable] public class Vector3IntEvent : UnityEvent<Vector3Int> { }
    [Serializable] public class Vector4Event : UnityEvent<Vector4> { }
    [Serializable] public class RectEvent : UnityEvent<Rect> { }
    [Serializable] public class RectIntEvent : UnityEvent<RectInt> { }
    [Serializable] public class QuaternionEvent : UnityEvent<Quaternion> { }
    [Serializable] public class ColorEvent : UnityEvent<Color> { }
    [Serializable] public class ObjectEvent : UnityEvent<UnityEngine.Object> { }
    [Serializable] public class SpriteEvent : UnityEvent<Sprite> { }
    [Serializable] public class MaterialEvent : UnityEvent<Material> { }
    [Serializable] public class AudioClipEvent : UnityEvent<AudioClip> { }
    [Serializable] public class GameObjectEvent : UnityEvent<GameObject> { }
    [Serializable] public class TransformEvent : UnityEvent<Transform> { }
    [Serializable] public class ComponentEvent : UnityEvent<Component> { }

    [Serializable] public class BooleanListEvent : UnityEvent<List<bool>> { }
    [Serializable] public class IntegerListEvent : UnityEvent<List<int>> { }
    [Serializable] public class FloatListEvent : UnityEvent<List<float>> { }
    [Serializable] public class CharListEvent : UnityEvent<List<char>> { }
    [Serializable] public class StringListEvent : UnityEvent<List<string>> { }
    [Serializable] public class Vector2ListEvent : UnityEvent<List<Vector2>> { }
    [Serializable] public class Vector2IntListEvent : UnityEvent<List<Vector2Int>> { }
    [Serializable] public class Vector3ListEvent : UnityEvent<List<Vector3>> { }
    [Serializable] public class Vector3IntListEvent : UnityEvent<List<Vector3Int>> { }
    [Serializable] public class Vector4ListEvent : UnityEvent<List<Vector4>> { }
    [Serializable] public class RectListEvent : UnityEvent<List<Rect>> { }
    [Serializable] public class RectIntListEvent : UnityEvent<List<RectInt>> { }
    [Serializable] public class QuaternionListEvent : UnityEvent<List<Quaternion>> { }
    [Serializable] public class ColorListEvent : UnityEvent<List<Color>> { }
    [Serializable] public class ObjectListEvent : UnityEvent<List<UnityEngine.Object>> { }
    [Serializable] public class SpriteListEvent : UnityEvent<List<Sprite>> { }
    [Serializable] public class MaterialListEvent : UnityEvent<List<Material>> { }
    [Serializable] public class AudioClipListEvent : UnityEvent<List<AudioClip>> { }
    [Serializable] public class GameObjectListEvent : UnityEvent<List<GameObject>> { }
    [Serializable] public class TransformListEvent : UnityEvent<List<Transform>> { }
    [Serializable] public class ComponentListEvent : UnityEvent<List<Component>> { }

}
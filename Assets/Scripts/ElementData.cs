using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementData", menuName = "Scriptable Objects/ElementData")]
public class ElementData : ScriptableObject {

    public string displayName;
    public Sprite sprite;
    public string tag;
    public ElementBehaviour prefab;

}

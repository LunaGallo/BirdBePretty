using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementData", menuName = "Scriptable Objects/ElementData")]
public class ElementData : ScriptableObject {

    public string displayName;
    public string displayDescription;
    public Sprite sprite;
    public List<string> tags;
    public ElementBehaviour prefab;

}

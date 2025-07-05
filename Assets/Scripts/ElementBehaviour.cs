using LunaLib;
using Shapes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElementBehaviour : SingletonGroup<ElementBehaviour> {

    public Vector2Int gridSize = Vector2Int.one;
    public Rectangle rectagleGizmo;
    public SpriteRenderer spriteRenderer;
    public float ghostAlpha = 0.5f;
    public ElementData data;

    public bool IsFixed { get; set; } = false;
    public IEnumerable<ElementBehaviour> AllFixed => InstanceList.Where(i => i.IsFixed);

}

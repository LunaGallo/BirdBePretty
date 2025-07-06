using LunaLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundTile : SingletonGroup<GroundTile> {

    public static bool AnyAt(Vector3 tilePos) => EnabledList.Any(i => i.TilePos == tilePos);

    public GameObject grassSprite;
    public bool ShowingGrassSprite {
        get => grassSprite != null && grassSprite.activeSelf;
        set {
            if (grassSprite != null) {
                grassSprite.SetActive(value);
            }
        }
    }

    public GameObject gridRect;
    private static bool showingGrid = true;
    public static bool ShowingGrid {
        get => showingGrid;
        set {
            showingGrid = value;
            InstanceList.ForEach(i => i.ShowingGridRect = value);
        }
    }
    public bool ShowingGridRect {
        get => gridRect.activeSelf;
        set => gridRect.SetActive(value); 
    }

    public List<RendererMaterialExposer> renderers;
    public float Alpha { 
        set => renderers.ForEach(r => r.MaterialReference.color = r.MaterialReference.color.WithAlfa(value));
    }

    protected override void OnEnable() {
        base.OnEnable();
        ShowingGridRect = ShowingGrid;
    }

    public Vector3 TilePos => transform.position;

    private void Update() {
        ShowingGrassSprite = !ElementBehaviour.IsTileOcupied(TilePos);
    }

}

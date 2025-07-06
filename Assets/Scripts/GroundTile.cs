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

    public string detectableLayer;
    public string nonDetectableLayer;
    public bool Detectable { 
        set => gameObject.layer = value ? LayerMask.NameToLayer(detectableLayer) : LayerMask.NameToLayer(nonDetectableLayer); 
    }

    protected override void OnEnable() {
        base.OnEnable();
        ShowingGridRect = ShowingGrid;
    }

    public Vector3 TilePos => transform.position.Rounded();

    private void Update() {
        ShowingGrassSprite = !ElementBehaviour.IsTileOcupied(TilePos);
    }

    public static GroundTile TileAt(Vector3 tilePos) => EnabledList.Find(i => i.TilePos == tilePos);

    public float heightOffset = 0f;
    public string terrainType;

}

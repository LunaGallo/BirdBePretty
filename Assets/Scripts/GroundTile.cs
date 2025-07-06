using LunaLib;
using System;
using System.Linq;
using UnityEngine;

public class GroundTile : SingletonGroup<GroundTile> {

    public static bool AnyAt(Vector3 tilePos) => EnabledList.Any(i => i.TilePos == tilePos);

    public GameObject grassSprite;
    public bool ShowingGrassSprite {
        get => grassSprite.activeSelf;
        set => grassSprite.SetActive(value);
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

    protected override void OnEnable() {
        base.OnEnable();
        ShowingGridRect = ShowingGrid;
    }

    public Vector3 TilePos => transform.position;

    private void Update() {
        ShowingGrassSprite = !ElementBehaviour.IsTileOcupied(TilePos);
    }

}

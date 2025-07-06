using LunaLib;
using System.Linq;
using UnityEngine;

public class GroundTile : SingletonGroup<GroundTile> {

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

}

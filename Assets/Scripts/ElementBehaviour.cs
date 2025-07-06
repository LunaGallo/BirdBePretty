using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LunaLib;
using Shapes;

public class ElementBehaviour : SingletonGroup<ElementBehaviour> {

    public enum Type {
        Object,
        TileGroup
    }
    public Type type;
    public Transform objectRoot;
    public Vector2Int gridSize = Vector2Int.one;
    public SpriteRenderer spriteRenderer;
    public List<GroundTile> tiles;
    public float ghostAlpha = 0.5f;
    public ElementData data;
    public List<string> compatibleTerrains;
    public Rectangle rectangleGizmo;

    public bool IsFlipped {
        get {
            if (spriteRenderer != null) {
                return spriteRenderer.flipX;
            }
            return transform.localRotation.eulerAngles.y != 0;
        }
        set {
            if (spriteRenderer != null) {
                spriteRenderer.flipX = value;
            }
            else {
                transform.localRotation = Quaternion.Euler(
                    transform.localRotation.x, 
                    IsFlipped ? 0f : 90f, 
                    transform.localRotation.z);
            }
        }
    }
    public void ToggleFlipped() => IsFlipped = !IsFlipped;

    public bool IsBeingGrabbed => GameController.Instance.GrabbedElement == this;
    public static IEnumerable<ElementBehaviour> AllFixed => EnabledList.Where(i => !i.IsBeingGrabbed);

    public bool OccupiesTile(Vector3 tilePos) {
        Vector3 localPos = transform.InverseTransformPoint(tilePos);
        return type == Type.Object && localPos.x < gridSize.x && localPos.x >= 0f && localPos.z < gridSize.y && localPos.z >= 0f;
    }
    public static bool IsTileOcupied(Vector3 tilePos) => AllFixed.Any(i => i.OccupiesTile(tilePos));
    public static ElementBehaviour FindOnTile(GroundTile tile) => AllFixed.Find(i => i.OccupiesTile(tile.TilePos));

    public IEnumerable<Vector3> TilesWithin {
        get {
            for (int i = 0; i < gridSize.x; i++) {
                for (int j = 0; j < gridSize.y; j++) {
                    yield return transform.position + transform.right * i + transform.forward * j;
                }
            }
            foreach (GroundTile tile in tiles) {
                yield return tile.TilePos;
            }
        }
    }
    public bool FitsThere() {
        return TilesWithin.All(t => 
        Environment.Current.IsTileWithinLimits(t) && 
        !IsTileOcupied(t) && 
        Environment.Current.ElementTileCount(t) <= 1 &&
        compatibleTerrains.Contains(GroundTile.TileAt(t).terrainType));
    }

    public float ObjectRootHeight {
        set {
            objectRoot.localPosition = Vector3.up * value;
        }
    }

    public Vector3 PivotOffset {
        get {
            if (type == Type.Object) {
                return Vector3.right * (gridSize.x - 1) / 2f + Vector3.forward * (gridSize.y - 1) / 2f;
            }
            return Vector3.zero;
        }
    }

    public void PositionFloating(Vector3 point) {
        transform.position = point;
    }
    public void PositionOverTile(GroundTile tile) {
        transform.position = tile.TilePos;
        if (type == Type.Object) {
            ObjectRootHeight = tile.heightOffset;
        }
    }


    public void Update() {
        if (rectangleGizmo != null) {
            rectangleGizmo.gameObject.SetActive(IsBeingGrabbed);
            rectangleGizmo.transform.localPosition = PivotOffset;
            rectangleGizmo.Width = gridSize.x;
            rectangleGizmo.Height = gridSize.y;
        }
        if (spriteRenderer != null) {
            spriteRenderer.color = spriteRenderer.color.WithAlfa(IsBeingGrabbed ? ghostAlpha : 1f);
        }
        tiles.ForEach(r => r.Alpha = IsBeingGrabbed ? ghostAlpha : 1f);
        tiles.ForEach(r => r.Detectable = !IsBeingGrabbed);
    }

}

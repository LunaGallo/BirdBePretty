using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Shapes;
using LunaLib;

public class ElementBehaviour : SingletonGroup<ElementBehaviour> {

    public Vector2Int gridSize = Vector2Int.one;
    public Rectangle rectagleGizmo;
    public SpriteRenderer spriteRenderer;
    public float ghostAlpha = 0.5f;
    public ElementData data;
    public Animator feedbackAnimator;
    public string errorFeedbackAnimatorTrigger;

    public bool IsFlipped { 
        get => spriteRenderer.flipX; 
        set => spriteRenderer.flipX = value; 
    }
    public void ToggleFlipped() => IsFlipped = !IsFlipped;

    public bool IsBeingGrabbed => GameController.Instance.GrabbedElement == this;
    public static IEnumerable<ElementBehaviour> AllFixed => EnabledList.Where(i => !i.IsBeingGrabbed);

    public bool OccupiesTile(Vector3 tilePos) {
        Vector3 localPos = transform.InverseTransformPoint(tilePos);
        return localPos.x < gridSize.x && localPos.x >= 0f && localPos.z < gridSize.y && localPos.z >= 0f;
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
        }
    }
    public bool FitsThere() => TilesWithin.All(t => GroundTile.AnyAt(t) && !IsTileOcupied(t));

    public void Update() {
        spriteRenderer.color = spriteRenderer.color.WithAlfa(IsBeingGrabbed ? ghostAlpha : 1f);
    }

    public void ShowErrorFeedback() {
        if (feedbackAnimator) {
            feedbackAnimator.SetTrigger(errorFeedbackAnimatorTrigger);
        }
    }

}

using System.Linq;
using UnityEngine;
using LunaLib;

[RequireComponent(typeof(RectTransform))]
public class InputBlockingRect : SingletonGroup<InputBlockingRect> {

    public RectTransform RectTransform => (RectTransform) transform;
    public bool ContainsScreenPoint(Vector2 screenPoint) => RectTransformUtility.RectangleContainsScreenPoint(RectTransform, screenPoint);
    public bool IsActive => gameObject.activeInHierarchy;
    public bool IsBlockingScreenPoint(Vector2 screenPoint) => IsActive && ContainsScreenPoint(screenPoint);

    public static bool AnyAreBlockingScreenPoint(Vector2 screenPoint) => EnabledList.Any(b => b.IsBlockingScreenPoint(screenPoint));

}

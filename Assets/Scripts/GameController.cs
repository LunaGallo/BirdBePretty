using LunaLib;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController> {

    public List<Environment> environments;
    public Transform cameraPivot;
    public float cameraSensitivity = 1f;
    public AudioSource takeAudioSource;
    public AudioSource putAudioSource;

    private static Plane groundPlane = new(Vector3.up, 0f);

    private int currentEnvironmentIndex = 0;
    public Environment CurrentEnvironment => environments[currentEnvironmentIndex];
    public ElementBehaviour GrabbedElement { get; set; }
    public bool IsGrabbing => GrabbedElement != null;
    private bool isRotatingCamera = false;

    public static bool IsMouseOverUI => InputBlockingRect.AnyAreBlockingScreenPoint(Input.mousePosition);

    private void Update() {
        bool isHoveringTile = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        groundPlane.Raycast(ray, out float enter);
        Vector3 hitPoint = ray.origin + ray.direction * enter;
        if (IsGrabbing) {
            hitPoint -= GrabbedElement.PivotOffset;
        }
        hitPoint = hitPoint.Rounded();
        GroundTile defaultHitTile = CurrentEnvironment.DefaultTileAt(hitPoint);
        GroundTile totalHitTile = GroundTile.TileAt(hitPoint);
        if (!IsMouseOverUI) {
            if (Input.GetMouseButtonDown(0) && totalHitTile != null && !IsGrabbing) {
                ElementBehaviour hitElement = totalHitTile.GetComponentInParent<ElementBehaviour>();
                if (hitElement != null) {
                    GrabElement(hitElement);
                }
            }
            if (defaultHitTile != null) {
                isHoveringTile = true;
                if (IsGrabbing) {
                    GrabbedElement.PositionOverTile(defaultHitTile);
                    if (Input.GetMouseButtonDown(0) && GrabbedElement.FitsThere()) {
                        StopGrabbingElement();
                    }
                }
                else {
                    ElementBehaviour hitElement = ElementBehaviour.FindOnTile(defaultHitTile);
                    if (hitElement != null && Input.GetMouseButtonDown(0)) {
                        GrabElement(hitElement);
                    }
                }
            }
            if (!isHoveringTile && Input.GetMouseButtonDown(0)) {
                isRotatingCamera = true;
            }
            if (IsGrabbing && Input.GetMouseButtonDown(1)) {
                GrabbedElement.ToggleFlipped();
            }
        }
        if (!isHoveringTile && IsGrabbing) {
            GrabbedElement.PositionFloating(hitPoint);
            if (Input.GetMouseButtonDown(0)) {
                DeleteGrabbedElement();
            }
        }
        if (Input.GetMouseButtonUp(0)) {
            isRotatingCamera = false;
        }
        if (isRotatingCamera) {
            cameraPivot.localRotation = Quaternion.Euler(
                cameraPivot.localRotation.eulerAngles.x,
                cameraPivot.localRotation.eulerAngles.y + Input.mousePositionDelta.x * cameraSensitivity,
                cameraPivot.localRotation.eulerAngles.z);
        }
        GroundTile.ShowingGrid = IsGrabbing;
    }

    public void CreateElementAndGrab(ElementData elementData) {
        if (GrabbedElement != null) {
            DeleteGrabbedElement();
        }
        GrabElement(Instantiate(elementData.prefab, CurrentEnvironment.elementContainer));
    }
    public void GrabElement(ElementBehaviour elementBehaviour) {
        GrabbedElement = elementBehaviour;
        takeAudioSource.Play();
    }

    public void DeleteGrabbedElement() {
        Destroy(GrabbedElement.gameObject);
        StopGrabbingElement();
    }
    public void StopGrabbingElement() {
        GrabbedElement = null;
        putAudioSource.Play();
    }

    public void GoToNextEnvironment() {
        currentEnvironmentIndex = (currentEnvironmentIndex + 1) % environments.Count;
        ApplyEnvironmentActivation();
    }
    public void GoToPreviousEnvironment() {
        currentEnvironmentIndex--;
        if (currentEnvironmentIndex < 0) {
            currentEnvironmentIndex = environments.Count - 1;
        }
        ApplyEnvironmentActivation();
    }
    public void ApplyEnvironmentActivation() {
        for (int i = 0; i < environments.Count; i++) {
            environments[i].gameObject.SetActive(i == currentEnvironmentIndex);
        }
    }

}

using LunaLib;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController> {

    public List<Environment> environments;
    public float inputRaycastMaxDist = 20f;
    public LayerMask inputRaycastLayerMask;
    public Transform cameraPivot;
    public float cameraSensitivity = 1f;

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
        if (!IsMouseOverUI) {
            if (Physics.Raycast(ray, out RaycastHit hitInfo, inputRaycastMaxDist, inputRaycastLayerMask)) {
                if (hitInfo.collider.TryGetComponent<GroundTile>(out var hitTile)) {
                    isHoveringTile = true;
                    if (IsGrabbing) {
                        GrabbedElement.transform.position = hitTile.transform.position;
                        if (Input.GetMouseButtonDown(0)) {
                            if (GrabbedElement.FitsThere()) {
                                StopGrabbingElement();
                            }
                        }
                    }
                    else {
                        ElementBehaviour hitElement = ElementBehaviour.FindOnTile(hitTile);
                        if (hitElement != null && !IsGrabbing && Input.GetMouseButtonDown(0)) {
                            GrabElement(hitElement);
                        }
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
            if (groundPlane.Raycast(ray, out float enter)) {
                GrabbedElement.transform.position = ray.origin + ray.direction * enter;
            }
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
    public void GrabElement(ElementBehaviour elementBehaviour) => GrabbedElement = elementBehaviour;
    public void DeleteGrabbedElement() {
        Destroy(GrabbedElement.gameObject);
        StopGrabbingElement();
    }
    public void StopGrabbingElement() => GrabbedElement = null;

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

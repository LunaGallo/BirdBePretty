using LunaLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using UnityEngine.Rendering;

public class Environment : MonoBehaviour {

    public static Environment Current => GameController.Instance.CurrentEnvironment;

    public Transform tileContainer;
    public Transform elementContainer;
    public SerializableComponentPool<Transform> birdPool;
    public List<Transform> birdPoints;
    public GameObject uiDot;
    public GameObject uiSliderContainers;
    public GameObject uiCheckmark;
    public Slider elementCountSlider;
    public TMP_Text elementCountText;
    public int elementCountGoal = 3;
    public Slider happynessCountSlider;
    public TMP_Text happynessCountText;
    public int happynessCountGoal = 2;

    [Serializable]
    public class Likes {
        public string tag;
        public int intensity;
    }
    public List<Likes> likes;

    public int ElementCount { get; set; } = 0;
    public int HappynessCount { get; set; } = 0;

    public bool IsComplete => ElementCount == elementCountGoal && HappynessCount == happynessCountGoal;

    public void SetBirdCount(int birdCount) {
        birdPool.SetActiveCount(birdCount);
        for (int i = 0; i < birdCount; i++) {
            birdPool.ActiveElements[i].transform.position = birdPoints[i%birdPoints.Count].position;
        }
    }

    private void OnEnable() {
        uiDot.SetActive(false);
        uiSliderContainers.SetActive(true);
        uiCheckmark.SetActive(false);
        ApplyState();
    }
    private void OnDisable() {
        uiDot.SetActive(!IsComplete);
        uiSliderContainers.SetActive(false);
        uiCheckmark.SetActive(IsComplete);
    }

    public void ApplyState() {
        elementCountSlider.maxValue = elementCountGoal;
        elementCountSlider.minValue = 0;

        happynessCountSlider.maxValue = happynessCountGoal;
        happynessCountSlider.minValue = 0;
        UpdateUIValues();
    }
    public void UpdateUIValues() {
        elementCountSlider.value = ElementCount;
        elementCountText.text = ElementCount + " / " + elementCountGoal;

        happynessCountSlider.value = HappynessCount;
        happynessCountText.text = HappynessCount + " / " + happynessCountGoal;
        SetBirdCount(HappynessCount + 1);
    }

    private void Update() {
        ElementBehaviour[] childElements = elementContainer.GetComponentsInChildren<ElementBehaviour>().Where(b => !b.IsBeingGrabbed).ToArray();
        ElementCount = Mathf.Clamp(childElements.Length,0,elementCountGoal);
        IEnumerable<ElementData> elementDatas = childElements.Select(b => b.data).WithoutDoubles();
        HappynessCount = Mathf.Clamp(
            elementDatas.Where(d => likes.Any(l => l.tag == d.tag)).Select(d => likes.Find(l => l.tag == d.tag)).Sum(l => l.intensity),
            0, happynessCountGoal);
        UpdateUIValues();

        List<GroundTile> defaultTiles = new(tileContainer.GetComponentsInChildren<GroundTile>(true));
        List<GroundTile> elementTiles = new(elementContainer.GetComponentsInChildren<GroundTile>());
        foreach(GroundTile tile in defaultTiles) {
            tile.gameObject.SetActive(elementTiles.All(e => e.TilePos != tile.TilePos));
        }
    }

    public bool IsTileWithinLimits(Vector3 tilePos) {
        return tileContainer.GetComponentsInChildren<GroundTile>(true).Any(t => t.TilePos == tilePos);
    }
    public int ElementTileCount(Vector3 tilePos) {
        return elementContainer.GetComponentsInChildren<GroundTile>(true).Count(t => t.TilePos == tilePos);
    }
    public GroundTile DefaultTileAt(Vector3 tilePos) {
        return tileContainer.GetComponentsInChildren<GroundTile>(true).Find(t => t.TilePos == tilePos);
    }

}

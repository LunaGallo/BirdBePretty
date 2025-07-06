using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElementUI : MonoBehaviour {

    public Image image;
    public TMP_Text text;

    private ElementData data;
    public void Configure(ElementData data) {
        this.data = data;
        image.sprite = data.sprite;
        text.text = data.displayName;
    }

    public void ButtonClicked() {
        GameController.Instance.CreateElementAndGrab(data);
    }

}

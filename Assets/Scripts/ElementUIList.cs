using LunaLib;
using UnityEngine;

public class ElementUIList : MonoBehaviour {

    public ElementDataLibrary elementDataLibrary;
    public SerializableComponentPool<ElementUI> elementUIPool;

    private void OnEnable() {
        ApplyList();
    }

    private void ApplyList() {
        elementUIPool.ReturnAll();
        foreach (ElementData data in elementDataLibrary.elements) {
            elementUIPool.GetInstance().Configure(data);
        }
        elementUIPool.container.ShuffleChildrenOrder();
    }

}

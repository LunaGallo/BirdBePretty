using UnityEngine;

public class RendererMaterialExposer : MonoBehaviour {

    public Renderer rendererReference;
    public Renderer RendererReference {
        get {
            if (rendererReference == null) {
                rendererReference = GetComponent<Renderer>();
            }
            return rendererReference;
        }
    }

    private Material materialReference;
    public Material MaterialReference {
        get {
            if (materialReference == null) {
                materialReference = RendererReference.material;
            }
            return materialReference;
        }
    }

}

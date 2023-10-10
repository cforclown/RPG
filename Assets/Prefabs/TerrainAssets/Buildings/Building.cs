using UnityEngine;

public class Building : MonoBehaviour {
  [SerializeField] private Renderer meshRenderer;
  private bool transparent = false;

  public void SetTransparent(bool _transparent) {
    if (transparent == _transparent) {
      return;
    }

    transparent = _transparent;

    Color currentColor = meshRenderer.material.color;
    if (transparent) {
      currentColor.a = 0.3f;
    }
    else {
      //Change the alpha of the color
      currentColor.a = 1.0f;
    }
    meshRenderer.material.color = currentColor;
  }
}

using TMPro;
using UnityEngine;

public class HitDamageText : MonoBehaviour {
  [SerializeField] private TextMeshProUGUI textCompoent;
  private Transform posRef;
  private RectTransform rectTransform;
  void Awake() {
    textCompoent.text = "";

    rectTransform = GetComponent<RectTransform>();
  }

  void Update() {
    if (posRef != null) {
      Vector2 pos = Camera.main.WorldToViewportPoint(posRef.position);
      rectTransform.anchoredPosition = new Vector3(
        GameUIManager.I.GetRectTransform().sizeDelta.x * pos.x,
        GameUIManager.I.GetRectTransform().sizeDelta.y * pos.y + 100f,
        0f
      );
    }
  }

  public void Init(int damage, Transform posRef = null) {
    textCompoent.text = damage.ToString();
    if (posRef != null && GameUIManager.I) {
      this.posRef = posRef;
      Vector2 pos = Camera.main.WorldToViewportPoint(posRef.position);
      rectTransform.anchoredPosition = new Vector3(
        GameUIManager.I.GetRectTransform().sizeDelta.x * pos.x,
        GameUIManager.I.GetRectTransform().sizeDelta.y * pos.y + 100f,
        0f
      );
    }
    else {
      rectTransform.localPosition = new Vector2(0f, 80f);
    }
  }

  private void Done() {
    Destroy(gameObject);
  }
}

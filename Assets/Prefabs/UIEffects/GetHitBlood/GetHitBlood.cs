using UnityEngine;
using UnityEngine.UI;

public class GetHitBlood : MonoBehaviour {
  private RectTransform rectTransform;
  private Image img;

  void Awake() {
    rectTransform = GetComponent<RectTransform>();
    img = GetComponent<Image>();
  }

  void Start() {
    AnchorUtils.SetRight(rectTransform, -Generator.RandomFloat(25, 100));
    AnchorUtils.SetLeft(rectTransform, -Generator.RandomFloat(25, 100));
    AnchorUtils.SetTop(rectTransform, -Generator.RandomFloat(25, 100));
    AnchorUtils.SetBottom(rectTransform, -Generator.RandomFloat(25, 100));
  }

  // called on animator event
  private void Done() {
    Destroy(gameObject);
  }
}

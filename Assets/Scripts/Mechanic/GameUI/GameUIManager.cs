using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum GameUIStates {
  NONE = 0,
  EXPLORING = 1,
  PAUSED = 2
}

public class GameUIManager : MonoBehaviour {
  public static GameUIManager I;

  public Canvas Canvas { get; private set; }

  private Animator animator;
  private int isHideHash;
  private bool isHide = false;
  public bool IsHide {
    get { return isHide; }
    private set {
      isHide = value;
      animator.SetBool(isHideHash, value);
    }
  }

  [SerializeField] public Transform GetHitIndicatorContainer;
  [SerializeField] private GameObject GitHitBloodPrefab;

  [SerializeField] private Image screenDoorImg;

  private void Awake() {
    I = this;
  }

  private void Start() {
    animator = GetComponent<Animator>();
    isHideHash = Animator.StringToHash("IsHide");

    Canvas = GetComponent<Canvas>();
    Canvas.renderMode = RenderMode.ScreenSpaceCamera;
    Canvas.planeDistance = 0.5f;
    Canvas.worldCamera = Camera.main;

    CombatEvents.OnEnemyHitPlayer += (Enemy enemy, Character player) => DisplayGetHitIndicator();
    NPCEvents.OnNPCInteractionStarted += (NPC npc) => { IsHide = true; };
    NPCEvents.OnNPCInteractionEnded += (NPC npc) => { IsHide = false; };
  }

  public RectTransform GetRectTransform() {
    return (RectTransform)transform;
  }

  public Canvas GetCanvas() {
    return Canvas;
  }

  private void DisplayGetHitIndicator() {
    Instantiate(GitHitBloodPrefab, GetHitIndicatorContainer);
  }

  public void OpenScreen() {
    if (screenDoorImg == null) {
      return;
    }

    screenDoorImg.color = new Color(0f, 0f, 0f, 1f);
    screenDoorImg.gameObject.SetActive(true);
    StartCoroutine(OpenScreenAnim());
  }

  public void CloseScreen(bool animate = true) {
    if (screenDoorImg == null) {
      return;
    }

    screenDoorImg.gameObject.SetActive(true);
    if (animate) {
      StartCoroutine(CloseScreenAnim());
    }
    else {
      screenDoorImg.color = new Color(0f, 0f, 0f, 1f);
    }
  }

  private IEnumerator OpenScreenAnim() {
    screenDoorImg.CrossFadeAlpha(0f, 2f, false);
    yield return new WaitForSeconds(1.5f);
    screenDoorImg.gameObject.SetActive(false);
  }

  private IEnumerator CloseScreenAnim() {
    screenDoorImg.CrossFadeAlpha(1, 0.1f, false);
    screenDoorImg.color = new Color(0f, 0f, 0f, 0f);
    while (screenDoorImg.color.a < 1f) {
      float currentAplha = screenDoorImg.color.a + (1f * Time.deltaTime);
      screenDoorImg.color = new Color(0f, 0f, 0f, currentAplha);
      yield return new WaitForSeconds(0.0001f);
    }
    screenDoorImg.color = new Color(0f, 0f, 0f, 1f);
  }
}

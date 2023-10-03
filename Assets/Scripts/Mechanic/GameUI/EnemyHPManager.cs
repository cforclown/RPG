using TMPro;
using UnityEngine;

public class EnemyHPManager : MonoBehaviour {
  [SerializeField] private GameObject container;
  [SerializeField] private RectTransform enemyHpBar;
  [SerializeField] private TextMeshProUGUI enemyNameText;

  private RectTransform rt;
  private float? containerWidth = null;

  private bool isShow = false;
  private float showTimeCounter = 0f;
  private const float showTimeMax = 5f;

  private void Awake() {
    rt = GetComponent<RectTransform>();
  }

  private void Start() {
    if (GameUIManager.I != null) {
      containerWidth = RectTransformUtility.PixelAdjustRect(rt, GameUIManager.I.GetCanvas()).width;
    }

    CombatEvents.OnEnemyHitPlayer += (Enemy enemy, Character player) => {
      // if already showed, that mean the hp bar is for enemy that player hit
      // if we call function Show(), it will replaced hp bar for enemy that hit the player, we dont want that
      // because we prioritize hp bar for enemy that player hit
      if (isShow) {
        return;
      }

      Show(enemy);
    };
    CombatEvents.OnPlayerHitEnemy += Show;

    Hide();
  }

  private void Update() {
    if (containerWidth == null) {
      containerWidth = RectTransformUtility.PixelAdjustRect(rt, GameUIManager.I.GetCanvas()).width;
    }

    if (isShow && showTimeCounter > showTimeMax) {
      Hide();
    }
    else {
      showTimeCounter += Time.deltaTime;
    }
  }

  private void Show(Enemy enemy) {
    Set(enemy.Stats);

    if (enemy.Stats.HP <= 0) {
      Hide();
      return;
    }

    isShow = true;
    showTimeCounter = 0f;
    container.SetActive(true);
  }

  private void Hide() {
    isShow = false;
    container.SetActive(false);
  }

  private void Set(EnemySO stats) {
    AnchorUtils.SetRight(enemyHpBar, (((float)(stats.MaxHP - stats.HP) / (float)stats.MaxHP) * containerWidth.Value));
    enemyNameText.text = stats.Name;
  }
}

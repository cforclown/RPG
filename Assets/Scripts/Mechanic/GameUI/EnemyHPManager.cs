using TMPro;
using UnityEngine;

public class EnemyHPManager : MonoBehaviour {
  [SerializeField] private GameObject container;
  [SerializeField] private RectTransform enemyHpBar;
  [SerializeField] private TextMeshProUGUI enemyNameText;

  private RectTransform rt;
  private float? containerWidth = null;

  private bool IsShow = false;
  private float showTimeCounter = 0f;
  private const float showTimeMax = 5f;

  private void Awake() {
    rt = GetComponent<RectTransform>();
  }

  private void Start() {
    if (GameUIManager.I != null) {
      containerWidth = RectTransformUtility.PixelAdjustRect(rt, GameUIManager.I.GetCanvas()).width;
    }

    CombatEvents.OnPostEnemyAttackHitPlayer += OnPostEnemyAttackHitPlayerEvent;
    CombatEvents.OnPostPlayerAttackHitEnemy += OnPostPlayerAttackHitEnemyEvent;

    Hide();
  }

  private void OnDestroy() {
    CombatEvents.OnPostEnemyAttackHitPlayer -= OnPostEnemyAttackHitPlayerEvent;
    CombatEvents.OnPostPlayerAttackHitEnemy -= OnPostPlayerAttackHitEnemyEvent;
  }

  private void Update() {
    if (containerWidth == null) {
      containerWidth = RectTransformUtility.PixelAdjustRect(rt, GameUIManager.I.GetCanvas()).width;
    }

    if (IsShow && showTimeCounter > showTimeMax) {
      Hide();
    }
    else {
      showTimeCounter += Time.deltaTime;
    }
  }

  private void OnPostEnemyAttackHitPlayerEvent(CharacterManager player, Enemy enemy, WeaponSO weapon) {
    // if already showed, that means the hp bar is for enemy that player hit
    // if we call function Show(), it will replaced hp bar for enemy that hit the player, we dont want that
    // because we prioritize hp bar for enemy that player hit
    if (IsShow) {
      return;
    }

    Show(enemy);
  }

  private void OnPostPlayerAttackHitEnemyEvent(CharacterManager player, Enemy enemy, WeaponSO weapon) => Show(enemy);

  private void Show(Enemy enemy) {
    Set(enemy.Stats);

    if (enemy.Stats.HP <= 0) {
      Hide();
      return;
    }

    IsShow = true;
    showTimeCounter = 0f;
    container.SetActive(true);
  }

  private void Hide() {
    IsShow = false;
    container.SetActive(false);
  }

  private void Set(EnemySO stats) {
    AnchorUtils.SetRight(enemyHpBar, (((float)(stats.MaxHP - stats.HP) / (float)stats.MaxHP) * containerWidth.Value));
    enemyNameText.text = stats.Name;
  }
}

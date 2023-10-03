using TMPro;
using UnityEngine;

public class HP_MP_EXP_Manager : MonoBehaviour {
  [SerializeField] private RectTransform hpBar;
  [SerializeField] private TextMeshProUGUI hpBarText;
  [SerializeField] private RectTransform mpBar;
  [SerializeField] private TextMeshProUGUI mpBarText;
  [SerializeField] private RectTransform expBar;

  private RectTransform rt;
  private float? containerWidth = null;

  private void Awake() {
    rt = GetComponent<RectTransform>();
  }

  private void Start() {
    if (GameUIManager.I != null) {
      containerWidth = RectTransformUtility.PixelAdjustRect(rt, GameUIManager.I.GetCanvas()).width;
    }

    CombatEvents.OnEnemyHitPlayer += (Enemy enemy, Character player) => EvaluateBars(player);
    PlayerEvents.OnPlayerStatsUpdated += EvaluateBars;
  }

  private void Update() {
    if (containerWidth == null) {
      containerWidth = RectTransformUtility.PixelAdjustRect(rt, GameUIManager.I.GetCanvas()).width;
    }
  }

  private void EvaluateBars(Character player) {
    AnchorUtils.SetRight(hpBar, (((float)(player.MaxHP - player.HP) / (float)player.MaxHP) * containerWidth.Value));
    hpBarText.text = string.Format("{0} / {1}", player.HP, player.MaxHP);

    AnchorUtils.SetRight(mpBar, (((float)(player.MaxMP - player.MP) / (float)player.MaxMP) * containerWidth.Value));

    int minCurrentLevelExp = ExpService.GetCurrentLevelMinExp(player.Level);
    int maxCurrentLevelExp = ExpService.GetNextLevelExpBreakpoint(player.Level);
    float expProgress = (player.Exp < minCurrentLevelExp ? 0f : (float)(player.Exp - minCurrentLevelExp)) / (float)(maxCurrentLevelExp - minCurrentLevelExp);
    AnchorUtils.SetRight(expBar, (1f - expProgress) * containerWidth.Value);
  }
}

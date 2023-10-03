using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour {
  private Button btnComponent;

  [SerializeField] private TextMeshProUGUI skillLevelText;
  [SerializeField] private Image btnBgImg;
  [SerializeField] private Image skillIconImg;

  private bool claimed = false;

  private Color unclaimedColor = new Color(0.78f, 0.78f, 0.78f);
  private Color claimedColor = new Color(1f, 0.7f, 0.49f);

  [SerializeField] SkillSO skill;

  private void Awake() {
    btnComponent = GetComponent<Button>();

    skillLevelText.gameObject.SetActive(false);

    btnComponent.onClick.AddListener(() => {
      if (!claimed) {
        ClaimSkill();
      }
      else {
        LevelUpSkill();
      }
    });
    btnBgImg.color = unclaimedColor;
  }

  public void Init(SkillSO skill) {
    this.skill = skill;
    if (skill.Level > 0) {
      claimed = true;
      btnBgImg.color = claimedColor;
    }
  }

  private void ClaimSkill() {
    claimed = true;
    skill.LevelUp();
    Evaluate();

    PlayerEvents.PlayerClaimSkill(skill);
  }

  private void LevelUpSkill() {
    skill.LevelUp();
    Evaluate();

    PlayerEvents.PlayerSkillLevelUp(skill);
  }

  private void Evaluate() {
    if (!claimed) {
      return;
    }

    skillLevelText.gameObject.SetActive(true);
    skillLevelText.text = string.Format("{0}/{1}", skill.Level, SkillSO.SKILL_MAX_LEVEL);
    skillLevelText.color = claimedColor;
  }

  public void Enable() => btnComponent.enabled = true;
  public void Disable() => btnComponent.enabled = false;

  // Pointer Enter event
  public void OnHover() => btnBgImg.color = claimedColor;

  // Pointer Exit event
  public void OnHoverEnd() {
    if (claimed) {
      return;
    }

    btnBgImg.color = unclaimedColor;
  }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour {
  public delegate void SkillBtnEventHandler(SkillSO skill);
  public static event SkillBtnEventHandler OnSkillBtnPressed;

  private Button btnComponent;
  [SerializeField] private TextMeshProUGUI skillLevelText;
  [SerializeField] private Image btnBgImg;
  [SerializeField] private Image skillIconImg;

  private bool claimed = false;

  private Color unclaimedColor = new Color(0.78f, 0.78f, 0.78f);
  private Color claimedColor = new Color(1f, 0.7f, 0.49f);

  [SerializeField] private SkillSO skillData;

  private void Awake() {
    btnComponent = GetComponent<Button>();

    skillLevelText.gameObject.SetActive(false);

    btnComponent.onClick.AddListener(() => {
      if (OnSkillBtnPressed == null) {
        return;
      }

      OnSkillBtnPressed(skillData);
    });
    btnBgImg.color = unclaimedColor;
    skillIconImg.sprite = skillData.Icon;

    PlayerEvents.OnPlayerSkillsUpdated += Evaluate;
  }

  private void Start() {
  }

  private void OnDestroy() {
    PlayerEvents.OnPlayerSkillsUpdated -= Evaluate;
  }

  private void Evaluate(PlayerSkills playerSkills) {
    SkillSO skill = playerSkills.Skills.Find(s => s.Id == skillData.Id);
    if (skill == null) {
      btnBgImg.color = unclaimedColor;
      claimed = false;
      return;
    }

    claimed = true;
    skillLevelText.gameObject.SetActive(true);
    skillLevelText.text = string.Format("{0}/{1}", skill.Level, SkillSO.SKILL_MAX_LEVEL);
    btnBgImg.color = claimedColor;

    if (skill.Level >= SkillSO.SKILL_MAX_LEVEL) {
      Disable();
    }
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

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillTriggerBtn : MonoBehaviour {
  private SkillSO skill;
  [SerializeField] private SkillTypes skillType;
  [SerializeField] private GameObject container;
  [SerializeField] private Button btnComponent;
  [SerializeField] private Image skillIcon;
  [SerializeField] private Image cdMaskImg;

  private bool IsCd = false;
  public bool IsEnabled { get; private set; }

  private void Awake() {
    PlayerEvents.OnPlayerSkillsUpdated += OnPlayerSkillsUpdatedEvent;
    btnComponent.onClick.AddListener(OnClick);
  }

  private void OnDestroy() {
    PlayerEvents.OnPlayerSkillsUpdated -= OnPlayerSkillsUpdatedEvent;
  }

  private void OnClick() {
    if (Player.I == null || !Player.I.AnimController.AttackMotionFinished || skill == null || IsCd || !IsEnabled) {
      return;
    }

    ControllerManager.SkillBtnPressed(skill);
    IsCd = true;
    btnComponent.enabled = false;
    StartCoroutine(CooldownAnim(skill.Cooldown));
  }

  private void Enable() {
    skillIcon.sprite = skill.Icon;
    container.SetActive(true);
    IsEnabled = true;
    IsCd = false;
  }

  private void Disable() {
    container.SetActive(false);
    IsEnabled = false;
    IsCd = true;
  }

  private void OnPlayerSkillsUpdatedEvent(PlayerSkills playerSkills) {
    skill = playerSkills.Skills.Find(s => s.SkillType == skillType);
    if (skill != null) {
      Enable();
    }
    else {
      Disable();
    }
  }

  private IEnumerator CooldownAnim(float duration) {
    float current = 0f;
    cdMaskImg.fillAmount = 1f;

    while (current < duration) {
      current += duration * Time.deltaTime;
      cdMaskImg.fillAmount = 1f - (current / duration);
      yield return new WaitForSeconds(0.0001f);
    }
    cdMaskImg.fillAmount = 0f;
    IsCd = false;
    btnComponent.enabled = true;
  }
}

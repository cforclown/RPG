using UnityEngine;
using UnityEngine.UI;

public class ControllerManager : MonoBehaviour {
  public delegate void ControllerEventHandler();
  public static event ControllerEventHandler OnBasicAttackPressed;
  public static event ControllerEventHandler OnSkill1BtnPressed;
  public static event ControllerEventHandler OnSkill2BtnPressed;
  public static event ControllerEventHandler OnSkill3BtnPressed;
  public static event ControllerEventHandler OnSkill4BtnPressed;

  [SerializeField] private Button basicAttackBtn;
  [SerializeField] private Button skill1Btn;
  [SerializeField] private Button skill2Btn;
  [SerializeField] private Button skill3Btn;
  [SerializeField] private Button skill4Btn;

  private void Awake() {
    PlayerEvents.OnPlayerSkillsUpdated += OnPlayerSkillsUpdated;
  }

  private void Start() {
    basicAttackBtn.onClick.AddListener(BasicAttack);

    skill1Btn.gameObject.SetActive(false);
    skill1Btn.onClick.AddListener(Skill1BtnPressed);

    skill2Btn.gameObject.SetActive(false);
    skill2Btn.onClick.AddListener(Skill2BtnPressed);

    skill3Btn.gameObject.SetActive(false);
    skill3Btn.onClick.AddListener(Skill3BtnPressed);

    skill4Btn.gameObject.SetActive(false);
    skill4Btn.onClick.AddListener(Skill4BtnPressed);
  }

  private void BasicAttack() {
    if (OnBasicAttackPressed == null) {
      return;
    }

    OnBasicAttackPressed();
  }

  private void OnPlayerSkillsUpdated(PlayerSkills playerSkills) {
    SkillSO skill = playerSkills.Skills.Find(s => s.SkillType == SkillTypes.FirstSkill);
    if (skill != null) {
      EnableSkill1();
    }

    skill = playerSkills.Skills.Find(s => s.SkillType == SkillTypes.SecondSkill);
    if (skill != null) {
      EnableSkill2();
    }

    skill = playerSkills.Skills.Find(s => s.SkillType == SkillTypes.ThirdSkill);
    if (skill != null) {
      EnableSkill3();
    }

    skill = playerSkills.Skills.Find(s => s.SkillType == SkillTypes.Ultimate);
    if (skill != null) {
      EnableSkill4();
    }
  }

  private void EnableSkill1() => skill1Btn.gameObject.SetActive(true);
  private void Skill1BtnPressed() {
    if (OnSkill1BtnPressed == null) {
      return;
    }

    OnSkill1BtnPressed();
  }

  private void EnableSkill2() => skill2Btn.gameObject.SetActive(true);
  private void Skill2BtnPressed() {
    if (OnSkill2BtnPressed == null) {
      return;
    }

    OnSkill2BtnPressed();
  }

  private void EnableSkill3() => skill3Btn.gameObject.SetActive(true);
  private void Skill3BtnPressed() {
    if (OnSkill3BtnPressed == null) {
      return;
    }

    OnSkill3BtnPressed();
  }

  private void EnableSkill4() => skill4Btn.gameObject.SetActive(true);
  private void Skill4BtnPressed() {
    if (OnSkill4BtnPressed == null) {
      return;
    }

    OnSkill4BtnPressed();
  }
}

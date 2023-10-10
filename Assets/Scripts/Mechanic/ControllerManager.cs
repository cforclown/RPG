using UnityEngine;
using UnityEngine.UI;

public class ControllerManager : MonoBehaviour {
  public delegate void ControllerEventHandler(SkillSO skill = null);
  public static event ControllerEventHandler OnBasicAttackPressed;
  public static event ControllerEventHandler OnSkillBtnPressed;

  [SerializeField] private Button basicAttackBtn;

  private void Start() {
    basicAttackBtn.onClick.AddListener(BasicAttack);
  }

  private void BasicAttack() {
    if (OnBasicAttackPressed == null) {
      return;
    }

    OnBasicAttackPressed(null);
  }

  public static void SkillBtnPressed(SkillSO? skill) {
    if (OnSkillBtnPressed == null) {
      return;
    }

    OnSkillBtnPressed(skill);
  }
}

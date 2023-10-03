using UnityEngine;
using UnityEngine.UI;

public class QuestGoalItem : MonoBehaviour {
  public static readonly float HEIGHT = 28f;

  [SerializeField] private Toggle toggler;
  [SerializeField] private Text togglerText;

  private void Awake() {
    if (toggler == null) {
      toggler = GetComponent<Toggle>();
    }
  }

  public void Set(QuestGoal questGoal) {
    if (toggler == null) {
      toggler = GetComponent<Toggle>();
    }
    toggler.isOn = questGoal.Completed;
    togglerText.text = questGoal.Desc;
  }

  public void SetValue(bool completed) {
    toggler.isOn = completed;
  }
}

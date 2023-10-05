using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanelManager : MonoBehaviour {
  [SerializeField] private GameObject container;
  [SerializeField] private Button popupBtn;
  [SerializeField] private Button closeBtn;

  [SerializeField] private TextMeshProUGUI jobText;

  public bool IsOpen { get; private set; }

  void Start() {
    popupBtn.onClick.AddListener(() => {
      if (IsOpen) {
        Close();
      }
      else {
        Open();
      }
    });
    closeBtn.onClick.AddListener(Close);

    Close();
  }

  private void Update() {
    if (Input.GetKeyUp(KeyCode.K)) {
      if (IsOpen) {
        Close();
      }
      else {
        Open();
      }
    }
  }

  private void Open() {
    if (Player.I == null) {
      return;
    }

    IsOpen = true;
    container.gameObject.SetActive(true);
    Init(Player.I.Character.Stats);
  }

  private void Close() {
    IsOpen = false;
    container.gameObject.SetActive(false);
  }

  private void Init(Character character) {
    jobText.text = string.Format("{0} {1}", GetJobName(character.Job), character.SkillPoint > 0 ? string.Format("({0})", character.SkillPoint) : "");
  }

  private string GetJobName(JobTypes job) {
    switch (job) {
      case JobTypes.Swordsman:
        return "Swordsman";
      case JobTypes.Knight:
        return "Knight";
      case JobTypes.Spellcaster:
        return "Spellcaster";
      default:
        return "Unknown";
    }
  }
}

using UnityEngine;
using UnityEngine.UI;

public class CharacterWindow : MonoBehaviour {
  [SerializeField] private Button popupBtn;
  [SerializeField] private Button closeBtn;
  private GameObject container;
  private bool initialized = false;
  public bool IsShow { get; private set; }

  private void Awake() {
    container = transform.GetChild(0).gameObject;
  }

  private void Start() {
    closeBtn.onClick.AddListener(() => Hide());
    popupBtn.onClick.AddListener(() => {
      if (!IsShow) {
        Show();
      }
      else {
        Hide();
      }
    });
    Hide();
  }

  private void Update() {
    if (Input.GetKeyUp(KeyCode.I)) {
      if (!IsShow) {
        Show();
      }
      else {
        Hide();
      }
    }
  }

  private void Show() {
    IsShow = true;
    container.SetActive(true);

    if (!initialized) {
      InventoryPanelManager.I.ResetPanel();
      EquipmentPanelManager.I.ResetPanel();
      initialized = true;
    }
    CharacterStatsPanelManager.I.SetPlayerStats(Player.I.Character.Stats);
    InventoryPanelManager.I.Init(Player.I.Character.Stats.Inventory);
    EquipmentPanelManager.I.Init(Player.I.Character.Stats.Equipment);
  }

  private void Hide() {
    IsShow = false;
    container.SetActive(false);
  }
}

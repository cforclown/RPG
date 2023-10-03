using UnityEngine;
using UnityEngine.UI;

public class CharacterPanelManager : MonoBehaviour {
  [SerializeField] private Button popupBtn;
  [SerializeField] private Button closeBtn;
  private GameObject container;
  private bool initialized = false;
  public bool IsOpen { get; private set; }

  private void Awake() {
    container = transform.GetChild(0).gameObject;
  }

  private void Start() {
    closeBtn.onClick.AddListener(() => Close());
    popupBtn.onClick.AddListener(() => {
      if (!IsOpen) {
        Open();
      }
      else {
        Close();
      }
    });
    Close();
  }

  private void Update() {
    if (Input.GetKeyUp(KeyCode.I)) {
      if (!IsOpen) {
        Open();
      }
      else {
        Close();
      }
    }
  }

  private void Open() {
    IsOpen = true;
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

  private void Close() {
    IsOpen = false;
    container.SetActive(false);
  }
}

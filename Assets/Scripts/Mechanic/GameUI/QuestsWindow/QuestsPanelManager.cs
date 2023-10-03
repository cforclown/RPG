using System.Collections.Generic;
using UnityEngine;

public class QuestsPanelManager : MonoBehaviour {
  private GameObject container;
  [SerializeField] private UnityEngine.UI.Button popupBtn;
  [SerializeField] private RectTransform scrollViewContent;
  [SerializeField] private GameObject itemPrefab;

  private bool isOpen = false;

  private List<QuestWindowItem> items;

  private void Awake() {
    items = new List<QuestWindowItem>();
    container = transform.GetChild(0).gameObject;
  }

  private void Start() {
    popupBtn.onClick.AddListener(() => {
      if (isOpen) {
        Close();
      }
      else {
        Open();
      }
    });
    Close();

    NPCEvents.OnPlayerAcceptNPCQuest += OnPlayerAcceptedNPCQuestEvent;
    QuestEvents.OnQuestFinished += OnPlayerFinishedQuest;
  }

  // Update is called once per frame
  private void Update() {
    if (Input.GetKeyUp(KeyCode.Q)) {
      if (!isOpen) {
        Open();
      }
      else {
        Close();
      }
    }
  }

  private void OnDestroy() {
    NPCEvents.OnPlayerAcceptNPCQuest -= OnPlayerAcceptedNPCQuestEvent;
    QuestEvents.OnQuestFinished -= OnPlayerFinishedQuest;
  }

  private void OnPlayerAcceptedNPCQuestEvent(NPC npc) {
    if (!isOpen) {
      return;
    }

    items.Add(InstantiateQuestWindowItem(npc.data.Quest));
  }

  private void OnPlayerFinishedQuest(QuestSO quest) {
    if (!isOpen) {
      return;
    }

    QuestWindowItem item = items.Find(i => i.Quest.Id == quest.Id);
    if (item == null) {
      return;
    }

    items.Remove(item);
    Destroy(item.gameObject);
  }

  public void Open() {
    if (Player.I == null) {
      return;
    }

    isOpen = true;
    container.SetActive(true);

    ClearItems();
    foreach (QuestSO quest in Player.I.Character.Stats.Quests.Quests) {
      QuestWindowItem item = InstantiateQuestWindowItem(quest);
      items.Add(item);
    }
  }

  public void Close() {
    isOpen = false;
    container.SetActive(false);
  }

  public void ClearItems() {
    if (items.Count > 0) {
      foreach (QuestWindowItem item in items) {
        Destroy(item.gameObject);
      }
    }
    items = new List<QuestWindowItem>();
  }

  public QuestWindowItem InstantiateQuestWindowItem(QuestSO quest) {
    GameObject itemObj = Instantiate(itemPrefab, scrollViewContent.transform);
    QuestWindowItem item = itemObj.GetComponent<QuestWindowItem>();
    item.Init(quest);

    return item;
  }
}

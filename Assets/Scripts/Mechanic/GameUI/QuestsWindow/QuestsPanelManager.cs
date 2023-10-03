using System.Collections.Generic;
using UnityEngine;

public class QuestsPanelManager : MonoBehaviour {
  private GameObject container;
  [SerializeField] private UnityEngine.UI.Button popupBtn;
  [SerializeField] private RectTransform scrollViewContent;
  [SerializeField] private GameObject itemPrefab;

  private bool isShow = false;

  private List<QuestWindowItem> items;

  private void Awake() {
    items = new List<QuestWindowItem>();
    container = transform.GetChild(0).gameObject;
  }

  private void Start() {
    popupBtn.onClick.AddListener(() => {
      if (isShow) {
        Hide();
      }
      else {
        Show();
      }
    });
    Hide();

    NPCEvents.OnPlayerAcceptNPCQuest += OnPlayerAcceptedNPCQuestEvent;
    QuestEvents.OnQuestDone += OnPlayerFinishedQuest;
  }

  // Update is called once per frame
  private void Update() {
    if (Input.GetKeyUp(KeyCode.Q)) {
      if (!isShow) {
        Show();
      }
      else {
        Hide();
      }
    }
  }

  private void OnDestroy() {
    NPCEvents.OnPlayerAcceptNPCQuest -= OnPlayerAcceptedNPCQuestEvent;
    QuestEvents.OnQuestDone -= OnPlayerFinishedQuest;
  }

  private void OnPlayerAcceptedNPCQuestEvent(NPC npc) {
    if (!isShow) {
      return;
    }

    items.Add(InstantiateQuestWindowItem(npc.data.Quest));
  }

  private void OnPlayerFinishedQuest(QuestSO quest) {
    if (!isShow) {
      return;
    }

    QuestWindowItem item = items.Find(i => i.Quest.Id == quest.Id);
    if (item == null) {
      return;
    }

    items.Remove(item);
    Destroy(item.gameObject);
  }

  public void Show() {
    if (Player.I == null) {
      return;
    }

    isShow = true;
    container.SetActive(true);

    ClearItems();
    foreach (QuestSO quest in Player.I.Character.Stats.Quests.Quests) {
      QuestWindowItem item = InstantiateQuestWindowItem(quest);
      items.Add(item);
    }
  }

  public void Hide() {
    isShow = false;
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

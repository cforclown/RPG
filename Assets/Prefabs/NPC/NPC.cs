using UnityEngine;

public class NPC : MonoBehaviour {
  [SerializeField] private GameObject questIndicator;
  [SerializeField] private GameObject questCompletedIndicator;

  [field: SerializeField] public NPC_SO data { get; private set; }

  public bool QuestAccepted { get; private set; } = false;

  private void Awake() {
    questIndicator = transform.GetChild(3).gameObject;
    if (questIndicator != null && data.Quest != null) {
      questIndicator.SetActive(true);
    }
    questCompletedIndicator = transform.GetChild(4).gameObject;
    questCompletedIndicator?.SetActive(false);
  }

  void Start() {
    QuestEvents.OnQuestCompleted += (QuestSO quest) => {
      if (!QuestAccepted || quest?.Id != data.Quest?.Id) {
        return;
      }
      OnQuestCompleted();
    };
    QuestEvents.OnQuestFinished += (QuestSO quest) => {
      if (!QuestAccepted || quest?.Id != data.Quest?.Id) {
        return;
      }
      OnQuestDone();
    };
  }

  public void PlayerAcceptedQuest() {
    QuestAccepted = true;
    data.Quest.Init();
    questIndicator?.SetActive(false);
    NPCEvents.PlayerAcceptNPCQuest(this);
  }

  private void OnQuestCompleted() {
    questCompletedIndicator?.SetActive(true);
  }

  private void OnQuestDone() {
    data.Quest.Done();
    QuestAccepted = false;
    questIndicator?.SetActive(true);
    questCompletedIndicator?.SetActive(false);
  }
}

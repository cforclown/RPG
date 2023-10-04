using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestPanelItem : MonoBehaviour {
  [SerializeField] private TextMeshProUGUI questTitle;
  [SerializeField] private TextMeshProUGUI questDesc;
  [SerializeField] private RectTransform questGoalsContainer;
  [SerializeField] private GameObject questGoalPrefab;

  public QuestSO Quest { get; private set; }
  private List<QuestGoalItem> questGoalsTogglers;

  private void Awake() {
    if (questGoalsTogglers == null) {
      questGoalsTogglers = new List<QuestGoalItem>();
    }
  }

  private void Start() {
    QuestEvents.OnQuestGoalCompleted += OnQuestGoalProgressEvent;
  }

  private void OnDestroy() {
    QuestEvents.OnQuestGoalCompleted -= OnQuestGoalProgressEvent;
  }

  public void Init(QuestSO quest) {
    this.Quest = quest;
    questTitle.text = quest.QuestTitle;
    questDesc.text = quest.QuestDesc;
    CreateQuestGoalsTogglers();
  }

  private void CreateQuestGoalsTogglers() {
    questGoalsTogglers = new List<QuestGoalItem>();
    GameObject togglerObj = Instantiate(questGoalPrefab, questGoalsContainer.transform);
    QuestGoalItem questGoalItem = togglerObj.GetComponent<QuestGoalItem>();
    questGoalItem.Set(Quest.QuestGoal);
    questGoalsContainer.sizeDelta = new Vector2(
      questGoalsContainer.sizeDelta.x,
      QuestGoalItem.HEIGHT
    );
    questGoalsTogglers.Add(questGoalItem);
  }

  private void OnQuestGoalProgressEvent(QuestGoal questGoal) {
    if (questGoal.Id != Quest.QuestGoal.Id) {
      return;
    }

    // currently on have 1 goal per quest
    if (questGoalsTogglers.Count == 0) {
      return;
    }

    questGoalsTogglers[0].SetValue(questGoal.Completed);
  }
}

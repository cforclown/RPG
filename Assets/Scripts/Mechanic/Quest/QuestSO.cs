using UnityEngine;

[CreateAssetMenu(fileName = "QuestSO", menuName = "ScriptableObjects/QuestSO")]
public class QuestSO : ScriptableObject, IQuest {
  [field: SerializeField] public string Id { get; private set; }
  [field: SerializeField] public string QuestTitle { get; private set; }
  [field: SerializeField] public string[] QuestBackStory { get; private set; }
  [field: SerializeField] public string QuestDesc { get; private set; }
  [field: SerializeField] public string[] QuestAcceptedTexts { get; private set; }
  [field: SerializeField] public string[] QuestCompletedTexts { get; private set; }
  [field: SerializeField] public QuestGoal QuestGoal { get; private set; }
  [field: SerializeField] public int ExpReward { get; private set; }

  [HideInInspector] public bool Completed { get; private set; }


  public void Init() {
    Completed = false;
    QuestGoal.Init();

    QuestEvents.OnQuestGoalCompleted += OnQuestGoalCompletedEvent;
  }

  public void Done() {
    QuestGoal.Done();
    QuestEvents.OnQuestGoalCompleted -= OnQuestGoalCompletedEvent;
  }

  private void OnQuestGoalCompletedEvent(QuestGoal questGoal) {
    if (questGoal.Id != QuestGoal.Id) {
      return;
    }
    OnQuestGoalCompleted();
  }

  private void OnQuestGoalCompleted() {
    Completed = true;
    QuestEvents.QuestCompleted(this);
  }

  object System.ICloneable.Clone() => (object)this.MemberwiseClone();
}

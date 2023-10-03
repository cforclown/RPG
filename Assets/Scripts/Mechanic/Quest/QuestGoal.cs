using UnityEngine;

public class QuestGoal : ScriptableObject, IQuestGoal {
  [HideInInspector] public string Id { get; private set; }
  [field: SerializeField] public string Desc { get; set; }
  [field: SerializeField] public int RequiredAmount { get; set; }
  [HideInInspector] public int CurrentAmount { get; set; }
  [HideInInspector] public bool Completed { get; set; }

  public virtual void Init() {
    Id = Generator.uuid();
    Completed = false;
    CurrentAmount = 0;
  }

  // any quest goal that inherit this class should override this method if they have subscription to any events
  public virtual void Done() {
  }

  public void Evaluate() {
    if (CurrentAmount >= RequiredAmount) {
      if (Completed) {
        return;
      }

      Complete();
    }
    else {
      Completed = false;
    }
  }

  public void Complete() {
    Completed = true;
    QuestEvents.QuestGoalCompleted(this);
  }

  public virtual string GetProgressText() {
    return string.Format("You have killed {0} of {1} of required kills", CurrentAmount, RequiredAmount);
  }
}

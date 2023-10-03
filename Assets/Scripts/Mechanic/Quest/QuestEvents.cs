public class QuestEvents {
  public delegate void QuestEventHandler(QuestSO quest);
  public static event QuestEventHandler OnQuestCompleted;
  public static event QuestEventHandler OnQuestDone;

  public delegate void QuestGoalEventHandler(QuestGoal questGoal);
  public static event QuestGoalEventHandler OnQuestProgress;
  public static event QuestGoalEventHandler OnQuestGoalCompleted;

  public static void QuestCompleted(QuestSO quest) {
    if (OnQuestCompleted == null) {
      return;
    }

    OnQuestCompleted(quest);
  }

  public static void QuestDone(QuestSO quest) {
    if (OnQuestDone == null) {
      return;
    }

    OnQuestDone(quest);
  }

  public static void QuestGoalCompleted(QuestGoal questGoal) {
    if (OnQuestGoalCompleted == null) {
      return;
    }

    OnQuestGoalCompleted(questGoal);
  }

  public static void QuestProgress(QuestGoal questGoal) {
    if (OnQuestProgress == null) {
      return;
    }

    OnQuestProgress(questGoal);
  }
}

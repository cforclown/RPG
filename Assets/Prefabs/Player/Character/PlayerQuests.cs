using System.Collections.Generic;

public class PlayerQuests {
  public List<QuestSO> Quests { get; private set; }

  public PlayerQuests(List<QuestSO> quests) {
    if (quests == null || quests.Count == 0) {
      Quests = new List<QuestSO>();
    }
    else {
      Quests = new List<QuestSO>(quests);
    }
  }

  public void AddQuest(QuestSO quest) {
    Quests.Add(quest);
  }

  public void RemoveQuest(QuestSO quest) {
    Quests.Remove(quest);
  }
}

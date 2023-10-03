using System;

public interface IQuest : ICloneable {
  public string Id { get; }
  public string QuestTitle { get; }
  public string[] QuestBackStory { get; }
  public string QuestDesc { get; }
  public string[] QuestAcceptedTexts { get; }
  public string[] QuestCompletedTexts { get; }
  public QuestGoal QuestGoal { get; }
  public int ExpReward { get; }
}

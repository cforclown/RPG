public interface IQuestGoal {
  public string Desc { get; }
  public bool Completed { get; }
  public int CurrentAmount { get; }
  public int RequiredAmount { get; }
}

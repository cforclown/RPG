using UnityEngine;

[CreateAssetMenu(fileName = "KillQuestGoalSO", menuName = "ScriptableObjects/KillQuestGoalSO")]
public class KillQuestGoalSO : QuestGoal {
  [field: SerializeField] public EnemySO Enemy;

  public KillQuestGoalSO(
    EnemySO enemy,
    string desc,
    bool completed,
    int currentAmount,
    int requiredAmount
  ) {
    Enemy = enemy;
    Desc = desc;
    Completed = completed;
    CurrentAmount = currentAmount;
    RequiredAmount = requiredAmount;
  }

  public override void Init() {
    base.Init();
    CombatEvents.OnEnemyDeath += EnemyDied;
  }

  public override void Done() {
    CombatEvents.OnEnemyDeath -= EnemyDied;
  }

  public void EnemyDied(Enemy enemy) {
    if (enemy.Stats.AssetId == Enemy.AssetId) {
      CurrentAmount++;
      Evaluate();
      QuestEvents.QuestProgress(this);
    }
  }

  public override string GetProgressText() {
    return string.Format("You have killed {0} of {1} {2}", CurrentAmount, RequiredAmount, Enemy.Name);
  }
}

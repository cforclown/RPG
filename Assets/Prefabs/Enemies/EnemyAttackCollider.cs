using UnityEngine;

public class EnemyAttackCollider : MonoBehaviour {
  [SerializeField] public Enemy controller;
  [SerializeField] public EnemyAnimStateController animController;

  public bool IsAttacking() {
    return animController.IsAttacking;
  }

  public bool IsAttackLanded() {
    return animController.IsAttackLanded;
  }

  public void AttackLanded() {
    animController.AttackLanded();
  }

  public IEnemySO GetStats() {
    return controller.Stats;
  }
}

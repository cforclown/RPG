using UnityEngine;

public class CombatEvents : MonoBehaviour {
  public delegate void EnemyCombatEvenyHandler(Enemy enemy);
  public static event EnemyCombatEvenyHandler OnEnemyDeath;
  public static event EnemyCombatEvenyHandler OnPlayerHitEnemy;

  public delegate void PlayerCombatEvenyHandler(Enemy enemy, Character player);
  public static event PlayerCombatEvenyHandler OnEnemyHitPlayer;

  public delegate void CombatDamageOutput(Transform objectHit, int damage);
  public static event CombatDamageOutput OnAttacktHit;

  public static void EnemyDied(Enemy enemy) {
    if (OnEnemyDeath == null) {
      return;
    }

    OnEnemyDeath(enemy);
  }

  public static void PlayerHitEnemy(Enemy enemy) {
    if (OnPlayerHitEnemy == null) {
      return;
    }

    OnPlayerHitEnemy(enemy);
  }

  public static void EnemyHitPlayer(Enemy enemy, Character player) {
    if (OnEnemyHitPlayer == null) {
      return;
    }

    OnEnemyHitPlayer(enemy, player);
  }

  public static void AttackHit(Transform objectHit, int damage) {
    if (OnAttacktHit == null) {
      return;
    }

    OnAttacktHit(objectHit, damage);
  }
}

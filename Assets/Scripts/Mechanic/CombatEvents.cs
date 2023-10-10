using UnityEngine;

public class CombatEvents : MonoBehaviour {
  #region PLAYER ATTACK MOTION EVENTs
  public delegate void PlayerAttackMotionEventHandler(PlayerAnimController playerAnimController);
  public static event PlayerAttackMotionEventHandler OnPlayerAttackLaunched;
  public static event PlayerAttackMotionEventHandler OnPlayerAttackFinished;
  public static event PlayerAttackMotionEventHandler OnPlayerAttackMotionFinished;

  public static void PlayerAttackLaunched(PlayerAnimController playerAnimController) {
    if (OnPlayerAttackLaunched == null) {
      return;
    }

    OnPlayerAttackLaunched(playerAnimController);

    if (playerAnimController.IsSkill == 0) {
      return;
    }
    SkillCalculator.ResetHitEnemies();
  }

  public static void PlayerAttackFinished(PlayerAnimController playerAnimController) {
    if (OnPlayerAttackFinished == null) {
      return;
    }

    OnPlayerAttackFinished(playerAnimController);

    if (playerAnimController.IsSkill == 0) {
      return;
    }
    SkillCalculator.ResetHitEnemies();
  }

  public static void PlayerAttackMotionFinished(PlayerAnimController playerAnimController) {
    if (OnPlayerAttackMotionFinished == null) {
      return;
    }

    OnPlayerAttackMotionFinished(playerAnimController);

    if (playerAnimController.IsSkill == 0) {
      return;
    }
    SkillCalculator.ResetHitEnemies();
  }
  #endregion



  #region ENEMY COMBAT EVENTs
  public delegate void EnemyCombatEvenyHandler(Enemy enemy);
  public static event EnemyCombatEvenyHandler OnEnemyDeath;

  public static void EnemyDied(Enemy enemy) {
    if (OnEnemyDeath == null) {
      return;
    }

    OnEnemyDeath(enemy);
  }
  #endregion



  #region ATTACK EVENTs
  public delegate void AttackHitEvents(CharacterManager player, Enemy enemy, WeaponSO weapon);
  public static event AttackHitEvents OnPlayerAttackHitEnemy;
  public static event AttackHitEvents OnPostPlayerAttackHitEnemy;
  public static event AttackHitEvents OnEnemyAttackHitPlayer;
  public static event AttackHitEvents OnPostEnemyAttackHitPlayer;

  public static void PlayerAttackHitEnemyEvent(CharacterManager player, Enemy enemy, WeaponSO weapon) {
    if (OnPlayerAttackHitEnemy == null) {
      return;
    }

    OnPlayerAttackHitEnemy(player, enemy, weapon);
    if (OnAttacktHit != null) {
      OnAttacktHit(enemy.transform, player.GetDamageOutput(enemy, weapon));
    }

    if (player.AnimController.IsSkill == 0) {
      return;
    }

    SkillSO skill = player.GetPerformedSkill();
    if (player.AnimController.IsSkill == 0) {
      return;
    }
    SkillCalculator.HitEnemy(skill, enemy);
  }

  public static void PostPlayerAttackHitEnemyEvent(CharacterManager player, Enemy enemy, WeaponSO weapon) {
    if (OnPostPlayerAttackHitEnemy == null) {
      return;
    }

    OnPostPlayerAttackHitEnemy(player, enemy, weapon);
  }

  public static void EnemyAttackHitPlayerEvent(CharacterManager player, Enemy enemy, WeaponSO weapon) {
    if (OnEnemyAttackHitPlayer == null) {
      return;
    }

    OnEnemyAttackHitPlayer(player, enemy, weapon);

    if (OnAttacktHit != null) {
      OnAttacktHit(player.transform, enemy.Stats.Damage);
    }
  }

  public static void PostEnemyAttackHitPlayerEvent(CharacterManager player, Enemy enemy, WeaponSO weapon) {
    if (OnPostEnemyAttackHitPlayer == null) {
      return;
    }

    OnPostEnemyAttackHitPlayer(player, enemy, weapon);
  }
  #endregion



  #region COMBAT ATTACK DAMAGE OUTPUT EVENTs
  public delegate void CombatDamageOutput(Transform objectHit, int damage);
  public static event CombatDamageOutput OnAttacktHit;
  #endregion
}

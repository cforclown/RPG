using System.Collections.Generic;
using UnityEngine;

public class SkillCalculator : MonoBehaviour {
  private static List<Enemy> HitEnemies = new List<Enemy>();

  public static void ResetHitEnemies() {
    HitEnemies.Clear();
  }

  public static void HitEnemy(SkillSO skill, Enemy enemy) {
    if (skill.SkillType == 0) {
      return;
    }

    if (HitEnemies.Contains(enemy)) {
      return;
    }
    HitEnemies.Add(enemy);
  }

  public static bool CheckHitEnemy(Enemy enemy) {
    return HitEnemies.Contains(enemy);
  }
}

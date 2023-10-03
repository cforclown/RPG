public class PlayerEvents {
  public delegate void PlayerEventHandler(Character player);
  public static event PlayerEventHandler OnPlayerStatsUpdated;

  public static void PlayerStatsUpdated(Character player) {
    if (OnPlayerStatsUpdated == null) {
      return;
    }

    OnPlayerStatsUpdated(player);
  }

  public delegate void PlayerSkillEventHandler(SkillSO skill);
  public static event PlayerSkillEventHandler OnPlayerClaimedSkill;
  public static event PlayerSkillEventHandler OnPlayerSkillLevelUp;

  public static void PlayerClaimSkill(SkillSO skill) {
    if (OnPlayerClaimedSkill == null) {
      return;
    }

    OnPlayerClaimedSkill(skill);
  }

  public static void PlayerSkillLevelUp(SkillSO skill) {
    if (OnPlayerSkillLevelUp == null) {
      return;
    }

    OnPlayerSkillLevelUp(skill);
  }
}

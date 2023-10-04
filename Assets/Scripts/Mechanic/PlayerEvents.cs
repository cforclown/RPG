public class PlayerEvents {
  public delegate void PlayerEventHandler(Character player);
  public static event PlayerEventHandler OnPlayerStatsUpdated;
  public static event PlayerEventHandler OnPlayerDied;

  public static void PlayerStatsUpdated(Character player) {
    if (OnPlayerStatsUpdated == null) {
      return;
    }

    OnPlayerStatsUpdated(player);
  }

  public static void PlayerDied(Character player) {
    if (OnPlayerDied == null) {
      return;
    }

    OnPlayerDied(player);
  }

  public delegate void PlayerSkillEventHandler(PlayerSkills playerSkills);
  public static event PlayerSkillEventHandler OnPlayerSkillsUpdated;

  public static void PlayerSkillsUpdated(PlayerSkills playerSkills) {
    if (OnPlayerSkillsUpdated == null) {
      return;
    }

    OnPlayerSkillsUpdated(playerSkills);
  }
}

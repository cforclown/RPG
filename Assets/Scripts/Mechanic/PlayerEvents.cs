public class PlayerEvents {
  public delegate void PlayerEventHandler(Character player);
  public static event PlayerEventHandler OnPlayerStatsUpdated;

  public static void PlayerStatsUpdated(Character player) {
    if (OnPlayerStatsUpdated == null) {
      return;
    }

    OnPlayerStatsUpdated(player);
  }
}

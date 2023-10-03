public static class ExpService {
  private static readonly int[] EXP_LEVELS = {
    50,
    200,
    500,
    1100,
    2300,
    4700,
    9500,
    19100
  };

  public static int GetNextLevelExpBreakpoint(int currentLvl) {
    return EXP_LEVELS[currentLvl - 1];
  }

  public static int GetCurrentLevelMinExp(int currentLvl) {
    if (currentLvl <= 1) {
      return 0;
    }

    return EXP_LEVELS[currentLvl - 2];
  }
}

public class PlayerEvents {
  public delegate void PlayerEventHandler(Character player);
  public static event PlayerEventHandler OnPlayerStatsUpdated;
  public static event PlayerEventHandler OnPlayerDied;

  public delegate void PlayerGrowHandler();
  public static event PlayerGrowHandler OnIncreaseStrengthAction;
  public static event PlayerGrowHandler OnIncreaseAgilityAction;
  public static event PlayerGrowHandler OnIncreaseIntelligenceAction;

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

  public static void IncreaseStrengthAction() {
    if (OnIncreaseStrengthAction == null) {
      return;
    }

    OnIncreaseStrengthAction();
  }

  public static void IncreaseAgilityAction() {
    if (OnIncreaseIntelligenceAction == null) {
      return;
    }

    OnIncreaseAgilityAction();
  }

  public static void IncreaseIntelligenceAction() {
    if (OnIncreaseIntelligenceAction == null) {
      return;
    }

    OnIncreaseIntelligenceAction();
  }

  public delegate void PlayerSkillEventHandler(PlayerSkills playerSkills);
  public static event PlayerSkillEventHandler OnPlayerSkillsUpdated;
  public static void PlayerSkillsUpdated(PlayerSkills inventory) {
    if (OnPlayerSkillsUpdated == null) {
      return;
    }

    OnPlayerSkillsUpdated(inventory);
  }

  public delegate void PlayerInventoryEventHandler(PlayerInventory inventory);
  public static event PlayerInventoryEventHandler OnPlayerInventoryUpdated;

  public static void PlayerInventoryUpdated(PlayerInventory inventory) {
    if (OnPlayerInventoryUpdated == null) {
      return;
    }

    OnPlayerInventoryUpdated(inventory);
  }

  public delegate void PlayerEquipmentEventHandler(PlayerEquipment equipments);
  public static event PlayerEquipmentEventHandler OnPlayerEquipmentUpdated;

  public static void PlayerEquipmentUpdated(PlayerEquipment equipments) {
    if (OnPlayerEquipmentUpdated == null) {
      return;
    }

    OnPlayerEquipmentUpdated(equipments);
  }
}

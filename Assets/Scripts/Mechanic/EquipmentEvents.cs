public class EquipmentEvents {
  public delegate void EquipmentEventHandler(ItemSO item, EquipPlaceholderTypes placeholder);
  public static event EquipmentEventHandler OnItemEquipped;
  public static event EquipmentEventHandler OnItemRemoved;

  public static void ItemEquipped(ItemSO item, EquipPlaceholderTypes placeholder) {
    if (OnItemEquipped == null) {
      return;
    }

    OnItemEquipped(item, placeholder);
  }

  public static void ItemRemoved(ItemSO item, EquipPlaceholderTypes placeholder) {
    if (OnItemRemoved == null) {
      return;
    }

    OnItemRemoved(item, placeholder);
  }
}

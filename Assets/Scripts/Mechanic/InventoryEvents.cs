public class InventoryEvents {
  public delegate void InventoryEventHandler(ItemSO item, int loc);
  public static event InventoryEventHandler OnItemAdded;
  public static event InventoryEventHandler OnItemRemoved;

  public static void ItemAdded(ItemSO item, int loc) {
    if (OnItemAdded == null) {
      return;
    }

    OnItemAdded(item, loc);
  }

  public static void ItemRemoved(ItemSO item, int loc) {
    if (OnItemRemoved == null) {
      return;
    }

    OnItemRemoved(item, loc);
  }
}

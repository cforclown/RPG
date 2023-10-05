using System.Collections.Generic;
using UnityEngine;

public enum InventoryAction {
  NONE = 0,
  DRAGGING_ITEM = 1
}

public class InventoryPanelManager : MonoBehaviour {
  public delegate void InventoryEventHandler(InventoryItem item);
  public static event InventoryEventHandler OnAddItemAction;
  public static event InventoryEventHandler OnRemoveItemAction;

  public static InventoryPanelManager I;

  public bool IsDragging { get; private set; }

  private List<InventoryItemPlaceholder> placeholders;

  private void Awake() {
    I = this;

    placeholders = new List<InventoryItemPlaceholder>(PlayerInventory.SIZE);
    int tileGameObjCount = transform.childCount;
    if (tileGameObjCount < PlayerInventory.SIZE) {
      Logger.Err(this.name, "Inventory container child count is not same with defined size");
    }
    for (int x = 0; x < tileGameObjCount; x++) {
      InventoryItemPlaceholder tile = transform.GetChild(x).GetComponent<InventoryItemPlaceholder>();
      tile.SetLocation(x);
      placeholders.Add(tile);
    }

    PlayerEvents.OnPlayerInventoryUpdated += Evaluate;
  }

  private void OnDestroy() {
    I = null;
  }

  public static void AddItemAction(InventoryItem item) {
    if (OnAddItemAction == null) {
      return;
    }

    OnAddItemAction(item);
  }

  public static void RemoveItemAction(InventoryItem item) {
    if (OnRemoveItemAction == null) {
      return;
    }

    OnRemoveItemAction(item);
  }

  public void Evaluate(PlayerInventory inventory) {
    foreach (InventoryItemPlaceholder placeholder in placeholders) {
      InventoryItem item = inventory.Items.Find(i => i.Location == placeholder.Location);
      if (item == null) {
        placeholder.Item = null;
      }
      else {
        placeholder.Item = item.Item;
      }
    }
  }

  public int GetAvailableLocation() {
    foreach (InventoryItemPlaceholder tile in placeholders) {
      if (tile.Item == null) {
        return tile.Location;
      }
    }

    return -1;
  }

  public void ResetPanel() {
    foreach (InventoryItemPlaceholder tile in placeholders.FindAll(t => t.Item != null)) {
      tile.Item = null;
    }
  }

  public InventoryItemPlaceholder CheckPlaceholderHover(Vector2 pos, ItemSO item) {
    if (item == null) {
      return null;
    }

    InventoryItemPlaceholder hoveredPlaceholder = null;
    foreach (InventoryItemPlaceholder placeholder in placeholders) {
      if (placeholder.IsHover(pos)) {
        placeholder.SetOnHoverColor();
        hoveredPlaceholder = placeholder;
      }
      else {
        placeholder.SetDefaultColor();
      }
    }

    return hoveredPlaceholder;
  }
}
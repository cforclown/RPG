using System.Collections.Generic;
using UnityEngine;

public enum InventoryAction {
  NONE = 0,
  DRAGGING_ITEM = 1
}

public class InventoryPanelManager : MonoBehaviour {
  public static InventoryPanelManager I;

  public bool IsDragging { get; private set; }

  private List<InventoryTile> tiles;

  private void Awake() {
    I = this;

    tiles = new List<InventoryTile>(PlayerInventory.SIZE);
    int tileGameObjCount = transform.childCount;
    if (tileGameObjCount < PlayerInventory.SIZE) {
      Logger.Err(this.name, "Inventory container child count is not same with defined size");
    }
    for (int x = 0; x < tileGameObjCount; x++) {
      InventoryTile tile = transform.GetChild(x).GetComponent<InventoryTile>();
      tile.SetLocation(x);
      tiles.Add(tile);
    }
  }

  public void Init(PlayerInventory inventory) {
    if (inventory == null) {
      return;
    }

    foreach (InventoryItem inventoryItem in inventory.Items) {
      tiles[inventoryItem.Location].Item = inventoryItem.Item;
    }
  }

  public int GetAvailableLocation() {
    foreach (InventoryTile tile in tiles) {
      if (tile.Item == null) {
        return tile.Location;
      }
    }

    return -1;
  }

  public void ResetPanel() {
    foreach (InventoryTile tile in tiles.FindAll(t => t.Item != null)) {
      tile.Item = null;
    }
  }

  public void AddItem(ItemSO item, int location) {
    tiles[location].Item = item;
    InventoryItem inventoryItem = new InventoryItem(item, location);
    Player.I.Character.Stats.AddItem(inventoryItem);
  }

  public void RemoveItem(ItemSO item, int location) {
    tiles[location].Item = null;
    InventoryItem inventoryItem = new InventoryItem(item, location);
    Player.I.Character.Stats.RemoveItem(inventoryItem);
  }

  public InventoryTile CheckInventoryTileHover(Vector2 pos, ItemSO item) {
    if (item == null) {
      return null;
    }

    InventoryTile hoveredTile = null;
    foreach (InventoryTile tile in tiles) {
      if (tile.IsHover(pos) && tile.IsEmpty()) {
        tile.HoverIndicator();
        hoveredTile = tile;
      }
      else {
        tile.DefaultIndicator();
      }
    }

    return hoveredTile;
  }
}
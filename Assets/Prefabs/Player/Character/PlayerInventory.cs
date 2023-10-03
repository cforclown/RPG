using System;
using System.Collections.Generic;

public class InventoryItem {
  public ItemSO Item;
  public int Location;

  public InventoryItem(ItemSO item, int inventoryLoc) {
    this.Item = item;
    this.Location = inventoryLoc;
  }
}

[Serializable]
public class PlayerInventory {
  public static int WIDTH = 7;
  public static int HEIGHT = 3;
  public static int SIZE = WIDTH * HEIGHT;

  public List<InventoryItem> Items;

  public PlayerInventory(InventoryItem[] items) {
    Items = new List<InventoryItem>(items);
  }

  public PlayerInventory() {
    Items = new List<InventoryItem>(SIZE);
  }

  public void AddItem(InventoryItem inventoryItem) {
    Items.Add(inventoryItem);
  }

  public void RemoveItem(InventoryItem inventoryItem) {
    Items.RemoveAll(x => x.Location == inventoryItem.Location && x.Item.Id == inventoryItem.Item.Id);
  }
}

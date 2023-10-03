using System;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes {
  WEAPON = 0,
  SHIELD = 1,
  ARMOR = 2,
  NECKLACE = 4,
  RING = 3,
  CRAFT_COMPONENT = 6,
  CONSUMABLE = 7,
}

public interface IItem : ICloneable {
  public string AssetId { get; }
  public string Id { get; }
  public string Name { get; }
  public ItemTypes Type { get; }
  public int[,] Grid { get; }
  public List<IEffect> Effects { get; }

  public Sprite Sprite { get; }
  public bool CanBeStacked { get; }
}
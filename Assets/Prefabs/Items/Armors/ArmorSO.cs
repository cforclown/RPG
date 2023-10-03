using UnityEngine;

public enum ArmorTypes {
  HEAD = 0,
  BODY = 1,
  SHOULDER = 2,
  HAND = 3,
  BELT = 4,
  LEG = 5,
  FOOT = 6
}

public class ArmorSO : ItemSO {
  [field: SerializeField] public int Armor { get; private set; }
  public override ItemTypes Type { get; } = ItemTypes.ARMOR;
  public virtual ArmorTypes ArmorType { get; }
}

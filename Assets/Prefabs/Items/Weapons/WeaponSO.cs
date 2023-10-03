using UnityEngine;

public enum WeaponTypes {
  MELEE = 0,
  RANGE = 1,
  SHIELD = 2,
  MAGIC_WAND = 3
}


public class WeaponSO : ItemSO {
  [field: SerializeField] public int MinDamage { get; private set; }
  [field: SerializeField] public int MaxDamage { get; private set; }
  [field: SerializeField] public float AttackRange { get; private set; }
  public override ItemTypes Type { get; } = ItemTypes.WEAPON;
  public virtual WeaponTypes WeaponType { get; }
}

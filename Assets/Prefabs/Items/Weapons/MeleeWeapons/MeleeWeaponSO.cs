using UnityEngine;

public enum MeleeWeaponTypes {
  SWORD = 0,
  GREAT_SWORD = 1,
  AXE = 2,
  MACE = 3,
  SPEAR = 4
}

[CreateAssetMenu(fileName = "MeleeWeaponSO", menuName = "ScriptableObjects/MeleeWeaponSO")]
public class MeleeWeaponSO : WeaponSO {
  public override WeaponTypes WeaponType { get; } = WeaponTypes.MELEE;
  [field: SerializeField] public MeleeWeaponTypes MeleeWeaponType { get; private set; }
}

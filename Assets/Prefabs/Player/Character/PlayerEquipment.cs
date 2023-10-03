using System;

[Serializable]
public class PlayerEquipment {
  public ArmorSO HeadArmor { get; private set; }
  public ItemSO Necklace { get; private set; }
  public ItemSO Ring1 { get; private set; }
  public ItemSO Ring2 { get; private set; }
  public ItemSO RightHandWeapon { get; private set; }
  public ItemSO LeftHandWeapon { get; private set; }
  public ArmorSO BodyArmor { get; private set; }
  public ArmorSO ShoulderArmor { get; private set; }
  public ArmorSO HandArmor { get; private set; }
  public ArmorSO LegArmor { get; private set; }
  public ArmorSO FootArmor { get; private set; }

  public void EquipItem(EquipPlaceholderTypes placeholderType, ItemSO item) {
    if (item == null) {
      return;
    }

    switch (placeholderType) {
      case EquipPlaceholderTypes.HEAD_ARMOR:
        HeadArmor = (ArmorSO)item;
        break;
      case EquipPlaceholderTypes.NECKLACE:
        Necklace = item;
        break;
      case EquipPlaceholderTypes.RING1:
        Ring1 = item;
        break;
      case EquipPlaceholderTypes.RING2:
        Ring2 = item;
        break;
      case EquipPlaceholderTypes.RIGHT_HAND_WEAPON:
        RightHandWeapon = item;
        break;
      case EquipPlaceholderTypes.LEFT_HAND_WEAPON:
        LeftHandWeapon = item;
        break;
      case EquipPlaceholderTypes.BODY_ARMOR:
        BodyArmor = (ArmorSO)item;
        break;
      case EquipPlaceholderTypes.SHOULDER_ARMOR:
        ShoulderArmor = (ArmorSO)item;
        break;
      case EquipPlaceholderTypes.HAND_ARMOR:
        HandArmor = (ArmorSO)item;
        break;
      case EquipPlaceholderTypes.LEG_ARMOR:
        LegArmor = (ArmorSO)item;
        break;
      case EquipPlaceholderTypes.FOOT_ARMOR:
        FootArmor = (ArmorSO)item;
        break;
      default:
        throw new Exception("EquimentPanelItems type not found");
    }
  }

  public void UnequipItem(EquipPlaceholderTypes placeholderType) {
    switch (placeholderType) {
      case EquipPlaceholderTypes.HEAD_ARMOR:
        HeadArmor = null;
        break;
      case EquipPlaceholderTypes.NECKLACE:
        Necklace = null;
        break;
      case EquipPlaceholderTypes.RING1:
        Ring1 = null;
        break;
      case EquipPlaceholderTypes.RING2:
        Ring2 = null;
        break;
      case EquipPlaceholderTypes.RIGHT_HAND_WEAPON:
        RightHandWeapon = null;
        break;
      case EquipPlaceholderTypes.LEFT_HAND_WEAPON:
        LeftHandWeapon = null;
        break;
      case EquipPlaceholderTypes.BODY_ARMOR:
        BodyArmor = null;
        break;
      case EquipPlaceholderTypes.SHOULDER_ARMOR:
        ShoulderArmor = null;
        break;
      case EquipPlaceholderTypes.HAND_ARMOR:
        HandArmor = null;
        break;
      case EquipPlaceholderTypes.LEG_ARMOR:
        LegArmor = null;
        break;
      case EquipPlaceholderTypes.FOOT_ARMOR:
        FootArmor = null;
        break;
      default:
        throw new Exception("EquimentPanelItems type not found");
    }
  }
}
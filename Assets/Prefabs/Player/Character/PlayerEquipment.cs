using System;

public class EquippedItem {
  ItemSO Item;
  EquipmentPlaceholderTypes PlaceholderType;
}

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

  public void EquipItem(ItemSO item, EquipmentPlaceholderTypes placeholderType) {
    if (item == null) {
      return;
    }

    switch (placeholderType) {
      case EquipmentPlaceholderTypes.HEAD_ARMOR:
        HeadArmor = (ArmorSO)item;
        break;
      case EquipmentPlaceholderTypes.NECKLACE:
        Necklace = item;
        break;
      case EquipmentPlaceholderTypes.RING1:
        Ring1 = item;
        break;
      case EquipmentPlaceholderTypes.RING2:
        Ring2 = item;
        break;
      case EquipmentPlaceholderTypes.RIGHT_HAND_WEAPON:
        RightHandWeapon = item;
        break;
      case EquipmentPlaceholderTypes.LEFT_HAND_WEAPON:
        LeftHandWeapon = item;
        break;
      case EquipmentPlaceholderTypes.BODY_ARMOR:
        BodyArmor = (ArmorSO)item;
        break;
      case EquipmentPlaceholderTypes.SHOULDER_ARMOR:
        ShoulderArmor = (ArmorSO)item;
        break;
      case EquipmentPlaceholderTypes.HAND_ARMOR:
        HandArmor = (ArmorSO)item;
        break;
      case EquipmentPlaceholderTypes.LEG_ARMOR:
        LegArmor = (ArmorSO)item;
        break;
      case EquipmentPlaceholderTypes.FOOT_ARMOR:
        FootArmor = (ArmorSO)item;
        break;
      default:
        throw new Exception("EquimentPanelItems type not found");
    }
  }

  public void UnequipItem(EquipmentPlaceholderTypes placeholderType) {
    switch (placeholderType) {
      case EquipmentPlaceholderTypes.HEAD_ARMOR:
        HeadArmor = null;
        break;
      case EquipmentPlaceholderTypes.NECKLACE:
        Necklace = null;
        break;
      case EquipmentPlaceholderTypes.RING1:
        Ring1 = null;
        break;
      case EquipmentPlaceholderTypes.RING2:
        Ring2 = null;
        break;
      case EquipmentPlaceholderTypes.RIGHT_HAND_WEAPON:
        RightHandWeapon = null;
        break;
      case EquipmentPlaceholderTypes.LEFT_HAND_WEAPON:
        LeftHandWeapon = null;
        break;
      case EquipmentPlaceholderTypes.BODY_ARMOR:
        BodyArmor = null;
        break;
      case EquipmentPlaceholderTypes.SHOULDER_ARMOR:
        ShoulderArmor = null;
        break;
      case EquipmentPlaceholderTypes.HAND_ARMOR:
        HandArmor = null;
        break;
      case EquipmentPlaceholderTypes.LEG_ARMOR:
        LegArmor = null;
        break;
      case EquipmentPlaceholderTypes.FOOT_ARMOR:
        FootArmor = null;
        break;
      default:
        throw new Exception("EquimentPanelItems type not found");
    }
  }
}
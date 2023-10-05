using System;
using UnityEngine;

public enum EquipmentPlaceholderTypes {
  HEAD_ARMOR = 0,
  BODY_ARMOR = 1,
  RIGHT_HAND_WEAPON = 2,
  LEFT_HAND_WEAPON = 3,
  SHOULDER_ARMOR = 4,
  HAND_ARMOR = 5,
  LEG_ARMOR = 6,
  FOOT_ARMOR = 7,
  NECKLACE = 8,
  RING1 = 9,
  RING2 = 10,
}

public class EquipmentPanelManager : MonoBehaviour {
  public delegate void EquipmentEventHandler(ItemSO item, EquipmentPlaceholderTypes placeholderType);
  public static event EquipmentEventHandler OnEquipItemAction;
  public static event EquipmentEventHandler OnUnequipItemAction;

  public static EquipmentPanelManager I;

  [SerializeField] private EquipmentPlaceholder rightHandPlaceholder;
  [SerializeField] private EquipmentPlaceholder leftHandPlaceholder;
  [SerializeField] private EquipmentPlaceholder headArmorItemPlaceholder;
  [SerializeField] private EquipmentPlaceholder bodyArmorItemPlaceholder;
  [SerializeField] private EquipmentPlaceholder shoulderArmorItemPlaceholder;
  [SerializeField] private EquipmentPlaceholder handArmorItemPlaceholder;
  [SerializeField] private EquipmentPlaceholder legArmorItemPlaceholder;
  [SerializeField] private EquipmentPlaceholder footArmorItemPlaceholder;
  [SerializeField] private EquipmentPlaceholder necklaceItemPlaceholder;
  [SerializeField] private EquipmentPlaceholder ring1ItemPlaceholder;
  [SerializeField] private EquipmentPlaceholder ring2ItemPlaceholder;

  private Color onHoverColor = new Color(0.8f, 0f, 0f, 0.5f);
  private Color defaultColor = new Color(0f, 0f, 0f, 0.5f);

  public bool IsDragging { get; private set; }

  private void Awake() {
    if (I == null) {
      I = this;
    }

    PlayerEvents.OnPlayerEquipmentUpdated += Evaluate;
  }

  private void OnDestroy() {
    PlayerEvents.OnPlayerEquipmentUpdated -= Evaluate;
  }

  public void ResetPanel() {
    headArmorItemPlaceholder.ResetPlaceholder();
    bodyArmorItemPlaceholder.ResetPlaceholder();
    rightHandPlaceholder.ResetPlaceholder();
    leftHandPlaceholder.ResetPlaceholder();
    shoulderArmorItemPlaceholder.ResetPlaceholder();
    handArmorItemPlaceholder.ResetPlaceholder();
    legArmorItemPlaceholder.ResetPlaceholder();
    necklaceItemPlaceholder.ResetPlaceholder();
    ring1ItemPlaceholder.ResetPlaceholder();
    ring2ItemPlaceholder.ResetPlaceholder();
  }

  public static void EquipItemAction(ItemSO item, EquipmentPlaceholderTypes placeholderType) {
    if (OnEquipItemAction == null) {
      return;
    }

    OnEquipItemAction(item, placeholderType);
  }

  public static void UnequipItemAction(ItemSO item, EquipmentPlaceholderTypes placeholderType) {
    if (OnUnequipItemAction == null) {
      return;
    }

    OnUnequipItemAction(item, placeholderType);
  }

  public void Evaluate(PlayerEquipment equipments) {
    if (equipments.HeadArmor != null) {
      headArmorItemPlaceholder.EquipItem(equipments.HeadArmor);
    }
    else {
      headArmorItemPlaceholder.RemoveItem();
    }

    if (equipments.BodyArmor != null) {
      bodyArmorItemPlaceholder.EquipItem(equipments.BodyArmor);
    }
    else {
      bodyArmorItemPlaceholder.RemoveItem();
    }

    if (equipments.RightHandWeapon != null) {
      rightHandPlaceholder.EquipItem(equipments.RightHandWeapon);
    }
    else {
      rightHandPlaceholder.RemoveItem();
    }

    if (equipments.LeftHandWeapon != null) {
      leftHandPlaceholder.EquipItem(equipments.LeftHandWeapon);
    }
    else {
      leftHandPlaceholder.RemoveItem();
    }

    if (equipments.ShoulderArmor != null) {
      shoulderArmorItemPlaceholder.EquipItem(equipments.ShoulderArmor);
    }
    else {
      shoulderArmorItemPlaceholder.RemoveItem();
    }

    if (equipments.HandArmor != null) {
      handArmorItemPlaceholder.EquipItem(equipments.HandArmor);
    }
    else {
      handArmorItemPlaceholder.RemoveItem();
    }

    if (equipments.LegArmor != null) {
      legArmorItemPlaceholder.EquipItem(equipments.LegArmor);
    }
    else {
      legArmorItemPlaceholder.RemoveItem();
    }

    if (equipments.FootArmor != null) {
      footArmorItemPlaceholder.EquipItem(equipments.FootArmor);
    }
    else {
      footArmorItemPlaceholder.RemoveItem();
    }

    if (equipments.Necklace != null) {
      necklaceItemPlaceholder.EquipItem(equipments.Necklace);
    }
    else {
      necklaceItemPlaceholder.RemoveItem();
    }

    if (equipments.Ring1 != null) {
      ring1ItemPlaceholder.EquipItem(equipments.Ring1);
    }
    else {
      ring1ItemPlaceholder.RemoveItem();
    }

    if (equipments.Ring2 != null) {
      ring2ItemPlaceholder.EquipItem(equipments.Ring2);
    }
    else {
      ring2ItemPlaceholder.RemoveItem();
    }
  }

  public EquipmentPlaceholder CheckPlaceholderHover(Vector2 pos, ItemSO item) {
    if (item == null) {
      return null;
    }

    // Check right hand placeholder
    if (rightHandPlaceholder.IsHover(pos)) {
      if (rightHandPlaceholder.IsAllowed(item)) {
        rightHandPlaceholder.AllowedIndicator();
        return rightHandPlaceholder;
      }
      rightHandPlaceholder.DisallowedIndicator();

      return null;
    }
    else {
      rightHandPlaceholder.DefaultIndicator();
    }

    // Check left hand placeholder
    if (leftHandPlaceholder.IsHover(pos)) {
      if (leftHandPlaceholder.IsAllowed(item)) {
        leftHandPlaceholder.AllowedIndicator();
        return leftHandPlaceholder;
      }
      leftHandPlaceholder.DisallowedIndicator();

      return null;
    }
    else {
      leftHandPlaceholder.DefaultIndicator();
    }

    // Check head armor placeholder
    if (headArmorItemPlaceholder.IsHover(pos)) {
      if (headArmorItemPlaceholder.IsAllowed(item)) {
        headArmorItemPlaceholder.AllowedIndicator();
        return headArmorItemPlaceholder;
      }
      headArmorItemPlaceholder.DisallowedIndicator();

      return null;
    }
    else {
      headArmorItemPlaceholder.DefaultIndicator();
    }

    // Check body armor placeholder
    if (bodyArmorItemPlaceholder.IsHover(pos)) {
      if (bodyArmorItemPlaceholder.IsAllowed(item)) {
        bodyArmorItemPlaceholder.AllowedIndicator();
        return bodyArmorItemPlaceholder;
      }
      bodyArmorItemPlaceholder.DisallowedIndicator();

      return null;
    }
    else {
      bodyArmorItemPlaceholder.DefaultIndicator();
    }

    // Check shoulder armor placeholder
    if (shoulderArmorItemPlaceholder.IsHover(pos)) {
      if (shoulderArmorItemPlaceholder.IsAllowed(item)) {
        shoulderArmorItemPlaceholder.AllowedIndicator();
        return shoulderArmorItemPlaceholder;
      }
      shoulderArmorItemPlaceholder.DisallowedIndicator();

      return null;
    }
    else {
      shoulderArmorItemPlaceholder.DefaultIndicator();
    }

    // TODO
    // check hand armor placeholder
    // check leg armor placeholder
    // check foot armor placeholder

    return null;
  }

  private EquipmentPlaceholder GetPlaceholder(EquipmentPlaceholderTypes placeholderType) {
    switch (placeholderType) {
      case EquipmentPlaceholderTypes.HEAD_ARMOR:
        return headArmorItemPlaceholder;
      case EquipmentPlaceholderTypes.NECKLACE:
        return necklaceItemPlaceholder;
      case EquipmentPlaceholderTypes.RING1:
        return ring1ItemPlaceholder;
      case EquipmentPlaceholderTypes.RING2:
        return ring2ItemPlaceholder;
      case EquipmentPlaceholderTypes.RIGHT_HAND_WEAPON:
        return rightHandPlaceholder;
      case EquipmentPlaceholderTypes.LEFT_HAND_WEAPON:
        return leftHandPlaceholder;
      case EquipmentPlaceholderTypes.BODY_ARMOR:
        return bodyArmorItemPlaceholder;
      case EquipmentPlaceholderTypes.SHOULDER_ARMOR:
        return shoulderArmorItemPlaceholder;
      case EquipmentPlaceholderTypes.HAND_ARMOR:
        return handArmorItemPlaceholder;
      case EquipmentPlaceholderTypes.LEG_ARMOR:
        return legArmorItemPlaceholder;
      case EquipmentPlaceholderTypes.FOOT_ARMOR:
        return footArmorItemPlaceholder;
      default:
        throw new Exception("EquimentPanelItems type not found");
    }
  }
}



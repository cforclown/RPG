using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum EquipPlaceholderTypes {
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

  public bool IsDragging { get; private set; }

  private void Awake() {
    if (I == null) {
      I = this;
    }
  }

  public void Init(PlayerEquipment equipments) {
    if (equipments.HeadArmor != null) {
      headArmorItemPlaceholder.EquipItem(equipments.HeadArmor);
    }
    else {
      headArmorItemPlaceholder.ResetPlaceholder();
    }

    if (equipments.BodyArmor != null) {
      bodyArmorItemPlaceholder.EquipItem(equipments.BodyArmor);
    }
    else {
      bodyArmorItemPlaceholder.ResetPlaceholder();
    }

    if (equipments.RightHandWeapon != null) {
      rightHandPlaceholder.EquipItem(equipments.RightHandWeapon);
    }
    else {
      rightHandPlaceholder.ResetPlaceholder();
    }

    if (equipments.LeftHandWeapon != null) {
      leftHandPlaceholder.EquipItem(equipments.LeftHandWeapon);
    }
    else {
      leftHandPlaceholder.ResetPlaceholder();
    }

    if (equipments.ShoulderArmor != null) {
      shoulderArmorItemPlaceholder.EquipItem(equipments.ShoulderArmor);
    }
    else {
      shoulderArmorItemPlaceholder.ResetPlaceholder();
    }

    if (equipments.HandArmor != null) {
      handArmorItemPlaceholder.EquipItem(equipments.HandArmor);
    }
    else {
      handArmorItemPlaceholder.ResetPlaceholder();
    }

    if (equipments.LegArmor != null) {
      legArmorItemPlaceholder.EquipItem(equipments.LegArmor);
    }
    else {
      legArmorItemPlaceholder.ResetPlaceholder();
    }

    if (equipments.FootArmor != null) {
      footArmorItemPlaceholder.EquipItem(equipments.FootArmor);
    }
    else {
      footArmorItemPlaceholder.ResetPlaceholder();
    }

    if (equipments.Necklace != null) {
      necklaceItemPlaceholder.EquipItem(equipments.Necklace);
    }
    else {
      necklaceItemPlaceholder.ResetPlaceholder();
    }

    if (equipments.Ring1 != null) {
      ring1ItemPlaceholder.EquipItem(equipments.Ring1);
    }
    else {
      ring1ItemPlaceholder.ResetPlaceholder();
    }

    if (equipments.Ring2 != null) {
      ring2ItemPlaceholder.EquipItem(equipments.Ring2);
    }
    else {
      ring2ItemPlaceholder.ResetPlaceholder();
    }

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

  public void EquipItem(EquipPlaceholderTypes placeholderType, ItemSO item) {
    switch (placeholderType) {
      case EquipPlaceholderTypes.HEAD_ARMOR:
        headArmorItemPlaceholder.EquipItem(item);
        break;
      case EquipPlaceholderTypes.BODY_ARMOR:
        bodyArmorItemPlaceholder.EquipItem(item);
        break;
      case EquipPlaceholderTypes.RIGHT_HAND_WEAPON:
        rightHandPlaceholder.EquipItem(item);
        break;
      case EquipPlaceholderTypes.LEFT_HAND_WEAPON:
        leftHandPlaceholder.EquipItem(item);
        break;
      case EquipPlaceholderTypes.SHOULDER_ARMOR:
        shoulderArmorItemPlaceholder.EquipItem(item);
        break;
      case EquipPlaceholderTypes.HAND_ARMOR:
        handArmorItemPlaceholder.EquipItem(item);
        break;
      case EquipPlaceholderTypes.LEG_ARMOR:
        legArmorItemPlaceholder.EquipItem(item);
        break;
      case EquipPlaceholderTypes.FOOT_ARMOR:
        footArmorItemPlaceholder.EquipItem(item);
        break;
      case EquipPlaceholderTypes.NECKLACE:
        necklaceItemPlaceholder.EquipItem(item);
        break;
      case EquipPlaceholderTypes.RING1:
        ring1ItemPlaceholder.EquipItem(item);
        break;
      case EquipPlaceholderTypes.RING2:
        ring2ItemPlaceholder.EquipItem(item);
        break;
      default:
        throw new Exception("placeholderType type not found");
    }
    Player.I.Character.EquipItem(placeholderType, item);
  }

  public void UnequipItem(EquipPlaceholderTypes placeholderType, ItemSO item) {
    Player.I.Character.UnequipItem(placeholderType, item);
  }


  public Vector2 GetItemPlaceholderPos(EquipPlaceholderTypes placeholder) {
    switch (placeholder) {
      case EquipPlaceholderTypes.HEAD_ARMOR:
        return headArmorItemPlaceholder.GetComponentInParent<RectTransform>().position;
      case EquipPlaceholderTypes.BODY_ARMOR:
        return bodyArmorItemPlaceholder.GetComponentInParent<RectTransform>().position;
      case EquipPlaceholderTypes.RIGHT_HAND_WEAPON:
        return rightHandPlaceholder.GetComponentInParent<RectTransform>().position;
      case EquipPlaceholderTypes.LEFT_HAND_WEAPON:
        return leftHandPlaceholder.GetComponentInParent<RectTransform>().position;
      case EquipPlaceholderTypes.SHOULDER_ARMOR:
        return shoulderArmorItemPlaceholder.GetComponentInParent<RectTransform>().position;
      case EquipPlaceholderTypes.HAND_ARMOR:
        return handArmorItemPlaceholder.GetComponentInParent<RectTransform>().position;
      case EquipPlaceholderTypes.LEG_ARMOR:
        return legArmorItemPlaceholder.GetComponentInParent<RectTransform>().position;
      case EquipPlaceholderTypes.FOOT_ARMOR:
        return footArmorItemPlaceholder.GetComponentInParent<RectTransform>().position;
      case EquipPlaceholderTypes.NECKLACE:
        return necklaceItemPlaceholder.GetComponentInParent<RectTransform>().position;
      case EquipPlaceholderTypes.RING1:
        return ring1ItemPlaceholder.GetComponentInParent<RectTransform>().position;
      case EquipPlaceholderTypes.RING2:
        return ring2ItemPlaceholder.GetComponentInParent<RectTransform>().position;
      default:
        throw new Exception("EquimentPanelItems type not found");
    }
  }

  public void OnPlaceholderStartHover(BaseEventData data) {
    data.selectedObject.GetComponent<Image>().color = new Color(0.8f, 0f, 0f, 0.5f);
  }
  public void OnPlaceholderExitHover(BaseEventData data) {
    data.selectedObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.5f);
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

    return null;
  }
}



using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentPlaceholder : MonoBehaviour {
  [SerializeField] private EquipPlaceholderTypes placeholderType;
  public EquipPlaceholderTypes PlaceholderType { get { return placeholderType; } }

  private Image placeholderImg;
  private Color defaultColor;

  [SerializeField] private Image itemImg;
  [SerializeField] private Image itemImgDragging;
  private ItemSO _item;
  public ItemSO Item {
    get { return _item; }
    private set {
      if (value != null) {
        itemImg.sprite = value.Sprite;
        itemImg.gameObject.SetActive(true);
        itemImgDragging.sprite = value.Sprite;
      }
      else {
        itemImg.sprite = null;
        itemImg.gameObject.SetActive(false);
        itemImgDragging.sprite = null;
      }
      _item = value;
    }
  }

  private void Awake() {
    placeholderImg = GetComponent<Image>();
    defaultColor = placeholderImg.color;
  }

  public void ResetPlaceholder() {
    Item = null;
    DefaultIndicator();
  }

  public void EquipItem(ItemSO item) {
    Item = item;
    DefaultIndicator();
  }

  public void UnequipItem() {
    EquipmentPanelManager.I.UnequipItem(placeholderType, Item);
    ResetPlaceholder();
  }

  public void OnBeginDrag() {
    if (Item == null) {
      return;
    }

    itemImgDragging.gameObject.SetActive(true);
  }

  public void OnDrag(BaseEventData eventData) {
    if (Item == null) {
      return;
    }

    Vector2 mousePos = ((PointerEventData)eventData).position;
    Vector2 pos;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(
      GameUIManager.I.GetRectTransform(),
      mousePos,
      GameUIManager.I.Canvas.worldCamera,
      out pos
    );

    itemImgDragging.GetComponent<RectTransform>().position = GameUIManager.I.GetRectTransform().TransformPoint(pos);
    InventoryPanelManager.I.CheckInventoryTileHover(mousePos, Item);
  }

  public void OnEndDrag(BaseEventData eventData) {
    if (Item == null) {
      return;
    }

    itemImgDragging.gameObject.SetActive(false);
    itemImgDragging.GetComponent<RectTransform>().position = itemImg.GetComponent<RectTransform>().position;

    InventoryTile inventoryTile = InventoryPanelManager.I.CheckInventoryTileHover(((PointerEventData)eventData).position, Item);
    if (inventoryTile == null) {
      return;
    }
    InventoryPanelManager.I.AddItem(Item, inventoryTile.Location);
    UnequipItem();
  }

  public void DefaultIndicator() {
    placeholderImg.color = defaultColor;
  }

  public void AllowedIndicator() {
    placeholderImg.color = new Color(0f, 0.7f, 0f, 0.4f);
  }

  public void DisallowedIndicator() {
    placeholderImg.color = new Color(0.7f, 0f, 0f, 0.4f);
  }

  public void DraggingIndicator() {
    placeholderImg.color = new Color(0.5f, 0.5f, 0f, 0.4f);
  }

  public bool IsHover(Vector2 pos) {
    return RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), pos, Camera.main);
  }

  public bool IsAllowed(ItemSO item) {
    if (item == null) {
      return false;
    }

    if (
      (
        (item.Type == ItemTypes.WEAPON || item.Type == ItemTypes.SHIELD) &&
        (PlaceholderType == EquipPlaceholderTypes.RIGHT_HAND_WEAPON || PlaceholderType == EquipPlaceholderTypes.LEFT_HAND_WEAPON)
      ) ||
      (
        item.Type == ItemTypes.ARMOR &&
        ((ArmorSO)item).ArmorType == ArmorTypes.HEAD &&
        PlaceholderType == EquipPlaceholderTypes.HEAD_ARMOR
      ) ||
      (
        item.Type == ItemTypes.ARMOR &&
        ((ArmorSO)item).ArmorType == ArmorTypes.BODY &&
        PlaceholderType == EquipPlaceholderTypes.BODY_ARMOR
      ) ||
      (
        item.Type == ItemTypes.ARMOR &&
        ((ArmorSO)item).ArmorType == ArmorTypes.SHOULDER &&
        PlaceholderType == EquipPlaceholderTypes.SHOULDER_ARMOR
      )
    ) {
      return true;
    }

    return false;
  }
}

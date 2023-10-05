using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemPlaceholder : MonoBehaviour {
  [SerializeField] private Sprite defaultSprite;
  [SerializeField] private Image itemImg;
  [SerializeField] private Image itemImgCopy;

  private Image placeholderImg;
  private Color placeholderImgDefaultColor;

  private ItemSO item;
  public ItemSO Item {
    get { return item; }
    set {
      if (value != null) {
        itemImg.sprite = value.Sprite;
        itemImg.gameObject.SetActive(true);
        itemImgCopy.sprite = value.Sprite;
      }
      else {
        itemImg.sprite = defaultSprite;
        itemImg.gameObject.SetActive(false);
        itemImgCopy.sprite = defaultSprite;
      }
      SetDefaultColor();
      item = value;
    }
  }

  public int Location { get; private set; }

  private void Awake() {
    placeholderImg = GetComponent<Image>();
    placeholderImgDefaultColor = placeholderImg.color;
  }

  private void Start() {
    SetDefaultColor();
  }

  public void SetLocation(int location) {
    Location = location;
  }

  // event trigger
  public void OnBeginDrag() {
    if (Item == null) {
      return;
    }

    SetOnDraggingColor();
    itemImgCopy.gameObject.SetActive(true);
  }

  // event trigger
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

    itemImgCopy.GetComponent<RectTransform>().position = GameUIManager.I.GetRectTransform().TransformPoint(pos);
    EquipmentPanelManager.I.CheckPlaceholderHover(mousePos, Item);
    InventoryPanelManager.I.CheckPlaceholderHover(mousePos, Item);
  }

  // event trigger
  public void OnEndDrag(BaseEventData eventData) {
    if (Item == null) {
      return;
    }

    SetDefaultColor();
    itemImgCopy.gameObject.SetActive(false);
    itemImgCopy.GetComponent<RectTransform>().position = itemImg.GetComponent<RectTransform>().position;

    Vector2 mousePos = ((PointerEventData)eventData).position;
    EquipmentPlaceholder equipmentPlaceholder = EquipmentPanelManager.I.CheckPlaceholderHover(mousePos, Item);
    // item dragged into equipment placeholder box
    if (equipmentPlaceholder != null) {
      if (equipmentPlaceholder.Item != null) {
        InventoryPanelManager.AddItemAction(new InventoryItem(equipmentPlaceholder.Item, InventoryPanelManager.I.GetAvailableLocation()));
        EquipmentPanelManager.UnequipItemAction(equipmentPlaceholder.Item, equipmentPlaceholder.PlaceholderType);
      }
      EquipmentPanelManager.EquipItemAction(Item, equipmentPlaceholder.PlaceholderType);
      InventoryPanelManager.RemoveItemAction(new InventoryItem(Item, Location));
      return;
    }

    // item dragged into another inventory placeholder
    InventoryItemPlaceholder anotherPlaceholder = InventoryPanelManager.I.CheckPlaceholderHover(mousePos, Item);
    if (anotherPlaceholder != null) {
      if (anotherPlaceholder.Item != null) {
        ItemSO anotherPlaceholderItem = anotherPlaceholder.Item.Clone();
        InventoryPanelManager.RemoveItemAction(new InventoryItem(anotherPlaceholder.Item, anotherPlaceholder.Location));
        InventoryPanelManager.AddItemAction(new InventoryItem(Item, anotherPlaceholder.Location));
        InventoryPanelManager.RemoveItemAction(new InventoryItem(Item, Location));
        InventoryPanelManager.AddItemAction(new InventoryItem(anotherPlaceholderItem, Location));
      }
      else {
        InventoryPanelManager.AddItemAction(new InventoryItem(Item, anotherPlaceholder.Location));
        InventoryPanelManager.RemoveItemAction(new InventoryItem(Item, Location));
      }
      return;
    }
  }

  public void Selected() {
    placeholderImg.color = new Color(0f, 0.7f, 0f, 0.4f);
  }

  public void SetDefaultColor() {
    placeholderImg.color = placeholderImgDefaultColor;
  }

  public void SetOnHoverColor() {
    placeholderImg.color = new Color(0f, 0.7f, 0f, 0.4f);
  }

  public void SetOnDraggingColor() {
    placeholderImg.color = new Color(0.5f, 0.5f, 0f, 0.4f);
  }

  public bool IsHover(Vector2 pos) {
    return RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), pos, Camera.main);
  }
}

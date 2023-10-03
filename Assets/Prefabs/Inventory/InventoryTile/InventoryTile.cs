using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryTile : MonoBehaviour {
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
      DefaultIndicator();
      item = value;
    }
  }

  public int Location { get; private set; }

  private void Awake() {
    placeholderImg = GetComponent<Image>();
    placeholderImgDefaultColor = placeholderImg.color;
  }

  private void Start() {
    DefaultIndicator();
  }

  public void SetLocation(int location) {
    Location = location;
  }

  public void OnBeginDrag() {
    if (Item == null) {
      return;
    }

    DraggingIndicator();
    itemImgCopy.gameObject.SetActive(true);
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

    itemImgCopy.GetComponent<RectTransform>().position = GameUIManager.I.GetRectTransform().TransformPoint(pos);
    EquipmentPanelManager.I.CheckPlaceholderHover(mousePos, Item);
    InventoryPanelManager.I.CheckInventoryTileHover(mousePos, Item);
  }

  public void OnEndDrag(BaseEventData eventData) {
    if (Item == null) {
      return;
    }

    DefaultIndicator();
    itemImgCopy.gameObject.SetActive(false);
    itemImgCopy.GetComponent<RectTransform>().position = itemImg.GetComponent<RectTransform>().position;

    Vector2 mousePos = ((PointerEventData)eventData).position;
    EquipmentPlaceholder equipmentPlaceholder = EquipmentPanelManager.I.CheckPlaceholderHover(mousePos, Item);
    if (equipmentPlaceholder != null) {
      if (equipmentPlaceholder.Item != null) {
        EquipmentPanelManager.I.UnequipItem(equipmentPlaceholder.PlaceholderType, equipmentPlaceholder.Item);
        InventoryPanelManager.I.AddItem(equipmentPlaceholder.Item, InventoryPanelManager.I.GetAvailableLocation());
      }
      EquipmentPanelManager.I.EquipItem(equipmentPlaceholder.PlaceholderType, Item);
      InventoryPanelManager.I.RemoveItem(Item, Location);
      return;
    }
    InventoryTile anotherTilePlaceholder = InventoryPanelManager.I.CheckInventoryTileHover(mousePos, Item);
    if (anotherTilePlaceholder != null) {
      InventoryPanelManager.I.AddItem(Item, anotherTilePlaceholder.Location);
      InventoryPanelManager.I.RemoveItem(Item, Location);
      return;
    }
  }

  public void Selected() {
    placeholderImg.color = new Color(0f, 0.7f, 0f, 0.4f);
  }

  public void DefaultIndicator() {
    placeholderImg.color = placeholderImgDefaultColor;
  }

  public void HoverIndicator() {
    placeholderImg.color = new Color(0f, 0.7f, 0f, 0.4f);
  }

  public void DraggingIndicator() {
    placeholderImg.color = new Color(0.5f, 0.5f, 0f, 0.4f);
  }

  public bool IsHover(Vector2 pos) {
    return RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), pos, Camera.main);
  }

  public bool IsEmpty() {
    return Item == null;
  }
}

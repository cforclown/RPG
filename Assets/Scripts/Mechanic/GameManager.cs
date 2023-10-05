using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {
  public static GameManager I;

  [SerializeField] private Transform playerRespawnPoint;
  [SerializeField] private GameObject playerPrefab;

  private Player player;

  // for testing ------------------------------
  public MeleeWeaponSO testLongSword;
  public ArmorSO testSpartanHelm;
  public ArmorSO testKnightHelm;
  public ArmorSO testKnightShoulderArmor;
  public ArmorSO testBodyArmor1;
  public MeleeWeaponSO testMace1;
  // ------------------------------------------

  void Awake() {
    I = this;
  }

  void Start() {
    GameUIManager.I.CloseScreen(false);
    Respawn();
  }

  private Player InstantiatePlayer() {
    GameObject playerObj = Instantiate(playerPrefab);
    playerObj.transform.position = playerRespawnPoint.position;
    Player player = playerObj.GetComponent<Player>();

    PlayerInventory inventory = new PlayerInventory(new InventoryItem[] {
      new InventoryItem(testSpartanHelm, 0),
      new InventoryItem(testKnightHelm, 1),
      new InventoryItem(testKnightShoulderArmor, 2),
      new InventoryItem(testBodyArmor1, 3),
      new InventoryItem(testMace1, 4)
    });
    PlayerEquipment equipments = new PlayerEquipment();
    PlayerQuests quests = new PlayerQuests(null);
    player.Character.Init(new Character(
      "Hafis",
      10,
      10,
      10,
      inventory,
      equipments,
      quests,
      new PlayerSkills()
    ));
    player.Character.EquipItem(testLongSword, EquipmentPlaceholderTypes.RIGHT_HAND_WEAPON);

    return playerObj.GetComponent<Player>();
  }

  public void Respawn() {
    StartCoroutine(RespawnAnim());
  }

  private IEnumerator RespawnAnim() {
    yield return new WaitForSeconds(player != null ? 2f : 0.1f);
    GameUIManager.I.CloseScreen(player != null ? true : false);

    if (player != null) {
      Destroy(player.gameObject);
    }
    yield return new WaitForSeconds(player != null ? 2f : 0.5f);

    player = InstantiatePlayer();
    CameraController.I.Init(player.gameObject.transform);

    yield return new WaitForSeconds(0.5f);

    GameUIManager.I.OpenScreen();
  }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CharacterManager : MonoBehaviour {
  public PlayerAnimController AnimController { get; private set; }

  [SerializeField] private NoWeaponCollider rightHandAttackCollider;
  [SerializeField] private NoWeaponCollider leftHandAttackColliderl;

  [SerializeField] private ParticleSystem levelUpParticle;

  [SerializeField] private Transform rightHandEquipParent;
  [SerializeField] private Transform leftHandEquipParent;
  [SerializeField] private Transform headArmorEquipParent;
  [SerializeField] private Transform bodyArmorEquipParent;
  [SerializeField] private Transform rightShoulderArmorEquipParent;
  [SerializeField] private Transform leftShoulderArmorEquipParent;
  [SerializeField] private Transform necklaceEquipParent;
  [SerializeField] private Transform ring1EquipParent;
  [SerializeField] private Transform ring2EquipParent;
  [SerializeField] private Transform handArmorEquipParent;
  [SerializeField] private Transform legArmorEquipParent;
  [SerializeField] private Transform footArmorEquipParent;
  private List<Transform> placeholders;

  public Character Stats { get; private set; }


  private void Awake() {
    AnimController = GetComponent<PlayerAnimController>();

    placeholders = new List<Transform>(11) {
      headArmorEquipParent,
      bodyArmorEquipParent,
      rightHandEquipParent,
      leftShoulderArmorEquipParent,
      rightShoulderArmorEquipParent,
      handArmorEquipParent,
      legArmorEquipParent,
      footArmorEquipParent,
      necklaceEquipParent,
      ring1EquipParent,
      ring2EquipParent
    };
  }

  private void Start() {
    StartCoroutine(HPRegenCoroutine());
    StartCoroutine(MPRegenCoroutine());

    rightHandAttackCollider.Init((Collider collider) => OnAttackHit(collider));
    leftHandAttackColliderl.Init((Collider collider) => OnAttackHit(collider));

    NPCEvents.OnPlayerAcceptNPCQuest += OnQuestAccepted;
    QuestEvents.OnQuestFinished += OnQuestFinished;
    SkillPanelBtn.OnSkillPanelBtnPressed += ClaimSkill;
    InventoryPanelManager.OnAddItemAction += AddItem;
    InventoryPanelManager.OnRemoveItemAction += RemoveItem;
    EquipmentPanelManager.OnEquipItemAction += EquipItem;
    EquipmentPanelManager.OnUnequipItemAction += UnequipItem;
    CombatEvents.OnEnemyAttackHitPlayer += GetHit;
    CombatEvents.OnEnemyDeath += OnEnemyKilledEvent;
    PlayerEvents.OnIncreaseStrengthAction += IncreaseStrength;
    PlayerEvents.OnIncreaseAgilityAction += IncreaseAgility;
    PlayerEvents.OnIncreaseIntelligenceAction += IncreaseIntelligence;
  }

  private void OnDestroy() {
    StopCoroutine(HPRegenCoroutine());
    StopCoroutine(MPRegenCoroutine());

    NPCEvents.OnPlayerAcceptNPCQuest -= OnQuestAccepted;
    QuestEvents.OnQuestFinished -= OnQuestFinished;
    SkillPanelBtn.OnSkillPanelBtnPressed -= ClaimSkill;
    InventoryPanelManager.OnAddItemAction -= AddItem;
    InventoryPanelManager.OnRemoveItemAction -= RemoveItem;
    EquipmentPanelManager.OnEquipItemAction -= EquipItem;
    EquipmentPanelManager.OnUnequipItemAction -= UnequipItem;
    CombatEvents.OnEnemyAttackHitPlayer -= GetHit;
    CombatEvents.OnEnemyDeath -= OnEnemyKilledEvent;
    PlayerEvents.OnIncreaseStrengthAction -= IncreaseStrength;
    PlayerEvents.OnIncreaseAgilityAction -= IncreaseAgility;
    PlayerEvents.OnIncreaseIntelligenceAction -= IncreaseIntelligence;
  }

  public void Init(Character player) {
    Stats = player;
    PlayerEvents.PlayerStatsUpdated(player);
    PlayerEvents.PlayerEquipmentUpdated(player.Equipment);
    PlayerEvents.PlayerInventoryUpdated(player.Inventory);
    PlayerEvents.PlayerSkillsUpdated(player.Skills);
  }

  #region STATS MUTATION
  private void HealHP(int heal) {
    Stats.HealHP(heal);
    PlayerEvents.PlayerStatsUpdated(Stats);
  }

  private void CastSpell(int amount) {
    Stats.ComsumeMP(amount);
    PlayerEvents.PlayerStatsUpdated(Stats);
  }

  private void HealMP(int amount) {
    Stats.HealMP(amount);
    PlayerEvents.PlayerStatsUpdated(Stats);
  }

  private void GrantExp(int amount) {
    int prevLevel = Stats.Level;
    Stats.GainExp(amount);
    if (prevLevel < Stats.Level) {
      Stats.LevelUp();
      StartCoroutine(LevelUpAnim());
    }
    PlayerEvents.PlayerStatsUpdated(Stats);
  }

  public void IncreaseStrength() {
    Stats.IncreaseStrength();
    PlayerEvents.PlayerStatsUpdated(Stats);
  }

  public void IncreaseAgility() {
    Stats.IncreaseAgility();
    PlayerEvents.PlayerStatsUpdated(Stats);
  }

  public void IncreaseIntelligence() {
    Stats.IncreaseIntelligence();
    PlayerEvents.PlayerStatsUpdated(Stats);
  }
  #endregion



  #region QUESTs
  private void OnQuestAccepted(NPC npc) {
    Stats.Quests.AddQuest(npc.data.Quest);
  }

  private void OnQuestFinished(QuestSO quest) {
    GrantExp(quest.ExpReward);
    Stats.Quests.RemoveQuest(quest);
  }
  #endregion



  #region SKILL RELATED METHODS
  private void ClaimSkill(SkillSO skill) {
    SkillSO currentSkill = Stats.Skills.Skills.Find(s => s.Id == skill.Id);
    if (currentSkill == null) {
      currentSkill = skill.Clone();
      Stats.ClaimSkill(currentSkill);
    }
    currentSkill.LevelUp();
    PlayerEvents.PlayerSkillsUpdated(Stats.Skills);
    PlayerEvents.PlayerStatsUpdated(Stats);
  }
  public SkillSO GetPerformedSkill() {
    if (AnimController.IsSkill == 0) {
      return null;
    }

    return Stats.Skills.Skills.Find(s => s.SkillType == (SkillTypes)AnimController.IsSkill);
  }
  #endregion



  #region COMBAT
  private void OnEnemyKilledEvent(Enemy enemy) => GrantExp(enemy.Stats.KilledExp);

  private void GetHit(CharacterManager player, Enemy enemy, WeaponSO weapon) {
    // TODO calculate enemy damage against player armor
    int damage = enemy.Stats.Damage;

    Stats.GetHit(damage);
    CombatEvents.PostEnemyAttackHitPlayerEvent(this, enemy, weapon);
    if (Stats.HP <= 0) {
      AnimController.Dead();
      GameManager.I.Respawn();
    }
  }

  private void OnTriggerEnter(Collider collider) {
    // player already dead
    if (Stats.HP <= 0) {
      return;
    }

    GameObject collideObj = collider.gameObject;
    if (collideObj == null) {
      return;
    }
    EnemyAttackCollider enemyAttackCollider = collider.GetComponent<EnemyAttackCollider>();
    if (enemyAttackCollider == null) {
      return;
    }
    if (!enemyAttackCollider.IsAttacking() || enemyAttackCollider.IsAttackLanded()) {
      return;
    }

    enemyAttackCollider.AttackLanded();
    CombatEvents.EnemyAttackHitPlayerEvent(this, enemyAttackCollider.controller, null);
  }

  private void OnAttackHit(Collider collider, WeaponSO weapon = null) {
    GameObject collideObj = collider.gameObject;
    if (collideObj == null) {
      return;
    }
    Enemy enemy = collider.GetComponent<Enemy>();
    if (enemy == null) {
      return;
    }

    if (
      !AnimController.IsAttackLaunched ||
      enemy.Stats.HP <= 0
    ) {
      return;
    }

    if (AnimController.IsSkill == 0) {
      AnimController.BasicAttackHit();
    }
    else if (SkillCalculator.CheckHitEnemy(enemy)) {
      return;
    }

    CombatEvents.PlayerAttackHitEnemyEvent(this, enemy, weapon);
  }

  public int GetDamageOutput(Enemy enemy, WeaponSO weapon = null) {
    int damage = 0;
    if (weapon == null) {
      damage = Stats.UnarmedDamage;
    }
    else if (AnimController.IsSkill != 0) {
      SkillSO skill = Stats.Skills.Skills.Find(s => s.SkillType == (SkillTypes)AnimController.IsSkill);
      if (skill == null) {
        damage = GetWeaponDamage(weapon);
      }
      else {
        damage = skill.Damage;
      }
    }
    else {
      damage = GetWeaponDamage(weapon);
    }

    return CalculateDamage(damage, enemy);
  }

  private int GetWeaponDamage(WeaponSO weapon) {
    return Generator.RandomInt(weapon.MinDamage, weapon.MaxDamage);
  }

  private int CalculateDamage(int playerDmg, Enemy enemy) {
    // TODO calculate player damage against enemy armor
    return playerDmg;
  }
  #endregion



  #region INVENTORY EVENTs
  public void AddItem(InventoryItem item) {
    Stats.AddItem(item);
    PlayerEvents.PlayerInventoryUpdated(Stats.Inventory);
  }

  public void RemoveItem(InventoryItem item) {
    Stats.RemoveItem(item);
    PlayerEvents.PlayerInventoryUpdated(Stats.Inventory);
  }
  #endregion



  #region EQUIPMENT EVENTs
  public void InitEquipments(PlayerEquipment equipments) {
    if (equipments.HeadArmor != null) {
      EquipItem(equipments.HeadArmor, EquipmentPlaceholderTypes.HEAD_ARMOR);
    }
    if (equipments.BodyArmor != null) {
      EquipItem(equipments.BodyArmor, EquipmentPlaceholderTypes.BODY_ARMOR);
    }
    if (equipments.RightHandWeapon != null) {
      EquipItem(equipments.RightHandWeapon, EquipmentPlaceholderTypes.RIGHT_HAND_WEAPON);
    }
    if (equipments.LeftHandWeapon != null) {
      EquipItem(equipments.LeftHandWeapon, EquipmentPlaceholderTypes.LEFT_HAND_WEAPON);
    }
    if (equipments.ShoulderArmor != null) {
      EquipItem(equipments.ShoulderArmor, EquipmentPlaceholderTypes.SHOULDER_ARMOR);
    }
    if (equipments.HandArmor != null) {
      EquipItem(equipments.HandArmor, EquipmentPlaceholderTypes.HAND_ARMOR);
    }
    if (equipments.LegArmor != null) {
      EquipItem(equipments.LegArmor, EquipmentPlaceholderTypes.LEG_ARMOR);
    }
    if (equipments.FootArmor != null) {
      EquipItem(equipments.FootArmor, EquipmentPlaceholderTypes.FOOT_ARMOR);
    }
    if (equipments.Necklace != null) {
      EquipItem(equipments.Necklace, EquipmentPlaceholderTypes.NECKLACE);
    }
    if (equipments.Ring1 != null) {
      EquipItem(equipments.Ring1, EquipmentPlaceholderTypes.RING1);
    }
    if (equipments.Ring2 != null) {
      EquipItem(equipments.Ring2, EquipmentPlaceholderTypes.RING2);
    }
  }

  public void EquipItem(ItemSO item, EquipmentPlaceholderTypes placeholderType) {
    if (item == null) {
      return;
    }

    Stats.Equipment.EquipItem(item, placeholderType);
    if (
      item.Type == ItemTypes.WEAPON &&
      ((WeaponSO)item).WeaponType == WeaponTypes.MELEE &&
      ((MeleeWeaponSO)item).MeleeWeaponType != MeleeWeaponTypes.GREAT_SWORD &&
      (
        placeholderType == EquipmentPlaceholderTypes.RIGHT_HAND_WEAPON ||
        placeholderType == EquipmentPlaceholderTypes.LEFT_HAND_WEAPON
      )
    ) {
      rightHandAttackCollider.Disable();
      leftHandAttackColliderl.Disable();
      GetComponent<PlayerAnimController>().OneHandedWeaponStyle();
    }

    // Instantiate item prefab
    AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(item.GetAssetPrefabName());
    asyncOperationHandle.Completed += (AsyncOperationHandle<GameObject> asyncOperationHandle) => {
      LoadItemPrefabOperationCompleted(asyncOperationHandle, placeholderType, item);
    };

    PlayerEvents.PlayerEquipmentUpdated(Stats.Equipment);
  }

  public void UnequipItem(ItemSO item, EquipmentPlaceholderTypes placeholderType) {
    if (
      item != null &&
      item.Type == ItemTypes.WEAPON &&
      (
        placeholderType == EquipmentPlaceholderTypes.RIGHT_HAND_WEAPON ||
        placeholderType == EquipmentPlaceholderTypes.LEFT_HAND_WEAPON
      ) &&
      !(
        Stats.Equipment.RightHandWeapon != null && Stats.Equipment.LeftHandWeapon != null
      )
    ) {
      rightHandAttackCollider.Enable();
      leftHandAttackColliderl.Enable();
      GetComponent<PlayerAnimController>().NoWeaponStyle();
    }

    Stats.Equipment.UnequipItem(placeholderType);
    RemoveItemGameObjectFromPlayer(placeholderType, item);

    PlayerEvents.PlayerEquipmentUpdated(Stats.Equipment);
  }

  private void LoadItemPrefabOperationCompleted(
    AsyncOperationHandle<GameObject> asyncOperationHandle,
    EquipmentPlaceholderTypes placeholderType,
    ItemSO item
  ) {
    if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded) {
      if (placeholderType == EquipmentPlaceholderTypes.RIGHT_HAND_WEAPON || placeholderType == EquipmentPlaceholderTypes.LEFT_HAND_WEAPON) {
        EquipWeapon(asyncOperationHandle.Result, placeholderType, item);
      }
      else if (placeholderType == EquipmentPlaceholderTypes.SHOULDER_ARMOR) {
        EquipShoulderArmor(asyncOperationHandle.Result, item);
      }
      else if (placeholderType == EquipmentPlaceholderTypes.HAND_ARMOR) {
        Debug.LogWarning("HAND ARMOR EQUIP NOT IMPLEMENTED YET!");
      }

      else if (placeholderType == EquipmentPlaceholderTypes.FOOT_ARMOR) {
        Debug.LogWarning("FOOT ARMOR EQUIP NOT IMPLEMENTED YET!");
      }
      else {
        GameObject obj = Instantiate(asyncOperationHandle.Result, placeholders[(int)placeholderType]);
        obj.name = item.Id;
      }
    }
    else {
      Debug.LogError("CharacterManager: failed to load item prefab");
    }
  }

  private void EquipWeapon(GameObject prefab, EquipmentPlaceholderTypes placeholder, ItemSO item) {
    WeaponSO weaponData = (WeaponSO)item;
    GameObject itemObj = Instantiate(
      prefab,
      placeholder == EquipmentPlaceholderTypes.RIGHT_HAND_WEAPON ? rightHandEquipParent : leftHandEquipParent
    );
    itemObj.name = item.Id;
    WeaponMono<WeaponSO> itemMono = itemObj.GetComponent<WeaponMono<WeaponSO>>();
    if (item.Type == ItemTypes.WEAPON && weaponData.WeaponType == WeaponTypes.MELEE) {
      itemMono.Init(weaponData, OnAttackHit);
    }
    else {
      itemMono.Init(weaponData);
    }
  }

  private void EquipShoulderArmor(GameObject prefab, ItemSO item) {
    GameObject rightArmor = Instantiate(prefab, rightShoulderArmorEquipParent);
    rightArmor.name = item.Id + "Right";
    rightArmor.transform.localPosition = Vector3.zero;
    rightArmor.transform.localEulerAngles = Vector3.zero;
    rightArmor.transform.localScale = Vector3.one;
    ArmorMono rightArmorMono = rightArmor.GetComponent<ArmorMono>();
    rightArmorMono.Init((ArmorSO)item);

    GameObject leftArmor = Instantiate(prefab, leftShoulderArmorEquipParent);
    leftArmor.name = item.Id + "Left";
    leftArmor.transform.localPosition = Vector3.zero;
    leftArmor.transform.localEulerAngles = Vector3.zero;
    leftArmor.transform.localScale = Vector3.one;
    ArmorMono leftArmorMono = leftArmor.GetComponent<ArmorMono>();
    leftArmorMono.Init((ArmorSO)item);
  }

  private void RemoveItemGameObjectFromPlayer(EquipmentPlaceholderTypes placeholderType, ItemSO item) {
    if (item == null) {
      return;
    }

    try {
      Transform itemObjTransform;
      switch (placeholderType) {
        case EquipmentPlaceholderTypes.RIGHT_HAND_WEAPON:
          itemObjTransform = rightHandEquipParent.Find(item.Id);
          if (itemObjTransform == null) {
            break;
          }
          Destroy(itemObjTransform.gameObject);
          break;
        case EquipmentPlaceholderTypes.LEFT_HAND_WEAPON:
          itemObjTransform = leftHandEquipParent.Find(item.Id);
          if (itemObjTransform == null) {
            break;
          }
          Destroy(itemObjTransform.gameObject);
          break;
        case EquipmentPlaceholderTypes.HEAD_ARMOR:
          itemObjTransform = headArmorEquipParent.Find(item.Id);
          if (itemObjTransform == null) {
            break;
          }
          Destroy(itemObjTransform.gameObject);
          break;
        case EquipmentPlaceholderTypes.BODY_ARMOR:
          itemObjTransform = bodyArmorEquipParent.Find(item.Id);
          if (itemObjTransform == null) {
            break;
          }
          Destroy(itemObjTransform.gameObject);
          break;
        case EquipmentPlaceholderTypes.SHOULDER_ARMOR:
          RemoveShoulderArmorGameObject(item);
          break;
        case EquipmentPlaceholderTypes.NECKLACE:
        case EquipmentPlaceholderTypes.RING1:
        case EquipmentPlaceholderTypes.RING2:
        case EquipmentPlaceholderTypes.HAND_ARMOR:
        case EquipmentPlaceholderTypes.LEG_ARMOR:
        case EquipmentPlaceholderTypes.FOOT_ARMOR:
          Debug.LogError("CharacterManager: Not implemented");
          break;
        default:
          throw new Exception("EquimentPanelItems type not found");
      }
    }
    catch (Exception e) {
      Debug.LogError(e.ToString());
    }
  }

  private void RemoveShoulderArmorGameObject(ItemSO item) {
    Transform rightShoulderArmor = rightShoulderArmorEquipParent.Find(item.Id + "Right");
    if (rightShoulderArmor == null) {
      return;
    }
    Destroy(rightShoulderArmor.gameObject);

    Transform leftShoulderArmor = leftShoulderArmorEquipParent.Find(item.Id + "Left");
    if (leftShoulderArmor == null) {
      return;
    }
    Destroy(leftShoulderArmor.gameObject);
  }
  #endregion



  #region TASKs
  private IEnumerator HPRegenCoroutine() {
    while (Stats.HP > 0) {
      yield return new WaitForSeconds(0.0001f);
      if (Stats.HP >= Stats.MaxHP) {
        continue;
      }
      HealHP((int)Stats.HPRegen);
      yield return new WaitForSeconds(1f);
    }
  }

  private IEnumerator MPRegenCoroutine() {
    while (Stats.HP > 0) {
      yield return new WaitForSeconds(0.0001f);
      if (Stats.MP >= Stats.MaxMP) {
        continue;
      }
      HealMP((int)Stats.MPRegen);
      yield return new WaitForSeconds(1f);
    }
  }

  private IEnumerator LevelUpAnim() {
    levelUpParticle.gameObject.SetActive(true);
    levelUpParticle.Play();
    yield return new WaitForSeconds(levelUpParticle.main.duration + 1f);
    levelUpParticle.gameObject.SetActive(false);
  }
  #endregion
}

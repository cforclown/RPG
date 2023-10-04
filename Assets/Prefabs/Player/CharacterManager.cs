using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CharacterManager : MonoBehaviour {
  private PlayerAnimStateController animController;

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
    animController = GetComponent<PlayerAnimStateController>();

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

    rightHandAttackCollider.Init(OnAttackHit);
    leftHandAttackColliderl.Init(OnAttackHit);

    CombatEvents.OnEnemyDeath += CombatEventsOnKillingEnemy;
    NPCEvents.OnPlayerAcceptNPCQuest += OnQuestAccepted;
    QuestEvents.OnQuestFinished += OnQuestDone;
    SkillBtn.OnSkillBtnPressed += ClaimSkill;
  }

  private void OnDestroy() {
    StopCoroutine(HPRegenCoroutine());
    StopCoroutine(MPRegenCoroutine());

    CombatEvents.OnEnemyDeath -= CombatEventsOnKillingEnemy;
    NPCEvents.OnPlayerAcceptNPCQuest -= OnQuestAccepted;
    QuestEvents.OnQuestFinished -= OnQuestDone;
    SkillBtn.OnSkillBtnPressed -= ClaimSkill;
  }

  public void Init(Character player) {
    Stats = player;
    InitEquipments(Stats.Equipment);
    PlayerEvents.PlayerStatsUpdated(player);
  }

  public float GetDamageOutput() {
    // TODO find out the damage coming from, right hand weapon or left hand weapon or unarmed damage

    return Stats.UnarmedDamage;
  }

  private void GetHit(Enemy enemy) {
    int rawDamage = enemy.Stats.Damage;
    // TODO calculate damage against armor

    Stats.GetHit(rawDamage);
    if (Stats.HP <= 0) {
      animController.Dead();
      GameManager.I.Respawn();
    }

    CombatEvents.AttackHit(transform, rawDamage);
    CombatEvents.EnemyHitPlayer(enemy, Stats);
  }

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

  private void CombatEventsOnKillingEnemy(Enemy enemy) => GrantExp(enemy.Stats.KilledExp);

  private void GrantExp(int amount) {
    int prevLevel = Stats.Level;
    Stats.GainExp(amount);
    if (prevLevel < Stats.Level) {
      Stats.LevelUp();
      StartCoroutine(LevelUpAnim());
    }
    PlayerEvents.PlayerStatsUpdated(Stats);
  }

  private void OnQuestAccepted(NPC npc) {
    Stats.Quests.AddQuest(npc.data.Quest);
  }

  private void OnQuestDone(QuestSO quest) {
    GrantExp(quest.ExpReward);
    Stats.Quests.RemoveQuest(quest);
  }

  private void ClaimSkill(SkillSO skill) {
    SkillSO currentSkill = Stats.Skills.Skills.Find(s => s.Id == skill.Id);
    if (currentSkill == null) {
      currentSkill = skill.Clone();
      Stats.Skills.Skills.Add(currentSkill);
    }
    currentSkill.LevelUp();
    PlayerEvents.PlayerSkillsUpdated(Stats.Skills);
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
    GetHit(enemyAttackCollider.controller);
  }

  private void OnAttackHit(Collider collider) {
    GameObject collideObj = collider.gameObject;
    if (collideObj == null) {
      return;
    }
    Enemy enemyController = collider.GetComponent<Enemy>();
    if (enemyController == null) {
      return;
    }
    if (
      animController.IsAttackHit ||
      enemyController.Stats.HP <= 0
    ) {
      return;
    }

    int damage = (int)GetDamageOutput();
    animController.AttackHit();
    enemyController.GetHit(damage);
  }

  private void OnMeleeWeaponHit(Collider collider, WeaponSO weapon) {
    GameObject collideObj = collider.gameObject;
    if (collideObj == null) {
      return;
    }
    Enemy enemyController = collider.GetComponent<Enemy>();
    if (enemyController == null) {
      return;
    }
    if (
      animController.IsAttackHit ||
      enemyController.Stats.HP <= 0
    ) {
      return;
    }

    animController.AttackHit();
    enemyController.GetHit(Generator.RandomInt(weapon.MinDamage, weapon.MaxDamage));
  }

  public void InitEquipments(PlayerEquipment equipments) {
    if (equipments.HeadArmor != null) {
      EquipItem(EquipPlaceholderTypes.HEAD_ARMOR, equipments.HeadArmor);
    }
    if (equipments.BodyArmor != null) {
      EquipItem(EquipPlaceholderTypes.BODY_ARMOR, equipments.BodyArmor);
    }
    if (equipments.RightHandWeapon != null) {
      EquipItem(EquipPlaceholderTypes.RIGHT_HAND_WEAPON, equipments.RightHandWeapon);
    }
    if (equipments.LeftHandWeapon != null) {
      EquipItem(EquipPlaceholderTypes.LEFT_HAND_WEAPON, equipments.LeftHandWeapon);
    }
    if (equipments.ShoulderArmor != null) {
      EquipItem(EquipPlaceholderTypes.SHOULDER_ARMOR, equipments.ShoulderArmor);
    }
    if (equipments.HandArmor != null) {
      EquipItem(EquipPlaceholderTypes.HAND_ARMOR, equipments.HandArmor);
    }
    if (equipments.LegArmor != null) {
      EquipItem(EquipPlaceholderTypes.LEG_ARMOR, equipments.LegArmor);
    }
    if (equipments.FootArmor != null) {
      EquipItem(EquipPlaceholderTypes.FOOT_ARMOR, equipments.FootArmor);
    }
    if (equipments.Necklace != null) {
      EquipItem(EquipPlaceholderTypes.NECKLACE, equipments.Necklace);
    }
    if (equipments.Ring1 != null) {
      EquipItem(EquipPlaceholderTypes.RING1, equipments.Ring1);
    }
    if (equipments.Ring2 != null) {
      EquipItem(EquipPlaceholderTypes.RING2, equipments.Ring2);
    }
  }

  public void EquipItem(EquipPlaceholderTypes placeholderType, ItemSO item) {
    if (item == null) {
      return;
    }

    Stats.Equipment.EquipItem(placeholderType, item);
    if (
      item.Type == ItemTypes.WEAPON &&
      ((WeaponSO)item).WeaponType == WeaponTypes.MELEE &&
      ((MeleeWeaponSO)item).MeleeWeaponType != MeleeWeaponTypes.GREAT_SWORD &&
      (
        placeholderType == EquipPlaceholderTypes.RIGHT_HAND_WEAPON ||
        placeholderType == EquipPlaceholderTypes.LEFT_HAND_WEAPON
      )
    ) {
      rightHandAttackCollider.Disable();
      leftHandAttackColliderl.Disable();
      GetComponent<PlayerAnimStateController>().OneHandedWeaponStyle();
    }

    // Instantiate item prefab
    AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(item.GetAssetPrefabName());
    asyncOperationHandle.Completed += (AsyncOperationHandle<GameObject> asyncOperationHandle) => {
      LoadItemPrefabOperationCompleted(asyncOperationHandle, placeholderType, item);
    };
  }

  public void UnequipItem(EquipPlaceholderTypes placeholderType, ItemSO item) {
    if (
      item != null &&
      item.Type == ItemTypes.WEAPON &&
      (
        placeholderType == EquipPlaceholderTypes.RIGHT_HAND_WEAPON ||
        placeholderType == EquipPlaceholderTypes.LEFT_HAND_WEAPON
      ) &&
      !(
        Stats.Equipment.RightHandWeapon != null && Stats.Equipment.LeftHandWeapon != null
      )
    ) {
      rightHandAttackCollider.Enable();
      leftHandAttackColliderl.Enable();
      GetComponent<PlayerAnimStateController>().NoWeaponStyle();
    }

    Stats.Equipment.UnequipItem(placeholderType);
    RemoveItemGameObjectFromPlayer(placeholderType, item);
  }

  private void LoadItemPrefabOperationCompleted(
    AsyncOperationHandle<GameObject> asyncOperationHandle,
    EquipPlaceholderTypes placeholderType,
    ItemSO item
  ) {
    if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded) {
      if (placeholderType == EquipPlaceholderTypes.RIGHT_HAND_WEAPON || placeholderType == EquipPlaceholderTypes.LEFT_HAND_WEAPON) {
        EquipWeapon(asyncOperationHandle.Result, placeholderType, item);
      }
      else if (placeholderType == EquipPlaceholderTypes.SHOULDER_ARMOR) {
        EquipShoulderArmor(asyncOperationHandle.Result, item);
      }
      else if (placeholderType == EquipPlaceholderTypes.HAND_ARMOR) {
        Debug.LogWarning("HAND ARMOR EQUIP NOT IMPLEMENTED YET!");
      }

      else if (placeholderType == EquipPlaceholderTypes.FOOT_ARMOR) {
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

  private void EquipWeapon(GameObject prefab, EquipPlaceholderTypes placeholder, ItemSO item) {
    WeaponSO weaponData = (WeaponSO)item;
    GameObject itemObj = Instantiate(
      prefab,
      placeholder == EquipPlaceholderTypes.RIGHT_HAND_WEAPON ? rightHandEquipParent : leftHandEquipParent
    );
    itemObj.name = item.Id;
    WeaponMono<WeaponSO> itemMono = itemObj.GetComponent<WeaponMono<WeaponSO>>();
    if (item.Type == ItemTypes.WEAPON && weaponData.WeaponType == WeaponTypes.MELEE) {
      itemMono.Init(weaponData, OnMeleeWeaponHit);
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

  private void RemoveItemGameObjectFromPlayer(EquipPlaceholderTypes placeholderType, ItemSO item) {
    if (item == null) {
      return;
    }

    try {
      Transform itemObjTransform;
      switch (placeholderType) {
        case EquipPlaceholderTypes.RIGHT_HAND_WEAPON:
          itemObjTransform = rightHandEquipParent.Find(item.Id);
          if (itemObjTransform == null) {
            break;
          }
          Destroy(itemObjTransform.gameObject);
          break;
        case EquipPlaceholderTypes.LEFT_HAND_WEAPON:
          itemObjTransform = leftHandEquipParent.Find(item.Id);
          if (itemObjTransform == null) {
            break;
          }
          Destroy(itemObjTransform.gameObject);
          break;
        case EquipPlaceholderTypes.HEAD_ARMOR:
          itemObjTransform = headArmorEquipParent.Find(item.Id);
          if (itemObjTransform == null) {
            break;
          }
          Destroy(itemObjTransform.gameObject);
          break;
        case EquipPlaceholderTypes.BODY_ARMOR:
          itemObjTransform = bodyArmorEquipParent.Find(item.Id);
          if (itemObjTransform == null) {
            break;
          }
          Destroy(itemObjTransform.gameObject);
          break;
        case EquipPlaceholderTypes.SHOULDER_ARMOR:
          RemoveShoulderArmorGameObject(item);
          break;
        case EquipPlaceholderTypes.NECKLACE:
        case EquipPlaceholderTypes.RING1:
        case EquipPlaceholderTypes.RING2:
        case EquipPlaceholderTypes.HAND_ARMOR:
        case EquipPlaceholderTypes.LEG_ARMOR:
        case EquipPlaceholderTypes.FOOT_ARMOR:
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
}

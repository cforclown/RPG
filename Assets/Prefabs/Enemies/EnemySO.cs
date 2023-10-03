using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObjects/EnemySO")]
public class EnemySO : ScriptableObject, IEnemySO {
  [field: SerializeField] public string AssetId { get; set; } = "<asset-id>";
  public string Id { get; set; } = "<enemy-id>";
  [field: SerializeField] public string Name { get; set; } = "<enemy-base-name>";

  [field: SerializeField] public EnemyType Type { get; private set; } = EnemyType.CREATURE;
  [field: SerializeField] public EnemyRace Race { get; private set; } = EnemyRace.MUTANT;
  [field: SerializeField] public EnemyBehaviour Behaviour { get; private set; } = EnemyBehaviour.AGGRESSIVE;
  [field: SerializeField] public EnemyIdleBehaviour IdleBehaviour { get; private set; } = EnemyIdleBehaviour.PATROL;

  public int Level { get; set; } = 1;

  [field: SerializeField] public int BaseMaxHP { get; private set; } = 100;
  [field: SerializeField] public int MaxHPPerLevel { get; private set; } = 10;
  [field: SerializeField] public float BaseHPRegen { get; private set; } = 1;
  [field: SerializeField] public float HPRegenPerLevel { get; private set; } = 0.1f;
  public int MaxHP { get; private set; }
  public float HPRegen { get; private set; }
  public int HP { get; private set; }

  [field: SerializeField] public int BaseMaxMP { get; private set; } = 0;
  [field: SerializeField] public int MaxMPPerLevel { get; private set; } = 0;
  [field: SerializeField] public float BaseMPRegen { get; private set; } = 0;
  [field: SerializeField] public float MPRegenPerLevel { get; private set; } = 0;
  public int MaxMP { get; private set; }
  public float MPRegen { get; private set; }
  public int MP { get; private set; }

  [field: SerializeField] public float BaseMvSpd { get; private set; } = 5f;
  [field: SerializeField] public float MvSpdPerLevel { get; private set; } = 0.1f;
  public float MvSpd { get; private set; }

  [field: SerializeField] public float BaseWalkSpd { get; private set; } = 2f;
  [field: SerializeField] public float WalkSpdPerLevel { get; private set; } = 0f;
  public float WalkSpd { get; private set; }

  [field: SerializeField] public int BaseDamage { get; private set; } = 10;
  [field: SerializeField] public int DamagePerLevel { get; private set; } = 2;
  public int Damage { get; private set; }

  [field: SerializeField] public float AggroRange { get; private set; } = 5f;

  [field: SerializeField] public float BaseAttackRange { get; private set; } = 1.5f;

  [field: SerializeField] public int BaseKilledExp { get; private set; } = 5;
  [field: SerializeField] public int KilledExpPerLevel { get; private set; } = 1;
  public int KilledExp { get; private set; }

  public string GetAssetPrefabName() {
    return this.AssetId + "Prefab.prefab";
  }

  public string GetAssetSpriteName() {
    return this.AssetId + "Sprite.png";
  }

  public void Init(string id, string name, int level) {
    this.Id = id;
    this.Name = name;
    this.Level = level;

    this.MaxHP = BaseMaxHP + (Level * MaxHPPerLevel);
    this.HPRegen = BaseHPRegen + (Level * HPRegenPerLevel);
    this.HP = MaxHP;

    this.MaxMP = BaseMaxMP + (Level * MaxMPPerLevel);
    this.MPRegen = BaseMPRegen + (Level * MPRegenPerLevel);
    this.MP = MaxMP;

    this.MvSpd = BaseMvSpd + (Level * MvSpdPerLevel);

    this.WalkSpd = BaseWalkSpd + (Level * WalkSpdPerLevel);

    this.Damage = BaseDamage + (Level * DamagePerLevel);

    this.KilledExp = BaseKilledExp + (Level * KilledExpPerLevel);
  }

  public void GetHit(int damage) {
    this.HP -= damage;
    if (this.HP < 0) {
      this.HP = 0;
    }
  }

  public void HealHP(int amount) {
    this.HP += amount;
    if (this.HP > this.MaxHP) {
      this.HP = this.MaxHP;
    }
  }

  public void HealMP(int amount) {
    this.MP += amount;
    if (this.MP > this.MaxMP) {
      this.MP = this.MaxMP;
    }
  }

  public void ConsumeMP(int amount) {
    try {
      if (this.MP < amount) {
        throw new Exception("Not enough mana");
      }

      this.MP -= amount;
      if (this.HP < 0) {
        this.HP = 0;
      }
    }
    catch (Exception e) {
      Debug.LogWarning(e.Message);
    }
  }

  object System.ICloneable.Clone() => (object)this.MemberwiseClone();
}

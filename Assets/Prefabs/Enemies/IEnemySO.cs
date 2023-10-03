using System;

public enum EnemyType {
  CREATURE = 0,
  BEAST = 1,
  HUMAN = 2
}

public enum EnemyRace {
  HUMAN = 0,
  MUTANT = 1,
  UNDEAD = 2,
  BEAST = 3
}

public enum EnemyBehaviour {
  NORMAL = 0,
  AGGRESSIVE = 1,
}

public enum EnemyIdleBehaviour {
  STAY_IN_SPAWN_POINT = 0,
  PATROL = 1,
}

public interface IEnemySO : ICloneable {
  public string AssetId { get; }
  public string Id { get; }
  public string Name { get; }

  public EnemyType Type { get; }
  public EnemyRace Race { get; }
  public EnemyBehaviour Behaviour { get; }
  public EnemyIdleBehaviour IdleBehaviour { get; }

  public int Level { get; }

  public int BaseMaxHP { get; }
  public float BaseHPRegen { get; }
  public int MaxHPPerLevel { get; }
  public float HPRegenPerLevel { get; }
  public int MaxHP { get; }
  public float HPRegen { get; }
  public int HP { get; }

  public int BaseMaxMP { get; }
  public float BaseMPRegen { get; }
  public int MaxMPPerLevel { get; }
  public float MPRegenPerLevel { get; }
  public int MaxMP { get; }
  public float MPRegen { get; }
  public int MP { get; }

  public int BaseDamage { get; }

  public float BaseMvSpd { get; }
  public float MvSpdPerLevel { get; }
  public float MvSpd { get; }

  public float BaseWalkSpd { get; }
  public float WalkSpdPerLevel { get; }
  public float WalkSpd { get; }

  public float AggroRange { get; }

  public float BaseAttackRange { get; }
  public int DamagePerLevel { get; }
  public int Damage { get; }

  public int BaseKilledExp { get; }
  public int KilledExpPerLevel { get; }
  public int KilledExp { get; }

  public string GetAssetPrefabName();

  public string GetAssetSpriteName();

  public void Init(string id, string name, int level);

  public void GetHit(int damage);

  public void HealHP(int amount);

  public void HealMP(int amount);

  public void ConsumeMP(int amount);
}

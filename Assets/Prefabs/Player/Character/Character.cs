using System;
using UnityEngine;

[Serializable]
public class Character {
  public string Name { get; private set; }
  public int Strength { get; private set; }
  public int Agility { get; private set; }
  public int Intelligence { get; private set; }

  public int Exp { get; private set; }
  public int Level { get; private set; } = 1;
  public int Gold { get; private set; }
  public int StatsPoint { get; private set; }

  public int BaseMaxHP { get; private set; } = 100;
  public int MaxHPPerStrength { get; private set; } = 10;
  public float BaseHPRegen { get; private set; } = 1;
  public float HPRegenPerStrength { get; private set; } = 0.1f;
  public int MaxHP { get; private set; }
  public int HP { get; private set; }
  public float HPRegen { get; private set; }

  public int BaseMaxMP { get; private set; } = 80;
  public int MaxMPPerIntelligence { get; private set; } = 10;
  public float BaseMPRegen { get; private set; } = 1;
  public float MPRegenPerIntelligence { get; private set; } = 0.1f;
  public int MaxMP { get; private set; }
  public int MP { get; private set; }
  public float MPRegen { get; private set; }

  public float BaseMvSpd { get; } = 5f;
  public float MvSpdPerAgility { get; private set; } = 0.05f;
  public float MvSpd { get; private set; }

  public float BaseAttackSpd { get; private set; } = 1f;
  public float AttackSpdPerAgility { get; private set; } = 0.025f;
  public float AttackSpd { get; private set; }

  public int BaseArmor { get; private set; } = 0;
  public float ArmorPerStrength { get; private set; } = 0.1f;
  public int Armor { get; private set; }

  public int BaseUnarmedDamage { get; private set; } = 3;
  public float UnarmedDamagePerStrength = 0.5f;
  public int UnarmedDamage { get; private set; }

  public PlayerInventory Inventory { get; private set; }
  public PlayerEquipment Equipment { get; private set; }
  public PlayerQuests Quests { get; private set; }


  public Character(
    string name,
    int str,
    int agi,
    int @int,
    PlayerInventory inventory,
    PlayerEquipment equipment,
    PlayerQuests quests
  ) {
    Name = name;

    Strength = str;
    Agility = agi;
    Intelligence = @int;

    Exp = 0;
    Level = 1;
    Gold = 0;

    EvaluateStats();

    Inventory = inventory;
    Equipment = equipment;
    Quests = quests;
  }

  public void GetHit(int damage) {
    // TODO: calculate damage against armor first
    HP -= damage;
    if (HP < 0) {
      HP = 0;
    }
  }

  public void HealHP(int heal) {
    HP += heal;
    if (HP > MaxHP) {
      HP = MaxHP;
    }
  }

  public void ComsumeMP(int amount) {
    MP -= amount;
    if (MP < 0) {
      MP = 0;
    }
  }

  public void HealMP(int amount) {
    MP += amount;
    if (MP > MaxMP) {
      MP = MaxMP;
    }
  }

  public void GainExp(int amount) {
    Exp += amount;
    int currentLvl = (int)Math.Ceiling((float)Exp / (float)ExpService.GetNextLevelExpBreakpoint(Level));
    if (currentLvl < Level) {
      return;
    }
    Level = currentLvl;
  }

  public void LevelUp() {
    StatsPoint += 1;
  }

  public void AddItem(InventoryItem inventoryItem) {
    Inventory.AddItem(inventoryItem);
  }

  public void RemoveItem(InventoryItem inventoryItem) {
    Inventory.RemoveItem(inventoryItem);
  }

  public void AcceptQuest(QuestSO quest) {
    Quests.AddQuest(quest);
  }

  public void IncreaseStrength() {
    if (StatsPoint <= 0) {
      throw new Exception("You dont have stats point");
    }

    Strength += 1;
    StatsPoint -= 1;
    EvaluateStats();
  }

  public void IncreaseAgility() {
    if (StatsPoint <= 0) {
      throw new Exception("You dont have stats point");
    }

    Agility += 1;
    StatsPoint -= 1;
    EvaluateStats();
  }

  public void IncreaseIntelligence() {
    if (StatsPoint <= 0) {
      throw new Exception("You dont have stats point");
    }

    Intelligence += 1;
    StatsPoint -= 1;
    EvaluateStats();
  }

  private void EvaluateStats() {
    MaxHP = BaseMaxHP + (Strength * MaxHPPerStrength);
    HPRegen = BaseHPRegen + (Strength * HPRegenPerStrength);
    HP = MaxHP;

    MaxMP = BaseMaxMP + (Intelligence * MaxMPPerIntelligence);
    MPRegen = BaseMPRegen + (Intelligence * MPRegenPerIntelligence);
    MP = MaxMP;

    MvSpd = BaseMvSpd + (Agility * MvSpdPerAgility);

    AttackSpd = BaseAttackSpd + (Agility * AttackSpdPerAgility);

    UnarmedDamage = BaseUnarmedDamage + (int)(Strength * UnarmedDamagePerStrength);

    Armor = BaseArmor + (int)(Strength * ArmorPerStrength);
  }

  public static Character FromJson(string data) {
    return JsonUtility.FromJson<Character>(data);
  }
}
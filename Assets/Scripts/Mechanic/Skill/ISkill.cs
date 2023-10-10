using System;
using UnityEngine;

public enum SkillTypes {
  FirstSkill = 1,
  SecondSkill = 2,
  ThirdSkill = 3,
  Ultimate = 4
}

public enum SkillCategory {
  MeleeSkill = 0,
  SpellSkill = 1,
  SummoningSkill = 2
}

public interface ISkill : ICloneable {
  public string Id { get; }
  public string Name { get; }
  public string Description { get; }
  public JobTypes RequiredJob { get; }
  public SkillTypes SkillType { get; }
  public SkillCategory Category { get; }
  public Sprite Icon { get; }

  public IEffect Effect { get; }

  public int Level { get; }

  public float DamageIncreasePerLevel { get; }  // percentage
  public int BaseDamage { get; }
  public int Damage { get; }
  public float Cooldown { get; }

  public void LevelUp();
}

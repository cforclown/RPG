using UnityEngine;

[CreateAssetMenu(fileName = "SkillSO", menuName = "ScriptableObjects/SkillSO")]
public class SkillSO : ScriptableObject, ISkill {
  public static readonly int SKILL_MAX_LEVEL = 10;
  [field: SerializeField] public string Id { get; private set; }
  [field: SerializeField] public string Name { get; private set; }
  [field: SerializeField] public string Description { get; private set; }
  [field: SerializeField] public JobTypes RequiredJob { get; private set; }
  [field: SerializeField] public SkillTypes SkillType { get; private set; }
  [field: SerializeField] public SkillCategory Category { get; }
  [field: SerializeField] public Sprite Icon { get; private set; }

  [HideInInspector] public IEffect Effect { get; private set; } // not implemented yet

  [HideInInspector] public int Level { get; private set; } = 0;

  [field: SerializeField]
  [field: Range(1, 100)] public float DamageIncreasePerLevel { get; private set; }  // percentage
  [field: SerializeField] public int BaseDamage { get; private set; }
  [HideInInspector] public int Damage { get; private set; } = 0;

  [field: SerializeField] public float Cooldown { get; private set; } = 1f;

  public void LevelUp() {
    if (Level >= SKILL_MAX_LEVEL) {
      return;
    }

    Level++;
    if (Level == 1) {
      Damage = BaseDamage;
      return;
    }
    Damage += Damage + (int)(Damage * DamageIncreasePerLevel);
  }

  public SkillSO Clone() => ((ISkill)this).Clone() as SkillSO;

  object System.ICloneable.Clone() => (object)this.MemberwiseClone();
}

public enum EffectTypes {
  STRENGTH = 0,
  AGILITY = 1,
  INTELLIGENCE = 2,
  AFFECT_HEALTH = 3,
  AFFECT_MANA = 4,
  DAMAGE = 5
}

public enum EffectCalcType {
  CONSTANT = 0,
  PERCENTAGE = 1,
}

public interface IEffect {
  public string Name { get; }
  public EffectTypes Types { get; }
  public string Value { get; }
  public EffectCalcType CalcType { get; }
}

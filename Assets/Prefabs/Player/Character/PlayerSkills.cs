using System.Collections.Generic;

public class PlayerSkills {
  public List<SkillSO> Skills { get; private set; }

  public PlayerSkills() {
    Skills = new List<SkillSO>();
  }

  public PlayerSkills(List<SkillSO> skills) {
    Skills = skills;
  }

  public void AddSkill(SkillSO skill) {
    Skills.Add(skill);
  }
}

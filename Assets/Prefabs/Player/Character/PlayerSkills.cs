using System.Collections.Generic;

public class PlayerSkills {
  public List<SkillSO> Skills { get; private set; }

  public PlayerSkills() {
    Skills = new List<SkillSO>();
  }

  public PlayerSkills(List<SkillSO> skills) {
    Skills = skills;
  }

  public void ClaimSkill(SkillSO skill) {
    Skills.Add(skill);
  }

  public void SkillLevelUp(SkillSO skill) {
    SkillSO currentSkill = Skills.Find(s => s.Id == skill.Id);
    if (currentSkill == null) {
      return;
    }

    currentSkill.LevelUp();
  }
}

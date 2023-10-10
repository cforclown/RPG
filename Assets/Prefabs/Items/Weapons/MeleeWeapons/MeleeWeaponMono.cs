using UnityEngine;

public class MeleeWeaponMono : WeaponMono<WeaponSO> {
  [SerializeField] private ParticleSystem skill1Effect;
  [SerializeField] private ParticleSystem skill2Effect;
  [SerializeField] private ParticleSystem skill3Effect;
  [SerializeField] private ParticleSystem skill4Effect;

  private void Awake() {
    CombatEvents.OnPlayerAttackLaunched += PerformSkill;
    CombatEvents.OnPlayerAttackFinished += AttackMotionFinished;

    AttackMotionFinished(null);
  }

  private void OnDestroy() {
    CombatEvents.OnPlayerAttackLaunched -= PerformSkill;
    CombatEvents.OnPlayerAttackFinished -= AttackMotionFinished;
  }

  private void PerformSkill(PlayerAnimController playerAnimController) {
    switch (playerAnimController.IsSkill) {
      case (int)SkillTypes.FirstSkill:
        skill1Effect.Play();
        break;
      case (int)SkillTypes.SecondSkill:
        skill2Effect.Play();
        break;
      case (int)SkillTypes.ThirdSkill:
        skill3Effect.Play();
        break;
      case (int)SkillTypes.Ultimate:
        skill4Effect.Play();
        break;
      default:
        break;
    }
  }

  private void AttackMotionFinished(PlayerAnimController playerAnimController) {
    skill1Effect.Stop();
    skill2Effect.Stop();
    skill3Effect.Stop();
    skill4Effect.Stop();
  }
}

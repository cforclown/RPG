using UnityEngine;

public class PlayerAnimController : MonoBehaviour {
  public enum PlayerIdleStyle {
    NO_WEAPON = 0,
    ONE_HANDED_WEAPON = 1,
    TWO_HANDED_WEAPON = 2
  }

  private Animator animator;
  [SerializeField] private RuntimeAnimatorController noWeaponAnimController;
  [SerializeField] private RuntimeAnimatorController oneHandedWeaponAnimController;

  private int isRunningHash;
  private int isBasicAttack1Hash;
  private int isBasicAttack2Hash;
  private int isDeadHash;
  private int isSkillHash;

  // animation state variables -----------------
  private bool isRunning = false;
  public bool IsRunning {
    get { return isRunning; }
    private set {
      isRunning = value;
      animator.SetBool(isRunningHash, value);
    }
  }

  private bool isBasicAttack1 = false;
  public bool IsBasicAttack1 {
    get { return isBasicAttack1; }
    private set {
      isBasicAttack1 = value;
      animator.SetBool(isBasicAttack1Hash, value);
    }
  }

  private bool isBasicAttack2 = false;
  public bool IsBasicAttack2 {
    get { return isBasicAttack2; }
    private set {
      isBasicAttack2 = value;
      animator.SetBool(isBasicAttack2Hash, value);
    }
  }

  private bool isDead = false;
  public bool IsDead {
    get { return isDead; }
    private set {
      isDead = value;
      animator.SetBool(isDeadHash, value);
    }
  }

  private int isSkill = 0;
  public int IsSkill {
    get { return isSkill; }
    private set {
      isSkill = value;
      animator.SetInteger(isSkillHash, value);
    }
  }
  // ------------------------------------------


  public bool RunningMotionStarted { get; private set; } = false;

  public bool AttackMotionFinished { get; private set; } = true;
  public bool IsAttackLaunched { get; private set; } = false;


  void Awake() {
    animator = GetComponent<Animator>();
    animator.runtimeAnimatorController = noWeaponAnimController;

    isRunningHash = Animator.StringToHash("IsRunning");
    isBasicAttack1Hash = Animator.StringToHash("IsBasicAttack1");
    isBasicAttack2Hash = Animator.StringToHash("IsBasicAttack2");
    isDeadHash = Animator.StringToHash("IsDead");
    isSkillHash = Animator.StringToHash("IsSkill");

    EquipmentPanelManager.OnEquipItemAction += OnEquippedItemRemoved;
  }

  private void Start() {
  }

  public void Idle() {
    IsRunning = false;
    RunningMotionStarted = false;
  }

  public void OnEquippedItemRemoved(ItemSO item, EquipmentPlaceholderTypes placeholder) {
    // TODO change anim controller from here
  }

  public void NoWeaponStyle() {
    animator.runtimeAnimatorController = noWeaponAnimController;
  }

  public void OneHandedWeaponStyle() {
    animator.runtimeAnimatorController = oneHandedWeaponAnimController;
  }

  public void Running() {
    IsRunning = true;
  }

  // Animation event
  private void OnRunningMotionStarted() {
    RunningMotionStarted = true;
  }

  public void PerformBasicAttack() {
    IsRunning = false;
    AttackMotionFinished = false;
    RunningMotionStarted = false;
    if (Generator.RandomInt(0, 100) % 2 == 0) {
      IsBasicAttack1 = true;
    }
    else {
      IsBasicAttack2 = true;
    }
  }

  public void PerformSkill(SkillTypes skillType) {
    IsRunning = false;
    AttackMotionFinished = false;
    RunningMotionStarted = false;
    IsSkill = (int)skillType;
  }

  // Animation event
  private void OnAttackLaunched() {
    IsAttackLaunched = true;
    CombatEvents.PlayerAttackLaunched(this);
  }

  // Animation event
  private void OnAttackFinished() {
    IsBasicAttack1 = false;
    IsBasicAttack2 = false;
    IsAttackLaunched = false;
    CombatEvents.PlayerAttackFinished(this);
  }

  // Animation event
  private void OnNextComboAttack() {
    SkillCalculator.ResetHitEnemies();
  }

  // Animation event
  private void OnAttackMotionFinished() {
    AttackMotionFinished = true;
    IsSkill = 0;
    CombatEvents.PlayerAttackMotionFinished(this);
  }

  public void BasicAttackHit() {
    IsAttackLaunched = false;
  }

  public bool IsAttacking() {
    return IsBasicAttack1 || IsBasicAttack2;
  }

  public void Dead() {
    IsDead = true;
    RunningMotionStarted = false;
  }
}

using UnityEngine;

public class EnemyAnimStateController : MonoBehaviour {

  private Animator animator;
  private int isWalkingHash;
  private int isRoaringHash;
  private int isRunningHash;
  private int isAttackingHash;
  private int isDeadHash;

  private bool isWalking = false;
  public bool IsWalking {
    get { return isWalking; }
    private set {
      isWalking = value;
      animator.SetBool(isWalkingHash, value);
    }
  }

  private bool isRoaring = false;
  public bool IsRoaring {
    get { return isRoaring; }
    private set {
      isRoaring = value;
      animator.SetBool(isRoaringHash, value);
    }
  }

  private bool isRunning = false;
  public bool IsRunning {
    get { return isRunning; }
    private set {
      isRunning = value;
      animator.SetBool(isRunningHash, value);
      if (!value) {
        StopMoving();
      }
    }
  }

  private bool isAttacking = false;
  public bool IsAttacking {
    get { return isAttacking; }
    private set {
      isAttacking = value;
      animator.SetBool(isAttackingHash, value);
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

  public bool Moving { get; private set; } = false;

  public bool IsAttackLanded { get; private set; } = true;


  void Start() {
    animator = GetComponent<Animator>();
    isWalkingHash = Animator.StringToHash("IsWalking");
    isRoaringHash = Animator.StringToHash("IsRoaring");
    isRunningHash = Animator.StringToHash("IsRunning");
    isAttackingHash = Animator.StringToHash("IsAttacking");
    isDeadHash = Animator.StringToHash("IsDead");
  }

  public void Idle() {
    IsWalking = false;
    IsAttacking = false;
    IsRunning = false;
  }

  public void Walking() {
    IsWalking = true;
  }

  public void Running() {
    IsRunning = true;
  }

  // triggered on animation event
  private void StartMoving() {
    Moving = true;
  }

  // triggered on animation event
  private void StopMoving() {
    Moving = false;
  }

  public void Attack() {
    IsRunning = false;
    IsAttacking = true;
  }

  public void AttackLaunched() {
    IsAttackLanded = false;
  }

  public void AttackingDone() {
    IsAttacking = false;
    IsRunning = false;
    IsAttackLanded = true;
  }

  public void AttackLanded() {
    IsAttackLanded = true;
  }

  public void Roar() {
    IsWalking = false;
    IsRunning = false;
    IsRoaring = true;
  }

  public void RoarDone() {
    IsRoaring = false;
  }

  public void Dead() {
    IsDead = true;
    IsAttacking = false;
    IsRunning = false;
    IsAttackLanded = true;
  }
}

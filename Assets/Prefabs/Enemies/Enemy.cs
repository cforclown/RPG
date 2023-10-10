using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour {
  public EnemySO Stats { get; private set; }

  EnemyAnimStateController animController;

  private bool playerFound = false;

  private const float TURN_SPD = 25f;

  private const float chasingTimeMax = 7.5f;
  private float chasingTimeCounter = 0f;

  private const float delayChasingMax = 5f;
  private float delayChasingCounter = delayChasingMax;
  private Vector3 spawnPoint;
  private bool patroling = false;
  private Vector3 patrolingMovePoint;

  private bool isDead = false;

  private void Awake() {
    CombatEvents.OnPlayerAttackHitEnemy += OnPlayerAttackHitEnemyEvent;
  }

  void Start() {
    animController = GetComponent<EnemyAnimStateController>();

    spawnPoint = transform.position;
    patrolingMovePoint = transform.position;
  }

  // Update is called once per frame
  void Update() {
    if (isDead) {
      return;
    }

    Vector3? playerPos = Player.GetPosition();

    if (playerPos != null && Stats.Behaviour == EnemyBehaviour.AGGRESSIVE) {
      // Player is far away or chasing too long => patrol
      if (
        (
          Vector3.Distance(transform.position, playerPos.Value) > Stats.AggroRange &&
          !playerFound
        ) ||
        delayChasingCounter < delayChasingMax
      ) {
        delayChasingCounter += Time.deltaTime;
        Patrol();
        return;
      }

      // logic below indicate player is near (within vision range)
      Vector3 lookAt = (playerPos.Value - transform.position).normalized;
      float degreeOfForwardAndPlayerPos = Vector3.Angle(transform.forward, playerPos.Value - transform.position);
      // if player was not found before (this indicate this enemy just found the player)
      if (!playerFound) {
        // if player is not in front => rotate
        if (degreeOfForwardAndPlayerPos > 10) {
          Rotate(lookAt);
        }
        // if player is front of us
        else {
          playerFound = true;
          animController.Roar();
        }
        chasingTimeCounter = 0f;
      }
      // if player found
      else {
        // if still roaring or attacking
        if (animController.IsRoaring || animController.IsAttacking) {
          Rotate(lookAt);
          return;
        }

        // if the player is within attack range
        if (Vector3.Distance(playerPos.Value, transform.position) <= Stats.BaseAttackRange) {
          // if the player is not in front => rotate
          if (degreeOfForwardAndPlayerPos > 10f) {
            Rotate(lookAt);
          }
          // if player is in front => attack
          else {
            // check if the player is dead 
            if (Player.I.Character.Stats.HP <= 0) {
              playerFound = false;
              animController.Idle();
              delayChasingCounter = 0f;
            }
            else {
              animController.Attack();
            }
          }

          // reset chasing time because the player already within attack range (probably already attack the player or just rotating)
          chasingTimeCounter = 0f;
        }
        // if player is not within attack range => chase
        else {
          ChasePlayer(lookAt);
        }
      }
    }
  }

  void OnDestroy() {
    StopCoroutine(HPRegenCoroutine());
    StopCoroutine(MPRegenCoroutine());
    CombatEvents.OnPlayerAttackHitEnemy -= OnPlayerAttackHitEnemyEvent;
  }

  public void Init(EnemySO stat) {
    this.Stats = stat;
    StartCoroutine(HPRegenCoroutine());
    StartCoroutine(MPRegenCoroutine());
  }



  #region MOVEMENTS
  private void Patrol() {
    // if far away, walk to spawn point
    if (
      Vector3.Distance(transform.position, spawnPoint) > 10f &&
      (!patroling || Vector3.Distance(transform.position, patrolingMovePoint) <= 0.1f)
    ) {
      Vector3 toward = (spawnPoint - transform.position).normalized;
      Rotate(toward);
      // if not big turn, walk to spawn point
      if (Vector3.Angle(transform.forward, toward) < 10f) {
        animController.Walking();
        if (animController.Moving) {
          Move(toward, true);
        }
      }
    }
    else {
      patroling = true;
      float distance = Vector3.Distance(transform.position, patrolingMovePoint);
      // if arrive at patrol point, generate new patrol point
      if (Vector3.Distance(transform.position, patrolingMovePoint) <= 2.5f) {
        patrolingMovePoint = transform.position + new Vector3(
          Generator.RandomFloat(-15f, 15f),
          transform.position.y,
          Generator.RandomFloat(-15f, 15f)
        );
      }
      Vector3 toward = (patrolingMovePoint - transform.position).normalized;
      Rotate(toward);
      float angle = Vector3.Angle(transform.forward, toward);
      if (Vector3.Angle(transform.forward, toward) < 15f) {
        animController.Walking();
        if (animController.Moving) {
          Move(toward, true);
        }
      }
      else {
        animController.Idle();
      }
    }
  }

  private void ChasePlayer(Vector3 toward) {
    animController.Running();
    Rotate(toward);
    if (animController.Moving) {
      Move(toward);
    }

    chasingTimeCounter += Time.deltaTime;
    if (chasingTimeCounter >= chasingTimeMax) {
      playerFound = false;
      animController.Idle();
      delayChasingCounter = 0f;
    }
  }

  private void Rotate(Vector3 forward) {
    transform.rotation = RotationUtils.LerpUp(
      transform.rotation,
      forward,
      TURN_SPD * Time.deltaTime
    );
  }

  private void Move(Vector3 toward, bool walking = false) {
    transform.Translate(toward * (walking ? Stats.WalkSpd : Stats.MvSpd) * Time.deltaTime, Space.World);
  }
  #endregion



  #region COMBAT EVENTs
  public void OnPlayerAttackHitEnemyEvent(CharacterManager player, Enemy enemy, WeaponSO weapon) {
    if (enemy.Stats.Id != Stats.Id) {
      return;
    }

    GetHit(player, enemy, weapon);
  }

  public void GetHit(CharacterManager player, Enemy enemy, WeaponSO weapon) {
    Stats.GetHit(player.GetDamageOutput(this, weapon));
    CombatEvents.PostPlayerAttackHitEnemyEvent(player, enemy, weapon);
    if (Stats.HP <= 0) {
      isDead = true;
      animController.Dead();
      StartCoroutine(DestroyGameObjectDelay());
      CombatEvents.EnemyDied(this);
    }
  }
  #endregion



  #region TASKs
  private IEnumerator HPRegenCoroutine() {
    while (Stats.HP > 0) {
      yield return new WaitForSeconds(0.0001f);
      if (Stats.HP >= Stats.MaxHP) {
        continue;
      }
      Stats.HealHP((int)Stats.HPRegen);
      yield return new WaitForSeconds(1f);
    }
  }

  private IEnumerator MPRegenCoroutine() {
    while (Stats.MP > 0) {
      yield return new WaitForSeconds(0.0001f);
      if (Stats.MP >= Stats.MaxHP) {
        continue;
      }
      Stats.HealMP((int)Stats.MPRegen);
      yield return new WaitForSeconds(1f);
    }
  }

  private IEnumerator DestroyGameObjectDelay() {
    yield return new WaitForSeconds(5f);
    Destroy(gameObject);
  }
  #endregion
}

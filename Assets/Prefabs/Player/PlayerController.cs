using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
  private CharacterManager characterManager;

  private CapsuleCollider physicCollider;

  private PlayerAnimController animController;
  private PlayerInput playerInput;

  private const float TURN_SPD = 25.0f;

  private NPC moveToNPCAndInteract = null;

  private void Awake() {
    physicCollider = GetComponent<CapsuleCollider>();
    animController = GetComponent<PlayerAnimController>();
    playerInput = GetComponent<PlayerInput>();
    characterManager = GetComponent<CharacterManager>();

    ControllerManager.OnBasicAttackPressed += PerformAttack;
    ControllerManager.OnSkillBtnPressed += PerformAttack;
  }

  private void OnDestroy() {
    ControllerManager.OnBasicAttackPressed -= PerformAttack;
    ControllerManager.OnSkillBtnPressed -= PerformAttack;
  }

  // Update is called once per frame
  void Update() {
    if (Input.GetMouseButton(0)) {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;
      if (Physics.Raycast(ray, out hit)) {
        NPC npc = hit.collider.GetComponent<NPC>();
        if (npc != null && npc.data.Dialogs != null && npc.data.Dialogs.Count() != 0) {
          if (Vector3.Distance(transform.position, npc.transform.position) > 1.5) {
            moveToNPCAndInteract = npc;
          }
          else {
            NPCEvents.PlayerInteractWithNPC(npc);
          }
        }
      }
    }
    if (NPCDialogUIManager.I.IsOpen && moveToNPCAndInteract == null) {
      return;
    }
    if (moveToNPCAndInteract != null) {
      MoveToPoint(moveToNPCAndInteract.transform.position);
      if (Vector3.Distance(transform.position, moveToNPCAndInteract.transform.position) <= 1.5f) {
        NPCEvents.PlayerInteractWithNPC(moveToNPCAndInteract);
        moveToNPCAndInteract = null;
      }
    }

    Movement();
    Attack();
  }

  void MoveToPoint(Vector3 point) {
    Vector3 forward = (point - transform.position).normalized;
    if (!animController.IsRunning) {
      animController.Running();
    }
    Rotate(forward);
    Move(forward);
  }

  void Movement() {
    if (!animController.AttackMotionFinished) {
      return;
    }

    Vector3 cameraForward = Camera.main.transform.forward;
    cameraForward.y = 0f;
    Vector3 cameraRight = Camera.main.transform.right;
    cameraRight.y = 0f;
    Vector2 axisInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    Vector2 joystickLeftInput = playerInput.actions["Move"].ReadValue<Vector2>();
    if (joystickLeftInput.x != 0f && joystickLeftInput.y != 0f) {
      Debug.Log(joystickLeftInput);
    }
    cameraForward = cameraForward * axisInput.y;
    cameraRight = cameraRight * axisInput.x;

    Vector3 joystickLeftInputRelativeCamera = Quaternion.AngleAxis(Camera.main.transform.localEulerAngles.y, Vector3.up) * VectorUtils.Pos2ToPos3(joystickLeftInput);
    Vector3 movementForward = (cameraForward + cameraRight + joystickLeftInputRelativeCamera).normalized;
    if (animController.RunningMotionStarted) {
      Move(movementForward);
    }

    // rotate player while moving
    if (movementForward.magnitude != 0f) {
      Rotate(movementForward);
      if (!animController.IsRunning) {
        animController.Running();
      }
      moveToNPCAndInteract = null;
    }
    else {
      if (moveToNPCAndInteract != null) {
        return;
      }
      animController.Idle();
    }
  }

  void Rotate(Vector3 forward) {
    transform.rotation = RotationUtils.LerpUp(
      transform.rotation,
      forward,
      TURN_SPD * Time.deltaTime
    );
  }

  void Move(Vector3 toward) {
    transform.Translate(toward * characterManager.Stats.MvSpd * Time.deltaTime, Space.World);
  }

  void Attack() {
    if (Input.GetMouseButton(1)) {
      BasicAttack();
    }
  }

  private void PerformAttack(SkillSO skill) {
    if (skill == null) {
      BasicAttack();
      return;
    }

    SkillAttack(skill.SkillType);
  }

  private void BasicAttack() {
    if (!IsGrounded() || !animController.AttackMotionFinished) {
      return;
    }

    animController.PerformBasicAttack();
  }

  private void SkillAttack(SkillTypes skillType) {
    if (!IsGrounded() || !animController.AttackMotionFinished) {
      return;
    }

    animController.PerformSkill(skillType);
  }

  private bool IsGrounded() {
    return GroundCheck();
  }

  private bool GroundCheck() {
    float extraDistance = 0.01f;
    return Physics.BoxCast(
      physicCollider.bounds.center,
      physicCollider.bounds.extents / 2f,
      Vector3.down,
      transform.rotation,
      physicCollider.bounds.extents.y / 2f + extraDistance
    );
  }

  //private void OnDrawGizmos() {
  //  bool hitGround = GroundCheck();
  //  if (hitGround) {
  //    Gizmos.DrawWireCube(bc.bounds.center, bc.bounds.size);
  //  }

  //  Gizmos.DrawLine(transform.position, transform.position + Camera.main.transform.forward * 2f);
  //}
}

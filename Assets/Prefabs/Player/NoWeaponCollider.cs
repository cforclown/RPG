using System;
using UnityEngine;

public class NoWeaponCollider : MonoBehaviour {
  private Collider attackCollider;
  private Action<Collider> onColliderTriggerEnterCallback = null;

  private void Awake() {
    attackCollider = GetComponent<Collider>();
  }

  public virtual void Init(Action<Collider> onColliderTriggerEnter) {
    onColliderTriggerEnterCallback = onColliderTriggerEnter;
  }

  private void OnTriggerEnter(Collider collider) {
    if (onColliderTriggerEnterCallback == null) {
      return;
    }

    onColliderTriggerEnterCallback(collider);
  }

  public void Disable() => attackCollider.enabled = false;

  public void Enable() => attackCollider.enabled = true;
}

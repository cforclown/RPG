using System;
using UnityEngine;

public class ItemMono<T> : MonoBehaviour {
  public T item { get; private set; }

  private Action<Collider, T> onColliderTriggerEnterCallback = null;

  public virtual void Init(T item) {
    this.item = item;
  }

  public virtual void Init(T item, Action<Collider, T> onColliderTriggerEnter) {
    this.item = item;
    onColliderTriggerEnterCallback = onColliderTriggerEnter;
  }

  private void OnTriggerEnter(Collider collider) {
    if (onColliderTriggerEnterCallback == null) {
      return;
    }

    onColliderTriggerEnterCallback(collider, item);
  }
}
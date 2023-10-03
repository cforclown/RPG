using UnityEngine;

public class Player : MonoBehaviour {
  public static Player I;

  public PlayerController Controller { get; private set; }
  public CharacterManager Character { get; private set; }

  void Awake() {
    I = this;

    Controller = GetComponent<PlayerController>();
    Character = GetComponent<CharacterManager>();
  }

  private void OnDestroy() {
    I = null;
  }

  public static Vector3? GetPosition() {
    if (I == null) {
      return null;
    }

    return I.transform.position;
  }

  public static Character GetStats() {
    if (I == null) {
      return null;
    }

    return I.Character.Stats;
  }
}

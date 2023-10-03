using UnityEngine;

public static class RotationUtils {
  public static Quaternion LerpUp(Quaternion sourceRot, Vector3 forward, float spd) {
    Quaternion rot = Quaternion.Lerp(
        sourceRot,
        Quaternion.LookRotation(forward, Vector3.up),
        spd
      );
    return Quaternion.Euler(new Vector3(0f, rot.eulerAngles.y, 0f));
  }
}

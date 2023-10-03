using UnityEngine;

public static class VectorUtils {
  public static Vector3 Pos2ToPos3(Vector2 source) => new Vector3(source.x, 0f, source.y);

  public static Vector3 Pos2ToPos3(Vector2 source, float y) => new Vector3(source.x, y, source.y);

  public static Vector2 Pos3ToPos2(Vector3 source) => new Vector2(source.x, source.z);

  public static Vector3 ZeroY(Vector3 source) => new Vector3(source.x, 0f, source.z);

  public static Vector2 GenerateRandomPosWithinRect(Rect rect) {
    return new Vector2(
      rect.x + Generator.RandomFloat(0, rect.width),
      rect.y + Generator.RandomFloat(0, rect.height)
    );
  }
}

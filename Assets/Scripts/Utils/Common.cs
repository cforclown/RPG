using System;
using UnityEngine;

public static class Generator {
  public static string uuid() => Guid.NewGuid().ToString();

  public static int RandomInt(int min, int max) => UnityEngine.Random.Range(min, max);

  public static float RandomFloat(float min, float max) => UnityEngine.Random.Range(min, max);
}

public static class Logger {
  public static void Err(string objName, string err) {
    Debug.LogError(string.Format("{0}: {1}", objName, err));
  }
  public static void Info(string objName, string info) {
    Debug.Log(string.Format("{0}: {1}", objName, info));
  }
}

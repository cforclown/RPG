using UnityEngine;

[CreateAssetMenu(fileName = "AreaNativeEnemySO", menuName = "ScriptableObjects/AreaNativeEnemySO")]
public class AreaNativeEnemySO : ScriptableObject {
  [field: SerializeField] public EnemySO Enemy { get; private set; }
  [field: SerializeField] public int LevelMin { get; private set; }
  [field: SerializeField] public int LevelMax { get; private set; }
  [field: SerializeField] public int MaxPopulation { get; private set; }
}

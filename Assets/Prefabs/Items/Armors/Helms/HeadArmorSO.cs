using UnityEngine;

[CreateAssetMenu(fileName = "HeadArmorSO", menuName = "ScriptableObjects/HeadArmorSO")]
public class HeadArmorSO : ArmorSO {
  public override ArmorTypes ArmorType { get; } = ArmorTypes.HEAD;
}

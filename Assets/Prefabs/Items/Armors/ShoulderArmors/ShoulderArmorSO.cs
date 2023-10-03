using UnityEngine;

[CreateAssetMenu(fileName = "ShoulderArmorSO", menuName = "ScriptableObjects/ShoulderArmorSO")]
public class ShoulderArmorSO : ArmorSO {
  public override ArmorTypes ArmorType { get; } = ArmorTypes.SHOULDER;
}

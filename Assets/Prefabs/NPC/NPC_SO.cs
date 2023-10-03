using UnityEngine;

[CreateAssetMenu(fileName = "NPC_SO", menuName = "ScriptableObjects/NPC_SO")]
public class NPC_SO : ScriptableObject, INPC {
  [field: SerializeField] public string Id { get; private set; }
  [field: SerializeField] public string Name { get; private set; }
  [field: SerializeField] public Sprite Sprite { get; private set; }

  [Tooltip("Quest [optional]")]
  [field: SerializeField] public string[] Dialogs { get; private set; }


  [Tooltip("Quest [optional]")]
  [field: SerializeField] public QuestSO Quest { get; private set; } = null;
}

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemSO : ScriptableObject, IItem {
  [field: SerializeField] public string AssetId { get; private set; }
  [field: SerializeField] public string Id { get; private set; }
  [field: SerializeField] public string Name { get; private set; }
  public virtual ItemTypes Type { get; }
  public int[,] Grid { get; } // not implemented yet
  public List<IEffect> Effects { get; } // not implemented yet

  [field: SerializeField] public Sprite Sprite { get; private set; }
  public bool CanBeStacked { get; private set; } = false;

  public string GetAssetPrefabName() => AssetId + "Prefab.prefab";

  public ItemSO Clone() => ((IItem)this).Clone() as ItemSO;

  object System.ICloneable.Clone() => (object)this.MemberwiseClone();
}

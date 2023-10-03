using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SpawnAreaManager : MonoBehaviour {
  [SerializeField] private List<AreaNativeEnemySO> nativeEnemies;
  [SerializeField] private float respawnDelay = 1f;

  private List<EnemySO> areaEnemies;

  void Start() {
    areaEnemies = new List<EnemySO>();
    StartCoroutine(Respawner());
    CombatEvents.OnEnemyDeath += BanishEnemyFromArea;
  }

  void OnDestroy() {
    StopCoroutine(Respawner());
  }

  private void BanishEnemyFromArea(Enemy enemy) {
    areaEnemies.Remove(enemy.Stats);
  }

  IEnumerator Respawner() {
    while (true) {
      yield return new WaitForSeconds(respawnDelay);
      foreach (AreaNativeEnemySO nativeEnemySO in nativeEnemies) {
        EnemySO enemySO = nativeEnemySO.Enemy;
        List<EnemySO> enemiesByAssetId = areaEnemies.FindAll(e => e.AssetId == enemySO.AssetId);
        if (enemiesByAssetId == null) {
          continue;
        }
        int currentPopulation = enemiesByAssetId.Count;
        if (currentPopulation < nativeEnemySO.MaxPopulation) {
          AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(enemySO.GetAssetPrefabName());
          asyncOperationHandle.Completed += (AsyncOperationHandle<GameObject> asyncOperationHandle) => {
            LoadEnemyPrefabCompleted(asyncOperationHandle, enemySO, nativeEnemySO);
          };
        }
      }
    }
  }

  private void LoadEnemyPrefabCompleted(
    AsyncOperationHandle<GameObject> asyncOperationHandle,
    EnemySO statsSO,
    AreaNativeEnemySO areaNativeEnemy
  ) {
    if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded) {
      EnemySO stats = (statsSO as IEnemySO).Clone() as EnemySO;
      stats.Init(Generator.uuid(), stats.Name, Generator.RandomInt(areaNativeEnemy.LevelMin, areaNativeEnemy.LevelMax));
      GameObject obj = Instantiate(asyncOperationHandle.Result);
      Enemy controller = obj.GetComponent<Enemy>();
      obj.name = statsSO.Name;
      controller.Init(stats);
      Vector3 areaSize = GetComponent<Renderer>().bounds.size;
      Vector3 areaHalfSize = GetComponent<Renderer>().bounds.extents;
      Vector2 spawnPoint = VectorUtils.GenerateRandomPosWithinRect(new Rect(
        transform.position.x - areaHalfSize.x,
        transform.position.z - areaHalfSize.z,
        areaSize.x,
        areaSize.z
      ));
      controller.transform.position = new Vector3(
        spawnPoint.x,
        0.25f,
        spawnPoint.y
      );
      areaEnemies.Add(stats);
    }
    else {
      Debug.LogError("CharacterManager: failed to load Enemy prefab");
    }
  }
}

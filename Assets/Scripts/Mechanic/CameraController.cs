using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
  public static CameraController I;
  [SerializeField] private CinemachineVirtualCamera cinemachineVirtual;
  private Transform playerTransform;
  private List<GameObject> transparentObjects;

  private void Awake() {
    I = this;
  }

  private void Start() {
    transparentObjects = new List<GameObject>();
  }

  private void FixedUpdate() {
    if (playerTransform == null) {
      return;
    }

    //Calculate the Vector direction
    Vector3 direction = playerTransform.position - transform.position + Vector3.up;
    //Calculate the length
    float length = Vector3.Distance(playerTransform.position, transform.position);
    //Draw the ray in the debug
    Debug.DrawRay(transform.position, direction.normalized * length, Color.red);

    RaycastHit rayFromCameraHit;
    RaycastHit rayFromPlayerHit;
    //Cast the ray and report the firt object hit filtering by "Building" layer mask
    if (Physics.Raycast(
      transform.position,
      direction,
      out rayFromCameraHit,
      length,
      LayerMask.GetMask("Building")
    )) {
      //Getting the script to change transparency of the hit object
      Building building = rayFromCameraHit.transform.GetComponent<Building>();
      if (building == null) {
        return;
      }

      building.SetTransparent(true);
      transparentObjects.Add(building.gameObject);
    }
    else if (Physics.Raycast(
      playerTransform.position,
      transform.position - playerTransform.position + Vector3.up,
      out rayFromPlayerHit,
      length,
      LayerMask.GetMask("Building")
    )) {
      //Getting the script to change transparency of the hit object
      Building building = rayFromPlayerHit.transform.GetComponent<Building>();
      if (building == null) {
        return;
      }

      building.SetTransparent(true);
      transparentObjects.Add(building.gameObject);
    }
    else if (transparentObjects.Count > 0) {
      ResetBuildingTransparency();
    }
  }

  private void ResetBuildingTransparency() {
    foreach (GameObject obj in transparentObjects) {
      Building building = obj.transform.GetComponent<Building>();
      if (building == null) {
        continue;
      }

      building.SetTransparent(false);
    }
    transparentObjects = new List<GameObject>();
  }

  public void Init(Transform playerTransform) {
    this.playerTransform = playerTransform;
    cinemachineVirtual.Follow = this.playerTransform;
  }
}

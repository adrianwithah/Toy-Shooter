using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDisjoint : MonoBehaviour {

  float timer;
  PlayerStatus playerStatus;
  float disjointCoolDown = 2f;
  public Image skillImage;
  PlayerMove playerMove;
  bool coolingDown = false;

  float camRayLength = 100f;
  int floorMask;
  RaycastHit rayHit;

  void Awake() {
    playerStatus = GetComponent<PlayerStatus>();
    playerMove = GetComponent<PlayerMove>();
    floorMask = LayerMask.GetMask("Floor");
  }

  void Update() {
    timer += Time.deltaTime;
    if (Input.GetButtonDown("Dodge") && !coolingDown) {
      playerStatus.EnterTargetMode(PlayerMove.PlayerCommand.DISJOINT);
    }
    if (playerStatus.TargetCommand == PlayerMove.PlayerCommand.DISJOINT) {
      if (Input.GetMouseButtonDown(0)) {
        if (playerStatus.IsRooted) {
          return;
        }
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(camRay, out rayHit, camRayLength, floorMask)) {
					playerMove.Stop();
					playerMove.TurnPlayer(rayHit.point);
					playerMove.NewCommand(PlayerMove.PlayerCommand.DISJOINT);
          playerStatus.ExitTargetMode();
				} else {
					return;
				}
      }
    }
  }

  public void StartDisjoint() {
    StartCoroutine("Disjoint");
  }

  IEnumerator Disjoint() {
    timer = 0f;
    coolingDown = true;
    skillImage.fillAmount = 0;
		playerStatus.DisjointPoint = transform.position;
		playerStatus.Disjointing = true;
		yield return null;
		playerStatus.Disjointing = false;
    Vector3 landingPoint = rayHit.point - playerMove.PlayerLookAt.normalized * 1f;
    playerMove.WarpAgent(landingPoint);
    while (timer < disjointCoolDown) {
      skillImage.fillAmount = timer / disjointCoolDown;
      yield return null;
    }
    coolingDown = false;
	}
}

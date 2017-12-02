using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCancel : MonoBehaviour {

  //cancel any existing targetting and return to default state.
  PlayerStatus playerStatus;

  void Awake() {
    playerStatus = GetComponent<PlayerStatus>();
  }
  void Update() {
    CheckCancel();
  }

  void CheckCancel() {
    if (Input.GetButtonDown("Cancel")) {
      playerStatus.ExitTargetMode();
    }
  }
}

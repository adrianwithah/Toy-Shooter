using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RootEffect : MonoBehaviour {

  float timer = 0f;
  float rootDuration;
  PlayerStatus targetStatus;
  Image image;
  public Image statusImage;
  GameObject statusPanel;

  void Awake() {
    statusPanel = GameObject.FindGameObjectWithTag("StatusPanel");
    if (gameObject.tag == "Player") {
      targetStatus = GetComponent<PlayerStatus>();
      GetComponent<PlayerMove>().Stop();
    }
  }
  void Start() {
    image = (Image) Instantiate(statusImage);
    image.transform.SetParent(statusPanel.transform);
    targetStatus.IsRooted = true;
  }

  public void SetDuration(float duration) {
    rootDuration = duration;
  }

  void Update() {
    timer += Time.deltaTime;
    image.fillAmount = 1 - (timer / rootDuration);
    if (timer >= rootDuration) {
      targetStatus.IsRooted = false;
      Destroy(this);
    }
  }

  void OnDestroy() {
    if (image != null) {
      Destroy(image.gameObject);
    }
  }
}

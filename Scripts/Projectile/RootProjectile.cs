using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RootProjectile : TrackingProjectile {

  float rootDuration = 3f;

  public Image statusImage;

  protected override float Speed {
    get {
      return 20f;
    }
  }

  void OnCollisionEnter(Collision collision) {
    if (collision.gameObject == target && !disjointed) {
      if (!targetStatus.IsDead) {
        ApplyEffect();
      }
      GetComponent<Renderer>().enabled = false;
      GetComponent<Collider>().enabled = false;
    }
  }

  void ApplyEffect() {
    RootEffect rootEffect = target.GetComponent<RootEffect>();
    if (rootEffect != null) {
      Destroy(rootEffect);
    }
    rootEffect = target.AddComponent<RootEffect>();
    rootEffect.statusImage = statusImage;
    rootEffect.SetDuration(rootDuration);
  }
}

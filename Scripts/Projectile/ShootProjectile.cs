using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : TrackingProjectile {

  void OnCollisionEnter(Collision collision) {
    if (collision.gameObject == target && !disjointed) {
      if (!targetStatus.IsDead) {
        targetStatus.TakeDamage(damagePerShot);
      }
      Destroy(gameObject);
    }
  }
}

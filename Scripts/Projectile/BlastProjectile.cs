using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastProjectile : MonoBehaviour {

  float speed = 20f;
  Rigidbody rb;
  int damagePerShot = 30;
  int shootableLayer;

  void Awake() {
    rb = GetComponent<Rigidbody>();
    shootableLayer = LayerMask.NameToLayer("Shootable");
  }

  void FixedUpdate() {
    rb.MovePosition(transform.position + transform.forward.normalized * speed * Time.deltaTime);
  }

  void OnCollisionEnter(Collision collision) {
    GameObject collisionObject = collision.gameObject;
    if (shootableLayer == collisionObject.layer) {
      if (collisionObject.tag == "Enemy") {
        collisionObject.GetComponent<EnemyHP>().TakeDamage(damagePerShot, new Vector3(0f, 0f, 0f));
      }
      Destroy(gameObject);
    }
  }
}

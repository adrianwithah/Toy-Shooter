using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingProjectile : MonoBehaviour {

  protected GameObject target;
  protected Rigidbody rb;
  protected bool disjointed = false;
  //an impossible disjointPoint initialised as we cannot set Vector3
  //to null/undefined.
  protected Vector3 disjointPoint = new Vector3(0f, -100f, 0f);
  protected PlayerStatus targetStatus;
  protected int damagePerShot = 20;

  protected virtual float Speed {
    get {
      return 10f;
    }
  }

  void Awake() {
    rb = GetComponent<Rigidbody>();
  }

  void FixedUpdate() {
    Move();
    Turn();
  }

  void Update() {
    CheckDisjoint();
  }

  public void SetTarget(GameObject target) {
    this.target = target;
    targetStatus = target.GetComponent<PlayerStatus>();
  }

  void Move() {
    rb.MovePosition(transform.position + transform.forward.normalized * Speed * Time.deltaTime);
    if ((transform.position - disjointPoint).magnitude <= 0.8f) {
      Destroy(gameObject);
    }
  }

  void CheckDisjoint() {
    if (targetStatus.Disjointing) {
      disjointed = true;
      disjointPoint = targetStatus.DisjointPoint;
    }
  }

  void Turn() {
    if (!disjointed) {
      Vector3 lookAt = target.transform.position - transform.position;
      lookAt.y = 0f;
      Quaternion newRotation = Quaternion.LookRotation(lookAt);
      rb.MoveRotation(newRotation);
    }
  }

  void OnCollisionEnter(Collision collision) {
    if (collision.gameObject == target && !disjointed) {
      if (!targetStatus.IsDead) {
        targetStatus.TakeDamage(damagePerShot);
      }
      Destroy(gameObject);
    }
  }
}

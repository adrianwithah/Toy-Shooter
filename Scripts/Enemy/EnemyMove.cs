using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {

  UnityEngine.AI.NavMeshAgent agent;
  GameObject target;
  EnemyShoot enemyShoot;
  EnemyRoot enemyRoot;
  Vector3 enemyLookAt;
  float rotateSpeed = 5f;
  bool isRotating = false;
  EnemyCommand queuedCommand;

  public enum EnemyCommand {
    NOTHING,
    SHOOT,
    ROOT
  }

  void Awake() {
    enemyShoot = GetComponent<EnemyShoot>();
    agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    target = GameObject.FindGameObjectWithTag("Player");
    enemyRoot = GetComponent<EnemyRoot>();
  }

  void Update() {
    Turn();
    Move();
    ExecuteCommand(queuedCommand);
  }

  void Stop() {
    agent.isStopped = true;
  }

  void Resume() {
    agent.isStopped = false;
  }

  //we do not want the enemy to move closer to the player than needed.
  void Move() {
    if (enemyShoot.PlayerInRange) {
      agent.isStopped = true;
    } else {
      agent.destination = target.transform.position;
      agent.isStopped = false;
    }
  }

  public bool IsRotating {
    get {
      return isRotating;
    }
  }

  void ExecuteCommand(EnemyCommand queuedCommand) {
    switch (queuedCommand) {
      case EnemyCommand.SHOOT:
        if (!isRotating) {
          if (enemyShoot.PlayerInRange) {
            enemyShoot.ShootProjectile(target);
          }
          this.queuedCommand = EnemyCommand.NOTHING;
        }
        return;
      case EnemyCommand.ROOT:
        if (!isRotating) {
          this.queuedCommand = EnemyCommand.NOTHING;
          if (enemyRoot.PlayerInRange) {
            enemyRoot.ShootRootProjectile(target);
          }
        }
        return;
      default:
        return;
    }
  }

  public void NewCommand(EnemyCommand command) {
    this.queuedCommand = command;
  }

  public void Turn() {
    enemyLookAt = target.transform.position - transform.position;
    enemyLookAt.y = 0.0f;
    Quaternion newRotation = Quaternion.LookRotation(enemyLookAt);
    if (Quaternion.Angle(transform.rotation, newRotation) < 30f) {
      isRotating = false;
    } else {
      isRotating = true;
    }
    transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotateSpeed);
  }
}

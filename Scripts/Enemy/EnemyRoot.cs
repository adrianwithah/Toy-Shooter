using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoot : MonoBehaviour {

  public GameObject projObj;
  GameObject player;
  float timer = 0f;
  float skillCooldown = 5f;
  float useSkillProb = 0.5f;
  bool playerInRange = false;
  float skillRange = 15f;
  EnemyMove enemyMove;

  void Awake() {
    enemyMove = GetComponent<EnemyMove>();
    player = GameObject.FindGameObjectWithTag("Player");
  }

  void Update() {
    Root();
  }

  public bool PlayerInRange {
		get {
      return playerInRange;
    }
	}
  //Start delibrating whether to use spell when enter range.
  //if using, send new command which will sync with rotation.
  //once rotated, if still in range (with a little buffer?) use spell.
  //if exit range, we need to move towards target first? do we
  //unqueue command or keep it in queue?
  void Root() {
    timer += Time.deltaTime;
    if ((player.transform.position - transform.position).magnitude < skillRange) {
			playerInRange = true;
      if (timer >= skillCooldown && Random.value < useSkillProb && !player.GetComponent<PlayerStatus>().IsDead) {
        enemyMove.NewCommand(EnemyMove.EnemyCommand.ROOT);
      }
		} else {
			playerInRange = false;
		}
  }

  public void ShootRootProjectile(GameObject target) {
    timer = 0f;
		Vector3 offset = new Vector3(0.0f,0.5f,0.0f);
		GameObject projectile = Instantiate(projObj, transform.position + offset, transform.rotation);
		projectile.GetComponent<RootProjectile>().SetTarget(target);
  }
}

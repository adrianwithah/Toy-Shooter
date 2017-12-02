using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour {

	public GameObject projectObj;
	GameObject player;
	float timer = 0f;
	float attackCooldown = 1.5f;
	bool playerInRange = false;
	float attackRange = 10f;
	EnemyMove enemyMove;

	void Awake() {
		enemyMove = GetComponent<EnemyMove>();
		player = GameObject.FindGameObjectWithTag("Player");
	}

	// Update is called once per frame
	void Update () {
		Attack();
	}

	public bool PlayerInRange {
		get {
			return playerInRange;
		}
	}

	void Attack() {
		timer += Time.deltaTime;
		if ((player.transform.position - transform.position).magnitude < attackRange) {
			playerInRange = true;
			if (timer >= attackCooldown && !player.GetComponent<PlayerStatus>().IsDead) {
				enemyMove.NewCommand(EnemyMove.EnemyCommand.SHOOT);
			}
		} else {
			playerInRange = false;
		}
	}

	public void ShootProjectile(GameObject target) {
		timer = 0f;
		Vector3 offset = new Vector3(0.0f,0.5f,0.0f);
		GameObject projectile = Instantiate(projectObj, transform.position + offset, transform.rotation);
		projectile.GetComponent<ShootProjectile>().SetTarget(target);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageEnemy : MonoBehaviour {

	int numToSpawn = 5;
	int numAlive = 0;
	float spawnInterval = 5f;
	public Transform[] spawnPoints;
	public GameObject enemy;
	public ManageGame manageGame;

	// Use this for initialization
	void Start () {
		InvokeRepeating("Spawn", spawnInterval, spawnInterval);
	}

	public int GetEnemiesLeft() {
		return numToSpawn + numAlive;
	}

	public bool NoEnemiesLeft() {
		return (numToSpawn == 0 && numAlive == 0);
	}

	public void RecordDeath() {
		numAlive -= 1;
	}

	void Spawn() {
		numToSpawn -= 1;
		numAlive += 1;
		int spawnPointIndex = Random.Range(0, spawnPoints.Length);
		Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
		if (numToSpawn <= 0) {
			CancelInvoke();
		}
	}
}

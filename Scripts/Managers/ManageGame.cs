using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ManageGame : MonoBehaviour {

	public PlayerStatus playerStatus;
	public PlayerMove playerMove;
	public ManageEnemy manageEnemy;
	Animator anim;
	public float restartDelay = 5f;
	Text scoreText;

	void Awake() {
		anim = GetComponent<Animator>();
		scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		DisplayEnemiesLeft();
		CheckGameOver();
		CheckWin();
	}

	void CheckGameOver() {
		if (playerStatus.IsDead) {
			anim.SetTrigger("GameOver");
		}
	}

	void CheckWin() {
		if (manageEnemy.NoEnemiesLeft()) {
			playerMove.enabled = false;
			anim.SetTrigger("Win");
		}
	}

	void RestartLevel() {
		playerStatus.RestartLevel();
	}

	void DisplayEnemiesLeft() {
		scoreText.text = "Enemies Left: " + manageEnemy.GetEnemiesLeft();
	}
}

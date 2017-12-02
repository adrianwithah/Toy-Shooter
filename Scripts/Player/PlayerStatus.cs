using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour {

	int startingHealth = 100;
	int currentHealth;
	bool isDead;

	public Texture2D attackCursorTexture;
	bool inTargetMode;
	PlayerMove.PlayerCommand targetCommand;

	PlayerMove playerMove;
	Animator anim;
	public AudioClip deathClip;
	public Slider healthSlider;
	AudioSource playerAudio;
	// PlayerMove playerMove;
	// PlayerShoot playerShoot;

	bool disjointing = false;
	Vector3 disjointPoint;
	bool isRooted = false;

	void Awake() {
		playerMove = GetComponent<PlayerMove>();
		playerAudio = GetComponent<AudioSource>();
		anim = GetComponent<Animator>();
	}
	// Use this for initialization
	void Start () {
		currentHealth = startingHealth;
		healthSlider.minValue = 0;
		healthSlider.maxValue = startingHealth;
		healthSlider.value = startingHealth;
	}

	// Update is called once per frame
	void Update () {

	}

	public bool IsDead {
		get {
			return isDead;
		}
	}

	public bool Disjointing {
		get {
			return disjointing;
		}
		set {
			this.disjointing = value;
		}
	}

	public Vector3 DisjointPoint {
		get {
			return disjointPoint;
		}
		set {
			disjointPoint = value;
		}
	}

	public void TakeDamage(int damage) {
		currentHealth -= damage;
		playerAudio.Play();
		healthSlider.value -= damage;
		Debug.Log(healthSlider.value);
		if (currentHealth <= 0 && !isDead) {
			Die();
		}
	}

	public void EnterTargetMode(PlayerMove.PlayerCommand command) {
		inTargetMode = true;
		targetCommand = command;
		Vector2 hotSpot = new Vector2(32, 32);
		Cursor.SetCursor(attackCursorTexture, hotSpot, CursorMode.Auto);
	}

	public void ExitTargetMode() {
		inTargetMode = false;
		targetCommand = PlayerMove.PlayerCommand.NOTHING;
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}

	public bool InTargetMode {
		get {
			return inTargetMode;
		}
	}

	public PlayerMove.PlayerCommand TargetCommand {
		get {
			return targetCommand;
		}
	}

	public bool IsRooted {
		get {
			return isRooted;
		}
		set {
			this.isRooted = value;
		}
	}

	void Die() {
		isDead = true;
		anim.SetTrigger("Die");
		playerAudio.clip = deathClip;
		playerAudio.Play();
		playerMove.enabled = false;
	}

	public void RestartLevel() {
		ExitTargetMode();
		SceneManager.LoadScene(0);
	}
}

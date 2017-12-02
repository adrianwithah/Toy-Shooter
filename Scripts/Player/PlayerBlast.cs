using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBlast : MonoBehaviour {

	float camRayLength = 100f;
	int floorMask;
	RaycastHit rayHit;
	AudioSource gunAudio;
	Light gunLight;
	float effectsDisplayTime = 0.2f;
	GameObject player;
	PlayerMove playerMove;
	float timer;

	PlayerStatus playerStatus;

	float blastCooldown = 2f;
	public Texture2D attackCursor;
	public GameObject blastProjectile;
	Vector3 blastForward;
	public Image skillImage;
	bool coolingDown = false;

	void Awake() {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerMove = player.GetComponent<PlayerMove>();
		gunLight = GetComponent<Light> ();
		gunAudio = GetComponent<AudioSource>();
		floorMask = LayerMask.GetMask("Floor");
		playerStatus = player.GetComponent<PlayerStatus>();
	}

	void Update() {
		timer += Time.deltaTime;
		CheckBlast();
		if (timer >= effectsDisplayTime) {
			gunLight.enabled = false;
		}
	}

	public void StartBlast() {
		StartCoroutine("Blast");
	}

	void CheckBlast() {
		if (Input.GetButtonDown("Blast") && !coolingDown) {
			playerStatus.EnterTargetMode(PlayerMove.PlayerCommand.BLAST);
		}
		if (playerStatus.TargetCommand == PlayerMove.PlayerCommand.BLAST) {
			if (Input.GetMouseButtonDown(0)) {
				Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(camRay, out rayHit, camRayLength, floorMask)) {
					playerMove.Stop();
					playerMove.TurnPlayer(rayHit.point);
					playerMove.NewCommand(PlayerMove.PlayerCommand.BLAST);
					playerStatus.ExitTargetMode();
				} else {
					return;
				}
			}
		}
	}

	IEnumerator Blast() {
		timer = 0f;
		gunAudio.Play();
		gunLight.enabled = true;
		blastForward = rayHit.point - transform.position;
		blastForward.y = 0.0f;
		Instantiate(blastProjectile, transform.position, Quaternion.LookRotation(blastForward));
		coolingDown = true;
		skillImage.fillAmount = 0;
		while (timer < blastCooldown) {
			skillImage.fillAmount = (timer / blastCooldown);
			yield return null;
		}
		coolingDown = false;
	}
}

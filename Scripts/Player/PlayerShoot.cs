using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

	float camRayLength = 100f;
	int floorMask;
	PlayerStatus playerStatus;
	float range = 20f;
	Ray shootRay = new Ray();
	RaycastHit shootRayHit;
	RaycastHit rayHit;
	ParticleSystem gunParticles;
	LineRenderer gunLine;
	AudioSource gunAudio;
	Light gunLight;
	float effectsDisplayTime = 0.2f;
	GameObject player;
	PlayerMove playerMove;
	float timer;
	int shootableMask;
	int damagePerShot = 5;

	public Texture2D attackCursor;

	void Awake() {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerMove = player.GetComponent<PlayerMove>();
		playerStatus = player.GetComponent<PlayerStatus>();
		gunParticles = GetComponent<ParticleSystem> ();
		gunLine = GetComponent <LineRenderer> ();
		gunAudio = GetComponent<AudioSource> ();
		gunLight = GetComponent<Light> ();
		floorMask = LayerMask.GetMask("Floor");
		shootableMask = LayerMask.GetMask("Shootable");
	}

	void Update() {
		timer += Time.deltaTime;
		Attack();
		if (timer >= effectsDisplayTime) {
			gunLine.enabled = false;
			gunLight.enabled = false;
		}
	}
	public void Shoot() {
		timer = 0f;
		gunAudio.Play();
		gunLight.enabled = true;
		gunParticles.Stop();
		gunParticles.Play();
		gunLine.enabled = true;
		gunLine.SetPosition(0, transform.position);
		shootRay.origin = transform.position;
		shootRay.direction = rayHit.point - transform.position;
		shootRay.direction = new Vector3(shootRay.direction.x, 0.0f, shootRay.direction.z);
		if (Physics.Raycast(shootRay, out shootRayHit, range, shootableMask)) {
			EnemyHP enemyHP = shootRayHit.collider.GetComponent<EnemyHP>();
			if (enemyHP != null) {
				enemyHP.TakeDamage(damagePerShot, shootRayHit.point);
			}
			gunLine.SetPosition(1, shootRayHit.point);
		} else {
			gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
		}
	}
	void Attack() {
		if (Input.GetButtonDown("Fire1")) {
			playerStatus.EnterTargetMode(PlayerMove.PlayerCommand.SHOOT);
		}
		if (Input.GetMouseButtonDown(0) && playerStatus.TargetCommand == PlayerMove.PlayerCommand.SHOOT) {
			Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(camRay, out rayHit, camRayLength, floorMask)) {
				playerMove.Stop();
				playerMove.TurnPlayer(rayHit.point);
				playerMove.NewCommand(PlayerMove.PlayerCommand.SHOOT);
				playerStatus.ExitTargetMode();
			} else {
				return;
			}
		}
	}
}

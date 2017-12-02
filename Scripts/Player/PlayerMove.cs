using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

	public enum PlayerCommand {
		NOTHING,
		MOVE,
		SHOOT,
		DISJOINT,
		BLAST,
		STOP
	};

	Animator anim;
	UnityEngine.AI.NavMeshAgent agent;
	Vector3 movement;
	float camRayLength = 100f;
	int floorMask;
	float rotateSpeed = 10f;
	Vector3 playerLookAt;
	bool isRotating = false;
	PlayerCommand queuedCommand = PlayerCommand.NOTHING;
	PlayerShoot playerShoot;
	PlayerDisjoint playerDisjoint;
	PlayerBlast playerBlast;
	RaycastHit rayHit;
	PlayerStatus playerStatus;


	public Texture2D attackCursor;

	void Awake() {
		anim = GetComponent<Animator>();
		floorMask = LayerMask.GetMask("Floor");
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		playerShoot = GameObject.FindGameObjectWithTag("GunBarrelEnd").GetComponent<PlayerShoot>();
		playerStatus = GetComponent<PlayerStatus>();
		playerBlast = GameObject.FindGameObjectWithTag("GunBarrelEnd").GetComponent<PlayerBlast>();
		playerDisjoint = GetComponent<PlayerDisjoint>();
	}

	void Start() {
		agent.updateRotation = false;
	}
	// Update is called once per frame
	//Here, player is a NavMeshAgent as he has to path-find.
	void Update() {
		Turn();
		CheckMove();
		ExecuteCommand(queuedCommand);
	}

	public void Stop() {
		agent.destination = transform.position;
		playerLookAt = transform.forward;
		anim.SetBool("IsWalking", false);
	}
	public void TurnPlayer(Vector3 rayHitPoint) {
		playerLookAt = rayHitPoint - transform.position;
	}
	public void WarpAgent(Vector3 destination) {
		agent.Warp(destination);
	}

	public Vector3 PlayerLookAt {
		get {
			return playerLookAt;
		}
	}

	//Commands have to be queued and executed because we want to
	//store the command we are trying to execute, until player is done
	//rotating. If during rotation another command is issued, we can simply
	//replace the command and the previous command never occurs.
	void ExecuteCommand(PlayerCommand command) {
		switch (command) {
			case PlayerCommand.STOP:
				Stop();
				queuedCommand = PlayerCommand.NOTHING;
				return;
			case PlayerCommand.MOVE:
				if (!isRotating) {
					Move();
					queuedCommand = PlayerCommand.NOTHING;
				}
				return;
			case PlayerCommand.SHOOT:
				if (!isRotating) {
					playerShoot.Shoot();
					queuedCommand = PlayerCommand.NOTHING;
				}
				return;
			case PlayerCommand.DISJOINT:
				if (!isRotating) {
					//additional check in case collision detected between
					//disjoint and click.
					if (!playerStatus.IsRooted) {
						playerDisjoint.StartDisjoint();
					}
					queuedCommand = PlayerCommand.NOTHING;
				}
				return;
			case PlayerCommand.BLAST:
				if (!isRotating) {
					playerBlast.StartBlast();
					queuedCommand = PlayerCommand.NOTHING;
				}
				return;
			default:
				return;
		}
	}

	public void NewCommand(PlayerCommand command) {
		queuedCommand = command;
	}

	void CheckMove() {
		if (Input.GetMouseButtonDown(1)) {
			Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(camRay, out rayHit, camRayLength, floorMask)) {
				playerStatus.ExitTargetMode();
				Stop();
				TurnPlayer(rayHit.point);
				if (!playerStatus.IsRooted) {
					NewCommand(PlayerCommand.MOVE);
				}
			}
		}
		if (Input.GetButtonDown("Stop")) {
			NewCommand(PlayerCommand.STOP);
		}
		if ((rayHit.point - transform.position).magnitude <= agent.stoppingDistance) {
			anim.SetBool("IsWalking", false);
		}
	}

	void Move() {
		agent.destination = rayHit.point;
		anim.SetBool("IsWalking", true);
	}

	void Turn() {
		if (playerLookAt == Vector3.zero) {
			return;
		}
		playerLookAt.y = 0.0f;
		Quaternion newRotation = Quaternion.LookRotation(playerLookAt);
		if (Quaternion.Angle(transform.rotation, newRotation) < 50f) {
			isRotating = false;
		} else {
			isRotating = true;
		}
		transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotateSpeed);
	}
}

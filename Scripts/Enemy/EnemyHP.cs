using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour {

  float startingHealth = 100;
  float currentHealth;
  ParticleSystem hitParticles;
  ManageEnemy manageEnemy;
  bool isDead;
  float sinkSpeed = 2.5f;
  bool isSinking = false;
  public AudioClip deathClip;
  AudioSource enemyAudio;
  EnemyMove enemyMove;

  public Image healthBarImage;
	GameObject canvas;
	Camera cam;
	Image healthBar;
	Vector3 healthBarOffset = new Vector3(0f, 1.5f, 0f);
  float originalScale;

  CapsuleCollider capsuleCollider;
  Animator anim;

  void Awake() {
    hitParticles = GetComponentInChildren<ParticleSystem>();
    currentHealth = startingHealth;
    canvas = GameObject.FindGameObjectWithTag("HUDCanvas");
    cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    manageEnemy = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<ManageEnemy>();
    capsuleCollider = GetComponent<CapsuleCollider>();
    anim = GetComponent<Animator>();
    enemyAudio = GetComponent<AudioSource>();
    enemyMove = GetComponent<EnemyMove>();
  }

  void Start() {
    healthBar = (Image) Instantiate(healthBarImage);
		healthBar.transform.SetParent(canvas.transform);
    healthBar.transform.SetSiblingIndex(4);
    originalScale = healthBar.transform.localScale.x;
  }

  void Update() {
    if (!isDead) {
      healthBar.transform.position = cam.WorldToScreenPoint(transform.position + healthBarOffset);
    }
    if (isSinking) {
        transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
    }
  }

  public void TakeDamage(int damage, Vector3 hitPoint) {
    if (isDead) {
      return;
    }
    currentHealth -= damage;
    hitParticles.transform.position = hitPoint;
    Vector3 tmpScale = healthBar.transform.localScale;
    tmpScale.x = currentHealth / startingHealth * originalScale;
    healthBar.transform.localScale = tmpScale;
    hitParticles.Play();
    if (currentHealth <= 0) {
      Die();
    }
  }

  void Die() {
    isDead = true;
    capsuleCollider.isTrigger = true;
    anim.SetTrigger("Dead");
    enemyAudio.clip = deathClip;
    enemyAudio.Play();
    manageEnemy.RecordDeath();
    Destroy(healthBar.gameObject);
    StartSinking();
  }

  public void StartSinking() {
    GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
    GetComponent<Rigidbody>().isKinematic = true;
    enemyMove.enabled = false;
    isSinking = true;
    Destroy(gameObject, 2f);
  }
}

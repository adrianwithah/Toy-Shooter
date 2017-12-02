using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

	int boundary = 10;
	int screenWidth;
	int screenHeight;
	float panSpeed = 60f;
	public float smoothing = 5f;
	Vector3 offset;
	// Use this for initialization
	void Awake() {
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		offset = transform.position - GameObject.FindGameObjectWithTag("Player").transform.position;
	}

	void Start () {

	}

	void Update() {
		CheckRefocus();
	}

	void FixedUpdate () {
		EdgePan();
	}

	void EdgePan() {
		Vector3 newPos = transform.position;
		if (Input.mousePosition.x < boundary) {
			newPos.x = Mathf.Clamp(newPos.x - panSpeed * Time.deltaTime, -20, 20);
			transform.position = Vector3.Lerp(transform.position, newPos, smoothing * Time.deltaTime);
		}
		if (Input.mousePosition.y < boundary) {
			newPos.z = Mathf.Clamp(newPos.z - panSpeed * Time.deltaTime, -26, 25);
			transform.position = Vector3.Lerp(transform.position, newPos, smoothing * Time.deltaTime);
		}
		if (Input.mousePosition.x > screenWidth - boundary) {
			newPos.x = Mathf.Clamp(newPos.x + panSpeed * Time.deltaTime, -20, 20);
			transform.position = Vector3.Lerp(transform.position, newPos, smoothing * Time.deltaTime);
		}
		if (Input.mousePosition.y > screenHeight - boundary) {
			newPos.z = Mathf.Clamp(newPos.z + panSpeed * Time.deltaTime, -26, 25);
			transform.position = Vector3.Lerp(transform.position, newPos, smoothing * Time.deltaTime);
		}
	}

	void CheckRefocus() {
		if (Input.GetButtonDown("PlayerFocus")) {
			FocusCamera(GameObject.FindGameObjectWithTag("Player"));
		}
	}

	void FocusCamera(GameObject gameObject) {
		transform.position = gameObject.transform.position + offset;
	}
}

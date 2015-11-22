﻿using UnityEngine;
using System.Collections;

public class Snowman : MonoBehaviour {
	public GameObject snowball;
	public GameObject gun;
	public GameObject hover;
	Vector3 gunTip;
	public string direction;
	public bool moving;
	public float moveSpeed;
	bool shooting;
	float bulletSpeed;
	bool loading;
	// Use this for initialization
	void Start () {
		bulletSpeed = 7f;
		moveSpeed = 1f;
		gunTip = new Vector3(gun.transform.position.x, gun.transform.position.y, gun.transform.position.z);
		shooting = true;
		StartCoroutine(ShootCoroutine());
		StartCoroutine(hoverAnim());
	}
	
	// Update is called once per frame
	void Update () {
		if(moving && !shooting && !loading) {
			if(direction == "+x") {
				GetComponent<Rigidbody>().velocity = new Vector3(moveSpeed, 0f, 0f);
			} else if (direction == "-x") {
				GetComponent<Rigidbody>().velocity = new Vector3(-moveSpeed, 0f, 0f);
			} else if (direction == "+z") {
				GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, moveSpeed);
			} else if (direction == "-z") {
				GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, -moveSpeed);
			}
		} else {
			GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
		}
		RaycastHit hit;
		Debug.Log (shooting);
		if(moving) {
			Debug.Log ("moving is true");
			if(direction == "+x" && Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 0.7f, transform.position.z), Vector3.right, out hit, 7f)) {
				if(hit.collider.name == "Player") {
					shooting = true;
					Debug.Log ("Snowman is chasing player");
				} else {
					Debug.Log ("setting shooting ot false");
					shooting = false;
				}
			} else if (direction == "-x" && Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 0.7f, transform.position.z), Vector3.left, out hit, 7f)) {
				if(hit.collider.name == "Player") {
					shooting = true;
					Debug.Log ("Snowman is chasing player");
				} else {
					Debug.Log ("setting shooting ot false");
					shooting = false;
				}
			} else if (direction == "+z" && Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 0.7f, transform.position.z), Vector3.forward, out hit, 7f)) {
				Debug.Log (hit.collider.name);
				if(hit.collider.name == "Player") {
					shooting = true;
					Debug.Log ("Snowman is chasing player");
				} else {
					Debug.Log ("setting shooting ot false");
					shooting = false;
				}
			} else if (direction == "-z" && Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 0.7f, transform.position.z), Vector3.back, out hit, 7f)) {
				if(hit.collider.name == "Player") {
					shooting = true;
					Debug.Log ("Snowman is chasing player");
				} else {
					Debug.Log ("setting shooting ot false");
					shooting = false;
				}
			}
		}
	}

	IEnumerator hoverAnim() {
		while(true) {
			GameObject childObject = (GameObject)Instantiate(hover, hover.transform.position, Quaternion.identity);
			childObject.transform.parent = transform;
			childObject.transform.localScale = hover.transform.localScale;
			yield return new WaitForSeconds(0.3f);
		}
	}

	IEnumerator ShootCoroutine() {
		while(true) {
			if(shooting) {
				Shoot ();
				loading = true;
				yield return new WaitForSeconds(2.5f);
				loading = false;
			}
			yield return 0;
		}
	}

	IEnumerator chargeGun() {
		Material mat = gun.GetComponent<SkinnedMeshRenderer>().material;
		Color currentColor = mat.color;
		Color targetColor = new Color(1f, 77/255f , 103f/255f);
		while(currentColor != targetColor) {
			mat = gun.GetComponent<SkinnedMeshRenderer>().material;
			currentColor = mat.color;
			mat.color = Color.Lerp (currentColor, targetColor, 0.5f);
			gun.GetComponent<SkinnedMeshRenderer>().material = mat;
			yield return new WaitForSeconds(0.05f);
		}
		StartCoroutine(dechargeGun());
	}

	IEnumerator dechargeGun() {
		Material mat = gun.GetComponent<SkinnedMeshRenderer>().material;
		Color currentColor = mat.color;
		Color targetColor = new Color(0.9f, 0.9f, 0.9f);
		while(currentColor != targetColor) {
			mat = gun.GetComponent<SkinnedMeshRenderer>().material;
			currentColor = mat.color;
			mat.color = Color.Lerp (currentColor, targetColor, 0.5f);
			gun.GetComponent<SkinnedMeshRenderer>().material = mat;
			yield return new WaitForSeconds(0.03f);
		}
	}

	void Shoot() {
		GameObject bullet = (GameObject)Instantiate (snowball, snowball.transform.position, Quaternion.identity);
		bullet.GetComponent<Snowball>().direction = direction;
		bullet.GetComponent<Snowball>().bulletSpeed = bulletSpeed;
		StartCoroutine(chargeGun());
	}
}
﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class End : MonoBehaviour {
	private Vector3 position;
	public string nextLevel;
	GameObject[] taggedGameObjects;
	public int threeStarMax;
	public int twoStarMax;
	public int oneStarMax;
	void Start() {
		position = transform.position;
		GameObject.Find ("Canvas").GetComponent<GameManager>().endGameTime();
		GameObject parObject = (GameObject)Instantiate(Resources.Load("Par"));
		parObject.transform.parent = GameObject.Find("Canvas").transform;
		parObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
		parObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);
		parObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -27f, 0f);
		parObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
		parObject.GetComponent<RectTransform>().sizeDelta = new Vector2(800f, 50f);
		parObject.GetComponent<Canvas>().sortingOrder = 1;
		parObject.transform.localEulerAngles = Vector3.zero;
		GameObject.Find("3StarText").GetComponent<Text>().text = threeStarMax.ToString();
		GameObject.Find("2StarText").GetComponent<Text>().text = twoStarMax.ToString();
		GameObject.Find("1StarText").GetComponent<Text>().text = oneStarMax.ToString();
	}
	void OnTriggerEnter(Collider coll) {
		if(coll.gameObject.name == "Player") {
			coll.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
			GameManager.started = false;
			StartCoroutine(fly (coll.gameObject, nextLevel));
			coll.gameObject.GetComponent<Rigidbody>().useGravity = false;
			coll.gameObject.GetComponent<Rigidbody>().isKinematic = true;
		}
	}

	IEnumerator fly(GameObject item, string nextLevel) {
		Destroy(GameObject.Find ("Player").GetComponent<Collider>());
		taggedGameObjects = GameObject.FindGameObjectsWithTag("Platform"); 
		item.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
		Material mat = item.GetComponentInChildren<MeshRenderer>().material;
		while (mat.color.a > 0f)
		{
			Color newColor = mat.color;
			newColor.a -= Time.deltaTime * 3f;
			item.GetComponentInChildren<MeshRenderer>().material.color = newColor;
			yield return null;
		}
		yield return new WaitForSeconds(0.6f);
		Debug.Log ("setting next level to: " + (int.Parse(Application.loadedLevelName) + 1));
		PlayerPrefs.SetInt ("nextLevel", int.Parse(Application.loadedLevelName) + 1);
		if(PlayerPrefs.GetInt ("permaNextLevel") < PlayerPrefs.GetInt ("nextLevel")) {
			PlayerPrefs.SetInt ("permaNextLevel", PlayerPrefs.GetInt ("nextLevel"));
		}
		if(int.Parse(GameObject.Find ("Canvas").GetComponent<GameManager>().clonesCount.GetComponent<Text>().text) <= threeStarMax) {
			PlayerPrefs.SetInt("levelStar" + Application.loadedLevelName, 3);
			PlayerPrefs.SetInt("PassedLevelStars", 3);
		} else if (int.Parse(GameObject.Find ("Canvas").GetComponent<GameManager>().clonesCount.GetComponent<Text>().text) <= twoStarMax) {
			if(PlayerPrefs.GetInt("levelStar" + Application.loadedLevelName) < 2) {
				PlayerPrefs.SetInt("levelStar" + Application.loadedLevelName, 2);
			}
			PlayerPrefs.SetInt("PassedLevelStars", 2);
		} else if (int.Parse(GameObject.Find ("Canvas").GetComponent<GameManager>().clonesCount.GetComponent<Text>().text) <= oneStarMax){
			if(PlayerPrefs.GetInt("levelStar" + Application.loadedLevelName) < 1) {
				PlayerPrefs.SetInt("levelStar" + Application.loadedLevelName, 1);
			}
			PlayerPrefs.SetInt("PassedLevelStars", 1);
		} else {
			if(PlayerPrefs.GetInt("levelStar") + Application.loadedLevelName != "0") {
				PlayerPrefs.SetInt("levelStar" + Application.loadedLevelName, -1);
			}
			PlayerPrefs.SetInt("PassedLevelStars", -1);
		}
		Application.LoadLevel ("LevelSelector");
	}
}

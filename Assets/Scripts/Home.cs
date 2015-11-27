﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Home : MonoBehaviour {
	public Text text;

	public RectTransform panel;
	public Button[] bttn;
	public RectTransform center;
	public Button selectedButton;
	public GameObject shopPanel;
	private float[] distance;
	private bool dragging = false;
	private int bttnDistance;
	private int minButtonNum;
	public Sprite unknown;

	public Sprite PinkGuy;
	public Sprite BlueGuy;
	public Sprite WhiteGuy;
	public Sprite BlackGuy;
	public Sprite Gold;
	public Sprite Ice;
	public Sprite Smile;
	public Sprite Ghost;
	public Sprite Radioactive;
	public Sprite Drake;

	public GameObject selectButton;
	void Start() {
		text.text = PlayerPrefs.GetInt ("coins").ToString();
		int bttnLength = bttn.Length;
		distance = new float[bttnLength];
		bttnDistance = (int)Mathf.Abs (bttn[1].GetComponent<RectTransform>().anchoredPosition.x - bttn[0].GetComponent<RectTransform>().anchoredPosition.x);
		foreach(Button button in bttn) {
			string[] splitString = button.name.Split('-');
			string itemName = splitString[0]; //name of item
			//Debug.Log (splitString[1]); //price
			if(PlayerPrefs.GetString (itemName) == "true") {
				button.GetComponent<Image>().color = Color.white;
			} else {
				button.GetComponent<Image>().sprite = unknown;
			}
		}
	}
	void Update() {
		for(int i = 0; i < bttn.Length; i++) {
			distance[i] = Mathf.Abs(center.transform.position.x - bttn[i].transform.position.x);
		}
		float minDistance = Mathf.Min (distance);
		for(int a = 0; a < bttn.Length; a++) {
			if(minDistance == distance[a]) {
				selectedButton = bttn[a];
				minButtonNum = a;
			}
		}
		if(!dragging) {
			LerpToBttn(minButtonNum * -bttnDistance);
		}

		if(shopPanel.activeSelf == true) {
		string[] splitString = selectedButton.name.Split('-');
		if(PlayerPrefs.GetString (splitString[0]) == "true") {
			selectButton.GetComponentInChildren<Text>().text = "Select";
		} else {
			selectButton.GetComponentInChildren<Text>().text = "Buy";
		}
		}
	}

	public void BuyOrSelect() {
		string[] splitString = selectedButton.name.Split('-');
		if(PlayerPrefs.GetString (splitString[0]) == "true") { //select
			PlayerPrefs.SetString ("selected", splitString[0]);
			shopPanel.SetActive(false);
		} else { //buy
			int coins = PlayerPrefs.GetInt ("coins");
			if(splitString[1] != "Free") {
				coins = coins - int.Parse(splitString[1]);
			}
			if(coins >= 0) {
				PlayerPrefs.SetString (splitString[0], "true");
				if(splitString[0] == "BlueGuy") {
					selectedButton.GetComponent<Image>().sprite = BlueGuy;
				} else if (splitString[0] == "WhiteGuy") {
					selectedButton.GetComponent<Image>().sprite = WhiteGuy;
				} else if (splitString[0] == "BlackGuy") {
					selectedButton.GetComponent<Image>().sprite = BlackGuy;
				} else if (splitString[0] == "Gold") {
					selectedButton.GetComponent<Image>().sprite = Gold;
				} else if (splitString[0] == "Ice") {
					selectedButton.GetComponent<Image>().sprite = Ice;
				} else if (splitString[0] == "Smile") {
					selectedButton.GetComponent<Image>().sprite = Smile;
				} else if (splitString[0] == "Ghost") {
					selectedButton.GetComponent<Image>().sprite = Ghost;
				} else if (splitString[0] == "Radioactive") {
					selectedButton.GetComponent<Image>().sprite = Radioactive;
				} else if (splitString[0] == "Drake") {
					selectedButton.GetComponent<Image>().sprite = Drake;
				}
				PlayerPrefs.SetInt ("coins", coins);
				text.text = PlayerPrefs.GetInt ("coins").ToString();
			}
		}
	}

	public void openShop() {
		shopPanel.SetActive (true);
	}

	void LerpToBttn(int position) {
		float newX = Mathf.Lerp (panel.anchoredPosition.x, position, Time.deltaTime * 10f);
		Vector2 newPosition = new Vector2(newX, panel.anchoredPosition.y);
		panel.anchoredPosition = newPosition;
	}

	public void StartDrag() {
		dragging = true;
	}

	public void EndDrag() {
		dragging = false;
	}

	public void LevelSelect() {
		Application.LoadLevel("LevelSelector");
	}

}
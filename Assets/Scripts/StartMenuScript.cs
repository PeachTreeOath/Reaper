using UnityEngine;
using System.Collections;

public class StartMenuScript : MonoBehaviour
{

	int numPlayers = 0;
	GameObject q1, q2, q3, q4, startButton;
	GlobalObject globalObj;
	private bool[] playerArr;

	void Start ()
	{
		playerArr = new bool[4];

		q1 = GameObject.Find ("Q1");
		q2 = GameObject.Find ("Q2");
		q3 = GameObject.Find ("Q3");
		q4 = GameObject.Find ("Q4");
		startButton = GameObject.Find ("Start");

		q1.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/PressA");
		q2.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/PressA");
		q3.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/PressA");
		q4.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/PressA");

		startButton.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/PressStart");
		globalObj = GameObject.Find ("GlobalObject").GetComponent<GlobalObject> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown ("Jump_p1")) {
			AddPlayer (1);
		}
		if (Input.GetButtonDown ("Jump_p2")) {
			AddPlayer (2);
		}
		if (Input.GetButtonDown ("Jump_p3")) {
			AddPlayer (3);
		}
		if (Input.GetButtonDown ("Jump_p4")) {
			AddPlayer (4);
		}

		if (Input.GetButtonDown ("Submit")) {
			Application.LoadLevel (1);
		}
	}

	private void AddPlayer (int playerNum)
	{
		numPlayers += 1;
		globalObj.numPlayers = numPlayers;

		switch (playerNum) {
		case 1:
			globalObj.p1JoyMap = GetJoyMap ();
			break;
		case 2:
			globalObj.p2JoyMap = GetJoyMap ();
			break;
		case 3:
			globalObj.p3JoyMap = GetJoyMap ();
			break;
		case 4:
			globalObj.p4JoyMap = GetJoyMap ();
			break;
		}

		switch (numPlayers) {
		case 1:
			q1.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("None");
			q1.GetComponent<Animator> ().runtimeAnimatorController = Resources.Load <RuntimeAnimatorController> ("Images/Kiwi/KiwiDanceController");
			break;
		case 2:
			q2.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("None");
			q2.GetComponent<Animator> ().runtimeAnimatorController = Resources.Load <RuntimeAnimatorController> ("Images/Kiwi/KiwiDanceController");
			break;
		case 3:
			q3.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("None");
			q3.GetComponent<Animator> ().runtimeAnimatorController = Resources.Load <RuntimeAnimatorController> ("Images/Kiwi/KiwiDanceController");
			break;
		case 4:
			q4.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("None");
			q4.GetComponent<Animator> ().runtimeAnimatorController = Resources.Load <RuntimeAnimatorController> ("Images/Kiwi/KiwiDanceController");
			break;
		}
	}

	private string GetJoyMap ()
	{
		switch (numPlayers) {
		case 1:
			return "_p1";
		case 2:
			return "_p2";
		case 3:
			return "_p3";
		case 4:
			return "_p4";
		}
		return "";
	}
}

using UnityEngine;
using System.Collections;

public class StartMenuScript : MonoBehaviour
{

	int numPlayers = 0;
	GameObject q1, q2, q3, q4, startButton;
	GlobalObject globalObj;
	private bool[] playerArr;
	private GameObject playerRes;

	void Start ()
	{
		playerArr = new bool[4];

		playerRes = Resources.Load<GameObject> ("Prefabs/SelectPlayer");
		q1 = GameObject.Find ("Q1");
		q2 = GameObject.Find ("Q2");
		q3 = GameObject.Find ("Q3");
		q4 = GameObject.Find ("Q4");
		startButton = GameObject.Find ("Start");

		q1.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/PressA");
		q2.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/PressA");
		q3.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/PressA");
		q4.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/PressA");

		globalObj = GameObject.Find ("GlobalObject").GetComponent<GlobalObject> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown ("Jump_p1")) {
			if (!playerArr [0]) {
				AddPlayer (1);
				playerArr [0] = true;
			}
		}
		if (Input.GetButtonDown ("Jump_p2")) {
			if (!playerArr [1]) {
				AddPlayer (2);
				playerArr [1] = true;
			}
		}
		if (Input.GetButtonDown ("Jump_p3")) {
			if (!playerArr [2]) {
				AddPlayer (3);
				playerArr [2] = true;
			}
		}
		if (Input.GetButtonDown ("Jump_p4")) {
			if (!playerArr [3]) {
				AddPlayer (4);
				playerArr [3] = true;
			}
		}

		if (numPlayers > 0 && Input.GetButtonDown ("Submit")) {
			Application.LoadLevel (2);
		}
	}

	private void AddPlayer (int playerNum)
	{
		startButton.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/PressStart");

		numPlayers += 1;
		globalObj.numPlayers = numPlayers;

		switch (numPlayers) {
		case 1:
			globalObj.p1JoyMap = "_p" + playerNum;
			break;
		case 2:
			globalObj.p2JoyMap = "_p" + playerNum;
			break;
		case 3:
			globalObj.p3JoyMap = "_p" + playerNum;
			break;
		case 4:
			globalObj.p4JoyMap = "_p" + playerNum;
			break;
		}

		switch (numPlayers) {
		case 1:
			q1.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("None");
			SelectPlayer playerObj1 = ((GameObject)Instantiate (playerRes, q1.transform.position, Quaternion.identity)).GetComponent<SelectPlayer> ();
			playerObj1.numPlayer = numPlayers - 1;
			playerObj1.transform.localScale = new Vector2 (2, 2);
			//q1.GetComponent<Animator> ().runtimeAnimatorController = Resources.Load <RuntimeAnimatorController> ("Images/Kiwi/KiwiDanceController");
			break;
		case 2:
			q2.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("None");
			SelectPlayer playerObj2 = ((GameObject)Instantiate (playerRes, q2.transform.position, Quaternion.identity)).GetComponent<SelectPlayer> ();
			playerObj2.numPlayer = numPlayers - 1;
			playerObj2.transform.localScale = new Vector2 (2, 2);
			//q2.GetComponent<Animator> ().runtimeAnimatorController = Resources.Load <RuntimeAnimatorController> ("Images/Kiwi/KiwiDanceController");
			break;
		case 3:
			q3.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("None");
			SelectPlayer playerObj3 = ((GameObject)Instantiate (playerRes, q3.transform.position, Quaternion.identity)).GetComponent<SelectPlayer> ();
			playerObj3.numPlayer = numPlayers - 1;
			playerObj3.transform.localScale = new Vector2 (2, 2);
			//q3.GetComponent<Animator> ().runtimeAnimatorController = Resources.Load <RuntimeAnimatorController> ("Images/Kiwi/KiwiDanceController");
			break;
		case 4:
			q4.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("None");
			SelectPlayer playerObj4 = ((GameObject)Instantiate (playerRes, q4.transform.position, Quaternion.identity)).GetComponent<SelectPlayer> ();
			playerObj4.numPlayer = numPlayers - 1;
			playerObj4.transform.localScale = new Vector2 (2, 2);
			//q4.GetComponent<Animator> ().runtimeAnimatorController = Resources.Load <RuntimeAnimatorController> ("Images/Kiwi/KiwiDanceController");
			break;
		}
	}
	/*
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
	*/
}

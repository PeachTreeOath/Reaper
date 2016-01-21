using UnityEngine;
using System.Collections;

public class StartMenuScript : MonoBehaviour {

	int numPlayers = 0; 
	GameObject q1, q2, q3, q4, startButton; 


	private float selectDelay = 0.2f;
	private float selectTime = 0;

	// Use this for initialization

	void Awake () {

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
	
	}

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKey (KeyCode.Space)) {
			selectTime += Time.deltaTime; 
			if (selectTime > selectDelay) {

				numPlayers += 1;
				selectTime = 0; 

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
		} else if ((numPlayers > 0) && (Input.GetKey (KeyCode.Return))) {
			Application.LoadLevel (1);
		}else{
			selectTime = 0;
		}

	}
}

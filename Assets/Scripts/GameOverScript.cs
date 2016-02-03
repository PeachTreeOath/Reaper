using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverScript : MonoBehaviour
{


	enum selectBox
	{
retry,
		menu}

	;

	int selectAt = 0;
	GlobalObject globalObj;
	// Use this for initialization
	void Start ()
	{
		GameObject globalGameObj = GameObject.Find ("GlobalObject");
		if (globalGameObj != null) {
			globalObj = globalGameObj.GetComponent<GlobalObject> ();
		}
		//	GameObject.Find ("ReachedLevel15").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/ReachedLevel15"); 
		GameObject.Find ("MenuButton").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/MenuButton");
		GameObject.Find ("RetryButton").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/RetryButton");
		GameObject.Find ("RetrySelect").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/bgSquareTransparent");
		//	GameObject.Find ("RetrySelect").transform.localScale = new Vector3(2.5F, 1.5F, 0); 
		if (globalObj != null) {
			GameObject.Find("Text").GetComponent<Text>().text = "YOU REACHED STAGE " + globalObj.highestStage + "!";
		}

	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Input.GetAxis ("Horizontal_p1") > 0 && (selectAt == (int)selectBox.retry)) {
			selectAt = (int)selectBox.menu; 
			GameObject.Find ("RetrySelect").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("None");
			GameObject.Find ("MenuSelect").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/bgSquareTransparent");
			//	GameObject.Find ("MenuSelect").transform.localScale = new Vector3(2.5F, 1.5F, 0); 
		} else if (Input.GetAxis ("Horizontal_p1") < 0 && (selectAt == (int)selectBox.menu)) {
			selectAt = (int)selectBox.retry;
			GameObject.Find ("RetrySelect").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/bgSquareTransparent");
			//	GameObject.Find ("RetrySelect").transform.localScale = new Vector3(2.5F, 1.5F, 0); 
			GameObject.Find ("MenuSelect").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("None");

		} else if (Input.GetButtonDown ("Submit")) {

			if (selectAt == (int)selectBox.menu) {
				if (globalObj != null) {
					globalObj.PlayMusic (0);
					globalObj.highestStage = 1;
				}
				Application.LoadLevel (2);
			} else if (selectAt == (int)selectBox.retry) {
				if (globalObj != null) {
					globalObj.PlayMusic (1);
					globalObj.highestStage = 1;
				}
				Application.LoadLevel (3);
			}
		}
	}
}

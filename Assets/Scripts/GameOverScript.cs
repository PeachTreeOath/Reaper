using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {


	enum selectBox{retry, menu}; 
	int selectAt = 0;  

	// Use this for initialization
	void Start () {
		
		GameObject.Find ("ReachedLevel15").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/ReachedLevel15"); 
		GameObject.Find ("MenuButton").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/MenuButton");
		GameObject.Find ("RetryButton").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/RetryButton");
		GameObject.Find ("RetrySelect").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/bgSquareTransparent");
		GameObject.Find ("RetrySelect").transform.localScale = new Vector3(2.5F, 1.5F, 0); 
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.RightArrow) && (selectAt ==  (int) selectBox.retry) ){
			selectAt = (int) selectBox.menu; 
			GameObject.Find ("RetrySelect").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("None");
			GameObject.Find ("MenuSelect").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/bgSquareTransparent");
			GameObject.Find ("MenuSelect").transform.localScale = new Vector3(2.5F, 1.5F, 0); 
		} else if (Input.GetKeyDown (KeyCode.LeftArrow) && ( selectAt == (int) selectBox.menu)) {
			selectAt = (int) selectBox.retry;
			GameObject.Find ("RetrySelect").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/bgSquareTransparent");
			GameObject.Find ("RetrySelect").transform.localScale = new Vector3(2.5F, 1.5F, 0); 
			GameObject.Find ("MenuSelect").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("None");

		} else if (Input.GetKeyDown (KeyCode.Return)) { 

			if (selectAt == (int) selectBox.menu) {
				Application.LoadLevel(1);
			}else if (selectAt == (int) selectBox.retry){
				Application.LoadLevel(2);
			}
		}
	}
}

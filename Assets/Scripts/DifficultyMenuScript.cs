using UnityEngine;
using System.Collections;

public class DifficultyMenuScript : MonoBehaviour
{

	enum arrow
	{
size,
		difficulty}

	;

	enum size
	{
five,
		seven,
		nine}

	;

	enum difficulty
	{
easy,
		medium,
		hard}

	;

	int arrowAt = 0;
	int difficultyAt = 1;
	int sizeAt = 1;
	GlobalObject globalObj;

	// Use this for initialization
	void Start ()
	{
		globalObj = GameObject.Find ("GlobalObject").GetComponent<GlobalObject>();

		GameObject.Find ("SizeText").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/Size"); 
		GameObject.Find ("DifficultyText").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/Difficulty");
		GameObject.Find ("StartText").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/PressStart");
		GameObject.Find ("SizeArrow").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/SelectionArrow");

		GameObject.Find ("Size").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/Seven"); 
		GameObject.Find ("Difficulty").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/Medium"); 
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Input.GetKeyDown (KeyCode.DownArrow) && (arrowAt == (int)arrow.size)) {
			arrowAt = (int)arrow.difficulty; 
			GameObject.Find ("SizeArrow").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("None");
			GameObject.Find ("DifficultyArrow").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/SelectionArrow");
		
		} else if (Input.GetKeyDown (KeyCode.UpArrow) && (arrowAt == (int)arrow.difficulty)) {
			arrowAt = (int)arrow.size;
			GameObject.Find ("SizeArrow").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/SelectionArrow");
			GameObject.Find ("DifficultyArrow").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("None");
		
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) { // decrease settings

			if (arrowAt == (int)arrow.size) {
				sizeAt = (int)Mathf.Clamp (sizeAt - 1, 0, (float)size.nine);
				globalObj.boardSize = sizeAt;
				switch (sizeAt) {
				case (int) size.five: 
					GameObject.Find ("Size").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/Five"); 
					break; 
				case (int) size.seven:
					GameObject.Find ("Size").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/Seven"); 
					break; 
				case (int) size.nine: 
					GameObject.Find ("Size").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/Nine"); 
					break;
				}
			} else if (arrowAt == (int)arrow.difficulty) {

				difficultyAt = (int)Mathf.Clamp (difficultyAt - 1, 0, (float)difficulty.hard);
				globalObj.difficulty = difficultyAt;
				switch (difficultyAt) {

				case (int) difficulty.easy:
					GameObject.Find ("Difficulty").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/Easy"); 
					break;
				case (int) difficulty.medium:
					GameObject.Find ("Difficulty").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/Medium"); 
					break;
				case (int) difficulty.hard:
					GameObject.Find ("Difficulty").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/Hard"); 
					break;
				}
			}

		} else if (Input.GetKeyDown (KeyCode.RightArrow)) { // increase settings

			if (arrowAt == (int)arrow.size) {
				sizeAt = (int)Mathf.Clamp (sizeAt + 1, 0, (float)size.nine);
				globalObj.boardSize = sizeAt;
				switch (sizeAt) {
				case (int) size.five: 
					GameObject.Find ("Size").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/Five"); 
					break; 
				case (int) size.seven:
					GameObject.Find ("Size").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/Seven"); 
					break; 
				case (int) size.nine: 
					GameObject.Find ("Size").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/Nine"); 
					break;
				}
			} else if (arrowAt == (int)arrow.difficulty) {

				difficultyAt = (int)Mathf.Clamp (difficultyAt + 1, 0, (float)difficulty.hard);
				globalObj.difficulty = difficultyAt;
				switch (difficultyAt) {

				case (int) difficulty.easy:
					GameObject.Find ("Difficulty").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/Easy"); 
					break;
				case (int) difficulty.medium:
					GameObject.Find ("Difficulty").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/Medium"); 
					break;
				case (int) difficulty.hard:
					GameObject.Find ("Difficulty").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/Hard"); 
					break;
				}
			}

		}
		if (Input.GetButtonDown ("Submit")) {
			Application.LoadLevel (2);
		}
	}

}

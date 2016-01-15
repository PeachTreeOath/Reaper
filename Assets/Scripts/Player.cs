using UnityEngine;
using System.Collections;

public class Player : BoardObject
{


	// Use this for initialization
	void Start ()
	{
		boardType = BoardType.PLAYER;
	}
	
	// Update is called once per frame
	void Update ()
	{
		base.Update ();
		if (!moving) {
			int direction = 0; // L R U D
			int destR = row;
			int destC = col;
			if (Input.GetAxis ("Horizontal") < 0) {
				destC = Mathf.Clamp (col - 1, 0, 6);
				direction = 1;
			} else if (Input.GetAxis ("Horizontal") > 0) {
				destC = Mathf.Clamp (col + 1, 0, 6);
				direction = 2;
			} else if (Input.GetAxis ("Vertical") < 0) {
				destR = Mathf.Clamp (row + 1, 0, 6);
				direction = 3;
			} else if (Input.GetAxis ("Vertical") > 0) {
				destR = Mathf.Clamp (row - 1, 0, 6);
				direction = 4;
			}
			if (direction != 0) {
				if (mgr.PushBlock (direction, destR, destC)) {
					Move (destR, destC);
				}
			}
		} 
	}

}

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
			if (Input.GetAxis ("Horizontal") < 0) {
				int dest = Mathf.Clamp (col - 1, 0, 7);
				Move (row, dest);
				direction = 1;
			} else if (Input.GetAxis ("Horizontal") > 0) {
				int dest = Mathf.Clamp (col + 1, 0, 7);
				Move (row, dest);
				direction = 2;
			} else if (Input.GetAxis ("Vertical") < 0) {
				int dest = Mathf.Clamp (row + 1, 0, 7);
				Move (dest, col);
				direction = 3;
			} else if (Input.GetAxis ("Vertical") > 0) {
				int dest = Mathf.Clamp (row - 1, 0, 7);
				Move (dest, col);
				direction = 4;
			}
			if (direction != 0) {
				bool push = true;//mgr.CheckForBlock (destRow, destCol);
				if (push) {
					mgr.PushBlock (direction, destRow, destCol);
				}
			}
		} 
	}

}

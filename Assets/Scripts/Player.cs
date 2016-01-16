using UnityEngine;
using System.Collections;

public class Player : BoardObject
{

	private Animator animator;

	// Use this for initialization
	void Start ()
	{
		boardType = BoardType.PLAYER;
		animator = GetComponent<Animator> ();
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
			}else {
					animator.SetBool ("Pushing", false);
				}

			animator.SetInteger ("Direction", direction);
			bool jump_key = Input.GetKey (KeyCode.RightShift) || Input.GetKey (KeyCode.LeftShift);
			bool pull_key = Input.GetKey (KeyCode.Tab);
			//Debug.Log (string.Format("shift_down equals", shift_down)); 
			if (direction != 0) {

				if ((mgr.GetPlayerInPosition (destR, destC) != null) && 
					(mgr.CheckForBlock(destR, destC) == false)) 
					return; //another player in your destination square, and no block underneath them

				if (jump_key) {
					//jump on top of existing block.  Just move player. 
					Move (destR, destC);
					mgr.SetPlayerPosition (destR, destC, this); 
					mgr.VacatePlayerPosition (row, col); 

				} else if (pull_key) {
					//pulling -- when pulling, player moves first
					animator.SetBool ("Pulling", true);
					Move (destR, destC);
					mgr.SetPlayerPosition (destR, destC, this); 
					mgr.VacatePlayerPosition (row, col); 

					//blocks move next to rol, col (where player was)
					mgr.PullBlock (direction, row, col); 

				} else if (mgr.CheckForBlock (destR, destC)) {
					//push if block is in the way
					animator.SetBool ("Pushing", true);
					if (mgr.PushBlock (direction, destR, destC)) {  //recursive call allows you to move a row of blocks 
						//when pushing, blocks move first
						//player moves next
						Move (destR, destC);
						mgr.SetPlayerPosition (destR, destC, this); 
						mgr.VacatePlayerPosition (row, col);
					}
				} else {
					//pure movement 
					Move (destR, destC);
					mgr.SetPlayerPosition (destR, destC, this); 
					mgr.VacatePlayerPosition (row, col);
					return; 
				}
			}
		} 
	}

}

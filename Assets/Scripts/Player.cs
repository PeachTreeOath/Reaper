using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : BoardObject
{

	private Animator animator;
	public bool isBot;
	public float pushDelay = 0.2f;
	public float jumpDelay = 0.2f;
	private bool isJumping;
	private bool isPulling;
	private float jumpTime;
	private int prevDirection = 3;
	private int pullDirection = 3;
	private bool jumpAgainstWall = false;
	private Vector2 jumpStartPosition;

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

		if (isJumping) {
			jumpTime += Time.deltaTime * speed;
	
			if (jumpTime >= 1) {
				isJumping = false;
				jumpAgainstWall = false;
				jumpStartPosition = Vector2.zero;
			} else {
				if (jumpAgainstWall) {
					transform.position = new Vector2 (jumpStartPosition.x, jumpStartPosition.y + GetJumpHeight (jumpTime));
				} else {
					transform.position = new Vector2 (transform.position.x, transform.position.y + GetJumpHeight (jumpTime));
				}
			}
		}
	
		if (!moving) {
			int direction = 0; // L R U D
			int destR = row;
			int destC = col;
			//bool pull_key;

			if (!isBot) {
				if (Input.GetKeyDown (KeyCode.Space) && !isBot && !isJumping) {
					Jump ();
					return;
				}

				if (Input.GetAxis ("Horizontal") < 0) {
					destC = Mathf.Clamp (col - 1, 0, mgr.boardSize + 1);
					direction = 1;
					prevDirection = direction;
				} else if (Input.GetAxis ("Horizontal") > 0) {
					destC = Mathf.Clamp (col + 1, 0, mgr.boardSize + 1);
					direction = 2;
					prevDirection = direction;
				} else if (Input.GetAxis ("Vertical") < 0) {
					destR = Mathf.Clamp (row + 1, 0, mgr.boardSize + 1);
					direction = 3;
					prevDirection = direction;
				} else if (Input.GetAxis ("Vertical") > 0) {
					destR = Mathf.Clamp (row - 1, 0, mgr.boardSize + 1);
					direction = 4;
					prevDirection = direction;
				} else {
					animator.SetBool ("Pushing", false);
				}

				animator.SetInteger ("Direction", direction);
				if (Input.GetKeyDown (KeyCode.Tab) && !isPulling) {
					pullDirection = prevDirection;
					isPulling = true;
				} else if (Input.GetKeyUp (KeyCode.Tab)) {
					pullDirection = 0;
					isPulling = false;
				}

			} else {
				direction = Random.Range (0, 11);
				//pull_key = Random.Range (0, 1) == 1;

				if (direction == 1) {
					destC = Mathf.Clamp (col - 1, 0, mgr.boardSize + 1);
				} else if (direction == 2) {
					destC = Mathf.Clamp (col + 1, 0, mgr.boardSize + 1);
				} else if (direction == 3) {
					destR = Mathf.Clamp (row + 1, 0, mgr.boardSize + 1);
				} else if (direction == 4) {
					destR = Mathf.Clamp (row - 1, 0, mgr.boardSize + 1);
				}
			}


			if (direction >= 1 && direction <= 4) {

				//Debug.Log("player in dest position " + (mgr.GetPlayerInPosition(destR, destC)!= null));
				//Debug.Log ("block in dest position " + (mgr.GetBlockInPosition (destR, destC) != null));
					

				if ((mgr.GetPlayerInPosition (destR, destC) != null) &&
				    (mgr.CheckForBlock (destR, destC) == false)) {
					return; //another player in your destination square, and no block underneath them

				} else if (isPulling) {
					
					//If pull key held down and move fwd, just push block
					if (pullDirection == direction) {
						PushBlock (direction, row, col, destR, destC);
					} else {
						PullBlock (direction, row, col, destR, destC);
					}
				} else if (mgr.CheckForBlock (destR, destC)) {
					PushBlock (direction, row, col, destR, destC);
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

	private void Jump ()
	{
		jumpTime = 0;
		isJumping = true;	

		//pure movement 
		int destR = row;
		int destC = col;
		if (prevDirection == 1) {
			destC = Mathf.Clamp (col - 1, 0, mgr.boardSize + 1);
		} else if (prevDirection == 2) {
			destC = Mathf.Clamp (col + 1, 0, mgr.boardSize + 1);
		} else if (prevDirection == 3) {
			destR = Mathf.Clamp (row + 1, 0, mgr.boardSize + 1);
		} else if (prevDirection == 4) {
			destR = Mathf.Clamp (row - 1, 0, mgr.boardSize + 1);
		}
		if (destR == row && destC == col) {
			jumpAgainstWall = true;
			jumpStartPosition = GetBoardPosition (row, col);
		}
		Move (destR, destC);
		mgr.SetPlayerPosition (destR, destC, this); 
		mgr.VacatePlayerPosition (row, col);
	}

	private float GetJumpHeight (float time)
	{
		float a = 1f;
		float diff = time - 0.5f;
		float height = (-a * diff * diff) + 0.25f;
		return Mathf.Clamp (height, 0, 100);
	}

	private void PushBlock (int direction, int row, int col, int destR, int destC)
	{
		animator.SetBool ("Pushing", true);
		if (mgr.PushBlock (direction, destR, destC) || //recursive call allows you to move a row of blocks 
		//when pushing, blocks moves first, player moves next
		    ((mgr.GetBlockInPosition (row, col) != null) &&
		    (mgr.GetPlayerInPosition (destR, destC) == null) &&
		    (mgr.GetBlockInPosition (destR, destC) != null))) {
			// or both current position and dest position is on a block, and no player on the dest block
			Move (destR, destC);
			mgr.SetPlayerPosition (destR, destC, this); 
			mgr.VacatePlayerPosition (row, col); 
		}
	}

	private void PullBlock (int direction, int row, int col, int destR, int destC)
	{
		//pulling -- when pulling, player moves first
		animator.SetBool ("Pulling", true);

		//Don't pull if there's a player or block behind you
		if (!mgr.CheckForBlock (destR, destC) && mgr.GetPlayerInPosition (destR, destC) == null) {
			Move (destR, destC);
			mgr.SetPlayerPosition (destR, destC, this); 
			mgr.VacatePlayerPosition (row, col); 

			//blocks move next to rol, col (where player was)
			mgr.PullBlock (direction, row, col); 
		}
	}
}

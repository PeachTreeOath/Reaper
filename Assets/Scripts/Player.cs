using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : BoardObject
{

	private Animator animator;
	public bool isBot;
	public float pushPullDelay = 0.2f;
	public float jumpDelay = 0.2f;
	private bool isJumping;
	private bool isPulling;
	private float jumpTime;
	private float pushPullTime;
	private int prevDirection = 3;
	public int pullDirection = 3;
	private bool jumpAgainstWall = false;
	private Vector2 jumpStartPosition;
	private Dictionary<int,int> pullMap;
	// Defines what directions you can pull in

	// Use this for initialization
	void Start ()
	{
		pullMap = new Dictionary<int,int> ();
		pullMap.Add (1, 2);
		pullMap.Add (2, 1);
		pullMap.Add (3, 4);
		pullMap.Add (4, 3);

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
			int direction = 0; // L R D U
			int destR = row;
			int destC = col;
			int pullSrcR = row;
			int pullSrcC = col;
			//bool pull_key;

			if (!isBot) {
				if (Input.GetKeyDown (KeyCode.Space) && !isBot && !isJumping) {
					Jump ();
					return;
				}
				animator.SetBool ("Pushing", false);
				animator.SetBool ("Pulling", false);

				if (Input.GetAxis ("Horizontal") < 0) {
					destC = Mathf.Clamp (col - 1, 0, mgr.boardSize + 1);
					pullSrcC = Mathf.Clamp (col + 1, 0, mgr.boardSize + 1);
					direction = 1;
					SetPrevDirectionVars (direction);
				} else if (Input.GetAxis ("Horizontal") > 0) {
					destC = Mathf.Clamp (col + 1, 0, mgr.boardSize + 1);
					pullSrcC = Mathf.Clamp (col - 1, 0, mgr.boardSize + 1);
					direction = 2;
					SetPrevDirectionVars (direction);
				} else if (Input.GetAxis ("Vertical") < 0) {
					destR = Mathf.Clamp (row + 1, 0, mgr.boardSize + 1);
					pullSrcR = Mathf.Clamp (row - 1, 0, mgr.boardSize + 1);
					direction = 3;
					SetPrevDirectionVars (direction);
				} else if (Input.GetAxis ("Vertical") > 0) {
					destR = Mathf.Clamp (row - 1, 0, mgr.boardSize + 1);
					pullSrcR = Mathf.Clamp (row + 1, 0, mgr.boardSize + 1);
					direction = 4;
					SetPrevDirectionVars (direction);
				} else {
					pushPullTime = 0;
				}

				animator.SetInteger ("Direction", direction);
				if (Input.GetKey (KeyCode.Tab) || Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
					animator.SetInteger ("Prevdirection", pullDirection);
					if (!isPulling) {
						pullDirection = prevDirection;
						isPulling = true;
						animator.SetBool ("TryPulling", true);

					} 
				} else if (!Input.GetKey (KeyCode.Tab)) {
					pullDirection = 0;
					isPulling = false;
					animator.SetBool ("TryPulling", false);
					animator.SetBool ("Pulling", false);

					//prevDirection = pullMap [animator.GetInteger ("Direction")];
				}

			} else {
				direction = Random.Range (0, 11);

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
					if (mgr.CheckForBlock (pullSrcR, pullSrcC)) {
						//If pull key held down and move fwd, just push block
						if (pullDirection == direction) {
							PushBlock (direction, row, col, destR, destC);
							return;
						} else {
							PullBlock (direction, row, col, destR, destC);
							return;
						}
					}
				} 
				if (mgr.CheckForBlock (destR, destC)) {
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

	private void SetPrevDirectionVars (int direction)
	{
		if (direction != prevDirection) {
			pushPullTime = 0;
		}
		prevDirection = direction;
		animator.SetInteger ("Prevdirection", prevDirection);
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
		pushPullTime += Time.deltaTime;
		if (pushPullTime < pushPullDelay) {
			return;
		}
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
		animator.SetBool ("Pulling", true);
		pushPullTime += Time.deltaTime;
		if (pushPullTime < pushPullDelay) {
			return;
		}
		//Don't pull if there's a player or block behind you
		if (!mgr.CheckForBlock (destR, destC) && mgr.GetPlayerInPosition (destR, destC) == null) {
			//pulling -- when pulling, player moves first
			Move (destR, destC);
			mgr.SetPlayerPosition (destR, destC, this); 
			mgr.VacatePlayerPosition (row, col); 

			//blocks move next to rol, col (where player was)
			mgr.PullBlock (direction, row, col); 
		}
	}
}

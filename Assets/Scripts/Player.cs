using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : BoardObject
{
	public enum SwapIndex
	{
		Outline = 39,
		LightSkin = 66,
		LightSkinOff = 46,
		// 1 pixel mistake
		DarkSkin = 51,
		BeakLight = 101,
		BeakDark = 89,
		Rune = 161,
	}

	private Texture2D mColorSwapTex;
	private Color[] mSpriteColors;

	private Animator animator;
	public bool isBot;
	public float pushPullDelay = 0.2f;
	public float landDelay = 0.2f;
	private bool isJumping;
	private bool isPulling;
	private bool isLanded;
	private float jumpTime;
	private float landTime;
	private float pushPullTime;
	public int prevDirection = 3;
	public int jumpDirection = 3;
	public int pullDirection = 3;
	private bool jumpAgainstWall = false;
	private Vector2 jumpStartPosition;
	private Dictionary<int,int> pullMap;
	// Defines what directions you can pull in

	public void InitColorSwapTex ()
	{
		Texture2D colorSwapTex = new Texture2D (256, 1, TextureFormat.RGBA32, false, false);
		colorSwapTex.filterMode = FilterMode.Point;
		for (int i = 0; i < colorSwapTex.width; i++) {
			colorSwapTex.SetPixel (i, 0, new Color (0f, 0f, 0f, 0f));
		}
		colorSwapTex.Apply ();
		GetComponent<SpriteRenderer> ().material.SetTexture ("_SwapTex", colorSwapTex);
		mSpriteColors = new Color[colorSwapTex.width];
		mColorSwapTex = colorSwapTex;
	}

	public void SwapColor (SwapIndex index, Color32 color32)
	{
		Color color = (Color)color32;
		mSpriteColors [(int)index] = color;
		mColorSwapTex.SetPixel ((int)index, 0, color);

		//For fixing mistakes with the sprite
		if (index == SwapIndex.LightSkin) {
			mSpriteColors [(int)SwapIndex.DarkSkin] = color;
			mColorSwapTex.SetPixel ((int)SwapIndex.DarkSkin, 0, color);
			mSpriteColors [(int)SwapIndex.LightSkinOff] = color;
			mColorSwapTex.SetPixel ((int)SwapIndex.LightSkinOff, 0, color);
		}
	}

	public void ChangeColor (int playerNum)
	{
		switch (playerNum) {
		case 0:
			SwapColor (SwapIndex.LightSkin, new Color32 (66, 33, 0, 255));
			break;
		case 1:
			SwapColor (SwapIndex.Outline, new Color32 (0, 12, 40, 255));
			SwapColor (SwapIndex.LightSkin, new Color32 (0, 58, 180, 255));
			SwapColor (SwapIndex.Rune, new Color32 (0, 100, 64, 255));
			break;
		case 2:
			SwapColor (SwapIndex.Outline, new Color32 (38, 0, 0, 255));
			SwapColor (SwapIndex.LightSkin, new Color32 (148, 0, 11, 255));
			SwapColor (SwapIndex.Rune, new Color32 (175, 0, 161, 255));
			break;
		case 3:
			SwapColor (SwapIndex.Outline, new Color32 (38, 38, 0, 255));
			SwapColor (SwapIndex.LightSkin, new Color32 (160, 160, 0, 255));
			SwapColor (SwapIndex.Rune, new Color32 (18, 0, 159, 255));
			break;
		}
		mColorSwapTex.Apply ();
	}
	// Use this for initialization
	void Start ()
	{
		InitColorSwapTex ();

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
				isLanded = true;
				jumpAgainstWall = false;
				landTime = 0;
				jumpStartPosition = Vector2.zero;
			} else {
				if (jumpAgainstWall) {
					transform.position = new Vector2 (jumpStartPosition.x, jumpStartPosition.y + GetJumpHeight (jumpTime));
				} else {
					transform.position = new Vector2 (transform.position.x, transform.position.y + GetJumpHeight (jumpTime));
				}
			}
		}

		if (isLanded) {
			landTime += Time.deltaTime;
			if (landTime < landDelay) {
				return;
			}
		}

		if (!moving) {
			int direction = 0; // L R D U
			int destR = row;
			int destC = col;
			int pullSrcR = row;
			int pullSrcC = col;

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
					jumpDirection = pullDirection;
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
				direction = Random.Range (0, 20);

				if (direction == 1) {
					destC = Mathf.Clamp (col - 1, 0, mgr.boardSize + 1);
					animator.SetInteger ("Direction", direction);
				} else if (direction == 2) {
					destC = Mathf.Clamp (col + 1, 0, mgr.boardSize + 1);
					animator.SetInteger ("Direction", direction);
				} else if (direction == 3) {
					destR = Mathf.Clamp (row + 1, 0, mgr.boardSize + 1);
					animator.SetInteger ("Direction", direction);
				} else if (direction == 4) {
					destR = Mathf.Clamp (row - 1, 0, mgr.boardSize + 1);
					animator.SetInteger ("Direction", direction);
				}

			}
				
			if (direction >= 1 && direction <= 4) {
				if ((mgr.GetPlayerInPosition (destR, destC) != null) &&
				    (mgr.CheckForBlock (destR, destC) == false)) {
					return; //another player in your destination square, and no block underneath them

				} else if (isPulling) {
					if (mgr.CheckForBlock (pullSrcR, pullSrcC)) {
						//If pull key held down and move fwd, just push block
						if (pullDirection == direction) {
							PushBlock (direction, row, col, destR, destC);
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
					pullDirection = prevDirection;
					return; 
				}
			}
		} 
	}

	private void SetPrevDirectionVars (int direction)
	{
		if (direction != prevDirection && !isPulling) {
			pushPullTime = 0;
		}
		prevDirection = direction;
		jumpDirection = direction;
		animator.SetInteger ("Prevdirection", prevDirection);
	}

	private void Jump ()
	{
		jumpTime = 0;
		isJumping = true;	

		//pure movement 
		int destR = row;
		int destC = col;
		if (jumpDirection == 1) {
			destC = Mathf.Clamp (col - 1, 0, mgr.boardSize + 1);
		} else if (jumpDirection == 2) {
			destC = Mathf.Clamp (col + 1, 0, mgr.boardSize + 1);
		} else if (jumpDirection == 3) {
			destR = Mathf.Clamp (row + 1, 0, mgr.boardSize + 1);
		} else if (jumpDirection == 4) {
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
		// Check if blocks are not locked w/o pushing
		if (mgr.PushBlock (direction, destR, destC, false)) {
			animator.SetBool ("Pushing", true);
			pushPullTime += Time.deltaTime;
			if (pushPullTime < pushPullDelay) {
				return;
			}
		} else if (((mgr.GetBlockInPosition (row, col) != null) &&
		           (mgr.GetPlayerInPosition (destR, destC) == null) &&
		           (mgr.GetBlockInPosition (destR, destC) != null))) { 
			// Walk freely over blocks if they are locked
			Move (destR, destC);
			mgr.SetPlayerPosition (destR, destC, this); 
			mgr.VacatePlayerPosition (row, col); 
		}

		// Push block if they are not locked, after push delay is over
		if (mgr.PushBlock (direction, destR, destC, true)) {
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
			prevDirection = pullMap [animator.GetInteger ("Direction")]; //TODO: Some kind of bug still with pulling in certain direction sequences

			//blocks move next to rol, col (where player was)
			mgr.PullBlock (direction, row, col); 
		}
	}
}

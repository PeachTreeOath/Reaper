using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

	private BoardObject[,] board;

	// Use this for initialization
	void Awake ()
	{
		board = new BoardObject[7, 7];
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public bool CheckForBlock (int row, int col)
	{
		if (board [row, col].boardType == BoardObject.BoardType.BLOCK) {
			return true;
		}

		return false;
	}

	public void PushBlock (int direction, int row, int col)
	{
		BoardObject block = board [row, col];
		if (block == null) {
			Debug.Log ("NULL");
			return;
		}

		int destRow = row;
		int destCol = col;

		if (direction == 1) {
			destCol = col - 1;	
		} else if (direction == 2) {
			destCol = col + 1;	
		} else if (direction == 3) {
			destRow = row + 1;
		} else if (direction == 4) {
			destRow = row - 1;
		}
		PushBlock (direction, destRow, destCol);
		block.Move (destRow, destCol);
		board [destRow, destCol] = block;
		board [row, col] = null;

	}

	public void PlaceBlock (BoardObject block, int row, int col)
	{
		board [row, col] = block;
	}

	public void SetPlayerPosition (int row, int col)
	{

	}
}
